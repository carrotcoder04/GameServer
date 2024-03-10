using GameOnlineServer.Rooms.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Rooms.Handlers
{
    internal class RoomManager : IRoomManager
    {
        public BaseRoom lobby { get; set; }
        private ConcurrentDictionary<string,BaseRoom> rooms {  get; set; }
        public RoomManager() 
        { 
            rooms = new ConcurrentDictionary<string,BaseRoom>();
            lobby = new BaseRoom();
        }
        public BaseRoom FindRoom(string id)
        {
            return rooms.FirstOrDefault(r => r.Key.Equals(id)).Value;
        }
        public bool RemoveRoom(string id)
        {
            var oldRoom = FindRoom(id);
            if (oldRoom != null)
            {
                rooms.TryRemove(id, out var room);
                return room != null;
            }
            return false;
        }
        public BaseRoom CreateRoom()
        {
            var newRoom = new BaseRoom();
            rooms.TryAdd(newRoom.id, newRoom);
            return newRoom;
        }
    }
}
