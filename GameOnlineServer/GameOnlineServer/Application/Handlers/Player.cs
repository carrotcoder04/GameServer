using GameDatabase.Mongodb.Handlers;
using GameDatabase.Mongodb.Interfaces;
using GameOnlineServer.Application.Interfaces;
using GameOnlineServer.Application.Logging;
using GameOnlineServer.Application.Messaging;
using GameOnlineServer.Application.Messaging.Constants;
using GameOnlineServer.GameModels;
using GameOnlineServer.GameModels.Handlers;
using GameOnlineServer.Rooms.Interfaces;
using MongoDB.Driver;
using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Application.Handlers
{
    public class Player : WsSession, IPlayer
    {
        public string sessionId { get;set; }
        public string name { get; set; }
        public bool isDisconnected { get; set; }
        public UserHandler userDb { get; set; }
        public User userInfo { get; set; }
        private readonly IGameLogger logger;
        public Player(WsServer server, IGameLogger logger, IMongoDatabase database) : base(server)
        {
            sessionId = this.Id.ToString();
            this.logger = logger;
            isDisconnected = false;
            userDb = new UserHandler(database);
        }
        public override void OnWsConnected(HttpRequest request)
        {
            logger.Info($"Player connected {sessionId}");
            isDisconnected = false;
        }
        public override void OnWsDisconnected()
        {
            OnDisconnected();
            base.OnWsDisconnected();
        }
        public override void OnWsReceived(byte[] buffer, long offset, long size)
        {
            var mess = Encoding.UTF8.GetString(buffer,(int)offset, (int)size);
            SendMessage("Server reply: " + mess);
            try
            {
                var wsMessage = GameHelper.ParseStruct<WsMessage<object>>(mess);
                switch (wsMessage.tags)
                {
                    case WsTags.Invalid:
                        break;
                    case WsTags.Login:
                        var loginData = GameHelper.ParseStruct<LoginData>(wsMessage.data.ToString());
                        var check = userDb.FindByUsername(loginData.username);
                        if(check != null)
                        {
                            var hashPass = GameHelper.HashPassword(loginData.password);
                            if(hashPass == check.password)
                            {
                                userInfo = check;
                                this.SendMessage("Login successful!");
                                this.PlayerJoinLobby();
                                return;
                            }
                        }
                        var invalidMess = new WsMessage<string>(WsTags.Invalid,"Username or Password is Invalid");
                        this.SendMessage(invalidMess);
                        break;
                    case WsTags.Register:
                        if (userInfo != null)
                        {
                            invalidMess = new WsMessage<string>(WsTags.Invalid, "You are loggined");
                            this.SendMessage(invalidMess);
                            return;
                        }
                        var regData = GameHelper.ParseStruct<RegisterData>(wsMessage.data.ToString());
                        check = userDb.FindByUsername(regData.username);
                        if(check != null)
                        {
                            invalidMess = new WsMessage<string>(WsTags.Invalid, "Username is Existed");
                            var x = GameHelper.ToByte(invalidMess);
                            var y = GameHelper.ToObject<WsMessage<string>>(x);
                            this.SendMessage(y);
                            return;
                        }
                        var newUser = new User(regData.username, regData.password,regData.displayName);
                        userInfo = userDb.Create(newUser);
                        this.SendMessage($"Created {regData.username} successful!");
                        if (userInfo != null)
                        {
                            this.PlayerJoinLobby();
                        }
                        break;
                    case WsTags.RoomInfo:
                        this.PlayerJoinLobby();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error: " + ex.Message);
            }
        }
        private void PlayerJoinLobby()
        {
            var lobby = ((WsGameServer) Server).roomManager.lobby;
            lobby.JoinRoom(this);
        }
        public void SetDisconnected(bool value)
        {
            isDisconnected = value;
        }
        public bool SendByte(byte[] mes)
        {
            return this.SendBinaryAsync(mes, 0, mes.Length);
        }
        public bool SendByte<T>(WsMessage<T> message)
        {
            var mes = GameHelper.ToByte(message);
            return this.SendBinaryAsync(mes,0,mes.Length);
        }
        public bool SendMessage(string mess)
        {
            return this.SendTextAsync(mess);
        }
        public bool SendMessage<T>(WsMessage<T> message)
        {
            var mes = GameHelper.ParseString(message);
            return this.SendMessage(mes);
        }
        public void OnDisConnected()
        {
            var lobby = ((WsGameServer)Server).roomManager.lobby;
            lobby.ExitRoom(this);
            logger.Info("Player disconnected");
        }

        public UserInfo GetUserInfo()
        {
            if(userInfo != null)
            {
                return new UserInfo
                {
                    displayName = userInfo.displayName,
                    username = userInfo.username,
                    amount = userInfo.amount,
                    avatar = userInfo.avatar,
                    level = userInfo.level,
                };
            }
            return new UserInfo();
        }
    }
}
