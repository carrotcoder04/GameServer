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
        public static string RandomString(int length) {
            var rand = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid() + $"{DateTime.Now}"));
            return rand[..length];
        }
    }
}
