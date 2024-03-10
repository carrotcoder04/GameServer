using GameOnlineServer.Application.Handlers;
using GameOnlineServer.Application.Interfaces;
using GameOnlineServer.Application.Messaging;
using GameOnlineServer.Application.Messaging.Constants;
using GameOnlineServer.Rooms.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Rooms.Handlers
{
    public class BaseRoom : IBaseRoom
    {
        public string id { get; set; }
        public ConcurrentDictionary<string, IPlayer> players { get; set; }
        public BaseRoom() {
            id = GameHelper.RandomString(10);
            players = new ConcurrentDictionary<string, IPlayer>();
        }
        public bool ExitRoom(IPlayer player)
        {
            return this.ExitRoom(player.sessionId);
        }

        public bool ExitRoom(string id)
        {
            var player = FindPlayer(id);
            if(player!=null)
            {
                players.TryRemove(player.sessionId, out player);
                this.RoomInfo();
                return true;
            }
            return false;
        }

        public IPlayer FindPlayer(string id)
        {
            return players.FirstOrDefault(p => p.Key.Equals(id)).Value;
        }

        public bool JoinRoom(IPlayer player)
        {
            if (FindPlayer(player.sessionId) == null)
            {
                if (players.TryAdd(player.sessionId, player))
                {
                    this.RoomInfo();
                    return true;
                }
            }
            return false;
        }

        public void SendMessage(string message)
        {
            lock(players)
            {
                foreach(var player in players.Values)
                {
                    player.SendMessage(message);
                }
            }
        }

        public void SendMessage<T>(WsMessage<T> message)
        {
            lock(players)
            {
                foreach (var player in players.Values)
                {
                    player.SendMessage(message);
                }
            }
        }
        private void RoomInfo()
        {
            var lobby = new LobbyInfo
            {
                players = players.Values.Select(p => p.GetUserInfo()).ToList()
            };
            var mess = new WsMessage<LobbyInfo>(WsTags.RoomInfo, lobby);
            this.SendMessage(mess);
        }
        public void SendMessage<T>(WsMessage<T> message, string idIgnore)
        {
            lock(players)
            {
                foreach (var player in players.Values.Where(p => p.sessionId != idIgnore))
                {
                    player.SendMessage(message);
                }
            }
        }
    }
}
