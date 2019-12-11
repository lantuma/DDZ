using System;
using System.Text.RegularExpressions;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UserCenter_UserInfoCptAwakeSystem : AwakeSystem<UserCenter_UserInfoCpt, GameObject, GameObject, ButtonGroup>
    {
        public override void Awake(UserCenter_UserInfoCpt self, GameObject button, GameObject panel, ButtonGroup group)
        {
            self.Awake(button, panel, group);
        }
    }

    [ObjectSystem]
    public class UserCenter_UserInfoCptStartSystem : StartSystem<UserCenter_UserInfoCpt>
    {
        public override void Start(UserCenter_UserInfoCpt self)
        {
            self.Start();
        }
    }

    public class UserCenter_UserInfoCpt : SimpleToggle
    {
        #region 引用

        public Text BindPhoneNumLab;
        private Text IDLab;
        private Text GoldTxtLab;
        private Text PlayerName;
        private Image HeadIcon;
        private GameObject ChangeHeadView;
        private GameObject NormalInfoView;
        private GameObject ChangeNameView;

        private Transform ChangeHeadObjs;
        private GameObject LastSelectHead;
        private int ChangeHeadIndex;

        private InputField _changeNameInput;
        private GameObject _confirmChangeName;
        private GameObject _changePlayerName;
        private GameObject _cancelChangeName;

        private GameObject UpgradeBtn;
        private GameObject ChangePwdBtn;
        private Vector3 pwdBtnOldPos;

        private Image changeHeadIcon;
        private InputField NameInputField;

        private bool _isRequest;

        #endregion

        public override void Init()
        {
            base.Init();

            ChangeHeadView = ReferenceCollector.Get<GameObject>("ChangeHeadView");
            ChangeNameView = ReferenceCollector.Get<GameObject>("ChangeNameView");
            NormalInfoView = ReferenceCollector.Get<GameObject>("NormalInfoView");
            _changePlayerName = ReferenceCollector.Get<GameObject>("ChangePlayerNameBtn");
            _confirmChangeName = ReferenceCollector.Get<GameObject>("ConfirmChangeName");
            _cancelChangeName = ReferenceCollector.Get<GameObject>("CancelChangeName");

            HeadIcon = ReferenceCollector.GetComponent<Image>("HeadIcon");
            PlayerName = ReferenceCollector.GetComponent<Text>("PlayerName");
            GoldTxtLab = ReferenceCollector.GetComponent<Text>("GoldTxtLab");
            IDLab = ReferenceCollector.GetComponent<Text>("IDLab");
            BindPhoneNumLab = ReferenceCollector.GetComponent<Text>("BindPhoneNumLab");

            _changeNameInput = ReferenceCollector.GetComponent<InputField>("ChangeNameInput");

            _changePlayerName.GetComponent<Button>().onClick.AddListener(OnChangeUserName);
            _confirmChangeName.GetComponent<Button>().onClick.AddListener(OnConfirmChangeName);
            _cancelChangeName.GetComponent<Button>().onClick.AddListener(OnCancelChangeName);
            ReferenceCollector.GetComponent<Button>("ChangeHeadBtn").onClick.AddListener(OnChangeHead);
            ReferenceCollector.GetComponent<Button>("CopyBtn").onClick.AddListener(OnCopyUserId);
            ReferenceCollector.GetComponent<Button>("ConfirmChangeHeadBtn").onClick.AddListener(OnConfirmChangeHead);
            ReferenceCollector.GetComponent<Button>("UpgradeBtn").onClick.AddListener(OnUpgradeBtn);
            ReferenceCollector.GetComponent<Button>("ChangePwdBtn").onClick.AddListener(OnChangePwd);
            ReferenceCollector.GetComponent<Button>("CloseChangeHead").onClick.Add(OnCloseChangeHead);

            changeHeadIcon = ReferenceCollector.GetComponent<Image>("changeHeadIcon");
            NameInputField = ReferenceCollector.GetComponent<InputField>("NameInputField");

            ButtonHelper.RegisterButtonEvent(ReferenceCollector, "SaveNameBtn", () =>
            {
                this.OnConfirmChangeName();
            });

            ButtonHelper.RegisterButtonEvent(ReferenceCollector, "CloseChangeName", () =>
            {
                ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);

                this.ChangeNameView.SetActive(false);

            });

            ChangeHeadObjs = ReferenceCollector.Get<GameObject>("ChangeHeadObjs").transform;
            for (int i = 0; i < ChangeHeadObjs.childCount; i++)
            {
                var index = i + 0;
                var btn = ChangeHeadObjs.GetChild(i);
                btn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
                    SelectChangeHead(btn.GetChild(2).gameObject, index);
                    
                });
            }

            ReferenceCollector.GetComponent<Button>("mask").onClick.AddListener(OnClickSubClose);

            UpgradeBtn = ReferenceCollector.Get<GameObject>("UpgradeBtn");

            ChangePwdBtn = ReferenceCollector.Get<GameObject>("ChangePwdBtn");

            pwdBtnOldPos = ChangePwdBtn.transform.localPosition;

            UpgradeBtn.SetActive(false);

            ChangePwdBtn.transform.localPosition = new Vector3(pwdBtnOldPos.x - 200, pwdBtnOldPos.y, 0);

            Initial();
        }

        public void OnClickSubClose()
        {
            GetComponent<UserCenter_ChangePwdCpt>()?.OnClose();
            GetComponent<UserCenter_UpgradeCpt>()?.OnClose();

            ReferenceCollector.Get<GameObject>("mask").SetActive(false);
        }

        public void Start()
        {
            ComponentFactory.CreateWithParent<MobileInputFieldAdaptionCpt, InputField>(this, _changeNameInput);
        }

        /// <summary>
        /// 打开修改密码界面
        /// </summary>
        private void OnChangePwd()
        {
            ReferenceCollector.Get<GameObject>("mask").SetActive(true);
            var changePwdCpt = GetComponent<UserCenter_ChangePwdCpt>() ?? AddComponent<UserCenter_ChangePwdCpt, Transform>(panelGo.transform);
            changePwdCpt.ShowPanel();
        }

        /// <summary>
        /// 显示或者隐藏升级按钮
        /// </summary>
        /// <param name="show"></param>
        public void ShowOrHideUpgradeAndChangePwdBtn(bool show)
        {
            ReferenceCollector.Get<GameObject>("UpgradeBtn")?.SetActive(show);
            ReferenceCollector.Get<GameObject>("ChangePwdBtn")?.SetActive(!show);

            //            if (UpgradeBtn.activeSelf)
            //            {
            //                ChangePwdBtn.transform.localPosition = pwdBtnOldPos;
            //            }
            //            else
            //            {
            //                ChangePwdBtn.transform.localPosition = new Vector3(pwdBtnOldPos.x - 200, pwdBtnOldPos.y, 0);
            //            }

        }

        /// <summary>
        /// 打开升级页面
        /// </summary>
        private void OnUpgradeBtn()
        {
            ReferenceCollector.Get<GameObject>("mask").SetActive(true);
            var upgradeCpt = GetComponent<UserCenter_UpgradeCpt>() ?? AddComponent<UserCenter_UpgradeCpt, Transform>(panelGo.transform);
            upgradeCpt.ShowPanel();
        }

        /// <summary>
        /// 取消修改用户名
        /// </summary>
        private void OnCancelChangeName()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            PlayerName.gameObject.SetActive(true);
            _changePlayerName.SetActive(true);
            _changeNameInput.text = "";
            _changeNameInput.gameObject.SetActive(false);
            _confirmChangeName.SetActive(false);
            _cancelChangeName.SetActive(false);
        }

        //        // 1.纯汉字,2-8位
        //        // 2.字母或数字,4-8位
        //        /// <summary>
        //        /// 昵称匹配正则表达式
        //        /// </summary>
        //        private string _nameRegexStr = @"^[\u4E00-\u9FA5]{2,8}$|^[A-Za-z0-9]{4,8}$";
        //
        //        // 3_a.汉字+字母或数字,共3-8位
        //        private string _nameRegexStrGroupA = @"^[\u4E00-\u9FA5A-Za-z0-9]{3,8}$";
        //        // 3_b.至少有一个汉字
        //        private string _nameRegexStrGroupB = @"[\u4E00-\u9FA5]";

        private string _nameRegexStr = @"^[\u4E00-\u9FA5A-Za-z0-9]{3,6}$";

        /// <summary>
        /// 确认修改用户名
        /// </summary>
        private async void OnConfirmChangeName()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);

            var changeName = NameInputField.text.Trim();
            if (string.IsNullOrEmpty(changeName) || changeName.Equals(PlayerName.text))
            {
                Game.PopupComponent.ShowMessageBox("昵称不可为空");
                OnCancelChangeName();
                return;
            }

            var isMatch = Regex.IsMatch(NameInputField.text, _nameRegexStr);
