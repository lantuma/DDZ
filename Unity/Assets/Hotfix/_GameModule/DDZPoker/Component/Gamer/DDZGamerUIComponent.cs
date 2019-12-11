/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 玩家UI组件 }                                                                                                                   
*         【修改日期】{ 2019年4月3日 }                                                                                                                        
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
    public class DDZGamerUIComponentStartSystem : StartSystem<DDZGamerUIComponent>
    {
        public override void Start(DDZGamerUIComponent self)
        {
            self.Start();
        }
    }

    public class DDZGamerUIComponent : Entity
    {
        public GameObject Panel { get; private set; }

        private ReferenceCollector _rf;

        public string NickName { get { return PlayerName.text; } }

        private Image PlayerHead;

        private Image bg;

        private Text PlayerName;

        private Text PlayerGold;

        public GameObject LandlordFlag;

        private GameObject CardNumObj;

        private Text CardNumLab;

        public GameObject Clock;

        private Text Num;

        private GameObject box;

        private GameObject PlayerGoldItem;

        public DDZClockComponent clockComponent;

        public int seatId = 0;

        private UserVO curUseVO = null;

        private Transform nongmin;

        private Transform dizhu;

        //变身特效
        private GameObject ChangeJieSeFx;

        public void Start()
        {

        }

        /// <summary>
        /// 重置头像
        /// </summary>
        public void Reset()
        {
            this.PlayerHead.gameObject.SetActive(false);

            this.PlayerName.text = "";

            this.PlayerGold.text = "";

            this.LandlordFlag.SetActive(false);

            this.CardNumObj.SetActive(false);

            this.CardNumLab.text = "0";

            this.Panel.SetActive(false);
            
            this.Clock.SetActive(false);

            this.box.SetActive(false);

            this.PlayerGoldItem.SetActive(false);
        }

        /// <summary>
        /// 指定头像UI
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="seatId"></param>
        public void SetPanel(GameObject panel, int seatId)
        {
            this.Panel = panel;

            _rf = this.Panel.GetComponent<ReferenceCollector>();

            this.seatId = seatId;

            this.PlayerHead = _rf.Get<GameObject>("PlayerHead").GetComponent<Image>();

            this.nongmin = this.PlayerHead.transform.Find("nongmin");

            this.dizhu = this.PlayerHead.transform.Find("dizhu");

            this.PlayerName = _rf.Get<GameObject>("PlayerName").GetComponent<Text>();

            this.PlayerGold = _rf.Get<GameObject>("PlayerGold").GetComponent<Text>();

            this.LandlordFlag = _rf.Get<GameObject>("LandlordFlag");

            this.CardNumObj = _rf.Get<GameObject>("CardNumObj");

            this.CardNumLab = _rf.Get<GameObject>("CardNumLab").GetComponent<Text>();

            this.PlayerGoldItem = _rf.Get<GameObject>("PlayerGoldItem");

            this.bg = _rf.Get<GameObject>("bg").GetComponent<Image>();

            this.box = _rf.Get<GameObject>("box");

            Clock = _rf.Get<GameObject>("Clock");

            Num = _rf.Get<GameObject>("Num").GetComponent<Text>();

            ChangeJieSeFx = _rf.Get<GameObject>("ChangeJieSeFx");

            Clock.SetActive(false);

            clockComponent = AddComponent<DDZClockComponent>().Awake(Num.gameObject);
        }

        
        /// <summary>
        /// 设置玩家头像
        /// </summary>
        /// <param name="vo"></param>
        public void SetPlayerUI(UserVO vo)
        {
            if (vo == null) {return; }

            this.curUseVO = vo;

            this.PlayerHead.sprite = SpriteHelper.GetSprite("ddzgame",this.GetHeadSpriteNameByIndex()) ;

            this.SetHeadTransformByIndex();

            this.PlayerGoldItem.SetActive(true);

            //由于自己位置给的空间比较大，昵称就尽量显示长一些。
            if (seatId != 0)
            {

                this.PlayerName.text = StringHelper.FormatNickName(vo.nickName);
            }
            else
            {
                this.PlayerName.text = StringHelper.FormatNickName(vo.nickName,20);
            }
            
            this.PlayerGold.text = NumberHelper.FormatMoney(vo.gold);
            
            this.PlayerHead.gameObject.SetActive(true);

            //this.LandlordFlag.SetActive(vo.IsLord == 1);
            
            this.Panel.SetActive(true);

            this.PlayerHead.SetNativeSize();
        }

        public void SetPlayerLordIcon(bool isLord = false)
        {
            this.curUseVO.IsLord = isLord ? 1:0;

            this.PlayerHead.sprite = SpriteHelper.GetSprite("ddzgame", this.GetHeadSpriteNameByIndex());

            this.SetHeadTransformByIndex();

            //播放变身特效

            if(isLord) this.PlayChangeJieSeFx();
        }

        private async void PlayChangeJieSeFx()
        {
            this.ChangeJieSeFx.SetActive(true);

            await Task.Delay(700);

            this.ChangeJieSeFx.SetActive(false);
        }

        //0:Idle 1:win 2.lose 3.think
        public void SetHeadTransformByIndex(int aniIndex = 0)
        {
            //地主
            if (curUseVO.IsLord == 1)
            {
                this.nongmin.gameObject.SetActive(false);

                this.dizhu.gameObject.SetActive(true);

                this.HideChild(dizhu);

                this.dizhu.GetChild(aniIndex).gameObject.SetActive(true);
            }
            else
            {
                this.nongmin.gameObject.SetActive(true);

                this.dizhu.gameObject.SetActive(false);

                this.HideChild(nongmin);

                this.nongmin.GetChild(aniIndex).gameObject.SetActive(true);
            }

        }

        private void HideChild(Transform tra)
        {
            foreach (Transform item in tra)
            {
                item.gameObject.SetActive(false);
            }
        }

        private string GetHeadSpriteNameByIndex()
        {
            if (this.seatId == 0)
            {
                return curUseVO.IsLord == 1 ? "DDZ2_dizhu" : "DDZ2_chupainongmin";
            }
            else if (this.seatId == 1)
            {
                return curUseVO.IsLord == 1 ? "DDZ2_dizhuyou" : "DDZ2_youbiannongmdengdai";
            }
            else if (this.seatId == 2)
            {
                return curUseVO.IsLord == 1 ? "DDZ2_dizhuyingzuo" : "DDZ2_chupainongmin";
            }

            return "";
        }

        /// <summary>
        /// 设置手牌数量
        /// </summary>
        /// <param name="cardNum"></param>
        /// <param name="isShow"></param>
        public void SetCardNum(int cardNum,bool isShow = true)
        {
            this.CardNumObj.SetActive(isShow);

            this.CardNumLab.text = cardNum.ToString();

            if (cardNum == 0) this.CardNumObj.SetActive(false);
        }
        
        /// <summary>
        /// 设置激活玩家背景框
        /// </summary>
        /// <param name="isActive"></param>
        public void SetActivePlayerIcon(bool isActive)
        {
            if (isActive)
            {
                this.box.SetActive(true);

                this.Clock.SetActive(true);
                
                int clockTime = DDZGameConfigComponent.Instance.ActiveOpTime > 0 ? 

                DDZGameConfigComponent.Instance.ActiveOpTime : DDZGameConfigComponent.Instance.CpLifeTime;

                this.clockComponent.Play(clockTime);

                //切换到思考模式

                this.SetHeadTransformByIndex(3);
            }
            else
            {
                this.box.SetActive(false);

                this.Clock.SetActive(false);

                //切换到待机模式

                this.SetHeadTransformByIndex(0);

            }
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
