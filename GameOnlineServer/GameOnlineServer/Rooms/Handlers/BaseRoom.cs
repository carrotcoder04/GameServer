using GameOnlineServer.Application.Handlers;
using GameOnlineServer.Application.Interfaces;
using GameOnlineServer.Application.Messaging.MessageBinary;
using GameOnlineServer.Rooms.Interfaces;
using System.Collections.Concurrent;
using static GameOnlineServer.Application.Actions.Actions;
namespace GameOnlineServer.Rooms.Handlers
{
    public class BaseRoom : IBaseRoom
    {
        public string idroom { get; set; }
        public ConcurrentDictionary<string, IPlayer> players { get; set; }
        List<byte> IDInRoom = new List<byte>();
        Random random = new Random();
        public BaseRoom()
        {
            idroom = GameHelper.RandomString(4);
            players = new ConcurrentDictionary< string, IPlayer>();
        }
        public bool ExitRoom(IPlayer player)
        {
            return this.ExitRoom(player.sessionId);
        }
        public bool ExitRoom(string id)
        {
            var player = FindPlayer(id);
            if (player != null)
            {
                players.TryRemove(id, out player);
                if(players.Count == 0)
                {
                    RequestRemoveRoom?.Invoke(idroom);
                    return true;
                }
                IDInRoom.Remove(player.IdInRoom);
                this.PlayerLeaveRoom(player);
                return true;
            }
            return false;
        }
        public bool ExitLobby(IPlayer player)
        {
            return this.ExitRoom(player.sessionId);
        }
        public bool ExitLobby(string id)
        {
            var player = FindPlayer(id);
            if (player != null)
            {
                players.TryRemove(id, out player);
                return true;
            }
            return false;
        }
        public IPlayer FindPlayer(string id)
        {
            return players.FirstOrDefault(p => p.Key.Equals(id)).Value;
        }

        public bool JoinRoom(IPlayer player)
        {
            if (FindPlayer(player.sessionId) == null)
            {
                if (players.TryAdd(player.sessionId, player))
                {
                    byte rand = (byte)random.Next(0, 255);
                    while(IDInRoom.Contains(rand))
                    {
                        rand = (byte)random.Next(0, 255);
                    }
                    player.IdInRoom = rand;
                    IDInRoom.Add(rand);
                    this.RoomInfo(player);
                    NewPlayerJoinRoom(player);
                    this.SendByte((byte)Room.ROOM_NAME, idroom,player);
                    return true;
                }
            }
            return false;
        }
        public bool JoinLobby(IPlayer player)
        {
            if (FindPlayer(player.sessionId) == null)
            {
                if (players.TryAdd(player.sessionId, player))
                {
                    return true;
                }
            }
            return false;
        }
        public void SendByte(byte tag,string message)
        {
            lock(players)
            {
                foreach(var player in players.Values)
                {
                    player.SendByte(tag,message);
                }
            }
        }
        public void SendByte(byte tag, string message,IPlayer _player)
        {
            lock (players)
            {
                _player.SendByte(tag,message);
            }
        }
        public void SendByte(byte tag)
        {
            lock (players)
            {
                foreach (var player in players.Values)
                {
                    player.SendByte(tag);
                }
            }
        }
        public void SendByte(byte tag,ISerializable data)
        {
            lock (players)
            {
                foreach (var player in players.Values)
                {
                    player.SendByte(tag,data);
                }
            }
        }
        public void SendByte(byte tag, ISerializable data,IPlayer player)
        {
            lock(players)
            {
                player.SendByte(tag, data);
            }
        }
        public void RoomInfo(IPlayer _player)
        {
            Dictionary<byte, string> dictionary = this.players.ToDictionary(pair => pair.Value.IdInRoom, pair => pair.Value.GetPlayerInfo());
            RoomMessage roomMessage = new RoomMessage(dictionary);
            this.SendByte((byte)Room.ROOM_INFO,roomMessage,_player);
        }
        public void NewPlayerJoinRoom(IPlayer player)
        {
            KeyValuePair<byte, string> kvp = new KeyValuePair<byte, string>(player.IdInRoom, player.GetPlayerInfo());
            RoomUpdate roomUpdate = new RoomUpdate(kvp);
            this.SendByte((byte)Room.NEW_PLAYER_JOIN_ROOM, roomUpdate);
        }
        public void PlayerLeaveRoom(IPlayer player)
        {
            KeyValuePair<byte, string> kvp = new KeyValuePair<byte, string>(player.IdInRoom,player.GetPlayerInfo());
            RoomUpdate roomUpdate = new RoomUpdate(kvp);
            this.SendByte((byte)Room.PLAYER_LEAVE_ROOM, roomUpdate);
        }

        public void SendByte(byte tag, byte[] data)
        {
            lock (players)
            {
                foreach (var player in players.Values)
                {
                    player.SendByte(tag, data);
                }
            }
        }
    }
}
