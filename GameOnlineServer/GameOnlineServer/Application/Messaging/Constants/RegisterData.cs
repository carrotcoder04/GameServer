using MemoryPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Application.Messaging.Constants
{
    [MemoryPackable]
    public partial struct RegisterData
    {
        public string username { get; set; }
        public string password { get; set; }
        public string displayName { get; set; }

    }
}
