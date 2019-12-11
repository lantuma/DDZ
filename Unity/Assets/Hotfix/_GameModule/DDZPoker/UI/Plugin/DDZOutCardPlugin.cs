/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 出牌区域插件}                                                                                                                   
*         【修改日期】{ 2019年5月28日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class DDZOutCardPluginAwakeSystem : AwakeSystem<DDZOutCardPlugin, GameObject>
    {
        public override void Awake(DDZOutCardPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class DDZOutCardPlugin : Component
    {
        private GameObject panel;

        private ReferenceCollector _rf;

        private GameObject OutCard0;

        private GameObject OutCard1;

        private GameObject OutCard1_1;

        private GameObject OutCard2;

        private GameObject OutCard2_1;

        private GameObject DDZOutCard;

        private Dictionary<int, Dictionary<int, DDZPokerItem>> outCardDic;


        public DDZOutCardPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            outCardDic = new Dictionary<int, Dictionary<int, DDZPokerItem>>();

            _rf = this.panel.GetComponent<ReferenceCollector>();

            OutCard0 = _rf.Get<GameObject>("OutCard0");

            OutCard1 = _rf.Get<GameObject>("OutCard1");

            OutCard1_1 = _rf.Get<GameObject>("OutCard1_1");

            OutCard2 = _rf.Get<GameObject>("OutCard2");

            OutCard2_1 = _rf.Get<GameObject>("OutCard2_1");

            DDZOutCard = _rf.Get<GameObject>("DDZOutCard");

            this.Init();

            return this;
        }

        public void Reset()
        {
            //隐藏所有的牌
            for (int i = 0; i < 3; i++)
            {
                this.ClearByIndex(i);
            }
        }

        private void Init()
        {
            //生成自己出牌区域的牌

            Dictionary<int, DDZPokerItem> p0 = new Dictionary<int, DDZPokerItem>();

            for (int i = 0; i < 20; i++)
            {
                GameObject _card = UnityEngine.Object.Instantiate(DDZOutCard, this.OutCard0.transform);

                _card.SetActive(false);

                _card.transform.localScale = Vector3.one * 1f;

                DDZPokerItem item = DDZUIFactory.pokerItem.Create(i, this);

                p0[i] = item;

                item.GetComponent<DDZPokerItemUIComponent>().SetPanel(_card, i);
            }

            outCardDic[0] = p0;
            
            //生成另外两家的出牌区域的牌

            Dictionary<int, DDZPokerItem> p1 = new Dictionary<int, DDZPokerItem>();

            Dictionary<int, DDZPokerItem> p2 = new Dictionary<int, DDZPokerItem>();

            this.GenerateOther(p1, OutCard1, OutCard1_1);

            this.GenerateOther(p2, OutCard2, OutCard2_1);

            outCardDic[1] = p1;

            outCardDic[2] = p2;

        }

        private void GenerateOther(Dictionary<int, DDZPokerItem> p1, GameObject out1,GameObject out1_1)
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject parentObj = i >= 10 ? out1_1 : out1;

                GameObject _card = UnityEngine.Object.Instantiate(DDZOutCard, parentObj.transform);

                _card.SetActive(false);

                _card.transform.localScale = Vector3.one * 1f;

                DDZPokerItem item = DDZUIFactory.pokerItem.Create(i, this);

                p1[i] = item;

                item.GetComponent<DDZPokerItemUIComponent>().SetPanel(_card, i);

            }
        }

        /// <summary>
        /// 将出的牌绘制出来
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="_card"></param>
        public void PlayCardBySeatID(int seatID, DDZCard _card)
        {
            if (_card.Card == null) { return; }

            var curOutList =  this.outCardDic[seatID];

            this.ClearOutList(curOutList);
            
            for (int i = 0; i < _card.Card.Length; i++)
            {
               var cardValue = _card.Card[i];

                DDZPokerItem item = curOutList[i];

                int lordID = DDZGameHelper.ChangeSeat(DDZGameConfigComponent.Instance.LordID);

                item.GetComponent<DDZPokerItemUIComponent>().SetPokerImageSprite(cardValue,lordID == seatID);
            }

        }

        /// <summary>
        /// 结束，摊牌
        /// </summary>
        /// <param name="list"></param>
        public void ShowHand(List<DDZCard> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                DDZCard cardData = list[i];

                var vo = DataCenterComponent.Instance.userInfo.getUserByUserID(cardData.UserId);

                if (vo != null && cardData != null)
                {
                    int realSeatID = DDZGameHelper.ChangeSeat(vo.seatID);

                    this.PlayCardBySeatID(realSeatID, cardData);

                    //重回时，赋值上一手的出牌.临时
                    if (realSeatID == 2 && cardData != null)
                    {
                        DDZGameConfigComponent.Instance.preOutCard = cardData;
                    }
                }
                
            }
        }

        /// <summary>
        /// 将指定列表中的牌隐藏掉
        /// </summary>
        /// <param name="list"></param>
        private void ClearOutList(Dictionary<int, DDZPokerItem> list)
        {
            foreach (var item in list)
            {
                item.Value.GetComponent<DDZPokerItemUIComponent>().Hide();
            }
        }

        /// <summary>
        /// 清除指定位置的牌
        /// </summary>
        /// <param name="index"></param>
        public void ClearByIndex(int index)
        {
            this.ClearOutList(this.outCardDic[index]);
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
