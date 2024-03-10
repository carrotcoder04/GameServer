using GameDatabase.Mongodb.Handlers;
using GameDatabase.Mongodb.Interfaces;
using GameOnlineServer.GameModels;
using MemoryPack;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Application.Handlers
{
    public static class GameHelper
    {
        public static string ParseString<T> (T data)
        {
            return JsonConvert.SerializeObject (data);
        }
        public static string RandomString(int length) {
            var rand = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid() + $"{DateTime.Now}"));
            return rand[..length];
        }
        public static T ParseStruct<T> (string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
        public static string HashPassword(string txt)
        {
            var crypt = new SHA256Managed();
            var hash = string.Empty;
            var bytes = crypt.ComputeHash(Encoding.UTF8.GetBytes(txt));
            return bytes.Aggregate(hash,(current,theByte) => current + theByte.ToString("x2"));
        }
        public static byte[] ToByte<T>(T obj)
        {
            return MemoryPackSerializer.Serialize(obj);
        }
        public static T ToObject<T>(byte[] bytes)
        {
            return MemoryPackSerializer.Deserialize<T>(bytes);   
        }
    }
}
