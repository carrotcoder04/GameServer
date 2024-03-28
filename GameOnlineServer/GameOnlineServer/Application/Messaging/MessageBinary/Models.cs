using MongoDB.Bson.Serialization.Serializers;
using System.Text;
using static GameOnlineServer.Application.Messaging.MessageBinary.BinaryConvert;
namespace GameOnlineServer.Application.Messaging.MessageBinary
{
    //Enum
    public enum Log : byte
    {
        USERNAME_OR_PASSWORD_INVALID = 0,
        LOGIN = 1, 
        REGISTER = 2,
        LOGOUT = 3,
        JOIN_LOBBY = 4,
        LOGIN_SUCCESS = 5,
        REGISTER_SUCCESS = 6,
        USERNAME_EXISTED = 7,
        JOIN_LOBBY_FAIL = 8,
        JOIN_LOBBY_SUCCESS = 9
    }
    public enum Lobby : byte
    {
        ROOM_NOT_FOUND = 0,
        JOIN_ROOM = 1,
        CREATE_ROOM = 2,
        JOIN_ROOM_SUCCESS = 3,
        ROOM_FULL = 4
    }
    public enum Room : byte
    {
        ERROR = 0,
        ROOM_INFO = 1,
        ROOM_CHAT = 2,
        ROOM_NAME = 3,
        PLAY = 4
    }
    public enum Game : byte
    {
        GAME_INFO = 0
    }
    //Interface
    public interface ISerializable
    {
        byte[] ConvertByte();
        void ConvertClass(byte[] data);
    }
    //Class Message
    public class UserMessage : ISerializable
    {
        public string username {  get; set; }
        public string password { get; set; }
        public UserMessage(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
        public UserMessage(byte[] bytes)
        {
            ConvertClass(bytes);
        }

        public byte[] ConvertByte()
        {
            byte[] username = Encoding.UTF8.GetBytes(this.username);
            byte[] password = Encoding.UTF8.GetBytes(this.password);
            byte userlength = (byte)username.Length;
            byte passlength = (byte)password.Length;
            username = AddFirst(255, username);
            username = Add(username, password);
            username = Append(username, userlength);
            username = Append(username, passlength);
            return username;
        }
        public void ConvertClass(byte[] data)
        {
            byte passlength = data[data.Length - 1];
            byte userlength = data[data.Length - 2];
            this.username = Encoding.UTF8.GetString(data,1,userlength);
            this.password = Encoding.UTF8.GetString(data, userlength + 1, passlength);
        }
    }
    public class LobbyMessage : ISerializable
    {
        public string idroom { get; set; }
        public LobbyMessage(string idroom)
        {
            this.idroom = idroom;
        }
        public LobbyMessage(byte[] bytes)
        {
            ConvertClass(bytes);
        }
        public byte[] ConvertByte()
        {
            byte[] idroom = Encoding.UTF8.GetBytes(this.idroom);
            idroom = AddFirst(255, idroom);
            return idroom;
        }

        public void ConvertClass(byte[] data)
        {
            this.idroom = Encoding.UTF8.GetString(data,1,data.Length - 1);
        }
    }
    public class RoomMessage : ISerializable
    {
        public List<string> namePlayer { get; set; }
        public RoomMessage(List<string> namePlayer)
        {
            this.namePlayer = namePlayer;
        }
        public RoomMessage(byte[] bytes)
        {
            namePlayer = new List<string>();
            ConvertClass(bytes);
        }
        public byte[] ConvertByte()
        {
            byte[] result = new byte[] { 255 };
            byte length = (byte) namePlayer.Count;
            byte[] len = new byte[length];
            for(byte i = 0;i < length;i++)
            {
                byte[] name = Encoding.UTF8.GetBytes(namePlayer[i]);
                len[i] = (byte)name.Length;
                result = Add(result,name);
            }
            for(byte i = 0;i < length;i++)
            {
                result = Append(result, len[i]);
            }
            result = Append(result, length);
            return result;
        }

        public void ConvertClass(byte[] data)
        {
            byte length = data[data.Length - 1];
            byte[] len = new byte[length];
            int indexStart = data.Length - length - 1;
            for (int i = indexStart;i < data.Length - 1;i++)
            {
                len[i - indexStart] = data[i];
            }
            byte index = 1;
            for(byte i = 0; i < length; i++)
            {
                namePlayer.Add(Encoding.UTF8.GetString(data, index, len[i]));
                index += len[i];
            }
        }
    }
}
