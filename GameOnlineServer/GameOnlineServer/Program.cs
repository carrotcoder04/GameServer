using GameOnlineServer.Application.Handlers;
using GameOnlineServer.Application.Interfaces;
using GameOnlineServer.Application.Logging;
using System;
using System.Net;
using GameDatabase.Mongodb.Handlers;
using GameOnlineServer.GameModels;
using GameOnlineServer.Rooms.Interfaces;
using GameOnlineServer.Rooms.Handlers;

namespace GameOnlineServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IGameLogger logger= new GameLogger();
            var mongodb = new MongoDb();
            IPlayerManager playerManager = new PlayerManager(logger);
            IRoomManager roomManager = new RoomManager();
            var wsSever = new WsGameServer(IPAddress.Any,8080,playerManager,logger,mongodb,roomManager);
            wsSever.StartServer();
            while (true)
            {
                var type = Console.ReadLine();
                if (type == "restart")
                {
                    logger.Print("Game Server Restarting...");
                    wsSever.RestartServer();
                }
                else if(type == "stop")
                {
                    logger.Print("Game Server Shutdown");
                    wsSever.StopServer();
                }
            }
        }
    }
}