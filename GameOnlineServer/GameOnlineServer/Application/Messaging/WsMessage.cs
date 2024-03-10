using MemoryPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Application.Messaging
{
    [MemoryPackable]
    public partial class WsMessage<T> : IMessging<T>
    {
        public WsTags tags { get; set; }
        public T data { get; set; }
        public WsMessage(WsTags tags,T data)
        {
            this.tags = tags;
            this.data = data;
        }
    }
}
