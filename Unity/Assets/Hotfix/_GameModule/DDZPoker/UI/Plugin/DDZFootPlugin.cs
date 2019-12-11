/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 底注插件}                                                                                                                   
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
    public class DDZFootPluginAwakeSystem : AwakeSystem<DDZFootPlugin, GameObject>
    {
        public override void Awake(DDZFootPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class DDZFootPlugin : Component
    {
        private GameObject panel;

        private ReferenceCollector _rf;


        public DDZFootPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            _rf = this.panel.GetComponent<ReferenceCollector>();

            ButtonHelper.RegisterButtonEvent(_rf, "bg", () =>
            {
                //SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZConfig.GameScene.DDZHandCardPlugin.ReSelect();
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
