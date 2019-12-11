using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class Setting_PersonalCptAwakeSystem : AwakeSystem<Setting_PersonalCpt, GameObject, GameObject, ButtonGroup>
    {
        public override void Awake(Setting_PersonalCpt self, GameObject a, GameObject b, ButtonGroup c)
        {
            self.Awake(a, b, c);
        }
    }

    public class Setting_PersonalCpt : SimpleToggle
    {
        private Slider _soundSlider;
        private Slider _musicSlider;

        private Image SoundIcon;
        private Image MusicIcon;

        private Sprite SoundIconOpen;

        private Sprite SoundIconClose;

        private Sprite MusicIconOpen;

        private Sprite MusicIconClose;

        private bool _isRequest;

        public override void Init()
        {
            base.Init();

            _soundSlider = ReferenceCollector.Get<GameObject>("SoundSlider").GetComponent<Slider>();
            _musicSlider = ReferenceCollector.Get<GameObject>("MusicSlider").GetComponent<Slider>();

            SoundIcon = ReferenceCollector.Get<GameObject>("SoundIcon").GetComponent<Image>();
            MusicIcon = ReferenceCollector.Get<GameObject>("MusicIcon").GetComponent<Image>();

            SoundIconOpen = SpriteHelper.GetSprite("commonui", "Common_yinl");

            SoundIconClose = SpriteHelper.GetSprite("commonui", "Common_yinl0");

            MusicIconOpen = SpriteHelper.GetSprite("commonui", "Common_yiny");

            MusicIconClose = SpriteHelper.GetSprite("commonui", "Common_yiny0");

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

            ReferenceCollector.Get<GameObject>("LogOutBtn").GetComponent<Button>().onClick.AddListener(Logout);
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
//                Game.PopupComponent.ShowMessageBox("出现未知错误!");
            }
        }

        public override void ChangeToggle(bool state)
        {
            base.ChangeToggle(state);
        }
    }
}