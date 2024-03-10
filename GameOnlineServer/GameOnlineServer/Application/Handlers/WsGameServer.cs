using GameDatabase.Mongodb.Handlers;
using GameOnlineServer.Application.Interfaces;
using GameOnlineServer.Application.Logging;
using GameOnlineServer.GameModels;
using GameOnlineServer.Rooms.Interfaces;
using NetCoreServer;
using System.Net;
using System.Net.Sockets;

namespace GameOnlineServer.Application.Handlers
{
    public class WsGameServer : WsServer, IWsGameServer
    {
        private int _port;
        public readonly IPlayerManager playerManager;
        private readonly IGameLogger logger;
        public readonly IRoomManager roomManager;
        private readonly MongoDb mongodb;
        public WsGameServer(IPAddress address, int port, IPlayerManager playerManager,IGameLogger logger,MongoDb mongodb,IRoomManager roomManager) : base(address, port)
        {
            _port = port;
            this.playerManager = playerManager;
            this.logger = logger;
            this.mongodb = mongodb;
            this.roomManager = roomManager;
        }
        protected override TcpSession CreateSession()
        {
            var player = new Player(this, logger, mongodb.GetDataBase());
            playerManager.AddPlayer(player);
            return player;
        }
        protected override void OnDisconnected(TcpSession session)
        {
            playerManager.RemovePlayer(session.Id.ToString());
            logger.Info($"Client disconnected: {session.Id}");
        }
        public void StartServer()
        {
            logger.Print("Game Server Starting...");
            if (this.Start())
            {
                logger.Print($"Server start at port: {_port}");
            }
        }
        protected override void OnError(SocketError error)
        {
            logger.Error($"Server start error: {error}");
        }
        public void StopServer()
        {
            if(this.Stop())
            {
                logger.Print("Server stopped");
            }
        }

        public void RestartServer()
        {
            if(this.Restart())
            {
                logger.Print("Server restart completed!");
            } 
        }
        public void SendAll(string mess)
        {
            this.MulticastText(mess);
        }
    }
}
