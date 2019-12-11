using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIRegisterCptAwakeSystem : AwakeSystem<UIRegisterCpt, GameObject>
    {
        public override void Awake(UIRegisterCpt self, GameObject a)
        {
            self.Awake(a);
        }
    }

    [ObjectSystem]
    public class UIRegisterCptStartSystem : StartSystem<UIRegisterCpt>
    {
        public override void Start(UIRegisterCpt self)
        {
            self.Start();
        }
    }

    public class UIRegisterCpt : Entity
    {
        private GameObject panelGo;

        private ReferenceCollector ReferenceCollector;

        private InputField accoutIF;

        private InputField passIF;

        private InputField repeatIF;

        private Text _accountTipsText;
        private Text _passwordTipsText;
        private Text _captchaTipsText;

        private bool _accountVertify;
        private bool _passwordVertify;
        private bool _captchaVertify;

        private RegisterCaptchaCpt _captchaCpt;

        private bool _isRequest;

        public void Awake(GameObject a)
        {
            panelGo = a;
            //            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            //            resourcesComponent.LoadBundle(UIType.Register.StringToAB());
            //            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(UIType.Register.StringToAB(), UIType.Register);
            //            panelGo = UnityEngine.Object.Instantiate(bundleGameObject, Parent.Parent.GameObject.transform);

            ReferenceCollector = panelGo.GetComponent<ReferenceCollector>();

            _captchaCpt = AddComponent<RegisterCaptchaCpt, ReferenceCollector>(ReferenceCollector);

            accoutIF = ReferenceCollector.GetComponent<InputField>("accountIF");
            accoutIF.onEndEdit.AddListener(OnEndEditAccountInputField);

            passIF = ReferenceCollector.GetComponent<InputField>("passwordIF");
            passIF.onEndEdit.AddListener(OnEndEditPasswordInputField);

            repeatIF = ReferenceCollector.GetComponent<InputField>("repeatIF");
            repeatIF.onEndEdit.AddListener(OnEndEditCaptchaInputField);

            _accountTipsText = ReferenceCollector.GetComponent<Text>("TipsTxt");
            _passwordTipsText = ReferenceCollector.GetComponent<Text>("TipsTxt2");
            _captchaTipsText = ReferenceCollector.GetComponent<Text>("TipsTxt3");

            var registerButton = ReferenceCollector.GetComponent<Button>("RegisterButton");

            registerButton.onClick.AddListener(OnRegisterButton);

            var GetVerifiCodeButton = ReferenceCollector.GetComponent<Button>("GetVerifiCodeButton");

            GetVerifiCodeButton.onClick.AddListener(OnGetVerifiCodeButton);

            var backButton = ReferenceCollector.GetComponent<Button>("BackButton");

            backButton.onClick.AddListener(OnBackButton);
        }

        /// <summary>
        /// 账号输入结束检测
        /// </summary>
        /// <param name="arg0"></param>
        private void OnEndEditAccountInputField(string arg0)
        {
            var res = GameHelper.VerifyAccountText(arg0.Trim(), out var verifyMsg);
            _accountTipsText.text = res ? "" : verifyMsg;
            _accountVertify = res;
        }

        /// <summary>
        /// 密码输入结束检测
        /// </summary>
        /// <param name="arg0"></param>
        private void OnEndEditPasswordInputField(string arg0)
        {
            var res = GameHelper.VerifyPasswordText(arg0, out var verifyMsg);
            _passwordTipsText.text = res ? "" : verifyMsg;
            _passwordVertify = res;
        }

        /// <summary>
        /// 验证码输入结束检测
        /// </summary>
        /// <param name="arg0"></param>
        private void OnEndEditCaptchaInputField(string arg0)
        {
            var validation = _captchaCpt.Validation(repeatIF.text);
            _captchaTipsText.text = validation;
            _captchaVertify = string.IsNullOrEmpty(validation);
        }

        public void Start()
        {
            ComponentFactory.CreateWithParent<MobileInputFieldAdaptionCpt, InputField>(this, accoutIF);
            ComponentFactory.CreateWithParent<MobileInputFieldAdaptionCpt, InputField>(this, passIF);
            ComponentFactory.CreateWithParent<MobileInputFieldAdaptionCpt, InputField>(this, repeatIF);
        }

        public void Open(string account, string password)
        {
            accoutIF.text = account;
            passIF.text = password;
            OnGetVerifiCodeButton();
            this.panelGo.SetActive(true);
        }

        /// <summary>
        /// 返回登录页面
        /// </summary>
        private void OnBackButton()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            accoutIF.text = "";
            passIF.text = "";
            repeatIF.text = "";
            _accountTipsText.text = "";
            _passwordTipsText.text = "";
            _captchaTipsText.text = "";

            this.panelGo.SetActive(false);
            _captchaCpt.HideCaptcha();
            //GetParent<UILoginCpt>().LoginView.SetActive(true);
        }

        /// <summary>
        /// 点击注册按钮
        /// </summary>
        private async void OnRegisterButton()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            if (_isRequest)
            {
                Game.PopupComponent.ShowTips(DataCenterComponent.Instance.tipInfo.IsLoadingTip);
                return;
            }

            OnEndEditAccountInputField(accoutIF.text);
            OnEndEditPasswordInputField(passIF.text);
            OnEndEditCaptchaInputField(repeatIF.text);

            if (!_accountVertify || !_passwordVertify || !_captchaVertify) return;

            Game.PopupComponent.ShowLoadingLockUI();
            _isRequest = true;
            try
            {
                // 创建一个ETModel层的Session
                ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>()
                    .Create(GlobalConfigComponent.Instance.GlobalProto.Address);

                // 创建一个ETHotfix层的Session, ETHotfix的Session会通过ETModel层的Session发送消息
                Session realmSession = ComponentFactory.Create<Session, ETModel.Session>(session);

                // 发送注册请求
                R2C_Register_Res r2CRegisterRes = (R2C_Register_Res)await realmSession.Call(
                    new C2R_Register_Req()
                    {
                        Account = this.accoutIF.text,
                        Password = this.passIF.text
                    });

                if (r2CRegisterRes.Error != 0)
                {
                    Game.PopupComponent.ShowMessageBox(r2CRegisterRes.Message);
                    //                    GameHelper.ShowMessageBox(r2CRegisterRes.Message);
                }
                else if (r2CRegisterRes.Error == 0)
                {
                    //注册上报
                    var openInstall = ETModel.Game.Scene.GetComponent<OpenInstallComponent>();
                    openInstall.reportRegister();

                    // 登录
                    GamePrefs.SetUserAccount(accoutIF.text);
                    GamePrefs.SetUserPwd(passIF.text);
                    //                    OnBackButton();
                    //this.GetParent<UILoginCpt>().RegisterLogin();

                }
                Game.PopupComponent.CloseLoadingLockUI();
            }
            catch (Exception e)
            {
                Game.PopupComponent.CloseLoadingLockUI();
                Debug.LogError("无法连接到服务器: " + e.Message);
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.ConnectServerFailTip);
            }
            finally
            {
                Game.PopupComponent.CloseLoadingLockUI();
                _isRequest = false;
            }
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        private void OnGetVerifiCodeButton()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            _captchaCpt.ShowCaptcha();
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
        }
    }
}