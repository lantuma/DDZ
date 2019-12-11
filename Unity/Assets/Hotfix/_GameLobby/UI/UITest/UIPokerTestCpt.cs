using System;
using System.Linq;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
/*
namespace ETHotfix
{
    [ObjectSystem]
    public class UIPokerTestCptAwakeSystem : AwakeSystem<UIPokerTestCpt, GameObject>
    {
        public override void Awake(UIPokerTestCpt self, GameObject go)
        {
            self.Awake(go);
        }
    }

    public class UIPokerTestCpt : Component
    {
        
        private GameObject _gameObject;
        private ReferenceCollector ReferenceCollector;
        public static  long userId { get; set; }
        private static UserInfo UserInfo;

        #region Reference

        private InputField UserNameInputField;
        private InputField PasswordInputField;
        private Text tipsText;
        private InputField changerUserInfoIF;

        #endregion

        public void Awake(GameObject go)
        {
            this._gameObject = go;
            ReferenceCollector = this._gameObject.GetComponent<ReferenceCollector>();

            this.UserNameInputField = this.ReferenceCollector.Get<InputField>("UserNameInputField");
            this.PasswordInputField = this.ReferenceCollector.Get<InputField>("PasswordInputField");
            UserNameInputField.text = "111";
            PasswordInputField.text = "111";

            this.ReferenceCollector.Get<Button>("RegisterBtn").onClick.Add(OnClickRegister);
            this.ReferenceCollector.Get<Button>("LoginBtn").onClick.Add(OnClickLogin);
            this.ReferenceCollector.Get<Button>("GetUserInfoButton").onClick.Add(OnClickGetUserInfoBtn);
            this.tipsText = this.ReferenceCollector.Get<Text>("tipsText");
            this.changerUserInfoIF = this.ReferenceCollector.Get<InputField>("ChangeUserIF");
            this.ReferenceCollector.Get<Button>("HeadChangerBtn").onClick.Add(OnCHeadButton);
            this.ReferenceCollector.Get<Button>("NickChangerBtn").onClick.Add(OnCNickNameButton);
            this.ReferenceCollector.Get<Button>("GenderChangerBtn").onClick.Add(OnCGenderButton);
            this.ReferenceCollector.Get<Button>("GetGameListBtn").onClick.Add(OnGetGameListButton);
            this.ReferenceCollector.Get<Button>("GetAreaListBtn").onClick.Add(OnGetNiuNiuArea);
            this.ReferenceCollector.Get<Button>("JionRoomBtn").onClick.Add(OnJionRoomButton);
            this.ReferenceCollector.Get<Button>("BetsButton").onClick.Add(OnBetsButton);
            this.ReferenceCollector.Get<Button>("QuitRoomButton").onClick.Add(OnQuitRoomButton);
            this.ReferenceCollector.Get<Button>("GetRoomInfoBtn").onClick.Add(OnGetRoomInfoButton);
            this.ReferenceCollector.Get<Button>("SitDownBtn").onClick.Add(OnSitDownBtn);
            this.ReferenceCollector.Get<Button>("StandUpBtn").onClick.Add(OnStandUpBtn);
            this.ReferenceCollector.Get<Button>("ShanZhuangBtn").onClick.Add(OnShangZhuangBtn);
            this.ReferenceCollector.Get<Button>("XiaZhuangBtn").onClick.Add(OnXiaZhuangBtn);
            this.ReferenceCollector.Get<Button>("LookCardBtn").onClick.Add(OnLookCardButton);
            this.ReferenceCollector.Get<Button>("DiscardsBtn").onClick.Add(OnDiscardButton);
            this.ReferenceCollector.Get<Button>("ComparisonCardButton").onClick.Add(OnComparisonCardButton);

        }

        #region 服务器协议

        public void ShowOrHide()
        {
            this._gameObject.SetActive(!this._gameObject.activeSelf);
        }

        #region  登录

        private async void OnClickLogin()
        {
            try
            {
                if (string.IsNullOrEmpty(this.UserNameInputField.text)) return;
                if (string.IsNullOrEmpty(this.PasswordInputField.text)) return;

                // 创建一个ETModel层的Session
                ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().
                        Create(GlobalConfigComponent.Instance.GlobalProto.Address);

                // 创建一个ETHotfix层的Session, ETHotfix的Session会通过ETModel层的Session发送消息
                Session realmSession = ComponentFactory.Create<Session, ETModel.Session>(session);
                //向服务器申请了一个Key
                R2C_Login r2CLogin = (R2C_Login)await realmSession.Call(new C2R_Login() { Account = this.UserNameInputField.text, Password = this.PasswordInputField.text });
                realmSession.Dispose();

                if (r2CLogin.Error != 0)
                {
                    Debug.Log(r2CLogin.Message);
                    return;
                }
                userId = r2CLogin.UserId;

                // 创建一个ETModel层的Session,并且保存到ETModel.SessionComponent中
                ETModel.Session gateSession = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(r2CLogin.Address);

                if (ETModel.Game.Scene.GetComponent<ETModel.SessionComponent>() == null)
                    ETModel.Game.Scene.AddComponent<ETModel.SessionComponent>().Session = gateSession;

                ETModel.Game.Scene.GetComponent<ETModel.SessionComponent>().Session = gateSession;


                // 创建一个ETHotfix层的Session, 并且保存到ETHotfix.SessionComponent中
                if (Game.Scene.GetComponent<SessionComponent>() == null)
                    Game.Scene.AddComponent<SessionComponent>();

                Game.Scene.GetComponent<SessionComponent>().Session = ComponentFactory.Create<Session, ETModel.Session>(gateSession);
                G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await SessionComponent.Instance.Session.Call(new C2G_LoginGate()
                { Key = r2CLogin.Key, UserId = userId });

                if (g2CLoginGate.Error == ErrorCode.ERR_ConnectGateKeyError)
                {
                    Debug.Log($"{g2CLoginGate.Message}");
                    Game.Scene.GetComponent<SessionComponent>().Session.Dispose();
                    return;
                }

                //  userId = g2CLoginGate.UserId;

                Debug.Log(" 登录成功 !!!");

            }
            catch (System.Exception)
            {
                throw;
            }
        }

        #endregion

        #region  注册

        private async void OnClickRegister()
        {
            try
            {
                if (string.IsNullOrEmpty(this.UserNameInputField.text)) return;
                if (string.IsNullOrEmpty(this.PasswordInputField.text)) return;

                // 创建一个ETModel层的Session
                ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().
                        Create(GlobalConfigComponent.Instance.GlobalProto.Address);

                // 创建一个ETHotfix层的Session, ETHotfix的Session会通过ETModel层的Session发送消息
                Session realmSession = ComponentFactory.Create<Session, ETModel.Session>(session);

                // 发送注册请求
                R2C_Register_Res r2CRegisterRes = (R2C_Register_Res)await realmSession.Call(
                    new C2R_Register_Req()
                    {
                        Account = this.UserNameInputField.text,
                        Password = this.PasswordInputField.text
                    });

                if (r2CRegisterRes.Error == ErrorCode.ERR_AccountAlreadyRegister)
                {
                    Debug.Log("账号已经存在");
                }
                else if (r2CRegisterRes.Error == 0)
                {
                    Debug.Log("注册成功");
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        #endregion

        #region  获取用户信息


        private async void OnClickGetUserInfoBtn()
        {
            G2C_GetUserInfo_Res getUserInfoRes = (G2C_GetUserInfo_Res)await SessionComponent.Instance.Session.Call
            (new C2G_GetUserInfo_Req() { UserId = userId });

            if (getUserInfoRes.Error != 0)
            {
                Debug.Log(getUserInfoRes.Message);
                return;
            }

            UserInfo = getUserInfoRes.UserInfo;

            Debug.Log($"获取用户信息成功 用户昵称为:{getUserInfoRes.UserInfo.Nickname},用户ID是:{getUserInfoRes.UserInfo.PlayerId}");
            tipsText.text = $"获取用户信息成功 用户昵称为:{getUserInfoRes.UserInfo.Nickname},用户ID是:{getUserInfoRes.UserInfo.PlayerId}";
        }

        #endregion

        #region  修改头像

        private async void OnCHeadButton()
        {
            G2C_ChangerUserInfo_Res chagerHead = (G2C_ChangerUserInfo_Res)await SessionComponent.Instance.Session.Call
             (new C2G_ChangerUserInfo_Req() { UserId = userId, Type = 0, HeadId = int.Parse(changerUserInfoIF.text) });
            if (chagerHead.Error != 0)
            {
                tipsText.text = chagerHead.Message;
                return;
            }

            tipsText.text = "修改头像成功";
            changerUserInfoIF.text = "";
        }

        #endregion

        #region  修改昵称

        private async void OnCNickNameButton()
        {
            G2C_ChangerUserInfo_Res chagerNickName = (G2C_ChangerUserInfo_Res)await SessionComponent.Instance.Session.Call
            (new C2G_ChangerUserInfo_Req() { UserId = userId, Type = 1, NickName = changerUserInfoIF.text });
            if (chagerNickName.Error != 0)
            {
                tipsText.text = chagerNickName.Message;
                return;
            }

            tipsText.text = "修改昵称成功";
            changerUserInfoIF.text = "";
        }

        #endregion

        #region  修改性别

        private async void OnCGenderButton()
        {
            G2C_ChangerUserInfo_Res chagerGender = (G2C_ChangerUserInfo_Res)await SessionComponent.Instance.Session.Call
          (new C2G_ChangerUserInfo_Req() { UserId = userId, Type = 2, Gender = int.Parse(changerUserInfoIF.text) });
            if (chagerGender.Error != 0)
            {
                tipsText.text = chagerGender.Message;
                return;
            }

            tipsText.text = "修改性别成功";
            changerUserInfoIF.text = "";
        }

        #endregion

        #region  获取游戏列表

        private async void OnGetGameListButton()
        {
            G2C_GetGameList_Res getGameList = (G2C_GetGameList_Res)await SessionComponent.Instance.Session.Call
            (new C2G_GetGameList_Req());

            if (getGameList.Error != 0)
            {
                tipsText.text = "获取玩家游戏列表";
                return;
            }

            tipsText.text = $"获取玩家游戏列表的个数是{ getGameList.GameInfo.count}";
            changerUserInfoIF.text = "";
        }

        #endregion

        #region  获取牛牛场

        private async void OnGetNiuNiuArea()
        {
            G2C_GetAreaList_Res areaList = (G2C_GetAreaList_Res)await SessionComponent.Instance.Session.Call
            (new C2G_GetAreaList_Req() { GameId = 2 });

            if (areaList.Error != 0)
            {
                tipsText.text = "获取游戏场信息失败";
                return;
            }

            tipsText.text = $"获取玩家游戏列表的个数是{ areaList.AreaInfo.count}";
            changerUserInfoIF.text = "";
        }

        #endregion

        #region  加入房间

        private async void OnJionRoomButton()
        {
            G2C_JionRoom_Res response = (G2C_JionRoom_Res)await SessionComponent.Instance.Session.Call
          (new C2G_JionRoom_Req() { GameId = 1, AreaId = 101, UserId = userId });

            if (response.Error != 0)
            {
                tipsText.text = "加入房间失败！！！";
                return;
            }

            tipsText.text = $"加入房间成功！！请继续输入获取房间信息请求";

            //  Game.EventSystem.Run(EventIdType.CreateUINiuNiuGamePanel);
        }

        //其他玩家加入房间回调
        [MessageHandler]
        public class JRHandler : AMHandler<Actor_JionRoom_Ntt>
        {
            protected override void Run(ETModel.Session session, Actor_JionRoom_Ntt message)
            {
                Debug.Log($"玩家{message.PlayerData.NickName}加入房间！！！");
            }
        }

        [MessageHandler]
        public class JRZjhHandler : AMHandler<Actor_JionZJHRoom_Ntt>
        {
            protected override void Run(ETModel.Session session, Actor_JionZJHRoom_Ntt message)
            {
                Debug.Log($"玩家{message.PlayerData.NickeName}加入炸金房间！！！");
            }
        }

        #endregion

        #region  获取房间信息

        private async void OnGetRoomInfoButton()
        {
            G2C_GetRoomInfo_Res response = (G2C_GetRoomInfo_Res)await SessionComponent.Instance.Session.Call
         (new C2G_GetRoomInfo_Req() { GameId = 1, AreaId = 101, UserId = userId });
            if (response.Error != 0)
            {
                tipsText.text = "获取房间信息失败！！！";
                return;
            }
            tipsText.text = $"获取房间信息成功！房间的玩家列表有{response.RoomData.PlayerData.count}个人";
        }

        #endregion

        #region  开始下注回调

        [MessageHandler]
        public class CDHandler : AMHandler<Actor_CountDown_Ntt>
        {
            protected override void Run(ETModel.Session session, Actor_CountDown_Ntt message)
            {
                Debug.Log($"现在可以开始下注了 有{message.LeftTime}秒的下注时间");
            }
        }

        #endregion

        #region  下注

        private async void OnBetsButton()
        {
            G2C_Bets_Res response = (G2C_Bets_Res)await SessionComponent.Instance.Session.Call
             (new C2G_Bets_Req() { GameId = 1, AreaId = 101, UserId = userId, ChipId = int.Parse(UserNameInputField.text), ChipPoolId = 0 });

            if (response.Error != 0)
            {
                tipsText.text = $"下注失败 下注错误信息{response.Message}！";
                return;
            }
            tipsText.text = $"下注成功 {response.ChipPoolId}彩池现在下注了{response.ChipPooData.AllChipNum}筹码";
        }


        [MessageHandler]
        public class OtherPlayerHandler : AMHandler<Actor_OtherPlayerBets_Ntt>
        {
            protected override void Run(ETModel.Session session, Actor_OtherPlayerBets_Ntt message)
            {
                UnityEngine.Debug.Log($"玩家{message.PlayerId}下了{message.Chips}筹码 ,他下注后的钱是{message.PlayerGod}");
            }
        }

        #endregion

        #region  离开房间

        private async void OnQuitRoomButton()
        {
            G2C_QuitRoom_Res response = (G2C_QuitRoom_Res)await SessionComponent.Instance.Session.Call
            (new C2G_QuitRoom_Req() { GameId = 1, AreaId = 101, UserId = userId });

            tipsText.text = "退出房间成功！！！";
        }

        [MessageHandler]
        public class ExitRoomHandler : AMHandler<Actor_ExitNNRoomg_Ntt>
        {
            protected override void Run(ETModel.Session session, Actor_ExitNNRoomg_Ntt message)
            {
                UnityEngine.Debug.Log($"玩家{message.PlayerId} 退出了房间！！！");
            }
        }

        #endregion

        #region  坐下

        private async void OnSitDownBtn()
        {
            G2C_SitDown_Res response = (G2C_SitDown_Res)await SessionComponent.Instance.Session.Call
         (new C2G_SitDown_Req() { GameId = 2, AreaId = 201, UserId = userId, ChairId = int.Parse(changerUserInfoIF.text) });

            if (response.Error != 0)
            {
                tipsText.text = response.Message;
                return;
            }

            tipsText.text = $"坐到{response.ChairId}位置成功";
        }

        //其他玩家坐下广播
        [MessageHandler]
        public class OtherSitDown_Ntt : AMHandler<Actor_OtherPlayerSitDown_Ntt>
        {
            protected override void Run(ETModel.Session session, Actor_OtherPlayerSitDown_Ntt message)
            {
                Debug.Log($"玩家{message.UserId} 坐在了{message.PlayerData.ChairId}位置！！！");
            }
        }

        #endregion

        #region  站起

        private async void OnStandUpBtn()
        {
            G2C_StandUp_Res response = (G2C_StandUp_Res)await SessionComponent.Instance.Session.Call
            (new C2G_StandUp_Req() { GameId = 2, AreaId = 201, UserId = userId, ChairId = int.Parse(changerUserInfoIF.text) });

            if (response.Error != 0)
            {
                tipsText.text = response.Message;
                return;
            }

            tipsText.text = $"从{response.ChairId}位置站起成功";
        }

        //其他玩家站起广播
        [MessageHandler]
        public class OtherStandUp_Ntt : AMHandler<Actor_OtherPlayerStand_Ntt>
        {
            protected override void Run(ETModel.Session session, Actor_OtherPlayerStand_Ntt message)
            {
                Debug.Log($"玩家{message.UserId} 从{message.ChairId}位置站起来了！！！");
            }
        }

        #endregion

        #region  请求上庄

        private async void OnShangZhuangBtn()
        {
            G2C_ShangZhuang_Res response = (G2C_ShangZhuang_Res)await SessionComponent.Instance.Session.Call
                     (new C2G_ShangZhuang_Req() { GameId = 2, AreaId = 201, UserId = userId });

            if (response.Message == "0")
                tipsText.text = "上庄成功！！！";
            else if (response.Message == "2")
                tipsText.text = "你的金钱不够上庄！！！";
            else
                tipsText.text = $"已经加入上庄排队列表 ,你当前排在第{response.WaitIndex}位";
        }

        //其他玩家上庄
        [MessageHandler]
        public class OtherShangZhuang_Ntt : AMHandler<Actor_OtherPlayerShangZhuang_Ntt>
        {
            protected override void Run(ETModel.Session session, Actor_OtherPlayerShangZhuang_Ntt message)
            {
                Debug.Log($"玩家{message.UserId} 上庄成功！！！");
            }
        }

        #endregion

        #region  请求下庄

        private async void OnXiaZhuangBtn()
        {
            G2C_XiaZhuang_Res response = (G2C_XiaZhuang_Res)await SessionComponent.Instance.Session.Call
                      (new C2G_XiaZhuang_Req() { GameId = 2, AreaId = 201, UserId = userId });
            switch (response.Message)
            {
                case "0":
                    tipsText.text = "下庄成功 换其他玩家上庄";
                    break;
                case "1":
                    tipsText.text = "下庄成功 现在是系统庄家";
                    break;
                case "2":
                    tipsText.text = "你不是庄家 不能发下庄请求！！！";
                    break;
            }
        }

        //其他玩家上庄
        [MessageHandler]
        public class OtherXiaZhuang_Ntt : AMHandler<Actor_OtherPlayerXiaZhuang_Ntt>
        {
            protected override void Run(ETModel.Session session, Actor_OtherPlayerXiaZhuang_Ntt message)
            {
                //Debug.Log($"玩家{message.UserId} 下庄成功！！！");
            }
        }

        #endregion

        #region 结算

        //其他玩家上庄
        [MessageHandler]
        public class Settlement_Ntt : AMHandler<Actor_Settlement_Ntt>
        {
            protected override void Run(ETModel.Session session, Actor_Settlement_Ntt message)
            {
                //Debug.Log($"庄家的得分是是{message.BankerScore},有座玩家的得分是{message.SeatsScore},无座玩家的得分是{message.NoSeat}");

                //foreach (var player in message.PlayerData)
                //{
                //    if(player.UserId== UIPokerTestCpt.UserInfo.PlayerId)
                //        Debug.Log($"这局自己的得分是{player.Score}");
                //}

            }
        }

        #endregion

        #region 炸金花扣取底分通知

        [MessageHandler]
        public class BottomScore_Ntt : AMHandler<Actor_BottomScore_Ntt>
        {
            protected override void Run(ETModel.Session session, Actor_BottomScore_Ntt message)
            {
                Debug.Log($"炸金花房进行底分押注操作");
            }
        }


        #endregion

        #region 通知玩家开始进行操作 

        public class PlayerOp_Ntt : AMHandler<Actor_ZjhTurnPlayer_Ntt>
        {
            protected override void Run(ETModel.Session session, Actor_ZjhTurnPlayer_Ntt message)
            {
                Debug.Log($"{message.PlayerId}可以进行操作");
            }
        }

        #endregion

        #region 玩家轮循通知
        public class ZjhTurn_Ntt : AMHandler<Actor_ZjhTurnPlayer_Ntt>
        {
            protected override void Run(ETModel.Session session, Actor_ZjhTurnPlayer_Ntt message)
            {
                Debug.Log($"轮到{message.PlayerId}玩家进行操作");
            }
        }


        #endregion

        #region 炸金花结算处理

        public class ZjhSettlement : AMHandler<Actor_ZjhSettlement_Req_Ntt>
        {
            protected override void Run(ETModel.Session session, Actor_ZjhSettlement_Req_Ntt message)
            {
                Debug.Log($"收到结算消息 赢得玩家id是{message.WinId}");
            }
        }


        #endregion

        #region 炸金花看牌

        private async void OnLookCardButton()
        {
            G2C_LookCard_Res response = (G2C_LookCard_Res)await SessionComponent.Instance.Session.Call
                      (new C2G_LookCard_Req() { GameId = 1, AreaId = 101, UserId = userId });

            if (response.Error != 0)
            {
                tipsText.text = "看牌失败！！！";
                return;
            }
            tipsText.text = $"收到牌的牌型是{response.Card.PaiXing}";
        }


        #endregion

        #region 炸金花弃牌

        private async void OnDiscardButton()
        {
            G2C_Discards_Res response = (G2C_Discards_Res)await SessionComponent.Instance.Session.Call
             (new C2G_Discards_Req() { GameId = 1, AreaId = 101, UserId = userId });

            if (response.Error != 0)
            {
                tipsText.text = "弃牌失败！！！";
                return;
            }
            tipsText.text = "弃牌成功!!!";
        }

        #endregion

        #region 炸金花比牌

        private async void OnComparisonCardButton()
        {
            G2C_ComparisonCard_Req_Res response = (G2C_ComparisonCard_Req_Res)await SessionComponent.Instance.Session.Call
          (new C2G_ComparisonCard_Req() { GameId = 1, AreaId = 101, UserId = userId, OtherPlayer = 123456 });

            if (response.Error != 0)
            {
                tipsText.text = "弃牌失败！！！";
                return;
            }
            if (response.Result)
                tipsText.text = "恭喜你 比牌赢了";
            else
                tipsText.text = "比牌输了,下吧加油鸭";
        }

        #endregion

        #endregion
    }

}
*/