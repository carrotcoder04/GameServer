using GameOnlineServer.Application.Interfaces;
using GameOnlineServer.Application.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Application.Handlers
{
    public class PlayerManager : IPlayerManager
    {
        public ConcurrentDictionary<string, IPlayer> Players { get;set; }
        private readonly IGameLogger logger;
        public PlayerManager(IGameLogger logger) { 
            Players = new ConcurrentDictionary<string, IPlayer>();
            this.logger = logger;
        }

        public void AddPlayer(IPlayer player)
        {
            if(FindPlayer(player) == null)
            {
                Players.TryAdd(player.sessionId, player);
                logger.Info($"List player: {Players.Count}");
            }
        }

        public IPlayer FindPlayer(string id)
        {
            return Players.FirstOrDefault(p => p.Key.Equals(id)).Value;
        }

        public IPlayer FindPlayer(IPlayer player)
        {
            return Players.FirstOrDefault(p => p.Value.Equals(player)).Value;
        }

        public List<IPlayer> GetPlayers()
        {
            return Players.Values.ToList();
        }

        public void RemovePlayer(string id)
        {
            if(this.FindPlayer(id) != null)
            {
                Players.TryRemove(id, out var player);
                logger.Info($"Remove {id} success");
                logger.Info($"List player: {Players.Count}");
            }
        }

        public void RemovePlayers(IPlayer player)
        {
            this.RemovePlayer(player.sessionId);
        }
    }
}
