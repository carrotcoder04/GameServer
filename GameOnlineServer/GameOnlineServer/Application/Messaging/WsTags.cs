using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Application.Messaging
{
    public enum WsTags
    {
        Invalid,
        Login,
        Register,
        UserInfo,
        RoomInfo
    }
}
