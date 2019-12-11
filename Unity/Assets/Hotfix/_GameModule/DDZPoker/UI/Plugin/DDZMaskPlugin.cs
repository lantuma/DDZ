/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ MASK插件}                                                                                                                   
*         【修改日期】{ 2019年5月28日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class DDZMaskPluginAwakeSystem : AwakeSystem<DDZMaskPlugin, GameObject>
    {
        public override void Awake(DDZMaskPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class DDZMaskPlugin : Component
    {
        private GameObject panel;

        private ReferenceCollector _rf;

        private Image Tip;

        public DDZMaskPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            _rf = this.panel.GetComponent<ReferenceCollector>();

            Tip = _rf.Get<GameObject>("Tip").GetComponent<Image>();
            
            return this;
        }

        /// <summary>
        /// 根据类型显示提示 0:出牌不符合规则  1:没有大过上家的牌 2：出牌不能为空
        /// </summary>
        /// <param name="type"></param>
        public async void Show(int type = 0)
        {
            this.panel.SetActive(true);

            if (type == 0)
            {
                Tip.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ_bufuhe");

                Tip.SetNativeSize();

                await Task.Delay(1000);

                this.panel.SetActive(false);
            }
            else if (type == 1)
            {
                Tip.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ_meiyoudag");

                Tip.SetNativeSize();

                await Task.Delay(2500);

                this.panel.SetActive(false);
            }
            else if (type == 2)
            {
                Tip.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ_OutCardNotEmpty");

                Tip.SetNativeSize();

                await Task.Delay(1000);

                this.panel.SetActive(false);
            }
            
        }

        public void Hide()
        {
            this.panel.SetActive(false);
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
