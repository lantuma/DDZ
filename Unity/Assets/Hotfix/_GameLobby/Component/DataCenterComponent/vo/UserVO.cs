/////////////////////////////////////////////////
/// 用户数据VO
/// zhouyu 2019.3.12
/////////////////////////////////////////////////

namespace ETHotfix
{
    public class UserVO
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long userID = 0;

        /// <summary>
        /// 昵称
        /// </summary>
        public string nickName = "";

        /// <summary>
        /// 积分
        /// </summary>
        public int point = 0;

        /// <summary>
        /// 用户IP
        /// </summary>
        public string IP = "";

        /// <summary>
        /// 玩家座位号 -1 通常表示未入座 0庄家位，123456玩家位置
        /// </summary>
        public int seatID = -1;//-1 通常表示未入座 0庄家位，123456玩家位置

        /// <summary>
        /// 玩家性别
        /// </summary>
        public int sex = 0;//更改:0 未知 1.男 2.女

        /// <summary>
        /// 用户头像地址
        /// </summary>
        public string headUrl = "";

        /// <summary>
        /// 用户头像Id
        /// </summary>
        public int headId = 0;

        /// <summary>
        /// 验证用户有效性
        /// </summary>
        public string skey = "";

        /// <summary>
        /// 是否是VIP
        /// </summary>
        public int is_vip = 0;//0,表示普通玩家

        /// <summary>
        /// 金币
        /// </summary>
        public float gold = 0;

        /// <summary>
        /// 玩家是否在线
        /// </summary>
        public bool IsOnline = true;

        /// <summary>
        /// 分数
        /// </summary>
        public int score = 0;

        /// <summary>
        /// 是否是地主 
        /// </summary>
        public int IsLord = 0;

        /// <summary>
        /// 是否准备 0未准备 1：准备
        /// </summary>
        public int IsReady = 0;
    }

    /// <summary>
    /// 性别类型
    /// </summary>
    public enum SEX_TYPE
    {
        unknow = 0,//未知

        boy = 1,//男

        girl = 2//女
    }
}
