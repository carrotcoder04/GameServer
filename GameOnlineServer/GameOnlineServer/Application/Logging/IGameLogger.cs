using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Application.Logging
{
    public interface IGameLogger
    {
        void Print(string msg);
        void Info(string msg);
        void Warning(string msg,Exception ex);
        void Error(string msg,Exception ex);
        void Error(string msg);
    }
}
