/******************************************************************************************
*         【模块】{ 通用模块 }                                                                                                                      
*         【功能】{ 加载等待时锁定动画}                                                                                                                   
*         【修改日期】{ 2019年5月17日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UILoadingLockAwakeSystem : AwakeSystem<UILoadingLock>
    {
        public override void Awake(UILoadingLock self)
        {
            self.Awake();
        }
    }

    public class UILoadingLock:Component
    {
        private ReferenceCollector _rf;

        private Text Text;

        private GameObject bg;

        private LoadingLockData data = null;

        public void Awake()
        {
            _rf = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            Text = _rf.Get<GameObject>("Text").GetComponent<Text>();

            bg = _rf.Get<GameObject>("bg");

            Tween tween = bg.transform.DORotate(new Vector3(0, 0, -360), 2f).SetEase(Ease.Linear).SetLoops(-1);

            tween.SetRelative();

            tween.PlayForward();
        }

        public async void OnOpen(object arg = null)
        {
            if (arg != null)
            {
                data = null;

                data = arg as LoadingLockData;

                Text.text = data.mes;

                await Task.Delay(data.timeOut * 1000);

                Game.PopupComponent.CloseLoadingLockUI();

                data.timeOutAction();
            }
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
