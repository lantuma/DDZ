using System;
using System.Collections.Generic;

namespace ETModel
{
    public class MyMailboxInfo : Component, ISerializeToEntity
    {
        public List<MyMailboxData> MyMail = new List<MyMailboxData>();
    }

    public class MyMailboxData : ISerializeToEntity
    {
        public string Title { get; set; }
      
        public DateTime SendTime { get; set; }
       
        public string Content { get; set; }
    }
}