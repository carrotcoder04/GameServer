using GameOnlineServer.Application.Messaging.MessageBinary;
using GameOnlineServer.Rooms.Handlers;

namespace GameOnlineServer.Application.Interfaces
{
    public interface IPlayer
    {
        public event Action<byte[]> PlayerMessage;
        public byte IdInRoom { get; set; }
        public string sessionId { get; set; }
        public string name { get; set; }
        public BaseRoom currentroom { get; set; }
        void SetDisconnected(bool value);
        void On_DisConnected();
        string GetPlayerInfo();
        void SendByte(byte tag,ISerializable data);
        void SendByte(byte tag,byte data);
        void SendByte(byte tag);
        void SendByte(byte tag, byte[] data);
        void SendByte(byte tag,string data);
        void ChangeRoom(BaseRoom room);
    }
}
