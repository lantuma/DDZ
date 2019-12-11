/******************************************************************************************
*         【模块】{ 大厅模块 }                                                                                                                      
*         【功能】{ 大厅顶部插件 }                                                                                                                   
*         【修改日期】{ 2019年7月18日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETModel;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameLobbyTopPluginAwakeSystem : AwakeSystem<GameLobbyTopPlugin, GameObject>
    {
        public override void Awake(GameLobbyTopPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class GameLobbyTopPlugin : Component
    {
        private GameObject panel;

        private ReferenceCollector _rf;

        private GameLobbyCpt lobby;

        public Text _PlayerIdText;

        public Text _GoldNumberText;

        public Text _PlayerNameText;

        public Image _HeadImage;

        public Text AdressCopyText;

        //public GameObject YuEBaoButton;

        //public GameObject PromotionButton;

        //动态加载图片
        public Image PlayerInfoBg;


        public GameLobbyTopPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            this.PlayerInfoBg = this.panel.transform.Find("PlayerInfoBg").GetComponent<Image>();

            this.lobby = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIHallPanel).GetComponent<GameLobbyCpt>();

            this._rf = this.panel.GetComponent<ReferenceCollector>();

            this._PlayerIdText = _rf.Get<GameObject>("PlayerIdText").GetComponent<Text>();

            this._GoldNumberText = _rf.Get<GameObject>("GoldNumberText").GetComponent<Text>();

            this._PlayerNameText = _rf.Get<GameObject>("PlayerNameText").GetComponent<Text>();

            this._HeadImage = _rf.Get<GameObject>("HeadImage").GetComponent<Image>();

            this.AdressCopyText = _rf.Get<GameObject>("AdressCopyText").GetComponent<Text>();

            //this.YuEBaoButton = _rf.Get<GameObject>("YuEBaoButton");

            //this.PromotionButton = _rf.Get<GameObject>("PromotionButton");

            ButtonHelper.RegisterButtonEvent(_rf, "AdressCopyButton", () => {

                Game.PopupComponent.SetClickLock();

                ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);

                if (UserDataHelper.UserInfo == null)
                {
                    Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.GetUrlWaitTip);

                    return;
                }

                GUIUtility.systemCopyBuffer = GameHelper.GetShareQRCodeURL() + UserDataHelper.UserInfo.PlayerId;

                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.CopyURLSuccTip);
                
            });

            //ButtonHelper.RegisterButtonEvent(_rf, "YuEBaoButton", () => {

            //    Game.PopupComponent.SetClickLock();

            //    SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
                
            //    if (lobby.GameYuEBaoCpt == null) lobby.GameYuEBaoCpt = lobby.AddComponent<GameYuEBaoCpt>();

            //    lobby.GameYuEBaoCpt.Open();

            //});

            //ButtonHelper.RegisterButtonEvent(_rf, "PromotionButton", () => {

            //    Game.PopupComponent.SetClickLock();

            //    SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
                
            //    if (lobby.GamePromoteCpt == null) lobby.GamePromoteCpt = lobby.AddComponent<GamePromoteCpt>();

            //    lobby.GamePromoteCpt.Open();

            //});

            ButtonHelper.RegisterButtonEvent(_rf, "IDCopyButton", () => {

                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                GUIUtility.systemCopyBuffer = _PlayerIdText.text;

                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.CopyIDSuccTip);

            });

            ButtonHelper.RegisterButtonEvent(_rf, "AdressCopyButton", () => {

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
            
            ButtonHelper.RegisterButtonEvent(_rf, "HeadButton", () => {

                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                if (lobby.GameUserCenterCpt == null) lobby.GameUserCenterCpt = lobby.AddComponent<GameUserCenterCpt>();

                lobby.GameUserCenterCpt.Open();
            });


            return this;
        }

        /// <summary>
        /// 设置用户信息
        /// </summary>
        public async void SetAccountInfo()
        {

            await UserDataHelper.AddDependView(new UserDataHelper.UserInfoView()
            {
                HeadImg = _HeadImage,

                NickNameText = _PlayerNameText,

                GoldText = _GoldNumberText,

                UserIdText = _PlayerIdText

            }, true);

            lobby.GameLobbyGameTypeSelectPlugin.GameTypeGoldNumberText.text = UserDataHelper.UserInfo.Gold.ToString("F2");
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
