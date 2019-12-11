/******************************************************************************************
*         【模块】{ 大厅模块 }                                                                                                                      
*         【功能】{ 游戏公告广播 }                                                                                                                   
*         【修改日期】{ 2019年5月15日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class ScrollToNoticeSystem:AMHandler<Actor_ScrollToNotice_Ntt>
    {
        protected override void Run(ETModel.Session session, Actor_ScrollToNotice_Ntt message)
        {
            var uc = Game.Scene.GetComponent<UIComponent>();

            if (uc.Get(UIType.UIScrollNoticePanel) != null)
            {
                ScrollNoticeComponent.Instance.Add(message.Content);
            }
        }
    }

    

    #region --------------------玩家信息数据更新接收,后台改完用户信息我会给你推送一次，充值或者提现审核后,都推送

    [MessageHandler]
    public class Actor_UpdateUserInfo_NttSystem : AMHandler<Actor_UpdateUserInfo_Ntt>
    {
        protected override async void Run(ETModel.Session session, Actor_UpdateUserInfo_Ntt message)
        {
            await UserDataHelper.GetUserInfo(true);

            Log.Debug("后台推送用户信息，在充值或者提现审核后!!");
        }
    }

    #endregion
}
