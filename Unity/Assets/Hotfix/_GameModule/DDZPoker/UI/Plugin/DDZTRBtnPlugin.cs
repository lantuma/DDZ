/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 右上插件}                                                                                                                   
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
    public class DDZTRBtnPluginAwakeSystem : AwakeSystem<DDZTRBtnPlugin, GameObject>
    {
        public override void Awake(DDZTRBtnPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class DDZTRBtnPlugin : Component
    {
        private GameObject panel;

        private ReferenceCollector _rf;


        public DDZTRBtnPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            _rf = this.panel.GetComponent<ReferenceCollector>();
            
            ButtonHelper.RegisterButtonEvent(_rf, "SettingBtn", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                Game.PopupComponent.ShowSettingPanel();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "HelpBtn", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZUIFactory.helpPanel.Create();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "TuoGuanBtn", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

            });

            return this;
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
