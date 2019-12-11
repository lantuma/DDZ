////////////////////////////////////////////////////////
///重回游戏信息
///zhouyu 2019.5.22
///////////////////////////////////////////////////////

namespace ETHotfix
{

    public class GameReBackInfo
    {
        /// <summary>
        /// 重回数据
        /// </summary>
        public GameReBackData backData = null;

        /// <summary>
        /// 是否重回游戏成功
        /// </summary>
        public bool isBackSuccess = false;

        /// <summary>
        /// 持久化存储的KEY
        /// </summary>
        public string reBackDataKey = "reBackData";
    }
}
