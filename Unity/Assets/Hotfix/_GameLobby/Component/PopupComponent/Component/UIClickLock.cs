/******************************************************************************************
*         【模块】{ 通用模块 }                                                                                                                      
*         【功能】{ 按钮点击频繁限制}                                                                                                                   
*         【修改日期】{ 2019年8月16日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIClickLockAwakeSystem : AwakeSystem<UIClickLock>
    {
        public override void Awake(UIClickLock self)
        {
            self.Awake();
        }
    }

    public class UIClickLock : Component
    {
        private ReferenceCollector _rf;

        private GameObject panel;

        public void Awake()
        {
            this._rf = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            this.panel = this.GetParent<UI>().GameObject;

            ButtonHelper.RegisterButtonEvent(_rf, "UIClickLock", () =>
            {
                Log.Debug("点击太频繁了!");
            });
        }

        public async void OnOpen(float limitTime = 0.3f)
        {
            this.panel.SetActive(true);

            await Task.Delay((int)(limitTime * 1000));

            this.panel.SetActive(false);
        }

        public void OnHide()
        {
            this.panel.SetActive(false);
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
