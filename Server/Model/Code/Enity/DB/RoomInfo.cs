namespace ETModel
{
   public class RoomInfo: Entity
    {
        public int GameId { get; set; }                      

        public int AreaId { get; set; }                      

        public int RoomId { get; set; }                   

        public int DiFen { get; set; }                       

        public int Threshold { get; set; }                
        
        public int MaxCount { get; set; }                 
    }
}
