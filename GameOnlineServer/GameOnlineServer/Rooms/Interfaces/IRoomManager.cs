using GameOnlineServer.Rooms.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Rooms.Interfaces
{
    public interface IRoomManager
    {
        BaseRoom FindRoom(string id);
        BaseRoom lobby { get; set; }
        bool RemoveRoom(string id);
        BaseRoom CreateRoom();
    }
}
