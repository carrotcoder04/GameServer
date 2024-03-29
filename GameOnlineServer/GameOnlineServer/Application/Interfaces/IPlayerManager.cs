using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Application.Interfaces
{
    public interface IPlayerManager
    {
        ConcurrentDictionary<string, IPlayer> Players { get;set; }
        void AddPlayer(IPlayer player);
        void RemovePlayer(string id);
        void RemovePlayers(IPlayer player);
        IPlayer FindPlayer(string id);
        IPlayer FindPlayer(IPlayer player);
        List<IPlayer> GetPlayers();
        IPlayer FindPlayerByUsername(string username);
    }
}
