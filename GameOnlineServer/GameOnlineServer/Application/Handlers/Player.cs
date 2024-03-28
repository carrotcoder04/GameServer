using GameOnlineServer.Application.Interfaces;
using GameOnlineServer.Application.Logging;
using GameOnlineServer.GameModels;
using GameOnlineServer.GameModels.Handlers;
using GameOnlineServer.Rooms.Handlers;
using GameOnlineServer.Rooms.Interfaces;
using MongoDB.Driver;
using NetCoreServer;
using System.Text;
using GameOnlineServer.Application.Messaging.MessageBinary;
using static GameOnlineServer.Application.Messaging.MessageBinary.BinaryConvert;
namespace GameOnlineServer.Application.Handlers
{
    public class Player : WsSession, IPlayer
    {
        public event Action<byte[]> PlayerMessage;
        public string sessionId { get;set; }
        public string name { get; set; }
        public bool isDisconnected { get; set; }
        public UserHandler userDb { get; set; }
        public User userInfo { get; set; }
        public BaseRoom currentroom { get; set; }
        private readonly IGameLogger logger;
       // public readonly IRoomManager roomManager;
        public Player(WsServer server, IGameLogger logger, IMongoDatabase database) : base(server)
        {
            currentroom = null;
            PlayerMessage = OnLogMessage;
            sessionId = this.Id.ToString();
            this.logger = logger;
            isDisconnected = false;
            userDb = new UserHandler(database);
        }
        /// <summary>
        /// Login + Register + Join lobby
        /// </summary>
        /// <param name="buffer">Data log nhận được</param>
        private void OnLogMessage(byte[] buffer)
        {
            try
            {
                Log info = (Log)buffer[0];
                if (info == Log.LOGIN)
                {
                    UserMessage user = new UserMessage(buffer);
                    logger.Info(user.username + " " + user.password);
                    var check = userDb.FindByUsername(user.username);
                    if (check != null)
                    {
                        if (user.password == check.password)
                        {
                            userInfo = check;
                            this.SendByte((byte)Log.LOGIN_SUCCESS);
                            return;
                        }
                    }
                    this.SendByte((byte)Log.USERNAME_OR_PASSWORD_INVALID);
                }
                else if (info == Log.REGISTER)
                {
                    UserMessage user = new UserMessage(buffer);
                    var check = userDb.FindByUsername(user.username);
                    if (check != null)
                    {
                        this.SendByte((byte)Log.USERNAME_OR_PASSWORD_INVALID);
                        return;
                    }
                    var newUser = new User(user.username, user.password);
                    userInfo = userDb.Create(newUser);
                    if (userInfo != null)
                    {
                        this.SendByte((byte)Log.REGISTER_SUCCESS);
                    }
                }
                else if(info == Log.JOIN_LOBBY)
                {
                    this.PlayerJoinLobby();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error " + ex.Message);
            }
        }
        /// <summary>
        /// Join Room + Create Room
        /// </summary>
        /// <param name="buffer"></param>
        private void OnLobbyMessage(byte[] buffer)
        {
            try
            {
                Lobby info = (Lobby)buffer[0];
                if (info == Lobby.JOIN_ROOM)
                {
                    var lobbyMessage = new LobbyMessage(buffer);
                    BaseRoom room = ((WsGameServer) Server).roomManager.FindRoom(lobbyMessage.idroom);
                    if (room != null && room.players.Count < 2)
                    {
                        this.PlayerJoinRoom(room);
                    }
                    else if(room == null)
                    {
                        this.SendByte((byte)Lobby.ROOM_NOT_FOUND);
                    }
                    else if(room.players.Count >= 2)
                    {
                        this.SendByte((byte)Lobby.ROOM_FULL);
                    }
                }
                else if (info == Lobby.CREATE_ROOM)
                {
                    this.PlayerCreateRoom();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error: " + ex.Message);
            }
        }
        private void OnRoomMessage(byte[] buffer)
        {
            try
            {
                Room info = (Room)buffer[0];
                if(info == Room.PLAY)
                {
                    if (currentroom.players.Count == 2)
                    {
                        currentroom.SendByte((byte)Room.PLAY);
                        foreach (var player in currentroom.players.Values)
                        {
                            player.PlayerMessage += OnGameMessage;
                            player.PlayerMessage -= OnRoomMessage;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error("Error " + ex.Message);
            }
        }

        private void OnGameMessage(byte[] buffer)
        {
            
        }

        public override void OnWsConnected(HttpRequest request)
        {
            isDisconnected = false;
        }
        public override void OnWsDisconnected()
        {
            On_DisConnected();
        }
        public override void OnWsReceived(byte[] buffer, long offset, long size)
        {
            PlayerMessage?.Invoke(buffer);
        }
        public void SendByte(byte tag, byte[] data)
        {
            data[0] = tag;
            this.SendBinary(data, 0, data.Length);
        }
        public void SendByte(byte tag, byte isSuccess)
        {
            byte[] data = new byte[2];
            data[0] = tag;
            data[1] = isSuccess;
            this.SendBinaryAsync(data, 0, data.Length);
        }
        public void SendByte(byte tag)
        {
            byte[] data = new byte[] {tag};
            this.SendBinaryAsync(data, 0, data.Length);
        }
        public void SendByte(byte tag, ISerializable data)
        {
            byte[] bytes = data.ConvertByte();
            bytes[0] = tag;
            this.SendBinaryAsync(bytes, 0, bytes.Length);
        }
        public void SendByte(byte tag, string data)
        {
            byte[] datas = Encoding.UTF8.GetBytes(data);
            datas = AddFirst(tag, datas);
            this.SendBinaryAsync(datas, 0, datas.Length);
        }
        private void PlayerJoinLobby()
        {
            ChangeRoom(((WsGameServer)Server).roomManager.lobby);
            if(currentroom.JoinLobby(this))
            {
                PlayerMessage = OnLobbyMessage;
                this.SendByte((byte)Log.JOIN_LOBBY_SUCCESS);
            }
            else
            {
                this.SendByte((byte)Log.JOIN_LOBBY_FAIL);
            }
        }
        private void PlayerCreateRoom()
        {
            BaseRoom newRoom = ((WsGameServer)Server).roomManager.CreateRoom();
            this.PlayerJoinRoom(newRoom);
            PlayerMessage = OnRoomMessage;
        }
        private void PlayerJoinRoom(BaseRoom room)
        {
            ChangeRoom(room);
            this.SendByte((byte)Lobby.JOIN_ROOM_SUCCESS);
            room.JoinRoom(this);
            PlayerMessage = OnRoomMessage;
        }
        public void SetDisconnected(bool value)
        {
            isDisconnected = value;
        }
        public void On_DisConnected()
        {
            currentroom.ExitRoom(this);
        }
        public string GetPlayerInfo()
        {
            if(userInfo != null)
            {
                return userInfo.username;
            }
            return null;
        }
        public void ChangeRoom(BaseRoom room)
        {
            if(currentroom != null) currentroom.ExitRoom(this);
            currentroom = room;
        }
    }
}
