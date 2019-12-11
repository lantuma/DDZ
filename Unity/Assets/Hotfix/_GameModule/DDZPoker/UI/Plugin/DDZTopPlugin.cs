/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 顶部区域插件}                                                                                                                   
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
    public class DDZTopPluginAwakeSystem : AwakeSystem<DDZTopPlugin, GameObject>
    {
        public override void Awake(DDZTopPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class DDZTopPlugin : Component
    {
        private GameObject panel;

        private ReferenceCollector _rf;

        private Text BaseScoreLab;

        private Text BeiSuLab;

        private GameObject DiPai;

        public Dictionary<int, DDZPokerItem> dpDic;

        public DDZTopPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            dpDic = new Dictionary<int, DDZPokerItem>();

            _rf = this.panel.GetComponent<ReferenceCollector>();

            BaseScoreLab = _rf.Get<GameObject>("BaseScoreLab").GetComponent<Text>();

            BeiSuLab = _rf.Get<GameObject>("BeiSuLab").GetComponent<Text>();

            DiPai = _rf.Get<GameObject>("DiPai");
            
            this.Init();

            return this;
        }

        public void Init()
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject _card = _rf.Get<GameObject>("dp" + i);

                DDZPokerItem item = DDZUIFactory.pokerItem.Create(i,this);

                dpDic[i] = item;

                item.GetComponent<DDZPokerItemUIComponent>().SetPanel(_card, i);
            }
        }


        public void Reset()
        {
            //BaseScoreLab.text = "0";

            BeiSuLab.text = "0";

            DiPai.SetActive(false);
        }

        /// <summary>
        /// 显示底牌
        /// </summary>
        /// <param name="diPaiCard"></param>
        public async void ShowDP(DDZCard diPaiCard,bool hasAni = false)
        {
            if (diPaiCard == null) { return; }
            
            DiPai.SetActive(true);

            for (int i = 0; i < diPaiCard.Card.Length; i++)
            {
                var cardValue = diPaiCard.Card[i];

                if (hasAni) await Task.Delay(100);

                this.dpDic[i].GetComponent<DDZPokerItemUIComponent>().SetPokerImageSprite(cardValue);
            }
        }

        /// <summary>
        /// 隐藏底牌
        /// </summary>
        public void HideDP()
        {
            DiPai.SetActive(false);
        }

        /// <summary>
        /// 设置底分
        /// </summary>
        public void SetBaseScore()
        {
            this.BaseScoreLab.text = "底分:" +  DDZGameConfigComponent.Instance.DiFen.ToString();
        }

        /// <summary>
        /// 设置倍数
        /// </summary>
        public void SetBeiSu()
        {
            this.BeiSuLab.text = DDZGameConfigComponent.Instance.Times.ToString();
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
