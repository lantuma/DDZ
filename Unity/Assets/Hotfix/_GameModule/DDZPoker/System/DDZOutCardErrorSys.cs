/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 事件处理-出牌错误 }                                                                                                                   
*         【修改日期】{ 2019年6月6日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.DDZOutCardError)]
    public class DDZOutCardErrorSys:AEvent<string>
    {
        public override void Run(string message)
        {
            if (!DDZGameHelper.IsJoinRoom || GameHelper.ApplicationIsPause) { return; }

            if (message == "这轮必须先出牌")
            {
                Game.PopupComponent.ShowTips("这轮您必须先出牌");
            }
            else
            {
                DDZConfig.GameScene.DDZMaskPlugin.Show();
            }

            DDZConfig.GameScene.DDZHandCardPlugin.ReSelect();
        }
    }
}
