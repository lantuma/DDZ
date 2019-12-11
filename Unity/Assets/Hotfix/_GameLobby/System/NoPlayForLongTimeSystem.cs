/******************************************************************************************
*         【模块】{ 大厅模块 }                                                                                                                      
*         【功能】{ 大厅-长时间未操作被踢处理 }                                                                                                                   
*         【修改日期】{ 2019年5月15日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.NoPlayForLongTime)]
    public class NoPlayForLongTimeSystem : AEvent
    {
        public override void Run()
        {
            Log.Info("由于长时间未操作而离线");
            MessageData messData = new MessageData();
            messData.ok = OkAction;
            messData.cancel = OkAction;
            messData.titleStr = DataCenterComponent.Instance.tipInfo.Tips;
            messData.mes = DataCenterComponent.Instance.tipInfo.NetworkErrorTip;
            messData.okStr = DataCenterComponent.Instance.tipInfo.OkStrTip;
            Game.PopupComponent.ShowMessageBox(messData.mes, messData);
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
            //清理客户端Session
            Game.Scene.GetComponent<SessionComponent>().Session.Dispose();
            //切换到登陆界面
            Game.EventSystem.Run(EventIdType.InitPokerSceneStart);
        }

        /// <summary>
        /// 取消回调
        /// </summary>
        public void NoAction()
        {

        }
    }
}