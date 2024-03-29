using GameOnlineServer.Application.Interfaces;
using GameOnlineServer.Application.Messaging;
using GameOnlineServer.Application.Messaging.MessageBinary;
using GameOnlineServer.Rooms.Handlers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Rooms.Interfaces
{
    public interface IBaseRoom
    {
        public string idroom { get; set; }
        public ConcurrentDictionary<string,IPlayer> players { get; set; }
        bool JoinRoom(IPlayer player);
        bool ExitRoom(IPlayer player);
        bool ExitRoom(string id);
        bool ExitLobby(string id);
        bool ExitLobby(IPlayer player);
        void PlayerLeaveRoom(IPlayer player);
        void NewPlayerJoinRoom(IPlayer player);
        void RoomInfo(IPlayer _player);
        IPlayer FindPlayer(string id);
        void SendByte(byte tag,string message);
        void SendByte(byte tag);
        void SendByte(byte tag, byte[] data);
        void SendByte(byte tag, ISerializable data);
        void SendByte(byte tag, string message, IPlayer _player);
        void SendByte(byte tag, ISerializable data,IPlayer player);
    }
}
