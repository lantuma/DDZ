using System;

namespace ETModel
{
    public class UserInfo : Entity
    {
        public string Account { get; set; }
        
        public int PlayerId { get; set; }
        
        public string Nickname { get; set; }
        
        public int HeadId { get; set; }
        
        public float Gold { get; set; } 
        
        public int Gender { get; set; }
        
        public float Scroe { get; set; }
        
        public bool IsOnline { get; set; }
        
        public DateTime RegistrTime { get; set; }
        
        public bool IsTourist { get; set; }
       
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

        }

    }
}
