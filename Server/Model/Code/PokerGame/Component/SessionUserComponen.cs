
using System.Collections.Generic;

namespace ETModel
{
    public class SessionUserComponent : Component
    {
        public long sessionId { get; set; }

        public UserInfo userInfo { get; set; }
    }
    
    public class MyRoomData
    {
        public int GameId { get; set; }

        public int AreaId { get; set; }

        public int RoomId { get; set; }
    }
    
}
