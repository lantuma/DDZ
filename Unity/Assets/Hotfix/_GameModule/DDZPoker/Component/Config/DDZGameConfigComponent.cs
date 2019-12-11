/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 配置组件 }                                                                                                                   
*         【修改日期】{ 2019年5月30日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class DDZGameConfigComponentAwakeSystem : AwakeSystem<DDZGameConfigComponent>
    {
        public override void Awake(DDZGameConfigComponent self)
        {
            self.Awake();
        }
    }

    public class DDZGameConfigComponent:Component
    {
        public static DDZGameConfigComponent Instance { get; private set; }

        /// <summary>
        /// 抢地主时间(配置)
        /// </summary>
        public int QdzLifeTime { get; set; }

        /// <summary>
        /// 操作时间(配置)
        /// </summary>
        public int CpLifeTime { get; set; }
        /// <summary>
        /// 结算时间（配置）
        /// </summary>
        public int JsLifeTime { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public int Times { get; set; }

        /// <summary>
        /// 底分
        /// </summary>
        public int DiFen { get; set; }

        /// <summary>
        /// 状态状态
        /// </summary>
        public int GameState { get; set; }

        /// <summary>
        /// 当前操作剩余时间
        /// </summary>
        public int LeftTime { get; set; }

        /// <summary>
        /// 保存轮循服务器发的时间
        /// </summary>
        public int ActiveOpTime = 0;
        
        /// <summary>
        /// 上一轮叫的分数
        /// </summary>
        public int PreCallScore = -1;

        /// <summary>
        /// 底牌
        /// </summary>
        public DDZCard DPCard = null;

        /// <summary>
        /// 地主的座位ID
        /// </summary>
        public int LordID = -1;

        /// <summary>
        /// 手牌选中时，上升高度
        /// </summary>
        public int cardMoveUpY = 10;

        /// <summary>
        /// 手牌大于17张
        /// </summary>
        public int handCardSortWidth = -38;

        /// <summary>
        /// 手牌等于小于17张
        /// </summary>
        public int handCardNormalSortWidth = -30;

        /// <summary>
        /// 保存自己的手牌
        /// </summary>
        public DDZCard myHandCard;

        /// <summary>
        /// 上家出的牌
        /// </summary>
        public DDZCard preOutCard;

        private int _ActiveChairId = -1;

        /// <summary>
        /// 当前活动玩家
        /// </summary>
        public int ActiveChairId
        {
            get { return _ActiveChairId; }
            set
            {
                _ActiveChairId = value;

                if (DDZConfig.GameScene != null)
                {
                    DDZConfig.GameScene.DDZPlayerHeadPlugin.SetActivePlayerHead(value);
                }
            }
        }

        public void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="roomData"></param>
        public void SaveConfig(DDZRoomData roomData)
        {
            this.QdzLifeTime = roomData.QdzLifeTime;

            this.CpLifeTime = roomData.CpLifeTime;

            this.JsLifeTime = roomData.JsLifeTime;

            this.Times = roomData.Times;

            this.DiFen = roomData.DiFen;

            this.GameState = roomData.GameState;
        }

        /// <summary>
        /// 重置参数
        /// </summary>
        public void Clear()
        {
            this.GameState = (int)DDZ_GameState.NoStart;
            
            this.ActiveChairId = -1;

            this.PreCallScore = -1;

            this.DPCard = null;

            this.LordID = -1;

            this.myHandCard = null;

            this.preOutCard = null;

            this.ActiveOpTime = 0;
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
