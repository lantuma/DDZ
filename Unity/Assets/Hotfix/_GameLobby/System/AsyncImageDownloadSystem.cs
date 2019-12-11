/******************************************************************************************
*         【模块】{ 大厅模块 }                                                                                                                      
*         【功能】{ 大厅-异步图片加载 }                                                                                                                   
*         【修改日期】{ 2019年11月25日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [Event(EventIdType.AsyncImageDownload)]
    public class AsyncImageDownloadSystem : AEvent
    {
        public override void Run()
        {

            var uic = Game.Scene.GetComponent<UIComponent>();

            var WebConfigInfo = DataCenterComponent.Instance.WebConfigInfo;

            var WebConfig = ClientComponent.Instance.webConfig;
            
            if (WebConfig != null)
            {
                //大厅背景图
                WebConfigInfo.lobby_Url_Bg = WebConfig.dtbjt;

                //大厅广告-Split
                if (!string.IsNullOrEmpty(WebConfig.ggytp))
                {
                    string[] ggytps = WebConfig.ggytp.Split(',');

                    if (ggytps.Length > 0) WebConfigInfo.lobby_Url_Adv1 = ggytps[0];

                    if (ggytps.Length > 1) WebConfigInfo.lobby_Url_Adv2 = ggytps[1];
                }

                //客服文字
                WebConfigInfo.lobby_KeFu_Msg = WebConfig.kfwa;

                //大厅顶部背景图
                WebConfigInfo.lobby_Url_TopBg = WebConfig.dtdbtp;

                //大厅底部背景图
                WebConfigInfo.lobby_Url_BottomBg = WebConfig.dtdibtp;

                //登陆背景图
                WebConfigInfo.login_Url_BackBg = WebConfig.dlbjt;

                //登陆Logo
                WebConfigInfo.login_Url_Logo = WebConfig.dllogo;
            }

            if (uic.Get(UIType.UIHallPanel) != null)
            {
                var lobbyCpt = uic.Get(UIType.UIHallPanel).GetComponent<GameLobbyCpt>();

                if (lobbyCpt != null)
                {
                    //大厅背景
                    AsyncImageDownloadComponent.instance.SetAsyncImage(WebConfigInfo.lobby_Url_Bg, lobbyCpt.GameLobbyBottomPlugin.lobbyBg);

                    //广告位
                    AsyncImageDownloadComponent.instance.SetAsyncImage(WebConfigInfo.lobby_Url_Adv1, lobbyCpt.GameLobbyCarousePlugin.DownGamePicImg);

                    AsyncImageDownloadComponent.instance.SetAsyncImage(WebConfigInfo.lobby_Url_Adv2, lobbyCpt.GameLobbyCarousePlugin.SaveDownURLPicImg);

                    //顶部背景图
                    AsyncImageDownloadComponent.instance.SetAsyncImage(WebConfigInfo.lobby_Url_TopBg, lobbyCpt.GameLobbyTopPlugin.PlayerInfoBg);

                    //底部背景图
                    AsyncImageDownloadComponent.instance.SetAsyncImage(WebConfigInfo.lobby_Url_BottomBg, lobbyCpt.GameLobbyBottomPlugin.BottomBg);
                    
                    
                    //公告
                    AsyncImageDownloadComponent.instance.SetAsyncImage(WebConfigInfo.lobby_Url_BtnGongGao, lobbyCpt.GameLobbyBottomPlugin.NoticeButtonBg);

                    //设置
                    AsyncImageDownloadComponent.instance.SetAsyncImage(WebConfigInfo.lobby_Url_BtnSet, lobbyCpt.GameLobbyBottomPlugin.PersonSettingButtonBg);

                    //排行榜
                    AsyncImageDownloadComponent.instance.SetAsyncImage(WebConfigInfo.lobby_Url_BtnRank, lobbyCpt.GameLobbyBottomPlugin.RankButtonBg);

                    //战绩
                    AsyncImageDownloadComponent.instance.SetAsyncImage(WebConfigInfo.lobby_Url_BtnZhanJi, lobbyCpt.GameLobbyBottomPlugin.MyRecordButtonBg);
                    
                }
            }

            if (uic.Get(UIType.Login) != null)
            {
                var loginCpt = uic.Get(UIType.Login).GetComponent<UILoginCpt>();

                if (loginCpt != null)
                {
                    //登陆-背景
                    AsyncImageDownloadComponent.instance.SetAsyncImage(WebConfigInfo.login_Url_BackBg, loginCpt.LoginBg);

                    //登陆-Logo
                    AsyncImageDownloadComponent.instance.SetAsyncImage(WebConfigInfo.login_Url_Logo, loginCpt.LoginLogo);
                }
            }
        }
        
    }
}
