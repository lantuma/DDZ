/******************************************************************************************
*         【模块】{ 轻量级本地化组件 }                                                                                                                      
*         【功能】{ 多语言切换 }                                                                                                                   
*         【修改日期】{7月31日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class LocalizationComponentAwakeSystem : AwakeSystem<LocalizationComponent>
    {
        public override void Awake(LocalizationComponent self)
        {
            self.Awake();
        }
    }

    public class LocalizationComponent:Component
    {
        private LocalizationManager LocalizationManager;

        public static LocalizationComponent instance;

        //支持语言对应字典

        private Dictionary<string, string> language_en_Dic;
        
        private Dictionary<string, string> language_cn_Dic;
        
        //支持的语言类型

        public const string chinese = "Chinese";

        public const string english = "English";

        public void Awake()
        {
            //Log.Debug("初始本地化组件成功");

            LocalizationManager = GameObject.Find("OpenInstall").AddComponent<LocalizationManager>();

            language_en_Dic = new Dictionary<string, string>();

            language_cn_Dic = new Dictionary<string, string>();

            this.LoadLanguageConfig();

            instance = this;
        }

        /// <summary>
        /// 加载语言配置文件
        /// </summary>
        public void LoadLanguageConfig()
        {
            Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");

            this.language_cn_Dic = this.ConverToDic(ConfigHelper.GetText(chinese));

            this.language_en_Dic = this.ConverToDic(ConfigHelper.GetText(english));

            Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");

            LocalizationManager.SetLanguageDic(this.language_en_Dic, this.language_cn_Dic);
        }

        /// <summary>
        /// 配置转换为字典
        /// </summary>
        /// <param name="languageStr"></param>
        /// <returns></returns>
        private Dictionary<string, string> ConverToDic(string languageStr)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string[] lines = languageStr.Split('\n');

            foreach (string line in lines)
            {
                if (line == null)
                {
                    continue;
                }

                string[] keyAndValue = line.Split('=');

                dic.Add(keyAndValue[0], keyAndValue[1]);
            }

            return dic;
        }

        /// <summary>
        /// 切换当前语言
        /// </summary>
        /// <param name="type"></param>
        public void SetupLanguage(GameLanguageType type)
        {
            LocalizationManager.SetupLanguage(type);
        }

        /// <summary>
        /// 获取对应语言字符
        /// </summary>
        /// <param name="key"></param>
        /// <param name="languageType"></param>
        /// <returns></returns>
        public string GetValue(string key, GameLanguageType languageType= GameLanguageType.Chinese)
        {
            return LocalizationManager.GetValue(key, languageType);
        }
    }

    /// <summary>
    /// 游戏语言
    /// </summary>
    public enum GameLanguageType
    {
        /// <summary>
        /// 中文
        /// </summary>
        Chinese,

        /// <summary>
        /// 英文
        /// </summary>
        English
    }
}
