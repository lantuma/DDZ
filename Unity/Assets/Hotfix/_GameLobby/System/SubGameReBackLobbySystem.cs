/******************************************************************************************
*         【模块】{ 大厅模块 }                                                                                                                      
*         【功能】{ 子游戏返回大厅 }                                                                                                                   
*         【修改日期】{ 2019年6月24日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [Event(EventIdType.SubGameReBackLobby)]
    public class SubGameReBackLobbySystem:AEvent
    {
        public override void Run()
        {
            //统一清理玩家信息
            DataCenterComponent.Instance.userInfo.deleteAllUserExcptMe();

            UI ui = GameLobbyFactory.Create();

            Game.Scene.GetComponent<UIComponent>().Add(ui);

            var cpt = ui.GetComponent<GameLobbyCpt>();

            //if (cpt != null) cpt.OnStartGameButton();
        }
    }
}
