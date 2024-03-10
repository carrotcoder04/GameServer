using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Application.Messaging.Constants
{
    public struct UserInfo
    {
        public string username { get; set; }
        public string displayName { get; set; }
        public string avatar { get; set; }
        public int level { get; set; }
        public long amount { get; set; }
    }
}
