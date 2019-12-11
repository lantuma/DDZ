using System;
using System.Collections.Generic;

namespace ETModel
{
    public class MyLastJionRoom : Component, ISerializeToEntity
    {
        public List<MyLastJionRoomData> MyLastJionRoomList = new List<MyLastJionRoomData>();
    }

    public class MyLastJionRoomData: ISerializeToEntity
    {
        public int GameId { get; set; }

        public int AreaId { get; set; }

        public int RoomId { get; set; }
    }
}