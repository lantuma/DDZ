
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameLobbyBottomPluginAwakeSystem : AwakeSystem<GameLobbyBottomPlugin, GameObject>
    {
        public override void Awake(GameLobbyBottomPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class GameLobbyBottomPlugin : Component
    {
        private GameObject panel;

        private ReferenceCollector _rf;

        private GameLobbyCpt lobby;

        public GameObject CashButton;

        public GameObject MyRecordButton;

        //public GameObject ServiceButton;

        public GameObject NoticeButton;

        public GameObject PersonSettingButton;

        //动态换图的引用
        public Image lobbyBg;

        //public Image ServiceButtonBg;

        public Image NoticeButtonBg;

        public Image PersonSettingButtonBg;

        public Image RankButtonBg;

        public Image MyRecordButtonBg;

        public Image BottomBg;

        public GameLobbyBottomPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            this.BottomBg = this.panel.transform.Find("Image (2)").GetComponent<Image>();

            this.lobby = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIHallPanel).GetComponent<GameLobbyCpt>();

            this._rf = this.panel.GetComponent<ReferenceCollector>();

            this.CashButton = _rf.Get<GameObject>("CashButton");

            this.MyRecordButton = _rf.Get<GameObject>("MyRecordButton");

            //this.ServiceButton = _rf.Get<GameObject>("ServiceButton");

            this.NoticeButton = _rf.Get<GameObject>("NoticeButton");

            this.PersonSettingButton = _rf.Get<GameObject>("PersonSettingButton");

            //动态替换

            this.lobbyBg = this.panel.transform.parent.Find("bg").GetComponent<Image>();

            //this.ServiceButtonBg = _rf.Get<GameObject>("ServiceButton").transform.Find("Image").GetComponent<Image>();

            this.NoticeButtonBg = _rf.Get<GameObject>("NoticeButton").transform.Find("Image").GetComponent<Image>();

            this.PersonSettingButtonBg = _rf.Get<GameObject>("PersonSettingButton").transform.Find("Image").GetComponent<Image>();

            this.RankButtonBg = _rf.Get<GameObject>("RankButton").transform.Find("Image").GetComponent<Image>();

            this.MyRecordButtonBg = _rf.Get<GameObject>("MyRecordButton").transform.Find("Image").GetComponent<Image>();
            

            //公告
            ButtonHelper.RegisterButtonEvent(_rf, "NoticeButton", () => {

                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                this.OnNoticeButton();
            });

            //设置
            ButtonHelper.RegisterButtonEvent(_rf, "PersonSettingButton", () => {

                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                if (lobby.GamePersonSettingCpt == null) lobby.GamePersonSettingCpt = lobby.AddComponent<GamePersonSettingCpt>();

                lobby.GamePersonSettingCpt.Open();
            });

            //排行
            ButtonHelper.RegisterButtonEvent(_rf, "RankButton", () => {

                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                if (lobby.UIRankPanelCpt == null)
                {
                    lobby.UIRankPanelCpt = lobby.AddComponent<UIRankPanelCpt>();
                }
                lobby.UIRankPanelCpt.OnOpen();
            });

            //战绩
            ButtonHelper.RegisterButtonEvent(_rf, "MyRecordButton", () => {

                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
                
                if (lobby.GameMyRecordCpt== null)
                {
                    lobby.GameMyRecordCpt = lobby.AddComponent<GameMyRecordCpt>();
                }

                lobby.GameMyRecordCpt.Open();

            });

            return this;
        }

        /// <summary>
        /// 公告
        /// </summary>
        private async void OnNoticeButton()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            if (lobby.GameNoticeCpt == null) lobby.GameNoticeCpt = lobby.AddComponent<GameNoticeCpt>();

            lobby.GameNoticeCpt.Open();
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
