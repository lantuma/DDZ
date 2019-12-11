/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 左上区域插件}                                                                                                                   
*         【修改日期】{ 2019年5月28日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class DDZLTPluginAwakeSystem : AwakeSystem<DDZLTPlugin, GameObject>
    {
        public override void Awake(DDZLTPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class DDZLTPlugin : Component
    {
        private GameObject panel;

        private ReferenceCollector _rf;

        private Button changeTableBtn;

        private Image pullDownBtnImg;


        public DDZLTPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            _rf = this.panel.GetComponent<ReferenceCollector>();

            this.changeTableBtn = _rf.Get<GameObject>("changeTableBtn").GetComponent<Button>();

            this.pullDownBtnImg = _rf.Get<GameObject>("pullDownBtn").GetComponent<Image>();

            this.changeTableBtn.gameObject.SetActive(false);
            
            ButtonHelper.RegisterButtonEvent(_rf, "exitBtn", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZConfig.GameScene.OnQuitGame();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "changeTableBtn", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZConfig.GameScene.OnQuitGame();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "pullDownBtn", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                this.pullDownBtnImg.sprite = SpriteHelper.GetSprite("commonui", "DDZ2_xialaanniuzhedie@2x");

                DDZConfig.GameScene.DDZDownPullPlugin.Show();
                

            });

            

            return this;
        }

        public void ResetPullDownBtn()
        {
            this.pullDownBtnImg.sprite = SpriteHelper.GetSprite("commonui", "DDZ2_xialaanniuxiala@2x");
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
