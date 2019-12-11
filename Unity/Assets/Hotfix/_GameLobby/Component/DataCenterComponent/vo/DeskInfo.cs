/////////////////////////////////////////////////
/// 桌子信息
/// zhouyu 2019.3.12
/////////////////////////////////////////////////

namespace ETHotfix
{
   public partial class DeskInfo
    {
        /// <summary>
        /// 桌子ID
        /// </summary>
        public int deskID = 0;

        /// <summary>
        /// 桌子拥有者ID
        /// </summary>
        public int ownerID = 0;

        /// <summary>
        /// 房间号
        /// </summary>
        public int deskCode = 0;

        /// <summary>
        /// 游戏ID
        /// </summary>
        public Game_ID gameID = Game_ID.GoldRoom;

        /// <summary>
        /// 游戏底分
        /// </summary>
        public int basePoint = 0;

        /// <summary>
        /// 桌子等级
        /// </summary>
        public int deskLevel = 0;

        /// <summary>
        /// 底注
        /// </summary>
        public int chip = 0;

        /// <summary>
        /// 当前玩家数量
        /// </summary>
        public int curPlayCount = 0;

        /// <summary>
        /// 当前坐下玩家数量
        /// </summary>
        public int curSitPeopleCount = 0;

        /// <summary>
        /// 桌子描述
        /// </summary>
        public string deskDesc = "";

        /// <summary>
        /// 桌子名称
        /// </summary>
        public string deskName = "";

        /// <summary>
        /// 是否游戏进行中
        /// </summary>
        public bool isGameRunning = false;

        public int deskNo = 0;

    }

    /// <summary>
    /// 房间类型
    /// </summary>
    public enum Game_ID
    {
        CardRoom = 0,//房卡

        GoldRoom = 1,//金币

        selfRoom = 2 //专属
    }
}
