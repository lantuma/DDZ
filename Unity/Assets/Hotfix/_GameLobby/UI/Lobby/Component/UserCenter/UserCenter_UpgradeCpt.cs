using System;
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UserCenter_UpgradeCptAwakeSystem : AwakeSystem<UserCenter_UpgradeCpt, Transform>
    {
        public override void Awake(UserCenter_UpgradeCpt self, Transform a)
        {
            self.Awake(a);
        }
    }

    [ObjectSystem]
    public class UserCenter_UpgradeCptStartSystem : StartSystem<UserCenter_UpgradeCpt>
    {
        public override void Start(UserCenter_UpgradeCpt self)
        {
            self.Start();
        }
    }

    public class UserCenter_UpgradeCpt : Entity
    {
        private GameObject panelGo;
        private ReferenceCollector Rc;

        private RegisterCaptchaCpt _captchaCpt;

        #region Reference

        private Tweener _panelTweener;

        private InputField _accountInput;
        private InputField _passwordInput;
        private InputField _captchaInput;

        private Text _accountTipsText;
        private Text _passwordTipsText;
        private Text _captchaTipsText;

        private bool _accountVertify;
        private bool _passwordVertify;
        private bool _captchaVertify;

        #endregion

        private bool _isRequest;

        public void Awake(Transform parent)
        {
            var resCpt = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            var gameObject = resCpt.GetAsset(UIType.UserCenterWin.StringToAB(), UIType.UpgradePanel);

            panelGo = (GameObject)UnityEngine.Object.Instantiate(gameObject, parent);
            Rc = panelGo.GetComponent<ReferenceCollector>();

            _panelTweener = panelGo.transform.DOScale(Vector3.one, 0.3f).Pause().SetEase(Ease.OutBack)
                .SetAutoKill(false);

            _captchaCpt = AddComponent<RegisterCaptchaCpt, ReferenceCollector>(Rc);

            _accountInput = Rc.GetComponent<InputField>("AccountInput");
            _accountInput.onEndEdit.AddListener(OnEndEditAccountInputField);

            _passwordInput = Rc.GetComponent<InputField>("PasswordInput");
            _passwordInput.onEndEdit.AddListener(OnEndEditPasswordInputField);

            _captchaInput = Rc.GetComponent<InputField>("CaptchaInput");
            _captchaInput.onEndEdit.AddListener(OnEndEditCaptchaInputField);

            _accountTipsText = Rc.GetComponent<Text>("AccountTipsText");
            _passwordTipsText = Rc.GetComponent<Text>("PasswordTipsText");
            _captchaTipsText = Rc.GetComponent<Text>("CaptchaTipsText");

            Rc.GetComponent<Button>("CloseBtn").onClick.AddListener(OnCloseMask);
            Rc.GetComponent<Button>("GetVerifiCodeButton").onClick.AddListener(OnVerifiCode);
        }

        public void Start()
        {
            ComponentFactory.CreateWithParent<MobileInputFieldAdaptionCpt, InputField>(this, _accountInput);
            ComponentFactory.CreateWithParent<MobileInputFieldAdaptionCpt, InputField>(this, _passwordInput);
            ComponentFactory.CreateWithParent<MobileInputFieldAdaptionCpt, InputField>(this, _captchaInput);
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
            var validation = _captchaCpt.Validation(_captchaInput.text);
            _captchaTipsText.text = validation;
            _captchaVertify = string.IsNullOrEmpty(validation);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        private void OnVerifiCode()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            _captchaCpt.ShowCaptcha();
        }

        private void OnCloseMask()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
            GetParent<UserCenter_UserInfoCpt>().OnClickSubClose();
        }

        public async void OnClose()
        {
            if (!panelGo.activeSelf) return;
            _panelTweener.PlayBackwards();
            await Task.Delay(300);

            _accountInput.text = "";
            _passwordInput.text = "";
            _captchaInput.text = "";
            _accountTipsText.text = "";
            _passwordTipsText.text = "";
            _captchaTipsText.text = "";

            _captchaCpt.HideCaptcha();


            panelGo?.SetActive(false);
        }

        public void ShowPanel()
        {
            OnVerifiCode();
            panelGo.transform.localScale = Vector3.zero;
            panelGo?.SetActive(true);
            _panelTweener.PlayForward();
        }

        public override void Dispose()
        {
            base.Dispose();
            _panelTweener?.Kill();
        }
    }
}