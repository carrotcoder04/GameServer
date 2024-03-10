using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Application.Messaging.Constants
{
    public struct LobbyInfo
    {
        public List<UserInfo> players { get; set; }

    }
}
