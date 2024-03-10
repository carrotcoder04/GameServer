using GameOnlineServer.Application.Interfaces;
using GameOnlineServer.Application.Messaging;
using GameOnlineServer.Rooms.Handlers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Rooms.Interfaces
{
    public interface IBaseRoom
    {
        public string id { get; set; } 
        public ConcurrentDictionary<string,IPlayer> players { get; set; }
        bool JoinRoom(IPlayer player);
        bool ExitRoom(IPlayer player);
        bool ExitRoom(string id);
        IPlayer FindPlayer(string id);
        void SendMessage(string message);
        void SendMessage<T>(WsMessage<T> message);
        void SendMessage<T>(WsMessage<T> message,string idIgnore);
        
    }
}
