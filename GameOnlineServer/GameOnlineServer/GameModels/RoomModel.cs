using GameOnlineServer.GameModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.GameModels
{
    public class RoomModel : BaseModel
    {
        public string roomName { get; set; }
        public RoomModel(string roomName) {
            this.roomName = roomName;
        }
    }
}
