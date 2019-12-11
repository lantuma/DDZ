/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 斗地主组件工厂 }                                                                                                                   
*         【修改日期】{ 2019年4月3日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class DDZUIFactory
    {

        private static string ATLAS_NAME = "ddzgame.unity3d";
        
        /// <summary>
        /// 玩家工厂
        /// </summary>
        public static DDZGamerFactory gamer;

        /// <summary>
        /// 游戏Sceene工厂
        /// </summary>
        public static DDZGameSceneFactory gameScene;

        /// <summary>
        /// 帮助面板
        /// </summary>
        public static DDZHelpPanelFactory helpPanel;

        /// <summary>
        /// 结算面板工厂
        /// </summary>
        public static DDZOverPanelFactory overPanel;
        
        /// <summary>
        /// 扑克ITEM 工厂
        /// </summary>
        public static DDZPokerItemFactory pokerItem;

        /// <summary>
        /// 选场工厂
        /// </summary>
        public static DDZFieldSceneFactory fieldScene;
        
        public static void Init()
        {
            gamer = new DDZGamerFactory();

            gameScene = new DDZGameSceneFactory();

            helpPanel = new DDZHelpPanelFactory();

            overPanel = new DDZOverPanelFactory();
            
            pokerItem = new DDZPokerItemFactory();

            fieldScene = new DDZFieldSceneFactory();


        }

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static UI Create(string type)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle(ATLAS_NAME);
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(ATLAS_NAME, type);
                GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
                
                UI ui = ComponentFactory.Create<UI, string, GameObject>(type, gameObject, false);

                Game.Scene.GetComponent<UIComponent>().Add(ui);

                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        /// <summary>
        /// 移除UI
        /// </summary>
        /// <param name="type"></param>
        public static void Remove(string type)
        {
            Game.Scene.GetComponent<UIComponent>().Remove(type);
        }

        public static void Clear()
        {
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(ATLAS_NAME);
        }
    }
}
