using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public static class UserDataHelper
    {
        public class UserInfoView
        {
            public Image HeadImg;
            public Text NickNameText;
            public Text GoldText;
            public Text UserIdText;
            public GameObject SexGo;
        }

        public static UserInfo UserInfo { get; private set; }

        public static int ChairId
        {
            get { return _chairId; }
            set
            {
                _chairId = value;
                // TODO 更新DataCenter自己的椅子信息
            }
        }

        private static int _chairId = -1;

        private static List<UserInfoView> _dependViews = new List<UserInfoView>();

        public static async ETTask<bool> AddDependView(UserInfoView view, bool request = false)
        {
            if (_dependViews.Contains(view)) return false;

            _dependViews.Add(view);

            await GetUserInfo(request);
            INTERNAL_SET_USERINFOVIEW(view, UserInfo);

            return true;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public static async ETTask<UserInfo> GetUserInfo(bool request = false)
        {
            if (UserInfo != null && !request) return UserInfo;
            if (!await INTERNAL_GET_USERINFO())
            {
                return null;
            }
            //修复金币负数情况下
            if (UserInfo.Gold < 0)
                UserInfo.Gold = 0;
            return UserInfo;
        }

        /// <summary>
        /// 刷新用户信息
        /// </summary>
        /// <returns></returns>
        public static async void RefreshUserInfo()
        {
            await INTERNAL_GET_USERINFO();
            SetMyDataCenter();
        }

        /// <summary>
        /// 设置我的数据
        /// </summary>
        public static void SetMyDataCenter()
        {
            /*
            var myUserVo = DataCenterComponent.Instance.userInfo.getMyUserVo();
            if (myUserVo == null)
            {
                myUserVo = new UserVO();
                DataCenterComponent.Instance.userInfo.httpUserInfo = myUserVo;
                DataCenterComponent.Instance.userInfo.addUser(myUserVo);
            }
            myUserVo.userID = UserInfo.PlayerId;
            myUserVo.headId = UserInfo.HeadId;
            myUserVo.sex = UserInfo.Gender;
            myUserVo.gold = UserInfo.Gold;
            myUserVo.nickName = UserInfo.Nickname;
            */
        }

        /// <summary>
        /// 内部获取用户信息
        /// </summary>
        /// <returns></returns>
        private static async ETTask<bool> INTERNAL_GET_USERINFO()
        {
            Game.PopupComponent.ShowLoadingLockUI();
            try
            {
                var getUserInfoRes = (G2C_GetUserInfo_Res)await SessionComponent.Instance.Session.Call
                    (new C2G_GetUserInfo_Req() { UserId = GamePrefs.GetUserId() });

                if (getUserInfoRes.Error != 0)
                {
                    Debug.LogError(getUserInfoRes.Message);
                    //                    GameHelper.ShowMessageBox(getUserInfoRes.Message);
                    Game.PopupComponent.ShowMessageBox(getUserInfoRes.Message);
                    Game.PopupComponent.CloseLoadingLockUI();
                    return false;
                }

                // 获取用户信息成功
                UserInfo = getUserInfoRes.UserInfo;

                // 设置我的数据
                SetMyDataCenter();

                // 通知所有引用
                for (var i = _dependViews.Count - 1; i >= 0; i--)
                {
                    if (_dependViews[i].HeadImg == null ||
                        _dependViews[i].NickNameText == null ||
                        _dependViews[i].GoldText == null)
                    {
                        _dependViews.RemoveAt(i);
                        continue;
                    }

                    INTERNAL_SET_USERINFOVIEW(_dependViews[i], UserInfo);
                }
                
                Game.PopupComponent.CloseLoadingLockUI();
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.GetPlayerInfoFailTip);
                Game.PopupComponent.CloseLoadingLockUI();
                throw;
            }
        }

        private static void INTERNAL_SET_USERINFOVIEW(UserInfoView view, UserInfo info)
        {
            //修复金币负数情况下
            if (info.Gold < 0)
                info.Gold = 0;
            view.GoldText.text = info.Gold.ToString("F2");
            UserHeadHelper.SetHeadImage(view.HeadImg, info.HeadId);
            view.NickNameText.text = info.Nickname;
            view.UserIdText.text = $"{info.PlayerId:000000}";
            view.SexGo?.transform.GetChild(0).gameObject.SetActive(info.Gender == 1);
            view.SexGo?.transform.GetChild(1).gameObject.SetActive(info.Gender == 2);


            //更新大厅二级界面金币显示
            var uic = Game.Scene.GetComponent<UIComponent>();

            if (uic.Get(UIType.UIHallPanel) != null)
            {
                 var lobbyCpt = uic.Get(UIType.UIHallPanel).GetComponent<GameLobbyCpt>();

                 if(lobbyCpt != null)
                 lobbyCpt.GameLobbyGameTypeSelectPlugin.GameTypeGoldNumberText.text = UserDataHelper.UserInfo.Gold.ToString("F2");
            }

            if (uic.Get(UIType.UIHallPanel) != null)
            {
                var lobbyCpt = uic.Get(UIType.UIHallPanel).GetComponent<GameLobbyCpt>();

                if (lobbyCpt != null)
                {
                }
            }

            //增加对个人中心数据刷新
            if (uic.Get(UIType.UserCenterWin) != null)
            {
                var userCenterCpt = uic.Get(UIType.UserCenterWin).GetComponent<GameUserCenterCpt>();

                if (userCenterCpt != null)
                {
                    userCenterCpt.RefBindPhoneNum();
                }
            }
        }
    }
}