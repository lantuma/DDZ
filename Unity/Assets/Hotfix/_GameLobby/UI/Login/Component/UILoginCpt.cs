using DG.Tweening;
using ETModel;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UILoginCptAwakeSystem : AwakeSystem<UILoginCpt, bool>
    {
        public override void Awake(UILoginCpt self, bool a)
        {
            self.Awake(a);
        }
    }

    [ObjectSystem]
    public class UILoginCptStartSystem : StartSystem<UILoginCpt>
    {
        public override void Start(UILoginCpt self)
        {
            self.Start();
        }
    }

    public class UILoginCpt : Entity
    {
        private ReferenceCollector rc;

        private GameObject _FixDialog;
        private GameObject _ChooseDialog;
        private GameObject _LoginDialog;

        private bool _isRequest;
        public bool IsLoginOut = false;

        //修复面板属性
        private Text _FixVersionText;
        private Text _AppVersionText;
        private Text _ResouceVersionText;

        //注册登录面板属性
        private GameObject _RegisterDialogBtn;
        private GameObject _LoginDialogBtn;
        private GameObject _RegisterTitleImage;
        private GameObject _LoginTitleImage;
        private InputField _CodeInputField;
        private InputField _MobileInputField;

        private GameObject RegisterBtn01;
        private GameObject RegisterBtn02;
        private GameObject RegisterBtn03;
        private GameObject LoginBtn01;
        private GameObject LoginBtn02;
        private GameObject LoginBtn03;

        /// <summary>
        /// 当前选中账号索引
        /// </summary>
        private int _CurrChooseAccountIndex;

        private bool _IsStartCountDown;
        private int _CountTime;
        private Text _CountTimeText;
        private GameObject GetCodeBtn;

        //动态替换

        public Image LoginBg;
        public Image LoginLogo;

        public void Awake(bool a)
        {
            IsLoginOut = a;

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            ButtonHelper.RegisterButtonEvent(rc, "MobileBtn", OnMobileBtn);
            ButtonHelper.RegisterButtonEvent(rc, "FreeePlayBtn", OnFreeePlayBtn);
            ButtonHelper.RegisterButtonEvent(rc, "RegisterBtn", OnRegisterBtn);
            ButtonHelper.RegisterButtonEvent(rc, "FixBtn", FixBtn);

            _FixDialog = rc.Get<GameObject>("FixDialog");
            _ChooseDialog = rc.Get<GameObject>("ChooseDialog");
            _LoginDialog = rc.Get<GameObject>("LoginDialog");

            LoginBg = _FixDialog.transform.parent.Find("Image").GetComponent<Image>();
            LoginLogo = LoginBg.transform.Find("Image (2)").GetComponent<Image>();


            _FixVersionText = rc.Get<GameObject>("FixVersionText").Get<Text>();
            _AppVersionText = rc.Get<GameObject>("AppVersionText").Get<Text>();
            _ResouceVersionText = rc.Get<GameObject>("ResouceVersionText").Get<Text>();

            ButtonHelper.RegisterButtonEvent(rc, "FixCloseBtn", OnFixCloseBtn);
            ButtonHelper.RegisterButtonEvent(rc, "FixCloseBtn02", OnFixCloseBtn);
            ButtonHelper.RegisterButtonEvent(rc, "OneFixBtn", OnOneFixBtn);
            ButtonHelper.RegisterButtonEvent(rc, "OneGetErrBtn", OnOneGetErrBtn);

            _RegisterDialogBtn = rc.Get<GameObject>("RegisterDialogBtn");
            _LoginDialogBtn = rc.Get<GameObject>("LoginDialogBtn");
            _RegisterTitleImage = rc.Get<GameObject>("RegisterTitleImage");
            _LoginTitleImage = rc.Get<GameObject>("LoginTitleImage");
            _CodeInputField = rc.Get<GameObject>("CodeInputField").Get<InputField>();
            _MobileInputField = rc.Get<GameObject>("MobileInputField").Get<InputField>();

            ButtonHelper.RegisterButtonEvent(rc, "LoginDialogClose", OnLoginDialogClose);
            ButtonHelper.RegisterButtonEvent(rc, "LoginDialogClose02", OnLoginDialogClose);
            ButtonHelper.RegisterButtonEvent(rc, "RegisterDialogBtn", OnRegisterDialogBtn);
            ButtonHelper.RegisterButtonEvent(rc, "LoginDialogBtn", OnLoginDialogBtn);
            ButtonHelper.RegisterButtonEvent(rc, "GetCode", OnGetCode);
            GetCodeBtn = rc.Get<GameObject>("GetCode");
            _CountTimeText = rc.Get<GameObject>("CountTimeText").Get<Text>();

            //-------选择账号面板 
            ButtonHelper.RegisterButtonEvent(rc, "ChooseDialogClose", OnChooseDialogClose);
            ButtonHelper.RegisterButtonEvent(rc, "ChooseDialogClose02", OnChooseDialogClose);
            ButtonHelper.RegisterButtonEvent(rc, "ChooseDialogLoginBtn", OnChooseDialogLoginBtn);
            RegisterBtn01 = rc.Get<GameObject>("RegisterBtn01");
            RegisterBtn02 = rc.Get<GameObject>("RegisterBtn02");
            RegisterBtn03 = rc.Get<GameObject>("RegisterBtn03");
            ButtonHelper.RegisterButtonEvent(rc, "RegisterBtn01", OnRegisterBtn01);
            ButtonHelper.RegisterButtonEvent(rc, "RegisterBtn02", OnRegisterBtn02);
            ButtonHelper.RegisterButtonEvent(rc, "RegisterBtn03", OnRegisterBtn03);
            LoginBtn01 = rc.Get<GameObject>("LoginBtn01");
            LoginBtn02 = rc.Get<GameObject>("LoginBtn02");
            LoginBtn03 = rc.Get<GameObject>("LoginBtn03");
            ButtonHelper.RegisterButtonEvent(rc, "LoginBtn01", OnLoginBtn01);
            ButtonHelper.RegisterButtonEvent(rc, "LoginBtn02", OnLoginBtn02);
            ButtonHelper.RegisterButtonEvent(rc, "LoginBtn03", OnLoginBtn03);
            ButtonHelper.RegisterButtonEvent(rc, "DeleteBtn01", OnDeleteBtn01);
            ButtonHelper.RegisterButtonEvent(rc, "DeleteBtn02", OnDeleteBtn01);
            ButtonHelper.RegisterButtonEvent(rc, "DeleteBtn03", OnDeleteBtn01);
            
            //开始大版本检测
            Game.EventSystem.Run(EventIdType.CheckLargeVersion, new Action(() =>
            {
            }));
        }

        public void Start()
        {
            _FixDialog.SetActive(false);
            _ChooseDialog.SetActive(false);
            _LoginDialog.SetActive(false);

            _AppVersionText.text = "";
            _ResouceVersionText.text = "";

            var webConfig = ClientComponent.Instance.webConfig;
            if (webConfig != null)
            {
                _AppVersionText.text = $"APP版本号：{webConfig.dbbh}";
                _ResouceVersionText.text = $"资源版本号：{webConfig.zybbh}";
                _FixVersionText.text = $"当前版本号：{webConfig.dbbh}";
            }
            
            _IsStartCountDown = true;
            StartRepateCountTime();

            //检测是否需要动态加载图片
            Game.EventSystem.Run(EventIdType.AsyncImageDownload);
        }

        #region ------------------ 倒计时

        /// <summary>
        /// 倒计时处理
        /// </summary>
        private async void StartRepateCountTime()
        {
            while (_IsStartCountDown)
            {
                if (_CountTime > 0)
                {
                    _CountTimeText.text = $"{_CountTime}s";
                    _CountTime--;
                }
                else
                {
                    if (_CountTimeText != null)
                    {
                        _CountTimeText.text = "";
                        _CountTime = 0;
                        _CountTimeText.gameObject.SetActive(false);
                        GetCodeBtn.SetActive(true);
                    }
                }
                await Task.Delay(1000);
            }
        }

        #endregion

        #region ------------------------------试玩登录

        /// <summary>
        /// 试玩登录
        /// </summary>
        private async void OnVisitorLoginButton()
        {
            Game.PopupComponent.SetClickLock();
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);

            if (_isRequest)
            {
                Game.PopupComponent.ShowTips(DataCenterComponent.Instance.tipInfo.IsLoadingTip);
                return;
            }
            _isRequest = true;
            try
            {
                var account = GamePrefs.GetGuestAccount();
                var pwd = GamePrefs.GetGuestPwd();
                if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(pwd))
                {
                    var errorCode = await LoginRequest(account, pwd, true);
                    return;
                }
                Game.PopupComponent.ShowLoadingLockUI();
                ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(GlobalConfigComponent.Instance.GlobalProto.Address);

                // 创建一个ETHotfix层的Session, ETHotfix的Session会通过ETModel层的Session发送消息
                Session realmSession = ComponentFactory.Create<Session, ETModel.Session>(session);

                var response = (G2C_TouristsLogin_Res)await realmSession.Call(new C2G_TouristsLogin_Req());
                if (response.Error != 0)
                {
                    Game.PopupComponent.ShowMessageBox(response.Message);
                    _isRequest = false;
                    Game.PopupComponent.CloseLoadingLockUI();
                    return;
                }
                _isRequest = false;
                GamePrefs.SetGuestAccount(response.Account);
                GamePrefs.SetGuestPwd(response.Password);
                await LoginRequest(response.Account, response.Password, true);
                Game.PopupComponent.CloseLoadingLockUI();
            }
            catch (Exception e)
            {
                _isRequest = false;
                Game.PopupComponent.CloseLoadingLockUI();
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.VisitorLoginFailTip);
                throw;
            }
            finally
            {
                _isRequest = false;
                Game.PopupComponent.CloseLoadingLockUI();
            }
        }

        #endregion

        /// <summary>
        /// 登录请求
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="isGuest"></param>
        private async Task<int> LoginRequest(string account, string password, bool isGuest = false)
        {
            int isScuess = -1;
            Game.PopupComponent.ShowLoadingLockUI();
            _isRequest = true;
            try
            {
                //登录网关服务器
                if (isGuest)
                    isScuess = await LoginHelper.OnLoginAsync(account, password, isGuest);
                else
                    isScuess = await LoginHelper.OnLoginAsyncToken(account, password);
                if (isScuess != 0)
                {
                    _isRequest = false;
                    if (isGuest && isScuess == ErrorCode.ERR_AccountDoesnExist)
                    {
                        AccountIsNotExist();
                        Log.Debug(DataCenterComponent.Instance.tipInfo.AccountNotExistTip);
                        OnVisitorLoginButton();
                        return isScuess;
                    }
                    Game.PopupComponent.CloseLoadingLockUI();
                    if (isScuess == 210006)
                    {
                        //账号不存在，删除本地存储的账号
                        PlayerPrefs.DeleteKey($"Account0{_CurrChooseAccountIndex}");
                        _ChooseDialog.SetActive(false);
                    }
                    if (isScuess == 210005)
                    {
                        _LoginDialog.SetActive(true);
                        SetLoginDialog(true);
                        _MobileInputField.text = account;
                        _CodeInputField.text = "";
                        PlayerPrefs.DeleteKey($"Account0{_CurrChooseAccountIndex}");
                        _ChooseDialog.SetActive(false);

                    }
                    return isScuess;
                }
                var openInstall = ETModel.Game.Scene.GetComponent<OpenInstallComponent>();
                openInstall.getInstall();

                //G2C_ActivityIsOpenReturn_Res resp = (G2C_ActivityIsOpenReturn_Res)await SessionComponent.Instance.Session.Call(new C2G_ActivityIsOpenAsk_Req());

                Game.EventSystem.Run(EventIdType.LoginPokerFinish);
                Game.EventSystem.Run(EventIdType.CreateGameLobby);
                _isRequest = false;
                Game.PopupComponent.CloseLoadingLockUI();
                return isScuess;
            }
            catch (Exception e)
            {
                _isRequest = false;
                Log.Error("无法连接到服务器: " + e.Message);
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.ConnectServerFailTip);
                return isScuess;
            }
            finally
            {
                _isRequest = false;
                Game.PopupComponent.CloseLoadingLockUI();
            }
        }

        /// <summary>
        /// 账号不存在
        /// </summary>
        private void AccountIsNotExist()
        {
            GamePrefs.ClearGuestInfo();
        }

        private void SetLoginDialog(bool isLogin)
        {
            _LoginTitleImage.SetActive(isLogin);
            _RegisterTitleImage.SetActive(!isLogin);
            _LoginDialogBtn.SetActive(isLogin);
            _RegisterDialogBtn.SetActive(!isLogin);
        }
        
        public void GetCode()
        {
            if (_MobileInputField.text == "")
            {
                Game.PopupComponent.ShowMessageBox("手机号不能为空");
                return;
            }
            if (!IsMobilePhone(_MobileInputField.text))
            {
                Game.PopupComponent.ShowMessageBox("请输入正确的手机号码");
                return;
            }
            if (IsHasLocalAccount(_MobileInputField.text))
            {
                var data = new MessageData();
                data.onlyOk = false;
                data.click = true;
                data.ok = RegisterLogin;
                Game.PopupComponent.ShowMessageBox("该账户已保存，是否使用该账号继续登录", data);
                return;
            }
        }

        /// <summary>
        /// 已存在账号登录
        /// </summary>
        private void RegisterLogin()
        {
            OnChooseDialogLoginBtn();
        }

        /// <summary>
        /// 验证是否是手机号
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public bool IsMobilePhone(string msg)
        {
            return Regex.IsMatch(msg, "^(0\\d{2,3}-?\\d{7,8}(-\\d{3,5}){0,1})|(((13[0-9])|(15([0-3]|[5-9]))|(18[0-9])|(17[0-9])|(14[0-9]))\\d{8})$");
        }

        /// <summary>
        /// 获取断线验证码成功回调
        /// </summary>
        private void GetVerifyCodeCallBack()
        {
            _CountTime = 60;
            GetCodeBtn.SetActive(false);
            _CountTimeText.gameObject.SetActive(true);
            _CountTimeText.text = $"{_CountTime}s";
        }

        /// <summary>
        /// 设置选中的状态
        /// </summary>
        private void SetChooseState(int index)
        {
            _ChooseDialog.TryGetInChilds<Transform>("LoginBtn01Image").gameObject.SetActive(false);
            _ChooseDialog.TryGetInChilds<Transform>("LoginBtn02Image").gameObject.SetActive(false);
            _ChooseDialog.TryGetInChilds<Transform>("LoginBtn03Image").gameObject.SetActive(false);
            _ChooseDialog.TryGetInChilds<Transform>($"LoginBtn0{index}Image").gameObject.SetActive(true);

            _CurrChooseAccountIndex = index;
        }

        private void OnLoginBtn()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            _LoginDialog.SetActive(true);
            SetLoginDialog(true);
            _MobileInputField.text = "";
            _CodeInputField.text = "";
        }

        private async void RegisterAccount(string account, string code)
        {
            ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(GlobalConfigComponent.Instance.GlobalProto.Address);
            
            Session realmSession = ComponentFactory.Create<Session, ETModel.Session>(session);
            G2C_MobileLogin_Res mobileLogin = (G2C_MobileLogin_Res)await realmSession.Call(
                new C2G_MobileLogin_Req()
                {
                    Mobile = account,
                    VerifyCode = code,
                });
            if (mobileLogin.Error != 0)
            {
                Game.PopupComponent.ShowMessageBox(mobileLogin.Message);
                return;
            }
            if (!PlayerPrefs.HasKey("Account01"))
            {
                PlayerPrefs.SetString("Account01", $"{mobileLogin.Account}-{mobileLogin.Token}");
            }
            else
            {
                if (!PlayerPrefs.HasKey("Account02"))
                {
                    PlayerPrefs.SetString("Account02", $"{mobileLogin.Account}-{mobileLogin.Token}");
                }
                else
                {
                    if (!PlayerPrefs.HasKey("Account03"))
                    {
                        PlayerPrefs.SetString("Account03", $"{mobileLogin.Account}-{mobileLogin.Token}");
                    }
                }
            }
            LoginRequest(mobileLogin.Account, mobileLogin.Token);
        }

        private void DeleteAccount()
        {
            PlayerPrefs.DeleteKey($"Account0{_CurrChooseAccountIndex}");
            SetAccountState();
        }

        #region -------------------------Button Events

        private void OnDeleteBtn01()
        {
            DeleteAccount();
        }

        /// <summary>
        /// 选择账号界面的登录选择账号按钮1
        /// </summary>
        private void OnLoginBtn01()
        {
            SetChooseState(1);
        }

        private void OnLoginBtn02()
        {
            SetChooseState(2);
        }

        private void OnLoginBtn03()
        {
            SetChooseState(3);
        }

        /// <summary>
        /// 悬着账号界面的登录按钮
        /// </summary>
        private void OnRegisterBtn01()
        {
            _ChooseDialog.SetActive(false);
            OnLoginBtn();
        }

        private void OnRegisterBtn02()
        {
            _ChooseDialog.SetActive(false);
            OnLoginBtn();
        }

        private void OnRegisterBtn03()
        {
            _ChooseDialog.SetActive(false);
            OnLoginBtn();
        }

        /// <summary>
        /// 选择账号界面 登录按钮
        /// </summary>
        private void OnChooseDialogLoginBtn()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
            var account = PlayerPrefs.GetString($"Account0{_CurrChooseAccountIndex}").Split('-');
            LoginRequest(account[0], account[1]);
        }

        private void OnChooseDialogClose()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
            _ChooseDialog.SetActive(false);
        }

        /// <summary>
        /// 获取验证码按钮
        /// </summary>
        private void OnGetCode()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
            GetCode();
        }

        /// <summary>
        /// 注册
        /// </summary>
        private void OnRegisterDialogBtn()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            if (_MobileInputField.text == "")
            {
                Game.PopupComponent.ShowTips("账号不能为空");
                return;
            }
            if (_CodeInputField.text == "")
            {
                Game.PopupComponent.ShowTips("验证码不能为空");
                return;
            }
            RegisterAccount(_MobileInputField.text, _CodeInputField.text);
        }

        private void OnLoginDialogBtn()
        {
            OnRegisterDialogBtn();
        }

        private void OnLoginDialogClose()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            _LoginDialog.SetActive(false);
        }

        /// <summary>
        /// 一键修复
        /// </summary>
        private void OnOneFixBtn()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
        }

        /// <summary>
        /// 一键报错
        /// </summary>
        private void OnOneGetErrBtn()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
        }

        private void OnFixCloseBtn()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            _FixDialog.SetActive(false);
        }

        /// <summary>
        /// 手机登录
        /// </summary>
        private void OnMobileBtn()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            Game.PopupComponent.ShowMessageBox("功能未开放！");
        }

        /// <summary>
        /// 设置选择账号界面 账号数据显示
        /// </summary>
        private void SetAccountState()
        {
            LoginBtn01.SetActive(PlayerPrefs.HasKey("Account01"));
            LoginBtn02.SetActive(PlayerPrefs.HasKey("Account02"));
            LoginBtn03.SetActive(PlayerPrefs.HasKey("Account03"));
            RegisterBtn01.SetActive(!PlayerPrefs.HasKey("Account01"));
            RegisterBtn02.SetActive(!PlayerPrefs.HasKey("Account02"));
            RegisterBtn03.SetActive(!PlayerPrefs.HasKey("Account03"));
            if (PlayerPrefs.HasKey("Account01") && LoginBtn01.activeInHierarchy)
            {
                string account01 = PlayerPrefs.GetString("Account01").Split('-')[0];
                _ChooseDialog.TryGetInChilds<Text>("LoginBtn01Text").text = account01;
            }
            if (PlayerPrefs.HasKey("Account02") && LoginBtn02.activeInHierarchy)
            {
                string account01 = PlayerPrefs.GetString("Account02").Split('-')[0];
                _ChooseDialog.TryGetInChilds<Text>("LoginBtn02Text").text = account01;
            }
            if (PlayerPrefs.HasKey("Account03") && LoginBtn03.activeInHierarchy)
            {
                string account01 = PlayerPrefs.GetString("Account03").Split('-')[0];
                _ChooseDialog.TryGetInChilds<Text>("LoginBtn03Text").text = account01;
            }
            if (PlayerPrefs.HasKey("Account01") && LoginBtn01.activeInHierarchy)
            {
                OnLoginBtn01();
            }
            else if (PlayerPrefs.HasKey("Account02") && LoginBtn02.activeInHierarchy)
            {
                OnLoginBtn02();
            }
            else if (PlayerPrefs.HasKey("Account03") && LoginBtn03.activeInHierarchy)
            {
                OnLoginBtn03();
            }
        }

        /// <summary>
        /// 判断本地是否含有此账号
        /// </summary>
        /// <param name="mobile"></param>
        private bool IsHasLocalAccount(string mobile)
        {
            if (PlayerPrefs.HasKey("Account01") && PlayerPrefs.GetString("Account01").Split('-')[0] == mobile)
            {
                _CurrChooseAccountIndex = 1;
                return true;
            }
            if (PlayerPrefs.HasKey("Account02") && PlayerPrefs.GetString("Account02").Split('-')[0] == mobile)
            {
                _CurrChooseAccountIndex = 2;
                return true;
            }
            if (PlayerPrefs.HasKey("Account03") && PlayerPrefs.GetString("Account03").Split('-')[0] == mobile)
            {
                _CurrChooseAccountIndex = 3;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 免费试玩
        /// </summary>
        private void OnFreeePlayBtn()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            OnVisitorLoginButton();
        }

        /// <summary>
        /// 注册
        /// </summary>
        private void OnRegisterBtn()
        {
            Game.PopupComponent.ShowMessageBox("功能未开放！");
        }

        /// <summary>
        /// 修复
        /// </summary>
        private void FixBtn()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            _FixDialog.SetActive(true);
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
            _IsStartCountDown = false;
            _CountTimeText.text = "";
        }
    }
    
}