//            if (!isMatch)
//            {
//                var matchA = Regex.IsMatch(_changeNameInput.text, _nameRegexStrGroupA);
//                if (matchA)
//                {
//                    // 找到了汉字和字母数字的混合
//                    isMatch = Regex.IsMatch(_changeNameInput.text, _nameRegexStrGroupB);
//                }
//            }

            if (!isMatch)
            {
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.NickNameNotMatchTip);
                return;
            }

            if (_isRequest)
            {
                Game.PopupComponent.ShowTips(DataCenterComponent.Instance.tipInfo.NotRepeatModifyTip);
                return;
            }

            _isRequest = true;
            try
            {
                Game.PopupComponent.ShowLoadingLockUI();
                G2C_ChangerUserInfo_Res chagerHead = (G2C_ChangerUserInfo_Res)await SessionComponent.Instance.Session.
                    Call(new C2G_ChangerUserInfo_Req()
                    {
                        UserId = GamePrefs.GetUserId(),
                        Type = 1,
                        NickName = changeName
                    });

                if (chagerHead.Error != 0)
                {
                    Debug.Log(chagerHead.Message);
                    //                    GameHelper.ShowMessageBox(chagerHead.Message);
                    Game.PopupComponent.ShowMessageBox(chagerHead.Message);
                    _isRequest = false;
                    Game.PopupComponent.CloseLoadingLockUI();
                    return;
                }

                _isRequest = false;
                await UserDataHelper.GetUserInfo(true);
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.ModifyNickNameSuccTip);
                PlayerName.gameObject.SetActive(true);
                _changePlayerName.SetActive(true);
                _changeNameInput.text = "";
                _changeNameInput.gameObject.SetActive(false);
                _confirmChangeName.SetActive(false);
                _cancelChangeName.SetActive(false);
                ChangeNameView.SetActive(false);
                Game.PopupComponent.CloseLoadingLockUI();
            }
            catch (Exception e)
            {
                Log.Debug($"修改昵称错误:{e.Message}");
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.ModifyNickNameFailTip);
                _isRequest = false;
                Game.PopupComponent.CloseLoadingLockUI();
            }
        }

        /// <summary>
        /// 取消修改头像
        /// </summary>
        private void OnCloseChangeHead()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);

            //修改:关闭的时候修改头像

            this.OnConfirmChangeHead();

            ChangeHeadView.SetActive(false);
            NormalInfoView.SetActive(true);
        }

        public override void ChangeToggle(bool state)
        {
            base.ChangeToggle(state);
        }

        public async void Initial()
        {
            await UserDataHelper.AddDependView(new UserDataHelper.UserInfoView()
            {
                GoldText = GoldTxtLab,
                HeadImg = HeadIcon,
                NickNameText = PlayerName,
                UserIdText = IDLab,
                SexGo = ReferenceCollector.Get<GameObject>("SexGroup")
            });

            if (UserDataHelper.UserInfo.IsTourist) ShowOrHideUpgradeAndChangePwdBtn(true);

            if (UserDataHelper.UserInfo.PhoneNumber != 0)
            {
                BindPhoneNumLab.text = UserDataHelper.UserInfo.PhoneNumber.ToString();
            }
            else
            {
                BindPhoneNumLab.text = DataCenterComponent.Instance.tipInfo.NoBindPhoneNumTip;
            }
        }

        /// <summary>
        /// 选择头像
        /// </summary>
        /// <param name="onOff"></param>
        /// <param name="index"></param>
        private void SelectChangeHead(GameObject onOff, int index)
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            ChangeHeadIndex = index;
            LastSelectHead?.SetActive(false);
            onOff.SetActive(true);
            LastSelectHead = onOff;

            //修改
            changeHeadIcon.sprite = onOff.transform.parent.Find("icon").GetComponent<Image>().sprite;
        }

        /// <summary>
        /// 确认切换头像
        /// </summary>
        private async void OnConfirmChangeHead()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);

            if (ChangeHeadIndex == UserDataHelper.UserInfo.HeadId)
            {
                ChangeHeadView.SetActive(false);
                NormalInfoView.SetActive(true);
                return;
            }

            if (_isRequest)
            {
                Game.PopupComponent.ShowTips(DataCenterComponent.Instance.tipInfo.NotRepeatModifyTip);
                return;
            }

            _isRequest = true;
            try
            {
                G2C_ChangerUserInfo_Res chagerHead = (G2C_ChangerUserInfo_Res)await SessionComponent.Instance.Session.
                    Call(new C2G_ChangerUserInfo_Req()
                    {
                        UserId = GamePrefs.GetUserId(),
                        Type = 0,
                        HeadId = ChangeHeadIndex
                    });

                if (chagerHead.Error != 0)
                {
                    Debug.Log(chagerHead.Message);
                    //                    GameHelper.ShowMessageBox(chagerHead.Message);
                    Game.PopupComponent.ShowMessageBox(chagerHead.Message);
                    _isRequest = false;
                    return;
                }

                ChangeHeadView.SetActive(false);
                NormalInfoView.SetActive(true);
                _isRequest = false;
            }
            catch (Exception e)
            {
                Log.Debug("修改头像错误:" + e.Message);
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.ModifyHeadIdFailTip);
                _isRequest = false;
            }

            //            GameHelper.ShowMessageBox("修改头像成功!");
            Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.ModifyHeadIdSuccTip);

            // 刷新显示
            await UserDataHelper.GetUserInfo(true);
        }

        /// <summary>
        /// 修改用户名
        /// </summary>
        private void OnChangeUserName()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            // _cancelChangeName.SetActive(true);
            // PlayerName.gameObject.SetActive(false);
            // _changePlayerName.SetActive(false);
            // _changeNameInput.text = PlayerName.text;
            // _changeNameInput.gameObject.SetActive(true);
            // _confirmChangeName.SetActive(true);

            ChangeNameView.SetActive(true);
        }

        /// <summary>
        /// 复制UserId
        /// </summary>
        private void OnCopyUserId()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            GUIUtility.systemCopyBuffer = IDLab.text;
            //            GameHelper.ShowMessageBox("复制ID成功!");
            Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.CopyIDSuccTip);
        }

        /// <summary>
        /// 打开修改头像页面
        /// </summary>
        private async void OnChangeHead()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            NormalInfoView.SetActive(false);
            ChangeHeadView.SetActive(true);

            // 选择默认的头像
            var userInfo = await UserDataHelper.GetUserInfo();

            if (ChangeHeadIndex != 0 && ChangeHeadIndex == userInfo.HeadId) return;
            LastSelectHead?.SetActive(false);

            ChangeHeadIndex = userInfo.HeadId;
            LastSelectHead = ChangeHeadObjs.GetChild(userInfo.HeadId - 0).GetChild(2).gameObject;
            LastSelectHead.SetActive(true);

            //修改
            changeHeadIcon.sprite = LastSelectHead.transform.parent.Find("icon").GetComponent<Image>().sprite;
        }
    }
}