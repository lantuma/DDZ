/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 玩家头像插件}                                                                                                                   
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
    public class DDZPlayerHeadPluginAwakeSystem : AwakeSystem<DDZPlayerHeadPlugin, GameObject>
    {
        public override void Awake(DDZPlayerHeadPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class DDZPlayerHeadPlugin : Component
    {
        private GameObject panel;

        private ReferenceCollector _rf;

        public Dictionary<int, GameObject> GamersPanel;

        public Dictionary<int, DDZGamer> GamersDic;
        

        public DDZPlayerHeadPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            _rf = this.panel.GetComponent<ReferenceCollector>();

            this.GamersDic = new Dictionary<int, DDZGamer>();

            GamersPanel = new Dictionary<int, GameObject>();

            for (int i = 0; i < 3; i++)
            {
                this.GamersPanel[i] = _rf.Get<GameObject>("PlayerPosition" + i);

                this.GamersPanel[i].SetActive(false);
            }
            
            for (int i = 0; i < 3; i++)
            {
                DDZGamer gamer = DDZUIFactory.gamer.Create(0, i,this);

                GamersDic[i] = gamer;

                gamer.GetComponent<DDZGamerUIComponent>().SetPanel(GamersPanel[i], i);
            }
            
            return this;
        }

        /// <summary>
        /// 设置自己的UI
        /// </summary>
        public void SetMyUserVO()
        {
            var userVO = DataCenterComponent.Instance.userInfo.getMyUserVo();

            DDZGamer myGamer = GamersDic[0];

            myGamer.GetComponent<DDZGamerUIComponent>().SetPlayerUI(userVO);
        }

        /// <summary>
        /// 清除座位玩家UI
        /// </summary>
        public void ClearOnSeatPlayer()
        {
            for (int i = 0; i < 3; i++)
            {
                DDZGamer gamer = GamersDic[i];

                gamer.GetComponent<DDZGamerUIComponent>().Reset();
            }
        }

        /// <summary>
        /// 座位玩家UI显示
        /// </summary>
        public void SetOnSeatPlayer()
        {
            this.ClearOnSeatPlayer();

            var _list = DataCenterComponent.Instance.userInfo.getOnSeatUsers();

            for (int i = 0; i < _list.Count; i++)
            {
                var userVo = _list[i];

                int realSeatId = DDZGameHelper.ChangeSeat(userVo.seatID);

                if (realSeatId < 3)
                {
                    DDZGamer gamer = GamersDic[realSeatId];

                    gamer.GetComponent<DDZGamerUIComponent>().SetPlayerUI(userVo);
                }
            }
        }

        /// <summary>
        /// 设置玩家的手牌数目
        /// </summary>
        /// <param name="seatId"></param>
        /// <param name="cardNum"></param>
        public void SetCardNum(int seatId,int cardNum)
        {
            int realSeatId = DDZGameHelper.ChangeSeat(seatId);

            DDZGamer gamer = GamersDic[realSeatId];

            gamer.GetComponent<DDZGamerUIComponent>().SetCardNum(cardNum);
        }

        /// <summary>
        /// 显示地主标识
        /// </summary>
        /// <param name="seatId"></param>
        public void SetLordHeadIcon(int seatId)
        {
            this.ClearAllLordHeadIcon();

            int realSeatID = DDZGameHelper.ChangeSeat(seatId);

            DDZGamer gamer = GamersDic[realSeatID];

            //gamer.GetComponent<DDZGamerUIComponent>().LandlordFlag.SetActive(true);

            gamer.GetComponent<DDZGamerUIComponent>().SetPlayerLordIcon(true);
        }

        /// <summary>
        /// 显示玩家高亮效果
        /// </summary>
        /// <param name="seatId"></param>
        public void SetActivePlayerHead(int seatId)
        {
            this.ClearAllActivePlayerIcon();
            
            int realSeatID = DDZGameHelper.ChangeSeat(seatId);

            if (realSeatID > -1)
            {

                DDZGamer gamer = GamersDic[realSeatID];

                gamer.GetComponent<DDZGamerUIComponent>().SetActivePlayerIcon(true);
            }
        }
        /// <summary>
        /// 清除指定的时钟
        /// </summary>
        /// <param name="seatId"></param>
        public void ClearClockByID(int seatId)
        {
            if (seatId > -1)
            {
                DDZGamer gamer = GamersDic[seatId];

                gamer.GetComponent<DDZGamerUIComponent>().Clock.SetActive(false);
            }
        }

        /// <summary>
        /// 清除所有地主头像
        /// </summary>
        public void ClearAllLordHeadIcon()
        {
            for (int i = 0; i < 3; i++)
            {
                DDZGamer gamer = GamersDic[i];

                gamer.GetComponent<DDZGamerUIComponent>().LandlordFlag.SetActive(false);

                gamer.GetComponent<DDZGamerUIComponent>().SetPlayerLordIcon(false);
            }
        }

        /// <summary>
        /// 清除所有玩家高亮效果
        /// </summary>
        public void ClearAllActivePlayerIcon()
        {
            for (int i = 0; i < 3; i++)
            {
                DDZGamer gamer = GamersDic[i];

                gamer.GetComponent<DDZGamerUIComponent>().SetActivePlayerIcon(false);
            }
        }
        
        public void Reset()
        {
            /*
            foreach (var item in GamersDic)
            {
                item.Value.Dispose();
            }

            GamersDic.Clear();
            */
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
