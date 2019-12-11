using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public class MyRecord : Component, ISerializeToEntity
    {
      public List<Record> recordsList = new List<Record>();
    }

     public class Record: ISerializeToEntity
    {
        public int GameId { get; set; }

        public  DateTime JionTime { get; set; }

        public DateTime QuitTime { get; set; }
        
        public float Income { get; set; }
    }
}
