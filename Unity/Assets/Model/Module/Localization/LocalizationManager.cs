using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager instance;
        
        private Dictionary<string, string> language_en_Dic = new Dictionary<string, string>();

        private Dictionary<string, string> language_cn_Dic = new Dictionary<string, string>();

        public static GameLanguageType languageType;

        public delegate void OnLanguageChanged(GameLanguageType type);

        public static OnLanguageChanged onLanguageChanged = null;

        public void Awake()
        {
            instance = this;
        }
        
        /// <summary>
        /// 从组件层传入数据
        /// </summary>
        /// <param name="language_en_Dic"></param>
        /// <param name="language_cn_Dic"></param>
        public void SetLanguageDic(Dictionary<string, string> language_en_Dic, Dictionary<string, string> language_cn_Dic)
        {
            this.language_en_Dic = language_en_Dic;

            this.language_cn_Dic = language_cn_Dic;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key, GameLanguageType languageType = GameLanguageType.Chinese)
        {
            if (languageType == GameLanguageType.English)
            {
                return language_en_Dic[key];
            }

            if (languageType == GameLanguageType.Chinese)
            {
                return language_cn_Dic[key];
            }

            return "N/A";
        }

        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="type"></param>
        public void SetupLanguage(GameLanguageType type)
        {
            if (onLanguageChanged != null)
            {
                onLanguageChanged(type);
            }
        }
        
    }
    
}
