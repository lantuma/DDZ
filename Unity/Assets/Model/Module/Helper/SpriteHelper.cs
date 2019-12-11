/******************************************************************************************
*         【模块】{ 基础模块 }                                                                                                                      
*         【功能】{ 精灵帮助类 }                                                                                                                   
*         【修改日期】{ 2019年4月8日 }                                                                                                                        
*         【贡献者】{ 周瑜(整合) }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using UnityEngine;

namespace ETModel
{
    public static class SpriteHelper
    {
        /// <summary>
        /// 扑克图集预设
        /// </summary>
        public const string POKER_ATLAS_NAME = "poker";

        /// <summary>
        /// 头像图集预设
        /// </summary>
        public const string HEAD_ATLAS_NAME = "userhead";

        /// <summary>
        /// 筹码图集预设
        /// </summary>
        public const string CHIPS_ATLAS_NAME = "chips";

        /// <summary>
        /// 新头像图集预设
        /// </summary>
        public const string PLAYERHEAD_ATLAS_NAME = "playerhead";

        /// <summary>
        /// 百家乐图集
        /// </summary>
        public static string BJL_ATLAS_NAME = "bjlgame";

        /// <summary>
        /// 龙虎斗图集
        /// </summary>
        public static string NHD_ATLAS_NAME = "nhdgame";

        /// <summary>
        /// 斗地主图集
        /// </summary>
        public static string DDZ_ATLAS_NAME = "ddzgame";

        //小王
        public static string BlackJoker = "poker_BlackJoker";

        //大王
        public static string RedJoker = "poker_RedJoker";


        /// <summary>
        /// 从图集获取精灵
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="spriteName"></param>
        /// <returns></returns>
        public static Sprite GetSprite(string abName, string spriteName)
        {
            var res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();

            res.LoadBundle(abName.StringToAB());

            if (Define.UseEditorResouces)
            {
                var texture = (Texture2D)res.GetAsset(abName.StringToAB(), spriteName);

                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                return (Sprite)res.GetAsset(abName.StringToAB(), spriteName);
            }
        }

        /// <summary>
        /// 获取扑克牌
        /// </summary>
        /// <param name="poker"></param>
        /// <param name="isPokerBg"></param>
        /// <returns></returns>
        public static Sprite GetPokerSprite(int poker, bool isPokerBg = false)
        {
            if (isPokerBg)
            {
                return GetSprite(POKER_ATLAS_NAME, "poker_paiBG@2x");
            }

            if (poker == 53)
            {
                return GetSprite(POKER_ATLAS_NAME, BlackJoker);
            }

            if (poker == 54)
            {
                return GetSprite(POKER_ATLAS_NAME, RedJoker);
            }
            
            int hua = poker / 13;

            int pokerValue = poker % 13;

            string huaMsg = "";

            switch (hua)
            {
                case 3:
                    huaMsg = "f";
                    break;
                case 1:
                    huaMsg = "t";
                    break;
                case 2:
                    huaMsg = "w";
                    break;
                case 0:
                    huaMsg = "h";
                    break;
            }

            return GetSprite(POKER_ATLAS_NAME, string.Format("poker_{0}{1}", huaMsg, pokerValue + 1));
        }

        /// <summary>
        /// 获取筹码精灵
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Sprite GetChipsSprite(string msg)
        {
            return GetSprite(CHIPS_ATLAS_NAME, msg);
        }
        
        /// <summary>
        /// 获取新玩家头像精灵
        /// </summary>
        /// <param name="headId"></param>
        /// <returns></returns>
        public static Sprite GetPlayerHeadSpriteName(int headId)
        {
            return GetSprite(PLAYERHEAD_ATLAS_NAME, string.Format("head_{0}", headId));
        }
    }
}
