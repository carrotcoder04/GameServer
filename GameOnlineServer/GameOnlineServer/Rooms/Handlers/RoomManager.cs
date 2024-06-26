﻿using GameOnlineServer.Rooms.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameOnlineServer.Application.Actions.Actions;
namespace GameOnlineServer.Rooms.Handlers
{
    public class RoomManager : IRoomManager
    {
        public BaseRoom lobby { get; set; }
        private ConcurrentDictionary<string,BaseRoom> rooms {  get; set; }
        public RoomManager()
        {
            RequestRemoveRoom += RemoveRoom;
            lobby = new BaseRoom();
            rooms = new ConcurrentDictionary<string,BaseRoom>();
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
                Console.WriteLine($"{id} :{room!=null}");
                return room != null;
            }
            return false;
        }
        public BaseRoom CreateRoom()
        {
            var newRoom = new BaseRoom();
            rooms.TryAdd(newRoom.idroom, newRoom);
            return newRoom;
        }
    }
}
