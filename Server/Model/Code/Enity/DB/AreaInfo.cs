namespace ETModel
{
    public class AreaInfo : Entity
    {
        public int GameId { get; set; }
        
        public int AreaId { get; set; }
        
        public string AreaType { get; set; }
        
        public int Score { get; set; }
        
        public int Threshold { get; set; }
    }
}
