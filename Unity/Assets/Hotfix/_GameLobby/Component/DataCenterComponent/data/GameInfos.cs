////////////////////////////////////////////////////////
///游戏信息
///zhouyu 2019.3.12
///////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
    public class GameInfos
    {
        /// <summary>
        /// 聊天信息
        /// </summary>
        public string[] Chat_Msg = new string[] 
        {
            "快点吧，我等的花儿谢了",

            "怎么又掉线啦，网络怎么这么差呀",

            "不要走，决战到天亮",

            "你的牌打得太好了",

            "和你合作真是太愉快了",

            "大家好，很高兴见到各位",

            "各位，真是不好意思，我得离开一会儿",

            "不要吵了，不要吵了，专心玩游戏嘛",

            "你是MM还是GG呀"
        };

        /// <summary>
        /// 聊天类型
        /// </summary>
        enum CHAT_TYPE
        {
            Common = 0, //常用

            Face = 1,   //表情

            Text = 2,   //自定义文本

            Voice = 3   //语音
        }

        /// <summary>
        /// 动作表情
        /// </summary>
        enum ACT_FACE
        {
            Boom = 1,

            FanQie,

            Stone,

            Zan,

            Kiss,

            Flower
        }

        /// <summary>
        /// 前端子游戏列表
        /// </summary>
        public Dictionary<int,SubGameInfo> SubGameInfoList = new Dictionary<int, SubGameInfo>
        {
            //[0] = new SubGameInfo { Index = 0, GameID = 2, PrefabName = "SubGame0", GameName = "百人牛牛", NeedLoad = true ,IsOpen = true},

            //[1] = new SubGameInfo { Index = 1, GameID = 6, PrefabName = "SubGame1", GameName = "红黑大战", NeedLoad = true,  IsOpen = true },

            //[2] = new SubGameInfo { Index = 2, GameID = 5, PrefabName = "SubGame2", GameName = "龙虎斗",   NeedLoad = true,  IsOpen = true },

            //[3] = new SubGameInfo { Index = 3, GameID = 3, PrefabName = "SubGame3", GameName = "百家乐",   NeedLoad = true,  IsOpen = true },

            //[4] = new SubGameInfo { Index = 4, GameID = 4, PrefabName = "SubGame4", GameName = "德州扑克", NeedLoad = true,  IsOpen = false },

            //[5] = new SubGameInfo { Index = 5, GameID = 9, PrefabName = "SubGame5", GameName = "抢庄牛牛", NeedLoad = true,  IsOpen = true },

            //[6] = new SubGameInfo { Index = 6, GameID = 1, PrefabName = "SubGame6", GameName = "炸金花",   NeedLoad = true,  IsOpen = true },

            //[7] = new SubGameInfo { Index = 7, GameID = 8, PrefabName = "SubGame7", GameName = "水果机",   NeedLoad = true,  IsOpen = true },

            [0]= new SubGameInfo  { Index = 0, GameID = 1, PrefabName = "SubGame0", GameName = "斗地主",   NeedLoad = true,  IsOpen = true }
        };

        /// <summary>
        /// 子游戏重回时等待重联时间：单位毫米
        /// </summary>
        public int ApplicationResumeWaitTime = 3000;
        
    }

    /// <summary>
    /// 子游戏信息
    /// </summary>
    public class SubGameInfo
    {
        public int Index = 0;

        public int GameID = 0;

        public string PrefabName = "";

        public string GameName = "";

        public bool NeedLoad = true;

        public bool IsOpen = true;
    }
}
