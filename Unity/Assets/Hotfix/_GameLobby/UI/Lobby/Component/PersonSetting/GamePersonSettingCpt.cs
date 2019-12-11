using System;
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class GamePersonSettingCptAwakeSystem : AwakeSystem<GamePersonSettingCpt>
    {
        public override void Awake(GamePersonSettingCpt self)
        {
            self.Awake();
        }
    }

    public class GamePersonSettingCpt : Entity
    {
        private GameObject panelGo;

        private ReferenceCollector ReferenceCollector;

        private Text Title;

        private Text Timestamp;

        private Text Content;

        private GameObject Root;

        private TweenEffectComponent _TEC;

        //private Button SettingToggle;

        //private Button ReportTableToggle;

        private Slider _soundSlider;
        private Slider _musicSlider;

        private Image SoundIcon;
        private Image MusicIcon;

        private Sprite SoundIconOpen;

        private Sprite SoundIconClose;

        private Sprite MusicIconOpen;

        private Sprite MusicIconClose;

        private bool _isRequest;

        private InputField _reportField;

        public void Awake()
        {
            var res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();

            res.LoadBundle(UIType.PersonSettingPanel.StringToAB());

            var gameObject = res.GetAsset(UIType.PersonSettingPanel.StringToAB(), UIType.PersonSettingPanel);

            this.panelGo = (GameObject)UnityEngine.Object.Instantiate(gameObject);

            UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.PersonSettingPanel, this.panelGo, false);

            Game.Scene.GetComponent<UIComponent>().Add(ui);

            this.ReferenceCollector = this.panelGo.GetComponent<ReferenceCollector>();

            this.Root = this.ReferenceCollector.Get<GameObject>("Root");

            _TEC = this.AddComponent<TweenEffectComponent>();

            _TEC.Add(TweenAnimationIdType.PersonSettingPanel, this.Root.transform);

            this.ReferenceCollector.Get<GameObject>("CloseButton").GetComponent<Button>().onClick.AddListener(Close);

            this.ReferenceCollector.Get<GameObject>("mask").Get<Button>().onClick.AddListener(Close);

            //this.SettingToggle = this.ReferenceCollector.Get<GameObject>("SettingToggle").GetComponent<Button>();

           // this.ReportTableToggle = this.ReferenceCollector.Get<GameObject>("ReportTableToggle").GetComponent<Button>();

            /*
            this.SettingToggle.onClick.AddListener(() =>
            {
                this.SettingToggle.transform.Find("isOn").gameObject.SetActive(true);

                this.SettingToggle.transform.Find("isOff").gameObject.SetActive(false);

                this.ReportTableToggle.transform.Find("isOn").gameObject.SetActive(false);

                this.ReportTableToggle.transform.Find("isOff").gameObject.SetActive(true);

                this.ReferenceCollector.Get<GameObject>("SettingView").SetActive(true);

                this.ReferenceCollector.Get<GameObject>("ReportView").SetActive(false);
            });

            this.ReportTableToggle.onClick.AddListener(() => {

                this.SettingToggle.transform.Find("isOn").gameObject.SetActive(false);

                this.SettingToggle.transform.Find("isOff").gameObject.SetActive(true);

                this.ReportTableToggle.transform.Find("isOn").gameObject.SetActive(true);

                this.ReportTableToggle.transform.Find("isOff").gameObject.SetActive(false);

                this.ReferenceCollector.Get<GameObject>("SettingView").SetActive(false);

                this.ReferenceCollector.Get<GameObject>("ReportView").SetActive(true);
            });
            */

            this.InitSettingView();

            //this.InitReportView();
        }

        #region 个人设置
        private void InitSettingView()
        {
            _soundSlider = ReferenceCollector.Get<GameObject>("SFxSlider").GetComponent<Slider>();

            _musicSlider = ReferenceCollector.Get<GameObject>("MusicSlider").GetComponent<Slider>();

            SoundIcon = ReferenceCollector.Get<GameObject>("SoundIcon").GetComponent<Image>();

            MusicIcon = ReferenceCollector.Get<GameObject>("MusicIcon").GetComponent<Image>();

            SoundIconOpen = SpriteHelper.GetSprite("commonui", "Setting2_yinliang_guan_1");

            SoundIconClose = SpriteHelper.GetSprite("commonui", "Setting2_yinliang_guan");

            MusicIconOpen = SpriteHelper.GetSprite("commonui", "Setting2_yinyue_kai");

            MusicIconClose = SpriteHelper.GetSprite("commonui", "Setting2_yinyue_guan");

            _musicSlider.value = SoundComponent.Instance.SoundVolume;
            _soundSlider.value = SoundComponent.Instance.SfxVolume;
            this.UpdateIcon();

            _soundSlider.onValueChanged.Add((value) =>
            {
                SoundComponent.Instance.SfxVolume = value;

                this.UpdateIcon();
            });

            _musicSlider.onValueChanged.Add((value) =>
            {
                SoundComponent.Instance.SoundVolume = value;

                this.UpdateIcon();
            });

            ButtonHelper.RegisterButtonEvent(ReferenceCollector, "SoundIcon", () =>
            {
                SoundComponent.Instance.SfxVolume = this.SoundIcon.sprite == SoundIconOpen ? 0 : 1;

                _soundSlider.value = SoundComponent.Instance.SfxVolume;

                this.UpdateIcon();

            });

            ButtonHelper.RegisterButtonEvent(ReferenceCollector, "MusicIcon", () =>
            {
                SoundComponent.Instance.SoundVolume = this.MusicIcon.sprite == MusicIconOpen ? 0 : 1;

                _musicSlider.value = SoundComponent.Instance.SoundVolume;

                this.UpdateIcon();
            });

            ButtonHelper.RegisterButtonEvent(ReferenceCollector, "OneKeyFixBtn", () =>
            {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.FixGameSuccessTip);
            });

            ButtonHelper.RegisterButtonEvent(ReferenceCollector, "ChangeAccountBtn", () =>
            {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                this.Logout();
            });
        }

        /// <summary>
        /// 更新禁音图标
        /// </summary>
        private void UpdateIcon()
        {
            this.MusicIcon.sprite = SoundComponent.Instance.SoundVolume != 0 ? MusicIconOpen : MusicIconClose;

            this.SoundIcon.sprite = SoundComponent.Instance.SfxVolume != 0 ? SoundIconOpen : SoundIconClose;
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        private async void Logout()
        {
            if (_isRequest)
            {
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.LoginOutNotRepeatTip);
                return;
            }
            _isRequest = true;

            try
            {
                _isRequest = false;
                Game.Scene.RemoveComponent<PingComponent>();
                Game.Scene.GetComponent<SessionComponent>()?.Session?.Dispose();
                Game.Scene.RemoveComponent<SessionComponent>();
                Game.EventSystem.Run(EventIdType.InitPokerSceneStart);
                Game.EventSystem.Run(EventIdType.RemoveGameLobby);
            }
            catch (Exception e)
            {
                _isRequest = false;
                Log.Debug("出现未知错误:" + e.Message);
            }
        }
        #endregion

        #region 问题反馈
        private void InitReportView()
        {
            _reportField = ReferenceCollector.Get<GameObject>("InputField").GetComponent<InputField>();

            ReferenceCollector.Get<GameObject>("SubmitBtn").GetComponent<Button>().onClick.AddListener(SubmitReport);
        }

        /// <summary>
        /// 发送反馈
        /// </summary>
        private void SubmitReport()
        {
            Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.ModuleNotOpenTip);
            _reportField.text = "";
            return;

            if (string.IsNullOrEmpty(_reportField.text))
            {
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.InputReportContentTip);
                return;
            }

            try
            {

            }
            catch (Exception e)
            {
                Game.PopupComponent.ShowMessageBox(e.Message);
                throw;
            }
        }
        #endregion

        public void Open()
        {
            this.panelGo.SetActive(true);

            this._TEC.Play(TweenAnimationIdType.PersonSettingPanel);
        }

        private async void Close()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
            this._TEC.PlayBackwards(TweenAnimationIdType.PersonSettingPanel);

            await Task.Delay(300);

            this.panelGo.SetActive(false);
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();

            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(UIType.PersonSettingPanel.StringToAB());
        }
    }
}