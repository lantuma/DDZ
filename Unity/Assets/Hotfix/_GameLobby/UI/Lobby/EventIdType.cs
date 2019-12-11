namespace ETHotfix
{
    public static partial class EventIdType
    {
        public const string CreateGameLobby = "CreateGameLobby";

        // 移除游戏大厅
        public const string RemoveGameLobby = "RemoveGameLobby";

        //心跳超时
        public const string HeartBeatTimeOut = "HeartBeatTimeOut";

        //玩家被顶号
        public const string SamePlayerLogin = "SamePlayerLogin";

        //长时间未操作
        public const string NoPlayForLongTime = "NoPlayForLongTime ";

        //程序从后台切回
        public const string OnApplicationResume = "OnApplicationResume";

        //检测游戏重回
        public const string CheckGameReBack = "CheckGameReBack";

        //点击返回
        public const string ClickBackGameList = "ClickBackGameList";

        //处理各子游戏重回
        public const string HandleSubGameReBack = "HandleSubGameReBack";

        //子游戏重新回到大厅
        public const string SubGameReBackLobby = "SubGameReBackLobby";

        //游戏大版本检测
        public const string CheckLargeVersion = "CheckLargeVersion";

        //异步图片下载
        public const string AsyncImageDownload = "AsyncImageDownload";

        //模块开启/关闭
        public const string ModuleEnable = "ModuleEnable";
    }
}