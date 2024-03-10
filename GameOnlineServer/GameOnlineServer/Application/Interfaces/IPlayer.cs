using GameOnlineServer.Application.Messaging;
using GameOnlineServer.Application.Messaging.Constants;
using GameOnlineServer.GameModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Application.Interfaces
{
    public interface IPlayer
    {
        public string sessionId { get; set; }
        public string name { get; set; }
        void SetDisconnected(bool value);
        bool SendMessage(string mess);
        void OnDisConnected();
        bool SendMessage<T>(WsMessage<T> message);
        UserInfo GetUserInfo();
    }
}
