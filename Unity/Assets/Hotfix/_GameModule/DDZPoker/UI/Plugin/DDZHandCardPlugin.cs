/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 手牌插件}                                                                                                                   
*         【修改日期】{ 2019年5月28日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class DDZHandCardPluginAwakeSystem : AwakeSystem<DDZHandCardPlugin, GameObject>
    {
        public override void Awake(DDZHandCardPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class DDZHandCardPlugin : Entity
    {
        private GameObject panel;

        private ReferenceCollector _rf;

        public Dictionary<int, DDZPokerItem> handCardDic;
        
        private GameObject DDZHandCard;

        private HorizontalLayoutGroup horizontalLayoutGroup;

        public List<byte> outCardList;

        public DDZDragSelectionComponent DragSelectionComponent;

        public DDZHandCardPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            outCardList = new List<byte>();

            handCardDic = new Dictionary<int, DDZPokerItem>();
            
            _rf = this.panel.GetComponent<ReferenceCollector>();

            horizontalLayoutGroup = this.panel.GetComponent<HorizontalLayoutGroup>();

            DDZHandCard = _rf.Get<GameObject>("DDZHandCard");

            DragSelectionComponent = AddComponent<DDZDragSelectionComponent>().Awake(this.panel);
            
            this.Init();

            return this;
        }

        

        public void Init()
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject _card = UnityEngine.Object.Instantiate(DDZHandCard,this.panel.transform);

                _card.SetActive(false);

                _card.name = i.ToString();

                _card.transform.localScale = Vector3.one * 1.81f;

                DDZPokerItem item = DDZUIFactory.pokerItem.Create(i,this);

                handCardDic[i] = item;

                item.GetComponent<DDZPokerItemUIComponent>().SetPanel(_card, i,true);
            }
        }

        /// <summary>
        /// 重选
        /// </summary>
        public void ReSelect()
        {
            foreach (var item in handCardDic)
            item.Value.GetComponent<DDZPokerItemUIComponent>().OnDown();
            
            outCardList.Clear();
        }

        /// <summary>
        /// 显示手牌
        /// </summary>
        /// <param name="handCard"></param>
        public async void ShowHandCard(DDZCard handCard,bool hasAni = false)
        {
            if (handCard == null) { return; }

            this.ReSelect();

            this.HideHandCard();

            if (hasAni) SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.DDZ_sound_dealcard);
            
            horizontalLayoutGroup.spacing = handCard.Card.Length > 17 ? DDZGameConfigComponent.Instance.handCardSortWidth : DDZGameConfigComponent.Instance.handCardNormalSortWidth;

            for (int i = 0; i < handCard.Card.Length; i++)
            {
                var cardValue = handCard.Card[i];

                DDZPokerItem item = handCardDic[i];

                if (hasAni)
                {
                    await Task.Delay(100);
                }

                int realId = DDZGameHelper.ChangeSeat(DDZGameConfigComponent.Instance.LordID);

                item.GetComponent<DDZPokerItemUIComponent>().SetPokerImageSprite(cardValue, realId == 0);
            }
        }

        /// <summary>
        /// 从手牌移除掉牌
        /// </summary>
        public void RemoveCard(DDZCard ddzCard)
        {
            for (int i = 0; i < ddzCard.Card.Length; i++)
            {
                var cardValue = ddzCard.Card[i];

                foreach (var item in handCardDic)
                {
                    var ui = item.Value.GetComponent<DDZPokerItemUIComponent>();

                    if (ui.pokerId == cardValue && ui.Panel.activeSelf)
                    {
                        ui.Hide();
                    }
                }
            }

            this.ReSelect();
        }

        /// <summary>
        /// 从手牌移除掉牌2(临时)
        /// </summary>
        public void RemoveCard2()
        {
            for (int i = 0; i < outCardList.Count; i++)
            {
                var cardValue = outCardList[i];

                foreach (var item in handCardDic)
                {
                    var ui = item.Value.GetComponent<DDZPokerItemUIComponent>();

                    if (ui.pokerId == cardValue && ui.Panel.activeSelf)
                    {
                        ui.Hide();
                    }
                }
            }

            this.ReSelect();
        }

        /// <summary>
        /// 选择提示的牌
        /// </summary>
        /// <param name="cards"></param>
        public void SelectTipCard(List<int> cards)
        {
            this.ReSelect();

            for (int i = 0; i < cards.Count; i++)
            {
                var cardIntValue = cards[i];

                foreach (var item in handCardDic)
                {
                    var ui = item.Value.GetComponent<DDZPokerItemUIComponent>();

                    if (ui.pokerValue == cardIntValue && ui.Panel.activeSelf && !ui.isUp)
                    {
                        ui.OnUp();

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 通过牌是否选中来获取出牌列表
        /// </summary>
        /// <returns></returns>
        public List<byte> GetOutCardList()
        {
            List<byte> outCardList = new List<byte>();

            foreach (var item in handCardDic)
            {
                var ui = item.Value.GetComponent<DDZPokerItemUIComponent>();

                if (ui.Panel.activeSelf && ui.isUp)
                {
                    outCardList.Add(ui.pokerId);
                }
            }

            return outCardList;
        }

        /// <summary>
        /// 隐藏所有手牌
        /// </summary>
        public void HideHandCard()
        {
            foreach (var item in handCardDic)
            {
                item.Value.GetComponent<DDZPokerItemUIComponent>().Hide();
            }
        }

        /// <summary>
        /// 通过Index获得组件
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DDZPokerItemUIComponent GetPokerComponentByIndex(int index)
        {
            DDZPokerItemUIComponent itemUI = null;

            foreach (var item in handCardDic)
            {
                var uc = item.Value.GetComponent<DDZPokerItemUIComponent>();

                if (item.Value.index == index)
                {
                    itemUI = uc;

                    break;
                }
            }

            return itemUI;
        }

        public void Reset(bool destroy = false)
        {
            this.HideHandCard();

            this.outCardList.Clear();

        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.Reset(true);
        }

    }
}
