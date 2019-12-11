/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 下拉菜单插件}                                                                                                                   
*         【修改日期】{ 2019年9月19日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using ETModel;
using UnityEngine;
using UnityEngine.UI;


namespace ETHotfix
{
    [ObjectSystem]
    public class DDZDownPullPluginAwakeSystem : AwakeSystem<DDZDownPullPlugin, GameObject>
    {

        public override void Awake(DDZDownPullPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }

    }

    public class DDZDownPullPlugin: Component
    {
        private GameObject panel;

        private ReferenceCollector _rf;

        private Image SettingButtonImg;

        public DDZDownPullPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            _rf = this.panel.GetComponent<ReferenceCollector>();

            this.SettingButtonImg = _rf.Get<GameObject>("SettingButtonImg").GetComponent<Image>();

            ButtonHelper.RegisterButtonEvent(_rf, "SettingMask", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                SoundHelper.ToggleSubGameSound(SoundCloseAction, SoundShowAction);

                //Game.PopupComponent.ShowSettingPanel();

                //this.Hide();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "HelpMask", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZUIFactory.helpPanel.Create();

                this.Hide();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "ChangeTableBtn", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZConfig.GameScene.RequestPrepare();

                this.Hide();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "BackMask", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                this.Hide();

                DDZConfig.GameScene.OnQuitGame();
                
            });

            this.panel.GetComponent<Button>().onClick.AddListener(() => { this.Hide(); });

            SoundHelper.InitSubGameSound(SoundCloseAction, SoundShowAction);

            return this;
        }
        

        private void SoundCloseAction()
        {
            SettingButtonImg.sprite = SpriteHelper.GetSprite("commonui", "DDZ2_guanbishengyin@2x");
        }


        private void SoundShowAction()
        {
            SettingButtonImg.sprite = SpriteHelper.GetSprite("commonui", "DDZ2_kaiqishengyin@2x");
        }

        public void Show()
        {
            this.panel.SetActive(true);
        }

        public void Hide()
        {
            this.panel.SetActive(false);

            DDZConfig.GameScene.DDZLTPlugin.ResetPullDownBtn();
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
