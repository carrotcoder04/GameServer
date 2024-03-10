using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Application.Messaging
{
    public interface IMessging<T>
    {
        public WsTags tags { get; set; }
        public T data { get; set; }
    }
}
