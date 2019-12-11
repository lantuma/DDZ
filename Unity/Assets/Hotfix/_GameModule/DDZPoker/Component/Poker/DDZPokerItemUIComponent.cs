/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 扑克UI组件 }                                                                                                                   
*         【修改日期】{ 2019年4月9日 }                                                                                                                        
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
    public class DDZPokerItemUIComponentStartSystem : StartSystem<DDZPokerItemUIComponent>
    {
        public override void Start(DDZPokerItemUIComponent self)
        {
            self.Start();
        }
    }

    public class DDZPokerItemUIComponent : Component
    {
        public GameObject Panel { get; private set; }

        /// <summary>
        /// 牌值
        /// </summary>
        private Image _poker;

        /// <summary>
        /// 地主牌标识
        /// </summary>
        private GameObject _dizhu;
        
        /// <summary>
        /// 第几张
        /// </summary>
        private int index = 0;

        /// <summary>
        /// 牌值（byte 类型)
        /// </summary>
        public byte pokerId;

        /// <summary>
        /// 牌值(Int类型)
        /// </summary>
        public int pokerInt;

        /// <summary>
        /// 牌值Value
        /// </summary>
        public int pokerValue;

        /// <summary>
        /// 是否是地主牌
        /// </summary>
        public bool isDiZhu = false;

        /// <summary>
        /// 是否可以点击
        /// </summary>
        private bool canClick = false;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool isUp = false;
        
        public void Start()
        {

        }

        public void Reset()
        {
            this.isUp = false;

            this.canClick = false;

            this.isDiZhu = false;

            this._dizhu.SetActive(false);
        }
        
        public void SetPanel(GameObject panel,int index,bool canClick = false)
        {
            this.Panel = panel;

            this.index = index;

            this.isUp = false;
            
            this._poker = this.Panel.transform.Find("Image").GetComponent<Image>();

            this._dizhu = this._poker.transform.Find("dizhu").gameObject;

            if (canClick) this.Panel.GetComponent<Button>().onClick.AddListener(OnClickCard);
        }

        public void OnClickCard()
        {
            if (!isUp)
            {
                this.OnUp();
            }
            else
            {
                this.OnDown();
            }
        }

        public void OnUp()
        {
            this._poker.transform.localPosition = new Vector3(0,DDZGameConfigComponent.Instance.cardMoveUpY,0);

            this.isUp = true;

            DDZConfig.GameScene.DDZHandCardPlugin.outCardList.Add(this.pokerId);

            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.DDZ_selectcard);
        }

        public void OnDown()
        {
            this._poker.transform.localPosition = Vector3.zero;

            this.isUp = false;

            if (DDZConfig.GameScene.DDZHandCardPlugin.outCardList.Contains(this.pokerId))
            {
                DDZConfig.GameScene.DDZHandCardPlugin.outCardList.Remove(this.pokerId);
            }
        }
        
        /// <summary>
        /// 设置扑克牌
        /// </summary>
        /// <param name="poker"></param>
        public void SetPokerImageSprite(byte pokerId,bool isDiZhu=false)
        {
            this.Panel.SetActive(true);

            this.pokerId = pokerId;

            this.isDiZhu = isDiZhu;

            this.pokerInt = PokerCardsHelper.GetPokerOfInt(pokerId);

            this.pokerValue = DDZGameHelper.GetPokerNum(pokerId);

            _poker.sprite = SpriteHelper.GetPokerSprite(PokerCardsHelper.GetPokerOfInt(pokerId));

            this._dizhu.SetActive(isDiZhu);

        }

        /// <summary>
        /// 显示牌背
        /// </summary>
        public void SetPaiBei()
        {
            this.Panel.SetActive(true);

            _poker.sprite = SpriteHelper.GetPokerSprite(0, true);

            this._dizhu.SetActive(false);
        }

        /// <summary>
        /// 隐藏牌
        /// </summary>
        public void Hide()
        {
            this.Panel.SetActive(false);
        }

        /// <summary>
        /// 显示牌
        /// </summary>
        public void Show()
        {
            this.Panel.SetActive(true);
        }

        /// <summary>
        /// 改变灰度
        /// </summary>
        public void setGray()
        {
            Color grayColor = new Color(0.5f, 0.5f, 0.5f, 1f);

            this._poker.color = grayColor;

            this._dizhu.GetComponent<Image>().color = grayColor;
        }

        /// <summary>
        /// 设置恢度
        /// </summary>
        public void resumeGray()
        {
            Color normalColor = new Color(1f, 1f, 1f, 1f);

            this._poker.color = normalColor;

           this._dizhu.GetComponent<Image>().color = normalColor;
        }


        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            Reset();
        }
    }
}
