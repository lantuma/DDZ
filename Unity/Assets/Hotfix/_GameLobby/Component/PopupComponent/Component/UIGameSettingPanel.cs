/******************************************************************************************
*         【模块】{ 通用模块 }                                                                                                                      
*         【功能】{ 设置面板}                                                                                                                   
*         【修改日期】{ 2019年5月5日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIGameSettingPanelAwakeSystem : AwakeSystem<UIGameSettingPanel>
    {
        public override void Awake(UIGameSettingPanel self)
        {
            self.Awake();
        }
    }

    public class UIGameSettingPanel : Component
    {
        private ReferenceCollector _rf;

        public GameObject Root;

        private Slider SFxSlider;

        private Slider MusicSlider;

        private Image SoundIcon;

        private Image MusicIcon;


        private Sprite SoundIconOpen;

        private Sprite SoundIconClose;

        private Sprite MusicIconOpen;

        private Sprite MusicIconClose;

        private bool _isRequest;

        public void Awake()
        {
            _rf = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            Root = _rf.Get<GameObject>("Root");

            SoundIcon = _rf.Get<GameObject>("SoundIcon").GetComponent<Image>();

            MusicIcon = _rf.Get<GameObject>("MusicIcon").GetComponent<Image>();

            SFxSlider = _rf.Get<GameObject>("SFxSlider").GetComponent<Slider>();

            MusicSlider = _rf.Get<GameObject>("MusicSlider").GetComponent<Slider>();

            SoundIconOpen = SpriteHelper.GetSprite("commonui", "Setting2_yinliang_guan_1");

            SoundIconClose = SpriteHelper.GetSprite("commonui", "Setting2_yinliang_guan");

            MusicIconOpen = SpriteHelper.GetSprite("commonui", "Setting2_yinyue_kai");

            MusicIconClose = SpriteHelper.GetSprite("commonui", "Setting2_yinyue_guan");

            ButtonHelper.RegisterButtonEvent(_rf, "CloseButton", OnClose);

            ButtonHelper.RegisterButtonEvent(_rf, "mask", OnClose);

            ButtonHelper.RegisterButtonEvent(_rf, "SoundIcon", () =>
            {
                SoundComponent.Instance.SfxVolume = this.SoundIcon.sprite == SoundIconOpen ? 0:1;

                SFxSlider.value = SoundComponent.Instance.SfxVolume;

                this.UpdateIcon();

            });

            ButtonHelper.RegisterButtonEvent(_rf, "MusicIcon", () =>
            {
                SoundComponent.Instance.SoundVolume = this.MusicIcon.sprite == MusicIconOpen ? 0:1;

                MusicSlider.value = SoundComponent.Instance.SoundVolume;

                this.UpdateIcon();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "OneKeyFixBtn", () =>
            {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.FixGameSuccessTip);
            });

            ButtonHelper.RegisterButtonEvent(_rf, "ChangeAccountBtn", () =>
            {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                this.Logout();
            });

            MusicSlider.value = SoundComponent.Instance.SoundVolume;

            SFxSlider.value = SoundComponent.Instance.SfxVolume;

            this.UpdateIcon();


            SFxSlider.onValueChanged.Add((value) =>
            {
                SoundComponent.Instance.SfxVolume = value;

                this.UpdateIcon();
            });

            MusicSlider.onValueChanged.Add((value) =>
            {
                SoundComponent.Instance.SoundVolume = value;

                this.UpdateIcon();
            });

        }

        public async void OnClose()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

            var ui = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIGameSettingPanel);

            ui.GetComponent<TweenEffectComponent>().PlayBackwards(TweenAnimationIdType.GameSettingPanel);

            await Task.Delay(200);

            Game.PopupComponent.CloseSettingPanel();
        }

        private void UpdateIcon()
        {
            this.MusicIcon.sprite = SoundComponent.Instance.SoundVolume != 0 ? MusicIconOpen : MusicIconClose;

            this.SoundIcon.sprite = SoundComponent.Instance.SfxVolume != 0 ? SoundIconOpen: SoundIconClose;
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        private async void Logout()
        {
            if (_isRequest)
            {
                Game.PopupComponent.ShowMessageBox("正在登出,请勿重复操作");
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

            this.Reset();
        }
    }
}
