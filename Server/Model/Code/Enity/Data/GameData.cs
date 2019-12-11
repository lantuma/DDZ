using System;
using System.Collections.Generic;

namespace ETModel
{
    #region
  
    public class Rule : Entity
    {
        public int GameId { get; set; }              

        public int RoomId { get; set; }                

        public float bestPercentage { get; set; }   

        public float RewardRate { get; set; }       

        public int DiFen { get; set; }                   

        public int Threshold { get; set; }            

        public int MaxCount { get; set; }             
    }

    public class DDZPlayerData : Entity
    {
        public long userId { get; set; }        

        public int Chairld { get; set; }          

        public UserInfo userInfo { get; set; }  
        
        public bool IsLandlord { get; set; }       
        
        public float score { get; set; }            
        
        public float JionGold { get; set; }       

        public bool IsDeposit { get; set; }     

        public DateTime StartTime { get; set; } 
        
        public bool IshaveOrder { get; set; }     
        
        public bool Isprepare { get; set; }     

        public int qdzSocre = -1;     

        public bool IsTimeOut { get; set; }    
        
        public bool IsRobat { get; set; }
    }

    #endregion
}
