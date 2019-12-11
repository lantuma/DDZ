/******************************************************************************************
*         【模块】{ 大厅模块 }                                                                                                                      
*         【功能】{ 大厅-心跳超时处理 }                                                                                                                   
*         【修改日期】{ 2019年5月15日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.HeartBeatTimeOut)]
    public class HeartBeatTimeOutHandler : AEvent
    {
        public override void Run()
        {
            string mes = DataCenterComponent.Instance.tipInfo.NetworkErrorTip;

            MessageData messData = new MessageData();
            messData.ok = OkAction;
            messData.cancel = OkAction;
            messData.titleStr = DataCenterComponent.Instance.tipInfo.NetworkExceptionTip;
            messData.mes = mes;
            messData.okStr = DataCenterComponent.Instance.tipInfo.OkStrTip;
            Game.PopupComponent.ShowMessageBox(mes, messData);
        }

        /// <summary>
        /// 确认回调
        /// </summary>
        public void OkAction()
        {
            var uc = Game.Scene.GetComponent<UIComponent>();

            foreach (var item in uc.uis.ToList())
            {
                if (item.Key != UIType.UIMessageBoxPanel)
                {
                    uc.Remove(item.Key);
                }
            }
            //清除玩家数据
            DataCenterComponent.Instance.userInfo.deleteAllUserExcptMe();

            //移除掉心跳组件
            Game.Scene.RemoveComponent<PingComponent>();

            //清理客户端Session
            Game.Scene.GetComponent<SessionComponent>()?.Session.Dispose();
            
            //切换到登陆界面
            Game.EventSystem.Run(EventIdType.InitPokerSceneStart);

        }

    }
}