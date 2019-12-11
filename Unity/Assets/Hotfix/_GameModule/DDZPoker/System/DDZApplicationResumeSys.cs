/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 事件处理-ApplicationResum }                                                                                                                   
*         【修改日期】{ 2019年5月22日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.OnApplicationResume)]
    public class DDZApplicationResumeSys : AEvent
    {
        public override void Run()
        {
            var uiC = Game.Scene.GetComponent<UIComponent>();

            //检测是否已经断线
            if (uiC.Get(UIType.UIMessageBoxPanel) != null)
            {
                Log.Info("存在断线重联界面，不走Resume逻辑!!");

                return;
            }

            //检测当前是否是龙虎斗
            if (uiC.Get(UIType.UIDDZGameScene) != null)
            {
                Log.Info("子游戏:斗地主Resume事件!!");

                var gameScene = uiC.Get(UIType.UIDDZGameScene).GetComponent<UIDDZGameScene>();

                gameScene.OnApplicationResume();
            }
        }
    }
}
