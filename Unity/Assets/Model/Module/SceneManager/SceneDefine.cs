using System.Collections.Generic;

namespace ETModel
{
    public class SceneDefine
    {
        /// <summary>
        /// 场景配置
        /// </summary>
        public static Dictionary<int, SceneConfig> SceneConfig = new Dictionary<int, SceneConfig>
        {
            [0] = {SceneType = 0,  SceneId = 0, SceneName = "LoginScene",SceneFolder = "/",SceneMusic = "LoginScene" },

            [1] = { SceneType = 1, SceneId = 1, SceneName = "HallScene", SceneFolder = "/", SceneMusic = "HallScene" },

            [2] = { SceneType = 2, SceneId = 2, SceneName = "BattleScene", SceneFolder = "/", SceneMusic = "BattleScene" },
        };

        /// <summary>
        /// 场景类型
        /// </summary>
        public enum SceneType
        {
            LOGIN = 0,

            HALL = 1,

            BATTLE = 2
        }

        /// <summary>
        /// 加载状态
        /// </summary>
        public enum LoadState
        {
            None = 0,

            LOADING = 1,

            LOADED = 2
        }
    }
}
