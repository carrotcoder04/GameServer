using GameOnlineServer.Application.Handlers;
using GameOnlineServer.GameModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.GameModels
{
    public class User : BaseModel
    {
        public string username {  get; set; }
        public string password { get; set; }
        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
