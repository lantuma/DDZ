/******************************************************************************************
*         【模块】{ 大厅模块 }                                                                                                                      
*         【功能】{ 大厅轮播插件 }                                                                                                                   
*         【修改日期】{ 2019年7月18日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETModel;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameLobbyCarousePluginAwakeSystem : AwakeSystem<GameLobbyCarousePlugin, GameObject>
    {
        public override void Awake(GameLobbyCarousePlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class GameLobbyCarousePlugin : Component
    {
        public GameObject panel;

        private ReferenceCollector _rf;

        private GameLobbyCpt lobby;

        private Image QRCode;

        //动态加载的图片
        public Image DownGamePicImg;

        public Image SaveDownURLPicImg;

        public GameLobbyCarousePlugin Awake(GameObject panel)
        {
            this.panel = panel;

            this.DownGamePicImg = this.panel.transform.Find("DownGamePic").GetComponent<Image>();

            this.SaveDownURLPicImg = this.panel.transform.Find("SaveDownURLPic").GetComponent<Image>();

            CarouseComponent.instance.SetHallDefault(this.panel);
            this._rf = this.panel.GetComponent<ReferenceCollector>();
            this.lobby = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIHallPanel).GetComponent<GameLobbyCpt>();
            this.QRCode = _rf.Get<GameObject>("QRCode").GetComponent<Image>();

            ButtonHelper.RegisterButtonEvent(_rf, "CopyQRCodeLink", () => {

                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                if (UserDataHelper.UserInfo == null)
                {
                    Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.GetUrlWaitTip);
                    return;
                }

                GUIUtility.systemCopyBuffer = GameHelper.GetShareQRCodeURL() + UserDataHelper.UserInfo.PlayerId;
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.CopyURLSuccTip);

            });

            ButtonHelper.RegisterButtonEvent(_rf, "QRCode", () => {

                Game.PopupComponent.SetClickLock();
                Game.PopupComponent.ShowQRCodePanel();

            });

            ButtonHelper.RegisterButtonEvent(_rf, "DownLoadGame", () =>
            {
                Game.PopupComponent.SetClickLock();
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
            });

            ButtonHelper.RegisterButtonEvent(_rf, "SaveDownUrl", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                GUIUtility.systemCopyBuffer = GameHelper.GetShareQRCodeURL() + UserDataHelper.UserInfo.PlayerId;

                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.CopyURLSuccTip);
            });

            this.LoadQRCode();

            return this;
        }

        /// <summary>
        /// 延时加载QRCode
        /// </summary>
        private async void LoadQRCode()
        {
            while (true)
            {
                await Task.Delay(100);
                if (UserDataHelper.UserInfo == null) continue;

                QRCode.sprite = ETModel.Game.Scene.GetComponent<QRCodeComponent>().GenerateQRImage(GameHelper.GetShareQRCodeURL() + UserDataHelper.UserInfo.PlayerId);

                this.lobby.GameLobbyTopPlugin.AdressCopyText.text = GameHelper.GetShareQRCodeURL() + UserDataHelper.UserInfo.PlayerId;

                return;
            }
        }

        public void Reset()
        {

        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
        }
    }
}
