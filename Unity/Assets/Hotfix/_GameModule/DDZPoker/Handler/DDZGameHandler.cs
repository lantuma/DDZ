using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 广播玩家进入房间
    /// </summary>
    [MessageHandler]
    public class Handler_Actor_JionRoom_Ntt3 : AMHandler<Actor_JionDDZRoom_Ntt>
    {
        protected override void Run(ETModel.Session session, Actor_JionDDZRoom_Ntt message)
        {
            if (!DDZGameHelper.IsJoinRoom || GameHelper.ApplicationIsPause) { return; }

            if (message.RoomId == 0) { Log.Error("异常:房间号为0了"); }

            if (DDZGameHelper.RoomId != message.RoomId) { return; }
            
            if (!DataCenterComponent.Instance.userInfo.isExist(message.PlayerData.UserId))
            {
                UserVO vo = new UserVO
                {
                    userID = message.PlayerData.UserId,

                    headId = message.PlayerData.HeadId,

                    sex = message.PlayerData.Gender,

                    gold = message.PlayerData.Gold,

                    nickName = message.PlayerData.NickeName,

                    seatID = message.PlayerData.ChairId,

                    point = message.PlayerData.QdzJiaoFen,

                    IsReady = message.PlayerData.IsPrepare?1:0
                };

                DataCenterComponent.Instance.userInfo.addUser(vo);
            }

            DDZConfig.GameScene.DDZJionRoom_Ntt(message);
        }
    }

    /// <summary>
    /// 广播玩家离开房间
    /// </summary>
    [MessageHandler]
    public class Handler_Actor_ExitNNRoomg_Ntt3 : AMHandler<Actor_QuitDDZHRoom_Ntt>
    {
        protected override void Run(ETModel.Session session, Actor_QuitDDZHRoom_Ntt message)
        {
            if (!DDZGameHelper.IsJoinRoom || GameHelper.ApplicationIsPause) { return; }

            if (message.RoomId == 0) { Log.Error("异常:房间号为0了"); }

            if (DDZGameHelper.RoomId != message.RoomId) { return; }

            DDZConfig.GameScene.ExitRoom_Ntt(message);
        }
    }
    
    /// <summary>
    /// 玩家广播(确立地主)
    /// </summary>
    [MessageHandler]
    public class OnActor_DDZMakeLord_Ntt : AMHandler<Actor_DDZMakeLord_Ntt>
    {
        protected override void Run(ETModel.Session session, Actor_DDZMakeLord_Ntt message)
        {
            if (!DDZGameHelper.IsJoinRoom || GameHelper.ApplicationIsPause) { return; }

            if (message.RoomId == 0) { Log.Error("异常:房间号为0了"); }

            if (DDZGameHelper.RoomId != message.RoomId) { return; }

            DDZConfig.GameScene.DDZMakeLord_Ntt(message);
        }
    }

    /// <summary>
    /// 发牌广播
    /// </summary>
    [MessageHandler]
    public class OnActor_DDZDealHandCard_Ntt : AMHandler<Actor_DDZDealHandCard_Ntt>
    {
        protected override void Run(ETModel.Session session, Actor_DDZDealHandCard_Ntt message)
        {
            if (!DDZGameHelper.IsJoinRoom || GameHelper.ApplicationIsPause) { return; }

            if (message.RoomId == 0) { Log.Error("异常:房间号为0了"); }

            if (DDZGameHelper.RoomId != message.RoomId) { return; }

            DDZConfig.GameScene.DDZDealHandCard_Ntt(message);
        }
    }

    /// <summary>
    /// 房间结算通知
    /// </summary>
    [MessageHandler]
    public class OnActor_DDZhSettlement_Req_Ntt : AMHandler<Actor_DDZhSettlement_Req_Ntt>
    {
        protected override void Run(ETModel.Session session, Actor_DDZhSettlement_Req_Ntt message)
        {
            if (!DDZGameHelper.IsJoinRoom || GameHelper.ApplicationIsPause) { return; }

            if (message.RoomId == 0) { Log.Error("异常:房间号为0了"); }

            if (DDZGameHelper.RoomId != message.RoomId) { return; }

            DDZConfig.GameScene.DDZhSettlement_Req_Ntt(message);
        }
    }

    /// <summary>
    /// 第一个抢地主的人通知
    /// </summary>
    [MessageHandler]
    public class OnActor_FirstQdz_Req_Ntt : AMHandler<Actor_FirstQdz_Req_Ntt>
    {
        protected override void Run(ETModel.Session session, Actor_FirstQdz_Req_Ntt message)
        {
            if (!DDZGameHelper.IsJoinRoom || GameHelper.ApplicationIsPause) { return; }

            if (message.RoomId == 0) { Log.Error("异常:房间号为0了"); }

            if (DDZGameHelper.RoomId != message.RoomId) { return; }
            DDZConfig.GameScene.FirstQdz_Req_Ntt(message);
        }
    }

    /// <summary>
    /// 玩家轮循广播
    /// </summary>
    [MessageHandler]
    public class OnActor_DDZTurnPlayer_Ntt : AMHandler<Actor_DDZTurnPlayer_Ntt>
    {
        protected override void Run(ETModel.Session session, Actor_DDZTurnPlayer_Ntt message)
        {
            if (!DDZGameHelper.IsJoinRoom || GameHelper.ApplicationIsPause) { return; }

            if (message.RoomId == 0) { Log.Error("异常:房间号为0了"); }

            if (DDZGameHelper.RoomId != message.RoomId) { return; }

            DDZConfig.GameScene.DDZTurnPlayer_Ntt(message);
        }
    }

    /// <summary>
    /// 确定地主后给地主的广播
    /// </summary>
    [MessageHandler]
    public class OnActor_DDZLord_Ntt : AMHandler<Actor_DDZLord_Ntt>
    {
        protected override void Run(ETModel.Session session, Actor_DDZLord_Ntt message)
        {
            if (!DDZGameHelper.IsJoinRoom || GameHelper.ApplicationIsPause) { return; }

            if (message.RoomId == 0) { Log.Error("异常:房间号为0了"); }

            if (DDZGameHelper.RoomId != message.RoomId) { return; }

            DDZConfig.GameScene.DDZLord_Ntt(message);
        }
    }

    /// <summary>
    /// 玩家出牌广播
    /// </summary>
    [MessageHandler]
    public class OnActor_OtherPlayCard_Ntt : AMHandler<Actor_OtherPlayCard_Ntt>
    {
        protected override void Run(ETModel.Session session, Actor_OtherPlayCard_Ntt message)
        {
            if (!DDZGameHelper.IsJoinRoom || GameHelper.ApplicationIsPause) { return; }

            if (message.RoomId == 0) { Log.Error("异常:房间号为0了"); }

            if (DDZGameHelper.RoomId != message.RoomId) { return; }

            DDZConfig.GameScene.OtherPlayCard_Ntt(message);
        }
    }

    /// <summary>
    /// 玩家准备广播
    /// </summary>
    [MessageHandler]
    public class OnActor_OtherPrepare_Ntt : AMHandler<Actor_OtherPrepare_Ntt>
    {
        protected override void Run(ETModel.Session session, Actor_OtherPrepare_Ntt message)
        {
            if (!DDZGameHelper.IsJoinRoom || GameHelper.ApplicationIsPause) { return; }

            if (message.RoomId == 0) { Log.Error("异常:房间号为0了"); }

            if (DDZGameHelper.RoomId != message.RoomId) { return; }

            DDZConfig.GameScene.OtherPrepare_Ntt(message);
        }
    }

    /// <summary>
    /// 玩家抢地主广播
    /// </summary>
    [MessageHandler]
    public class OnActor_OtherQdz_Ntt : AMHandler<Actor_OtherQdz_Ntt>
    {
        protected override void Run(ETModel.Session session, Actor_OtherQdz_Ntt message)
        {
            if (!DDZGameHelper.IsJoinRoom || GameHelper.ApplicationIsPause) { return; }

            if (message.RoomId == 0) { Log.Error("异常:房间号为0了"); }

            if (DDZGameHelper.RoomId != message.RoomId) { return; }

            DDZConfig.GameScene.OtherQdz_Ntt(message);
        }
    }

}