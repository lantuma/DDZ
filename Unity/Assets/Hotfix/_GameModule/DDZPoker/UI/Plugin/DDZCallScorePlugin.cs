/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 叫分插件 }                                                                                                                   
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
    public class DDZCallScorePluginAwakeSystem : AwakeSystem<DDZCallScorePlugin, GameObject>
    {
        public override void Awake(DDZCallScorePlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class DDZCallScorePlugin : Entity
    {
        private GameObject panel;

        private ReferenceCollector _rf;
        
        private GameObject Clock;//闹钟

        private Text Num;

        public int callScore = 0;

        public DDZClockComponent clockComponent;

        private Button fen1Obj;

        private Button fen2Obj;

        private Button fen3Obj;

        public DDZCallScorePlugin Awake(GameObject panel)
        {
            this.panel = panel;

            _rf = this.panel.GetComponent<ReferenceCollector>();
            
            Clock = _rf.Get<GameObject>("Clock");

            Num = _rf.Get<GameObject>("Num").GetComponent<Text>();

            fen1Obj = _rf.Get<GameObject>("fen" + 1 + "Btn").GetComponent<Button>();

            fen2Obj = _rf.Get<GameObject>("fen" + 2 + "Btn").GetComponent<Button>();

            fen3Obj = _rf.Get<GameObject>("fen" + 3 + "Btn").GetComponent<Button>();

            this.Clock.SetActive(false);

            clockComponent = AddComponent<DDZClockComponent>().Awake(Num.gameObject);

            ButtonHelper.RegisterButtonEvent(_rf, "fen0Btn", () =>
            {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                callScore = 0;

                SendCallScore();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "fen1Btn", () =>
            {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                callScore = 1;

                SendCallScore();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "fen2Btn", () =>
            {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                callScore = 2;

                SendCallScore();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "fen3Btn", () =>
            {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                callScore = 3;

                SendCallScore();
            });
            
            return this;
        }

        private void SendCallScore()
        {
            DDZConfig.GameScene.RequestAskScore(this.callScore);
        }

        public void Reset()
        {
            this.Num.text = "0";

            this.Clock.SetActive(false);

            this.Hide();

            fen1Obj.interactable = true;

            fen2Obj.interactable = true;

            fen3Obj.interactable = true;
        }

        public void Show(int QdzLifeTime = 0)
        {
            this.CheckPreCallScore();

            this.panel.SetActive(true);

            this.Clock.SetActive(true);

            int clockTime = QdzLifeTime > 0 ? QdzLifeTime : DDZGameConfigComponent.Instance.QdzLifeTime;

            this.clockComponent.Play(clockTime, ()=> {

                callScore = 0; SendCallScore();
                
            });
        }
        
        public void SetBtnInterActable(int _id,bool _interactable)
        {
            var _btn = _rf.Get<GameObject>("fen" + _id + "Btn").GetComponent<Button>();

            _btn.interactable = _interactable;
        }

        /// <summary>
        /// 检测上一个玩家的叫分
        /// </summary>
        public void CheckPreCallScore()
        {
            int preCallScore = DDZGameConfigComponent.Instance.PreCallScore;

            if (preCallScore != -1)
            {
                if (preCallScore == 0)
                {
                    fen1Obj.interactable = true;

                    fen2Obj.interactable = true;

                    fen3Obj.interactable = true;
                }
                else if (preCallScore == 1)
                {
                    fen1Obj.interactable = false;

                    fen2Obj.interactable = true;

                    fen3Obj.interactable = true;
                }
                else if (preCallScore == 2)
                {
                    fen1Obj.interactable = false;

                    fen2Obj.interactable = false;

                    fen3Obj.interactable = true;
                }
                else
                {
                    Log.Error("叫分异常了！！！");
                }
            }

        }

        public void Hide()
        {
            this.Clock.SetActive(false);

            this.panel.SetActive(false);
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
