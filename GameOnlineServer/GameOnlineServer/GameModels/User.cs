using GameOnlineServer.Application.Handlers;
using GameOnlineServer.GameModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.GameModels
{
    public class User:BaseModel
    {
        public string username {  get; set; }
        public string password { get; set; }
        public string displayName { get; set; }
        public string avatar { get; set; }
        public int level { get; set; }
        public long amount { get; set; }
        public User(string username, string password, string displayName)
        {
            this.username = username;
            this.password = GameHelper.HashPassword(password);
            this.displayName = displayName;
            avatar = "";
            level = 1;
            amount = 0;
        }
    }
}
