/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 准备插件}                                                                                                                   
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
    public class DDZReadyPluginAwakeSystem : AwakeSystem<DDZReadyPlugin, GameObject>
    {
        public override void Awake(DDZReadyPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class DDZReadyPlugin : Component
    {
        private GameObject panel;

        private ReferenceCollector _rf;

        private GameObject SelfNoReady;

        private GameObject SelfReady;

        private GameObject WaitOtherEnterTip;

        public DDZReadyPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            _rf = this.panel.GetComponent<ReferenceCollector>();

            SelfNoReady = _rf.Get<GameObject>("SelfNoReady");

            SelfReady = _rf.Get<GameObject>("SelfReady");

            WaitOtherEnterTip = _rf.Get<GameObject>("WaitOtherEnterTip");

            WaitOtherEnterTip.SetActive(false);

            ButtonHelper.RegisterButtonEvent(_rf, "ChangeTableBtn", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZConfig.GameScene.RequestChangeRoom();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "ChangeTableBtn1", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZConfig.GameScene.RequestChangeRoom();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "ReadyBtn", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                //如果玩家点X，再手动点准备，则需要重置
                DDZConfig.GameScene.Reset();

                DDZConfig.GameScene.RequestPrepare();

            });

            this.Reset();

            return this;
        }

        public void Reset()
        {
            SelfNoReady.SetActive(false);

            SelfReady.SetActive(false);
            
        }

        public void Show(int type = 0)
        {
            if (type == 0)
            {
                this.SelfNoReady.SetActive(true);

                this.SelfReady.SetActive(false);
            }
            else
            {
                this.SelfNoReady.SetActive(false);

                this.SelfReady.SetActive(true);
            }
        }

        public void SetReadyByIndex(int _index,bool _Show)
        {
            if (_index >= 0)
            {
                _rf.Get<GameObject>(_index.ToString()).SetActive(_Show);
            }
            else
            {
                Log.Debug("异常，座位ID为负了");
            }
        }

        public void ClearAllReady()
        {
            for (int i = 0; i < 3; i++)
            {
                _rf.Get<GameObject>(i.ToString()).SetActive(false);
            }
        }

        public void SetEnterTipState(bool isShow)
        {
            this.WaitOtherEnterTip.SetActive(isShow);
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
