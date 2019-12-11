/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 游戏场景}                                                                                                                   
*         【修改日期】{ 2019年4月2日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIDDZGameSceneAwakeSystem : AwakeSystem<UIDDZGameScene>
    {
        public override void Awake(UIDDZGameScene self)
        {
            self.Awake();
        }

    }

    public class UIDDZGameScene : Entity
    {
        /// <summary>
        /// 脚注插件
        /// </summary>
        public DDZFootPlugin DDZFootPlugin;

        /// <summary>
        /// 左上插件
        /// </summary>
        public DDZLTPlugin DDZLTPlugin;

        /// <summary>
        /// 右上插件
        /// </summary>
        public DDZTRBtnPlugin DDZTRBtnPlugin;

        /// <summary>
        /// 顶部插件
        /// </summary>
        public DDZTopPlugin DDZTopPlugin;

        /// <summary>
        /// 头像插件
        /// </summary>
        public DDZPlayerHeadPlugin DDZPlayerHeadPlugin;

        /// <summary>
        /// 准备插件
        /// </summary>
        public DDZReadyPlugin DDZReadyPlugin;

        /// <summary>
        /// 叫分插件
        /// </summary>
        public DDZCallScorePlugin DDZCallScorePlugin;

        /// <summary>
        /// 操作提示插件
        /// </summary>
        public DDZOpTipPlugin DDZOpTipPlugin;

        /// <summary>
        /// 交互插件
        /// </summary>
        public DDZInteractivePlugin DDZInteractivePlugin;

        /// <summary>
        /// MASK插件
        /// </summary>
        public DDZMaskPlugin DDZMaskPlugin;

        /// <summary>
        /// 出牌区域插件
        /// </summary>
        public DDZOutCardPlugin DDZOutCardPlugin;

        /// <summary>
        /// 手牌插件
        /// </summary>
        public DDZHandCardPlugin DDZHandCardPlugin;

        /// <summary>
        /// 下拉菜单插件
        /// </summary>
        public DDZDownPullPlugin DDZDownPullPlugin;

        /// <summary>
        /// 特效层插件
        /// </summary>
        public DDZFXLayerPlugin DDZFXLayerPlugin;

        /// <summary>
        /// AI组件
        /// </summary>
        public DDZAIComponent DDZAIComponent;

        /// <summary>
        /// 玩家列表面板缓存
        /// </summary>
        public UIGamePlayerListsPanel PlayerListsPanel;

        private ReferenceCollector _ref;
        
        private SoundInfo soundInfo = DataCenterComponent.Instance.soundInfo;

        private bool _BackHall;

        /// <summary>
        /// 还剩1张牌语音是否播放过
        /// </summary>
        private Dictionary<int, bool> hadPlayOneCardSound = new Dictionary<int, bool> { { 0, false }, { 1, false }, { 2, false } };

        /// <summary>
        /// 还剩2张牌语音是否播放过
        /// </summary>
        private Dictionary<int, bool> hadPlayTwoCardSound = new Dictionary<int, bool> { { 0, false }, { 1, false }, { 2, false } };

        private string CurrentSoundName = "";

        public void Awake()
        {
            _ref = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            DDZConfig.GameScene = this;

            InitData();

            AddComponent<DDZGameConfigComponent>();

            DDZAIComponent = AddComponent<DDZAIComponent>();
            
            DDZFootPlugin = AddComponent<DDZFootPlugin>().Awake(_ref.Get<GameObject>("DDZFootPlugin"));

            DDZLTPlugin = AddComponent<DDZLTPlugin>().Awake(_ref.Get<GameObject>("DDZLTPlugin"));

            DDZTRBtnPlugin = AddComponent<DDZTRBtnPlugin>().Awake(_ref.Get<GameObject>("DDZTRBtnPlugin"));

            DDZTopPlugin = AddComponent<DDZTopPlugin>().Awake(_ref.Get<GameObject>("DDZTopPlugin"));

            DDZPlayerHeadPlugin = AddComponent<DDZPlayerHeadPlugin>().Awake(_ref.Get<GameObject>("DDZPlayerHeadPlugin"));

            DDZReadyPlugin = AddComponent<DDZReadyPlugin>().Awake(_ref.Get<GameObject>("DDZReadyPlugin"));

            DDZCallScorePlugin = AddComponent<DDZCallScorePlugin>().Awake(_ref.Get<GameObject>("DDZCallScorePlugin"));

            DDZOpTipPlugin = AddComponent<DDZOpTipPlugin>().Awake(_ref.Get<GameObject>("DDZOpTipPlugin"));

            DDZInteractivePlugin = AddComponent<DDZInteractivePlugin>().Awake(_ref.Get<GameObject>("DDZInteractivePlugin"));

            DDZMaskPlugin = AddComponent<DDZMaskPlugin>().Awake(_ref.Get<GameObject>("DDZMaskPlugin"));

            DDZOutCardPlugin = AddComponent<DDZOutCardPlugin>().Awake(_ref.Get<GameObject>("DDZOutCardPlugin"));

            DDZHandCardPlugin = AddComponent<DDZHandCardPlugin>().Awake(_ref.Get<GameObject>("DDZHandCardPlugin"));

            DDZDownPullPlugin = AddComponent<DDZDownPullPlugin>().Awake(_ref.Get<GameObject>("DDZDownPullPlugin"));

            DDZFXLayerPlugin = AddComponent<DDZFXLayerPlugin>().Awake(_ref.Get<GameObject>("DDZFXLayerPlugin"));

            this.Init();
            
        }

        public async void Init()
        {
            DDZGameHelper.IsGetRoomDetails = false;

            DDZGameHelper.IsJoinRoom = true;

            Game.PopupComponent.ShowNoticeUI(DataCenterComponent.Instance.MarqueeInfo.SubGameDefault);

            //进入存储重回数据
            this.SaveReBackData();
 
            //请求房间详情
            var resp = (G2C_GetRoomInfo_Res)await SessionComponent.Instance.Session.Call(
                   new C2G_GetRoomInfo_Req()
                   {
                       GameId = DDZGameHelper.CurrentGameInfo.GameId,
                       AreaId = GameHelper.AreaList[DDZGameHelper.CurrentGameInfo.GameId][DDZGameHelper.CurrentFieldId].AreaId,
                       UserId = GamePrefs.GetUserId(),
                       RoomId = DDZGameHelper.RoomId
                   });
            On_G2C_GetRoomInfo_Res(resp);
        }

        private void InitData()
        {
            //初始化自己的信息
            UserVO myVo = new UserVO()
            {
                userID = GamePrefs.GetUserId(),

                headId = UserDataHelper.UserInfo.HeadId,

                sex = UserDataHelper.UserInfo.Gender,

                gold = UserDataHelper.UserInfo.Gold,

                nickName = UserDataHelper.UserInfo.Nickname,

                seatID = 0
            };

            DataCenterComponent.Instance.userInfo.httpUserInfo = myVo;

            DataCenterComponent.Instance.userInfo.addUser(myVo);

            //播放背景音乐

            SoundComponent.Instance.Stop(DataCenterComponent.Instance.soundInfo.bg_hall);
            
            var _soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1);

            this.CurrentSoundName = DataCenterComponent.Instance.soundInfo.ddzbg + UnityEngine.Random.Range(1, 3);

            SoundComponent.Instance.PlayMusic(this.CurrentSoundName, 0, _soundVolume, true);
        }

        /// <summary>
        /// 房间详情返回
        /// </summary>
        /// <param name="resp"></param>
        private void On_G2C_GetRoomInfo_Res(G2C_GetRoomInfo_Res resp)
        {
            DDZGameHelper.IsGetRoomDetails = true;

            DDZRoomData roomData = resp.RoomData.DdzRoomData;

            //将游戏配置参数保存
            DDZGameConfigComponent.Instance.SaveConfig(roomData);

            //保存倒计时
            DDZGameConfigComponent.Instance.LeftTime = resp.RoomData.LeftTime;

            //保存自己的信息,并刷新一次

            DataCenterComponent.Instance.userInfo.deleteAllUserExcptMe();

            var myVo = DataCenterComponent.Instance.userInfo.getMyUserVo();

            for (int i = 0; i < roomData.PlayerData.count; i++)
            {
                var playerData = roomData.PlayerData[i];

                // 保存谁是地主
                if (playerData.IsLord) { DDZGameConfigComponent.Instance.LordID = playerData.ChairId; }

                if (!DataCenterComponent.Instance.userInfo.isExist(playerData.UserId))
                {
                    UserVO vo = new UserVO
                    {
                        userID = playerData.UserId,

                        headId = playerData.HeadId,

                        sex = playerData.Gender,

                        seatID = playerData.ChairId,

                        gold = playerData.Gold,

                        nickName = playerData.NickeName,

                        IsLord = playerData.IsLord ? 1 : 0,

                        IsReady = playerData.IsPrepare ? 1:0,

                        point = playerData.QdzJiaoFen //-1 没抢 1,2,3 0:不叫
                    };

                    DataCenterComponent.Instance.userInfo.addUser(vo);
                }
                else
                {
                    var vo = DataCenterComponent.Instance.userInfo.getUserByUserID(playerData.UserId);

                    vo.seatID = playerData.ChairId;

                    vo.IsLord = playerData.IsLord ? 1 : 0;

                    vo.gold = playerData.Gold;

                    vo.IsReady = playerData.IsPrepare?1:0;

                    vo.point = playerData.QdzJiaoFen;
                }
            }
            
            //显示座位玩家
            DDZPlayerHeadPlugin.SetOnSeatPlayer();

            //刷新倍数，底注

            this.DDZTopPlugin.SetBeiSu();

            this.DDZTopPlugin.SetBaseScore();
            
            //刷新底牌
            this.DDZTopPlugin.ShowDP(roomData.DpCards);

            //显示各个玩家的手牌数量

            if (roomData.SurCardsNum != null)
            {
                for (int i = 0; i < roomData.SurCardsNum.count; i++)
                {
                    int cardNum = roomData.SurCardsNum[i];

                    this.DDZPlayerHeadPlugin.SetCardNum(i, cardNum);
                }
            }
            
            //绘制玩家手牌
            if (roomData.Card != null)
            {
                this.DDZHandCardPlugin.ShowHandCard(roomData.Card);

                //刷新最新的手牌
                DDZGameConfigComponent.Instance.myHandCard = roomData.Card;
            }

            //绘制各玩家上一手的出牌

            if (roomData.PlayLastCircleCards != null)
            {
                var playLastCircleCards = roomData.PlayLastCircleCards.ToList();

                this.DDZOutCardPlugin.ShowHand(playLastCircleCards);
            }

            #region 处理各阶段状态

            DDZGameHelper.IsStartGame = (roomData.GameState == 0) ? false : true;

            //判断当前的游戏状态
            if (roomData.GameState == (int)DDZ_GameState.NoStart)
            {
                Log.Debug("当前游戏状态：未开局");

                this.CheckReadyState();
            }
            else if (roomData.GameState == (int)DDZ_GameState.Ready)
            {
                Log.Debug("当前游戏状态：准备");

                this.CheckReadyState();
            }
            else if (roomData.GameState == (int)DDZ_GameState.FaPai)
            {
                Log.Debug("当前游戏状态：发牌");
                
            }
            else if (roomData.GameState == (int)DDZ_GameState.CallScore)
            {
                Log.Debug("当前游戏状态：叫地主");

                //检测之前的叫分状态
                this.CheckCallScoreState();

                //采用服务器的倒计时
                DDZGameConfigComponent.Instance.ActiveOpTime = resp.RoomData.LeftTime;

                DDZGameConfigComponent.Instance.ActiveChairId = roomData.ActiveChairId;

                int realID = DDZGameHelper.ChangeSeat(roomData.ActiveChairId);

                if (realID == 0)
                {
                    this.DDZCallScorePlugin.Show(resp.RoomData.LeftTime);
                }

            }
            else if (roomData.GameState == (int)DDZ_GameState.DaPai)
            {
                Log.Debug("当前游戏状态:打牌");

                //采用服务器的倒计时
                DDZGameConfigComponent.Instance.ActiveOpTime = resp.RoomData.LeftTime;

                //判断当前活动玩家是否是自己，如果是：弹出抢分.
                
                DDZGameConfigComponent.Instance.ActiveChairId = roomData.ActiveChairId;

                int realID = DDZGameHelper.ChangeSeat(roomData.ActiveChairId);

                if (realID == 0)
                {
                    this.DDZInteractivePlugin.Show(resp.RoomData.LeftTime);
                }

            }
            else if (roomData.GameState == (int)DDZ_GameState.JieSuan)
            {
                Log.Debug("当前游戏状态：结算");
            }
            #endregion
        }

        /// <summary>
        /// 检测准备状态
        /// </summary>
        /// <param name="roomData"></param>
        private void CheckReadyState()
        {
            bool needShowWaitTip = false;

            for (int i = 0; i < 3; i++)
            {
                var vo = DataCenterComponent.Instance.userInfo.getUserBySeatID(i);

                if (vo != null)
                {
                    //判断当前玩家是否准备

                    int realSeatID = DDZGameHelper.ChangeSeat(vo.seatID);

                    bool isReady = vo.IsReady == 1 ? true : false;

                    this.DDZReadyPlugin.SetReadyByIndex(realSeatID, isReady);

                    if (realSeatID == 0 && !isReady)
                    {
                        this.DDZReadyPlugin.Show();
                    }
                }

                if (vo == null) needShowWaitTip = true;
            }

            this.DDZReadyPlugin.SetEnterTipState(needShowWaitTip);
        }

        /// <summary>
        /// 检测叫分状态
        /// </summary>
        /// <param name="roomData"></param>
        private void CheckCallScoreState()
        {
            for (int i = 0; i < 3; i++)
            {
                var vo = DataCenterComponent.Instance.userInfo.getUserBySeatID(i);

                if (vo != null)
                {
                    //判断当前玩家是否准备

                    int realSeatID = DDZGameHelper.ChangeSeat(vo.seatID);

                    int callScore = vo.point;

                    if (callScore > -1)
                    {
                        this.DDZOpTipPlugin.CallScoreById(realSeatID, callScore);
                    }

                    if (realSeatID == 0 )
                    {
                        DDZGameConfigComponent.Instance.PreCallScore = callScore;
                    }
                }
            }
        }

        /// <summary>
        /// 请求准备
        /// </summary>
        public async void RequestPrepare(bool _IsPrepare = true)
        {
            var resp = (G2C_DDZPrepare_Res)await SessionComponent.Instance.Session.Call(
                   new C2G_DDZPrepare_Req()
                   {
                       GameId = DDZGameHelper.CurrentGameInfo.GameId,
                       AreaId = GameHelper.AreaList[DDZGameHelper.CurrentGameInfo.GameId][0].AreaId,
                       UserId = GamePrefs.GetUserId(),
                       RoomId = DDZGameHelper.RoomId,
                       IsPrepare = _IsPrepare
                   });
            if (resp.Error != 0)
            {
                Game.PopupComponent.ShowMessageBox(resp.Message);
                return;
            }

            //将准备面板销毁

            this.DDZReadyPlugin.Reset();

            //将准备标识显示出来

            this.DDZReadyPlugin.SetReadyByIndex(0, true);

        }

        /// <summary>
        /// 请求叫地主
        /// </summary>
        /// <param name="_Score">叫分 0-不抢</param>
        public async void RequestAskScore(int _Score = 0)
        {
            var resp = (G2C_DDZAskScore_Res)await SessionComponent.Instance.Session.Call(
                   new C2G_DDZAskScore_Req()
                   {
                       GameId = DDZGameHelper.CurrentGameInfo.GameId,
                       AreaId = GameHelper.AreaList[DDZGameHelper.CurrentGameInfo.GameId][0].AreaId,
                       UserId = GamePrefs.GetUserId(),
                       RoomId = DDZGameHelper.RoomId,
                       Score = _Score
                   });
            if (resp.Error != 0)
            {
                Game.PopupComponent.ShowMessageBox(resp.Message);
                return;
            }

            //流局
            if (resp.NextChairId == -1)
            {
                Log.Debug("当前是流局啦!!!!");

                this.DDZCallScorePlugin.Hide();

                DDZGameConfigComponent.Instance.ActiveChairId = -1;

                this.DDZOpTipPlugin.Reset();

                return;
            }

            //显示叫了几分
            int realSeatID = DDZGameHelper.ChangeSeat(resp.ChairId);

            this.DDZOpTipPlugin.CallScoreById(realSeatID, _Score);

            //把叫分面板隐藏掉

            this.DDZCallScorePlugin.Hide();

            //如果下一个玩家的操作是出牌
            if (resp.DoType < 2)
            {
                //下一个玩家操作
                int nextSeatID = DDZGameHelper.ChangeSeat(resp.NextChairId);

                //高亮显示头像
                DDZGameConfigComponent.Instance.ActiveChairId = resp.NextChairId;

                Log.Debug("玩家" + nextSeatID + "操作!!");
            }

            //播放音效
            SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_Call_prefix + _Score);
        }

        /// <summary>
        /// 请求出牌
        /// </summary>
        /// <param name="_PlayCardStatue">出牌状态 0-出牌 1-要不起</param>
        /// <param name="_PlayCard">要出的牌数组</param>
        public async void RequestPlayCard(int _PlayCardStatue , DDZCard _PlayCard)
        {
            var resp = (G2C_DDZPlayCard_Res)await SessionComponent.Instance.Session.Call(
                   new C2G_DDZPlayCard_Req()
                   {
                       GameId = DDZGameHelper.CurrentGameInfo.GameId,
                       AreaId = GameHelper.AreaList[DDZGameHelper.CurrentGameInfo.GameId][0].AreaId,
                       UserId = GamePrefs.GetUserId(),
                       RoomId = DDZGameHelper.RoomId,
                       PlayCardStatue = _PlayCardStatue,
                       PlayCard = _PlayCard
                   });
           
            //由于服务器修正了，这里还是走DDZOutCardError事件
            if (resp.Error != 0 && resp.Error != 200225)
            {
                Game.PopupComponent.ShowMessageBox(resp.Message);

                this.DDZHandCardPlugin.ReSelect();

                return;
            }

            if (resp.Error == 200225) { return; }

            //隐藏掉交互界面
            this.DDZInteractivePlugin.Hide();

            //刷新最新的手牌
            if (resp.Card != null)
            {
                DDZGameConfigComponent.Instance.myHandCard = resp.Card;

                //修改：用服务器返回的数据刷新
                //this.DDZHandCardPlugin.ShowHandCard(resp.Card);

            }

            //根据出的牌，将牌隐藏掉,如果是不出，则不移。
            if (_PlayCardStatue != 1)
            {
                this.DDZHandCardPlugin.RemoveCard2();//临时
            }
            else
            {
                this.DDZHandCardPlugin.ReSelect();
            }

            //刷新倍数

            DDZGameConfigComponent.Instance.Times = resp.Times;

            this.DDZTopPlugin.SetBeiSu();

            //播放语音
            this.PlayOutCardSound(resp.PlayCard);
        }
        
        /// <summary>
        /// 播放出牌语音
        /// </summary>
        /// <param name="resp"></param>
        public void PlayOutCardSound(DDZCard card ,int chairID=0)
        {
            int px = card.PaiXing;
            
            if (px == (int)DDZ_POKER_TYPE.DDZ_PASS)
            {
                Log.Debug("过牌，不出");

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_pass + ETModel.RandomHelper.RandomNumber(1,3));
            }
            else if (px == (int)DDZ_POKER_TYPE.Single)
            {
                Log.Debug("单张");

                //特殊处理大小王
                if (card.Value == 15) card.Value = 18;

                if (card.Value == 16) card.Value = 19;

                //特殊处理2
                if (card.Value == 2) card.Value = 15; 

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_prefix + card.Value);
            }
            else if (px == (int)DDZ_POKER_TYPE.TWIN)
            {
                Log.Debug("对儿");

                //特殊处理2
                if (card.Value == 2) card.Value = 15;

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_double_prefix + (card.Value));
            }
            else if (px == (int)DDZ_POKER_TYPE.TRIPLE)
            {
                Log.Debug("数值相同的三张牌(如三个J)");

                //特殊处理2
                if (card.Value == 2) card.Value = 15;

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_triple_prefix + (card.Value));
            }
            else if (px == (int)DDZ_POKER_TYPE.TRIPLE_WITH_SINGLE)
            {
                Log.Debug("数值相同的三张牌 + 一张单牌或一对牌。例如：333+6 或 444+99");

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_sandaiyi);
            }
            else if (px == (int)DDZ_POKER_TYPE.TRIPLE_WITH_TWIN)
            {
                Log.Debug("三带二");

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_sandaiyidui);
            }
            else if (px == (int)DDZ_POKER_TYPE.STRAIGHT_SINGLE)
            {
                Log.Debug("单顺   五张或更多的连续单牌（如：45678 或 78910JQK）。不包括 2 点和双王");

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_shunzi);

                this.DDZFXLayerPlugin.PlayFX(DDZ_FX_TYPE.ShunZi, chairID);
            }
            else if (px == (int)DDZ_POKER_TYPE.STRAIGHT_TWIN)
            {
                Log.Debug("双顺  三对或更多的连续对牌（如：334455 、77 88 99 1010 JJ）。不包括 2 点和双王");

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_liandui);

                this.DDZFXLayerPlugin.PlayFX(DDZ_FX_TYPE.LianDui, chairID);
            }
            else if (px == (int)DDZ_POKER_TYPE.PLANE_PURE)
            {
                Log.Debug("飞机   二个或更多的连续三张牌（如：333444 、 555 666 777 888）。不包括 2 点和双王。");

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_feiji) ;

                this.DDZFXLayerPlugin.PlayFX(DDZ_FX_TYPE.Plane,chairID);
            }
            else if (px == (int)DDZ_POKER_TYPE.PLANE_WITH_SINGLE)
            {
                Log.Debug("飞机带单");

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_feiji);

                this.DDZFXLayerPlugin.PlayFX(DDZ_FX_TYPE.Plane, chairID);
            }
            else if (px == (int)DDZ_POKER_TYPE.PLANE_WITH_TWIN)
            {
                Log.Debug("飞机带双");

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_feiji);

                this.DDZFXLayerPlugin.PlayFX(DDZ_FX_TYPE.Plane, chairID);
            }
            else if (px == (int)DDZ_POKER_TYPE.FOUR_WITH_SINGLE)
            {
                Log.Debug("四带两单");

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_sidaier);
            }
            else if (px == (int)DDZ_POKER_TYPE.FOUR_WITH_TWIN)
            {
                Log.Debug("四带对");

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_sidaier);
            }
            else if (px == (int)DDZ_POKER_TYPE.FOUR_BOMB)
            {
                Log.Debug("炸弹");

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_zhadan);

                this.DDZFXLayerPlugin.PlayFX(DDZ_FX_TYPE.Bomb, chairID);

            }
            else if (px == (int)DDZ_POKER_TYPE.KING_BOMB)
            {
                Log.Debug("火箭");

                SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_wangzha);

                this.DDZFXLayerPlugin.PlayFX(DDZ_FX_TYPE.WangFire,chairID);
            }
            else
            {
                Log.Error("异常：未知牌型");
            }
        }

        /// <summary>
        /// 广播玩家进入房间
        /// </summary>
        /// <param name="ntt"></param>
        public void DDZJionRoom_Ntt(Actor_JionDDZRoom_Ntt ntt)
        {
            //重绘坐位玩家

            this.DDZPlayerHeadPlugin.SetOnSeatPlayer();

            this.CheckSeatIsFull();
        }

        public void CheckSeatIsFull()
        {
            bool hasFull = true;

            for (int i = 0; i < 3; i++)
            {
                var vo = DataCenterComponent.Instance.userInfo.getUserBySeatID(i);

                if (vo == null) hasFull = false;
            }

            this.DDZReadyPlugin.SetEnterTipState(!hasFull);
        }

        /// <summary>
        /// 广播玩家离开房间
        /// </summary>
        /// <param name="Ntt"></param>
        public void ExitRoom_Ntt(Actor_QuitDDZHRoom_Ntt Ntt)
        {
            //清除准备标识
            var vo = DataCenterComponent.Instance.userInfo.getUserByUserID(Ntt.UserId);

            var myVo = DataCenterComponent.Instance.userInfo.getMyUserVo();

            var limitGold = 0;

            //如果通知自己退出房间，则直接将自已踢出房间至大厅

            if (vo.userID == myVo.userID && myVo.gold <= limitGold)
            {
                Game.PopupComponent.ShowTips(DataCenterComponent.Instance.tipInfo.SelfGoldNotEnoughTip);

                this._BackHall = true;

                this.FastQuitGame();

                return;
            }

            if (vo.userID == myVo.userID && (Ntt.Message1.Contains("金额不足") || Ntt.Message1.Contains("系统检测到")))
            {
                Game.PopupComponent.ShowTips(Ntt.Message1);

                this._BackHall = true;

                this.FastQuitGame();

                return;
            }

            if (vo != null)
            {
                int realSeatID = DDZGameHelper.ChangeSeat(vo.seatID);

                this.DDZReadyPlugin.SetReadyByIndex(realSeatID, false);
            }
            else
            {
                Log.Error("异常了！！！");
            }

            if (DataCenterComponent.Instance.userInfo.isExist(Ntt.UserId))
            {
                DataCenterComponent.Instance.userInfo.deleteUser(Ntt.UserId);
            }
            //重绘坐位玩家
            this.DDZPlayerHeadPlugin.SetOnSeatPlayer();

            this.CheckSeatIsFull();
        }

        /// <summary>
        /// 确定地主广播
        /// </summary>
        /// <param name="resp"></param>
        public void DDZMakeLord_Ntt(Actor_DDZMakeLord_Ntt resp)
        {
            //画出谁是地主

            this.DDZPlayerHeadPlugin.SetLordHeadIcon(resp.LordId);

            //保存地主位置
            DDZGameConfigComponent.Instance.LordID = resp.LordId;

            //高亮显示头像
            DDZGameConfigComponent.Instance.ActiveChairId = resp.LordId;

            //把叫分的UI清掉
            this.DDZOpTipPlugin.Reset();
            
            //刷新倍数

            DDZGameConfigComponent.Instance.Times = resp.Times;

            this.DDZTopPlugin.SetBeiSu();

            //刷新手牌数量

            for (int i = 0; i < resp.CardsNum.count; i++)
            {
                int cardNum = resp.CardsNum[i];

                this.DDZPlayerHeadPlugin.SetCardNum(i, cardNum);
            }

            //清除别的UI

            this.DDZCallScorePlugin.Hide();

            //将顶部底牌绘制出来

            if (DDZGameConfigComponent.Instance.DPCard != null)
            {

                this.DDZTopPlugin.ShowDP(DDZGameConfigComponent.Instance.DPCard,true);
            }
        }

        /// <summary>
        /// 发牌广播
        /// </summary>
        /// <param name="resp"></param>
        public void DDZDealHandCard_Ntt(Actor_DDZDealHandCard_Ntt resp)
        {
            DDZGameHelper.IsStartGame = true;

            this.DDZReadyPlugin.SetEnterTipState(false);

            //显示手牌
            this.DDZHandCardPlugin.ShowHandCard(resp.Card,true);

            //刷新最新的手牌
            DDZGameConfigComponent.Instance.myHandCard = resp.Card;

            //清除准备标识
            this.DDZReadyPlugin.ClearAllReady();

            //清除叫分的标识
            this.DDZOpTipPlugin.Reset();
            
            //保存底牌
            DDZGameConfigComponent.Instance.DPCard = resp.DpCard;

            //清除是否播放过语音的标识

            this.ResetHadPlaySound();
        }

        /// <summary>
        /// 斗地主房间结算通知
        /// </summary>
        /// <param name="resp"></param>
        public async void DDZhSettlement_Req_Ntt(Actor_DDZhSettlement_Req_Ntt resp)
        {
            DDZGameHelper.IsStartGame = false;

            DDZGameHelper.settle = resp;

            //摊牌
            var showHandData = resp.ShowHand.ToList();

            this.DDZOutCardPlugin.ShowHand(showHandData);

            //将手牌全部清掉
            this.DDZHandCardPlugin.Reset();

            //清除操作提示

            this.DDZOpTipPlugin.Reset();

            //刷新所有玩家数据
            if (DDZGameHelper.settle != null)
            {
                foreach (var item in DDZGameHelper.settle.PlayerData)
                {
                    var vo = DataCenterComponent.Instance.userInfo.getUserByUserID(item.UserId);

                    if (vo != null)
                    {
                        vo.gold = item.Gold;

                        vo.score = (int)item.Score;
                    }
                }
            }

            //刷新座位玩家
            DDZPlayerHeadPlugin.SetOnSeatPlayer();

            //播放角色胜利失败特效

            await Task.Delay(500);

            int lordID = DDZGameHelper.ChangeSeat(DDZGameConfigComponent.Instance.LordID);

            var myVO = DataCenterComponent.Instance.userInfo.getMyUserVo();

            /*
            if (lordID == 0)
            {
                //如果我是地主

                if (myVO.score > 0)
                {
                    this.DDZFXLayerPlugin.PlayFX(DDZ_FX_TYPE.LordWin, 0);
                }
                else
                {
                    this.DDZFXLayerPlugin.PlayFX(DDZ_FX_TYPE.LordLost, 0);
                }
            }
            else
            {
                //如果我是农民

                if (myVO.score > 0)
                {
                    this.DDZFXLayerPlugin.PlayFX(DDZ_FX_TYPE.NMWin, 0);
                }
                else
                {
                    this.DDZFXLayerPlugin.PlayFX(DDZ_FX_TYPE.NMLost, 0);
                }
            }
            */

            //根据玩家的分数情况，切换至对应的胜利，失败状态

            if (DDZGameHelper.settle != null)
            {
                foreach (var item in DDZGameHelper.settle.PlayerData)
                {
                    var playerData = item;

                    int realSeatID = DDZGameHelper.ChangeSeat(playerData.ChairId);

                     DDZGamer gamer = DDZPlayerHeadPlugin.GamersDic[realSeatID];

                    if (playerData.Score > 0)
                    {
                        gamer.GetComponent<DDZGamerUIComponent>().SetHeadTransformByIndex(1);
                    }
                    else
                    {
                        gamer.GetComponent<DDZGamerUIComponent>().SetHeadTransformByIndex(2);
                    }
                }

            }


            //显示结算界面
            if (DDZGameHelper.IsJoinRoom)
            {
                await Task.Delay(3000);

                DDZUIFactory.overPanel.Create();
            }

        }

        /// <summary>
        /// 第一个抢地主的人通知
        /// </summary>
        /// <param name="resp"></param>
        public void FirstQdz_Req_Ntt(Actor_FirstQdz_Req_Ntt resp)
        {
            //第一个抢地主的人
            int realSeatID = DDZGameHelper.ChangeSeat(resp.ChairId);

            //赋值
            DDZGameConfigComponent.Instance.ActiveOpTime = 15;

            //高亮显示头像
            DDZGameConfigComponent.Instance.ActiveChairId = resp.ChairId;
            
            if (realSeatID == 0)
            {
                this.DDZCallScorePlugin.Show(15);
            }
            else
            {
                Log.Debug("当前是座位:" + realSeatID + "抢地主中!");
            }

            //隐藏准备界面
            this.DDZReadyPlugin.Reset();
        }

        /// <summary>
        /// 玩家轮循广播
        /// </summary>
        /// <param name="resp"></param>
        public async void DDZTurnPlayer_Ntt(Actor_DDZTurnPlayer_Ntt resp)
        {
            //剩余时间
            DDZGameConfigComponent.Instance.ActiveOpTime = resp.LeftTime;

            //判断当前游戏状态
            if (resp.State == (int)DDZ_GameState.CallScore)
            {
                Log.Debug("游戏状态：抢地主");

                int realSeatID = DDZGameHelper.ChangeSeat(resp.OperateId);

                if (realSeatID == 0) this.DDZCallScorePlugin.Show(resp.LeftTime);

                //高亮显示头像
                DDZGameConfigComponent.Instance.ActiveChairId = resp.OperateId;

            }
            else if (resp.State == (int)DDZ_GameState.DaPai)
            {
                Log.Debug("游戏状态：出牌");

                int realSeatID = DDZGameHelper.ChangeSeat(resp.OperateId);

                //清除之前出牌界面
                this.DDZOutCardPlugin.ClearByIndex(realSeatID);

                if (realSeatID == 0)
                {
                    //弹出交互操作BAR
                    this.DDZInteractivePlugin.Show(resp.LeftTime);
                }

                //高亮显示头像
                DDZGameConfigComponent.Instance.ActiveChairId = resp.OperateId;

                //隐藏操作提示
               
                this.DDZOpTipPlugin.HideByIndex(realSeatID);

                //检测连续两个都是不出，则清屏
                await Task.Delay(200);

                this.DDZOpTipPlugin.CheckClearALL(realSeatID);
            }
            else if (resp.State == (int)DDZ_GameState.JieSuan)
            {
                Log.Debug("游戏状态：结算");
            }
        }

        /// <summary>
        /// 确认地主后给地主的广播
        /// </summary>
        /// <param name="resp"></param>
        public void DDZLord_Ntt(Actor_DDZLord_Ntt resp)
        {
            //将地主的手牌刷新一下

            this.DDZHandCardPlugin.ShowHandCard(resp.HandCards);

            //刷新最新的手牌
            DDZGameConfigComponent.Instance.myHandCard = resp.HandCards;

        }

        /// <summary>
        /// 玩家出牌广播
        /// </summary>
        /// <param name="resp"></param>
        public async void OtherPlayCard_Ntt(Actor_OtherPlayCard_Ntt resp)
        {
            //获取真实座位ID
            int realSeatID = DDZGameHelper.ChangeSeat(resp.ChairId);

            //如果是自己出牌，则隐藏掉操作面板，并且刷新自己的手牌
            if (realSeatID == 0)
            {
                this.DDZInteractivePlugin.Hide();

                //手动移除掉手牌
                if (resp.PlayCard != null && resp.PlayCard.Card != null)
                {
                    this.DDZHandCardPlugin.RemoveCard(resp.PlayCard);
                }

                //将手牌位置重置

                this.DDZHandCardPlugin.ReSelect();
            }
            
            //清除倒计时时钟
            this.DDZPlayerHeadPlugin.ClearClockByID(realSeatID);
            
            //如果出的牌为空，表示要不起

            if (resp.PlayCard == null || resp.PlayCard.Card== null)
            {
                //清除该区域出的牌
                this.DDZOutCardPlugin.ClearByIndex(realSeatID);
                
                //播放要不起音效,自己就不用了播放
                if (realSeatID != 0)
                {
                    SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_pass + ETModel.RandomHelper.RandomNumber(1, 3));
                }

                //显示要不起提示
                this.DDZOpTipPlugin.ShowOpTipById(realSeatID, 0);

                return;
            }
            
            //播放出牌音效
            SoundComponent.Instance.PlayClip(soundInfo.DDZ_sound_discard);

            //将出的牌显示出来
            this.DDZOutCardPlugin.PlayCardBySeatID(realSeatID, resp.PlayCard);

            //保存上家牌
            DDZGameConfigComponent.Instance.preOutCard = resp.PlayCard;

            //刷新倍数

            DDZGameConfigComponent.Instance.Times = resp.Times;

            this.DDZTopPlugin.SetBeiSu();

            //播放读牌音效
            this.PlayOutCardSound(resp.PlayCard, realSeatID);

            //更新玩家的手牌数量

            for (int i = 0; i < 3; i++)
            {
                int cardNum = resp.SurCardsNum[i];
                
                this.DDZPlayerHeadPlugin.SetCardNum(i, cardNum);

                int realSeatId = DDZGameHelper.ChangeSeat(i);

                //判断谁的手牌只有1张或者两张，播放警告语音

                if (realSeatId != -1)
                {

                    if (cardNum == 1|| cardNum == 2)
                    {
                        await Task.Delay(1000);

                        if (!this.CheckHadPlaySound(realSeatId, cardNum))
                        {
                            SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_warning_prefix + cardNum);

                            this.SaveHadPlaySound(realSeatId, cardNum);
                        }
                    }
                }

                //显示警报器
                if (realSeatId != 0)
                {
                    if (cardNum == 1|| cardNum == 2)
                    {
                        this.DDZOpTipPlugin.ShowWarnById(realSeatId);
                    }
                }

            }
            
        }

        /// <summary>
        /// 检测当前位置是否需要播放语音
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="curLastNum"></param>
        /// <returns></returns>
        private bool CheckHadPlaySound(int pos, int curLastNum)
        {
            bool hadPlay = false;

            if (curLastNum == 2) { hadPlay = this.hadPlayTwoCardSound[pos]; }

            if (curLastNum == 1) { hadPlay = this.hadPlayOneCardSound[pos]; }

            return hadPlay;
        }

        /// <summary>
        /// 保存当前位置是否播放过语音
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="curLastNum"></param>
        private void SaveHadPlaySound(int pos, int curLastNum)
        {
            if (curLastNum == 2) { hadPlayTwoCardSound[pos] = true; }

            if (curLastNum == 1) { hadPlayOneCardSound[pos] = true; }
        }

        /// <summary>
        /// 清除该位置是否播放过语音标记
        /// </summary>
        private void ResetHadPlaySound()
        {
            for (int i = 0; i < this.hadPlayOneCardSound.Count; i++)
            {
                this.hadPlayOneCardSound[i] = false;
            }

            for (int i = 0; i < this.hadPlayTwoCardSound.Count; i++)
            {
                this.hadPlayTwoCardSound[i] = false;
            }
        }

        /// <summary>
        /// 玩家准备广播
        /// </summary>
        /// <param name="resp"></param>
        public void OtherPrepare_Ntt(Actor_OtherPrepare_Ntt ntt)
        {
            int realSeatID = DDZGameHelper.ChangeSeat(ntt.ChairId);

            this.DDZReadyPlugin.SetReadyByIndex(realSeatID, true);
        }

        /// <summary>
        /// 玩家抢地主广播
        /// </summary>
        /// <param name="resp"></param>
        public void OtherQdz_Ntt(Actor_OtherQdz_Ntt resp)
        {
            //显示谁抢了多少分
            int realSeatID = DDZGameHelper.ChangeSeat(resp.ChairId);

            //如果是自己叫分，则隐藏掉操作面板
            if (realSeatID == 0)
            {
                this.DDZCallScorePlugin.Hide();
            }
            
            //清除倒计时时钟
            this.DDZPlayerHeadPlugin.ClearClockByID(realSeatID);

            int score = resp.Score;
            
            this.DDZOpTipPlugin.CallScoreById(realSeatID, score);

            //缓存分数，用于判断下次叫分是否灰掉

            if (score != 0)
            {
                DDZGameConfigComponent.Instance.PreCallScore = score;
            }

            //播放叫分音效
            SoundComponent.Instance.PlayClip(soundInfo.DDZ_male_Call_prefix + score);
            
        }

        /// <summary>
        /// 请求换桌
        /// </summary>
        public async void RequestChangeRoom()
        {
            G2C_ChangerRoom_Res response = (G2C_ChangerRoom_Res)await SessionComponent.Instance.Session.Call(new C2G_ChangerRoom_Req()
            {
                GameId = DDZGameHelper.CurrentGameInfo.GameId,

                AreaId = GameHelper.AreaList[DDZGameHelper.CurrentGameInfo.GameId][DDZGameHelper.CurrentFieldId].AreaId,

                RoomId = DDZGameHelper.RoomId,

                UserId = GamePrefs.GetUserId()
            });

            if (response.Error == 0)
            {
                Game.PopupComponent.ShowTips(DataCenterComponent.Instance.tipInfo.ChangeTableTip);

                //手动退出房间

                this._BackHall = false;

                this.QuitGame();

                try
                {
                    var respons = (G2C_JionRoom_Res)await SessionComponent.Instance.Session.Call(
                        new C2G_JionRoom_Req()
                        {
                            AreaId = GameHelper.AreaList[DDZGameHelper.CurrentGameInfo.GameId][DDZGameHelper.CurrentFieldId].AreaId,

                            GameId = DDZGameHelper.CurrentGameInfo.GameId,

                            UserId = GamePrefs.GetUserId(),

                            RoomId = response.RoomId
                        });

                    if (respons.Error != 0)
                    {
                        Game.PopupComponent.ShowMessageBox(respons.Message);
                        return;
                    }

                    DDZGameHelper.RoomId = respons.RoomId;

                    DDZUIFactory.gameScene.Create();
                }
                catch (Exception e)
                {
                    Game.PopupComponent.ShowMessageBox(e.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Resume 事件处理
        /// </summary>
        public async void OnApplicationResume()
        {
            Game.PopupComponent.ShowLoadingLockUI(DataCenterComponent.Instance.tipInfo.ResumeSceneTip);

            await Task.Delay(DataCenterComponent.Instance.gameInfo.ApplicationResumeWaitTime);

            GameHelper.ApplicationIsPause = false;

            var uiC = Game.Scene.GetComponent<UIComponent>();

            if (uiC.Get(UIType.UIMessageBoxPanel) != null)
            {
                Log.Info("存在断线重联界面，不走Resume逻辑!!");

                Game.PopupComponent.CloseLoadingLockUI();

                return;
            }

            //销毁当前界面，但不重新创建大厅，否则相关数据会被清掉

            this._BackHall = false;

            this.QuitGame();

            Game.PopupComponent.CloseLoadingLockUI();
            
            try
            {
                var response = (G2C_JionRoom_Res)await SessionComponent.Instance.Session.Call(
                    new C2G_JionRoom_Req()
                    {
                        AreaId = GameHelper.AreaList[DDZGameHelper.CurrentGameInfo.GameId][0].AreaId,
                        GameId = DDZGameHelper.CurrentGameInfo.GameId,
                        UserId = GamePrefs.GetUserId(),
                    });

                if (response.Error != 0)
                {
                    Game.PopupComponent.ShowMessageBox(response.Message);
                    return;
                }

                DDZGameHelper.RoomId = response.RoomId;
                
                DDZUIFactory.gameScene.Create();
            }
            catch (Exception e)
            {
                Game.PopupComponent.ShowMessageBox(e.Message);
                throw;
            }
        }

        /// <summary>
        /// 存储重回数据
        /// </summary>
        public void SaveReBackData()
        {
            GameReBackData reBackData = new GameReBackData
            {
                GameId = DDZGameHelper.CurrentGameInfo.GameId,

                AreaId = GameHelper.AreaList[DDZGameHelper.CurrentGameInfo.GameId][0].AreaId,

                RoomId = DDZGameHelper.RoomId,

                index = DDZGameHelper.CurrentFieldId
            };

            DataCenterComponent.Instance.GameReBackInfo.backData = reBackData;

            DataCenterComponent.Instance.GameReBackInfo.isBackSuccess = false;

            //持久化存储
            string _jsonData = JsonHelper.ToJson(reBackData);

            PlayerPrefs.SetString(DataCenterComponent.Instance.GameReBackInfo.reBackDataKey, _jsonData);
        }

        /// <summary>
        /// 清除重回数据
        /// </summary>
        public void ClearReBackData()
        {
            DataCenterComponent.Instance.GameReBackInfo.backData = null;

            DataCenterComponent.Instance.GameReBackInfo.isBackSuccess = false;

            PlayerPrefs.DeleteKey(DataCenterComponent.Instance.GameReBackInfo.reBackDataKey);
        }

        /// <summary>
        /// 重置插件
        /// </summary>
        public void Reset()
        {
            DDZFootPlugin.Reset();

            DDZLTPlugin.Reset();

            DDZTRBtnPlugin.Reset();

            DDZTopPlugin.Reset();

            DDZPlayerHeadPlugin.Reset();

            DDZReadyPlugin.Reset();

            DDZCallScorePlugin.Reset();

            DDZOpTipPlugin.Reset();

            DDZInteractivePlugin.Reset();

            DDZMaskPlugin.Reset();

            DDZHandCardPlugin.Reset();

            DDZOutCardPlugin.Reset();

            DDZFXLayerPlugin.Reset();

            DDZDownPullPlugin.Reset();
        }

        public void OnQuitGame(bool backHall = true)
        {
            _BackHall = backHall;

            var data = new MessageData();

            data.onlyOk = false;

            data.click = true;

            data.fastClose = true;

            data.ok = QuitGame;

            string tipStr = DDZGameHelper.IsStartGame ? DataCenterComponent.Instance.tipInfo.ExitSubGameWhenStartTip :

                DataCenterComponent.Instance.tipInfo.ExitSubGameTip;

            Game.PopupComponent.ShowMessageBox(tipStr, data);
        }

        public void FastQuitGame()
        {
            //正常退出，则清除

            Game.PopupComponent.CloseNoticeUI();

            ClearReBackData();

            DDZGameHelper.Reset();

            DDZUIFactory.gameScene.Remove();

            UI overUI = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIDDZOverPanel);

            if (overUI != null) { DDZUIFactory.overPanel.Remove(); }

            this.Reset();

            DataCenterComponent.Instance.userInfo.deleteAllUserExcptMe();

            if (DDZConfig.GameScene.PlayerListsPanel != null)
            {
                Game.PopupComponent.ClosePlayerListPanel();
            }

            if (_BackHall)
            {
                Game.EventSystem.Run(EventIdType.SubGameReBackLobby);

                //音乐切换为大厅

                SoundHelper.SubGameExitResetSound();

                SoundComponent.Instance.Stop(CurrentSoundName);

                var _soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1);

                SoundComponent.Instance.PlayMusic(DataCenterComponent.Instance.soundInfo.bg_hall, 0, _soundVolume, true);
            }
        }

        /// <summary>
        /// 离开游戏
        /// </summary>
        private async void QuitGame()
        {
            var resp = (G2C_QuitRoom_Res)await SessionComponent.Instance.Session.Call(
                   new C2G_QuitRoom_Req()
                   {
                       GameId = DDZGameHelper.CurrentGameInfo.GameId,

                       AreaId = GameHelper.AreaList[DDZGameHelper.CurrentGameInfo.GameId][DDZGameHelper.CurrentFieldId].AreaId,

                       UserId = GamePrefs.GetUserId(),

                       RoomId = DDZGameHelper.RoomId
                   });
            if (resp.Error == 0)
            {
                this.FastQuitGame();
            }
            else
            {
                Game.PopupComponent.ShowTips(resp.Message);
            }
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
