/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 交互插件}                                                                                                                   
*         【修改日期】{ 2019年5月28日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;
using UnityEngine.UI;


namespace ETHotfix
{
    [ObjectSystem]
    public class DDZInteractivePluginAwakeSystem : AwakeSystem<DDZInteractivePlugin, GameObject>
    {
        public override void Awake(DDZInteractivePlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class DDZInteractivePlugin : Entity
    {
        private GameObject panel;

        private ReferenceCollector _rf;

        private GameObject Clock;

        private Text Num;

        public DDZClockComponent clockComponent;

        private Button TipBtn;
        
        private int tipCardIndex = -1;

        public DDZInteractivePlugin Awake(GameObject panel)
        {
            this.panel = panel;

            _rf = this.panel.GetComponent<ReferenceCollector>();

            Clock = _rf.Get<GameObject>("Clock");

            Num = _rf.Get<GameObject>("Num").GetComponent<Text>();

            TipBtn = _rf.Get<GameObject>("TipBtn").GetComponent<Button>();
            
            Clock.SetActive(false);

            clockComponent = AddComponent<DDZClockComponent>().Awake(Num.gameObject);
            
            ButtonHelper.RegisterButtonEvent(_rf, "NotOutBtn", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZConfig.GameScene.RequestPlayCard(1, new DDZCard());
            });

            ButtonHelper.RegisterButtonEvent(_rf, "ReSetBtn", () =>
            {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZConfig.GameScene.DDZHandCardPlugin.ReSelect();

            });

            ButtonHelper.RegisterButtonEvent(_rf, "TipBtn", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZConfig.GameScene.DDZHandCardPlugin.ReSelect();

                var tipCardDic = DDZConfig.GameScene.DDZAIComponent.GetTipCards();
                
                if (tipCardDic != null && tipCardDic.Count > 0)
                {
                    Log.Debug("获得的提示长度是:" + tipCardDic.Count);

                    if (tipCardIndex == -1) tipCardIndex = 0;

                    if (tipCardIndex > tipCardDic.Count - 1) tipCardIndex = 0;

                    List<int> tipCards = tipCardDic[tipCardIndex];

                    DDZConfig.GameScene.DDZHandCardPlugin.SelectTipCard(tipCards);

                    tipCardIndex++;
                }
                else
                {
                    Game.PopupComponent.ShowTips(DataCenterComponent.Instance.tipInfo.NoTipCardsTip);
                }

            });

            ButtonHelper.RegisterButtonEvent(_rf, "OutBtn", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZCard _data = new DDZCard();
                
                List<byte> outCardList = DDZConfig.GameScene.DDZHandCardPlugin.GetOutCardList();
                
                _data.Card = PbHelper.CopyFrom(outCardList);

                foreach (var item in outCardList)
                {
                    Log.Debug("牌值:" + PokerCardsHelper.GetPokerOfInt(item));

                    Log.Debug("中文牌:" + PokerCardsHelper.GetPokerString(item));
                }

                _data.CardsNum = outCardList.Count;

                Log.Debug("出牌的长度:" + _data.CardsNum);

                //增加判断：出牌是否为空
                if (_data.CardsNum <= 0)
                {
                    DDZConfig.GameScene.DDZMaskPlugin.Show(2);
                }
                else
                {
                    DDZConfig.GameScene.RequestPlayCard(0, _data);
                }
            });

            this.Reset();

            return this;
        }

        /// <summary>
        /// 按钮是否激活 0.不出 1.重选 2.提示 3.出牌
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_interactable"></param>
        public void SetBtnInterActable(int _index, bool _interactable)
        {
            var _btn= this.panel.transform.GetChild(_index).GetComponent<Button>();

            _btn.interactable = _interactable;
        }

        public void Reset()
        {
            this.Num.text = "";

            this.Hide();
        }
        
        public void Show(int CpLifeTime = 0)
        {
            this.panel.SetActive(true);

            this.Clock.SetActive(true);

            int clockTime = CpLifeTime > 0 ? CpLifeTime : DDZGameConfigComponent.Instance.CpLifeTime;

            this.clockComponent.Play(DDZGameConfigComponent.Instance.CpLifeTime,()=> {

                DDZConfig.GameScene.RequestPlayCard(1, new DDZCard());

            });

            //检测手牌中是否有大于上家出牌的牌
            bool _showOutCard = DDZConfig.GameScene.DDZAIComponent.CheckShowOutCard();

            Debug.Log("检则当前是否要得起:" + _showOutCard);

            bool _selfIsFirstOut = DDZConfig.GameScene.DDZOpTipPlugin.SelfIsFirstOut();

            if (!_showOutCard && !_selfIsFirstOut)
            {
                DDZConfig.GameScene.DDZMaskPlugin.Show(1);
            }
        }

        public void Hide()
        {
            this.panel.SetActive(false);

            this.Clock.SetActive(false);

            if (DDZConfig.GameScene.DDZMaskPlugin != null)
            {
                DDZConfig.GameScene.DDZMaskPlugin.Hide();
            }

            this.tipCardIndex = -1;
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
