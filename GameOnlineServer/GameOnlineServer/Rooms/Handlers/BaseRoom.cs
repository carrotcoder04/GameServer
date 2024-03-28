using GameOnlineServer.Application.Handlers;
using GameOnlineServer.Application.Interfaces;
using GameOnlineServer.Application.Messaging.MessageBinary;
using GameOnlineServer.Rooms.Interfaces;
using System.Collections.Concurrent;
namespace GameOnlineServer.Rooms.Handlers
{
    public class BaseRoom : IBaseRoom
    {
        public string id { get; set; }
        public ConcurrentDictionary<string, IPlayer> players { get; set; }
        public BaseRoom() {
            id = GameHelper.RandomString(4);
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
                    this.SendByte((byte)Room.ROOM_NAME, id);
                    return true;
                }
            }
            return false;
        }
        public bool JoinLobby(IPlayer player)
        {
            if (FindPlayer(player.sessionId) == null)
            {
                if (players.TryAdd(player.sessionId, player))
                {
                    return true;
                }
            }
            return false;
        }
        public void SendByte(byte tag,string message)
        {
            lock(players)
            {
                foreach(var player in players.Values)
                {
                    player.SendByte(tag,message);
                }
            }
        }
        public void SendByte(byte tag)
        {
            lock (players)
            {
                foreach (var player in players.Values)
                {
                    player.SendByte(tag);
                }
            }
        }
        public void SendByte(byte tag,ISerializable data)
        {
            lock (players)
            {
                foreach (var player in players.Values)
                {
                    player.SendByte(tag,data);
                }
            }
        }
        private void RoomInfo()
        {
            var player = players.Values.Select(p => p.GetPlayerInfo()).ToList();
            RoomMessage roomMessage = new RoomMessage(player);
            this.SendByte((byte)Room.ROOM_INFO,roomMessage);
        }
    }
}
