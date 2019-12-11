/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 事件处理-子游戏重回 }                                                                                                                   
*         【修改日期】{ 2019年6月24日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [Event(EventIdType.HandleSubGameReBack)]
    public class DDZHandleReBackSys : AEvent<int, int, GameReBackData>
    {
        public override void Run(int GameId, int RoomId, GameReBackData backData)
        {
            if (GameId == 7)
            {
                DDZGameHelper.CurrentGameInfo = GameHelper.GetGameInfo(GameType.DouDiZhu);

                GameLobbyFactory.Remove();

                DDZUIFactory.Init();

                DDZGameHelper.RoomId = RoomId;

                DDZGameHelper.CurrentFieldId = backData.index;

                //清除重回数据
                backData = null;

                DataCenterComponent.Instance.GameReBackInfo.backData = null;

                DataCenterComponent.Instance.GameReBackInfo.isBackSuccess = false;

                PlayerPrefs.DeleteKey(DataCenterComponent.Instance.GameReBackInfo.reBackDataKey);

                DDZUIFactory.gameScene.Create();
            }
        }
    }
}
