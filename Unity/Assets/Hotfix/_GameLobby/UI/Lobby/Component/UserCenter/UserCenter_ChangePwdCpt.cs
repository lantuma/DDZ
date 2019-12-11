using System;
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UserCenter_ChangePwdCptAwakeSystem : AwakeSystem<UserCenter_ChangePwdCpt, Transform>
    {
        public override void Awake(UserCenter_ChangePwdCpt self, Transform a)
        {
            self.Awake(a);
        }
    }

    [ObjectSystem]
    public class UserCenter_ChangePwdCptStartSystem : StartSystem<UserCenter_ChangePwdCpt>
    {
        public override void Start(UserCenter_ChangePwdCpt self)
        {
            self.Start();
        }
    }

    public class UserCenter_ChangePwdCpt : Component
    {
        private GameObject panelGo;
        private ReferenceCollector Rc;

        #region Reference

        private Tweener _panelTweener;

        private Text _nickNameText;
        private InputField _oldPwdInput;
        private InputField _newPwdInput;
        private InputField _confirmPwdInput;

        private Text _oldPwdTipsText;
        private Text _newPwdTipsText;
        private Text _confirmPwdTipsText;

        private bool _oldPwdVertify;
        private bool _newPwdVertify;
        private bool _confirmPwdVertify;

        #endregion

        private bool _isRequest;

        public void Awake(Transform parent)
        {
            var resCpt = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            var gameObject = resCpt.GetAsset(UIType.UserCenterWin.StringToAB(), UIType.ChangePwdPanel);

            panelGo = (GameObject)UnityEngine.Object.Instantiate(gameObject, parent);
            Rc = panelGo.GetComponent<ReferenceCollector>();

            _panelTweener = panelGo.transform.DOScale(Vector3.one, 0.3f).Pause().SetEase(Ease.OutBack)
                .SetAutoKill(false);

            _nickNameText = Rc.GetComponent<Text>("NickNameText");
            _oldPwdInput = Rc.GetComponent<InputField>("OldPwdInput");
            _oldPwdInput.onEndEdit.AddListener(OnEndEditOldPwdInputField);
            _newPwdInput = Rc.GetComponent<InputField>("NewPwdInput");
            _newPwdInput.onEndEdit.AddListener(OnEndEditNewPwdInputField);
            _confirmPwdInput = Rc.GetComponent<InputField>("ConfirmPwdInput");
            _confirmPwdInput.onEndEdit.AddListener(OnEndEditConfirmPwdInputField);

            _oldPwdTipsText = Rc.GetComponent<Text>("OldPwdTipsText");
            _newPwdTipsText = Rc.GetComponent<Text>("NewPwdTipsText");
            _confirmPwdTipsText = Rc.GetComponent<Text>("ConfirmPwdTipsText");

            Rc.GetComponent<Button>("CloseBtn").onClick.AddListener(OnCloseMask);
            Rc.GetComponent<Button>("ConfirmChangeBtn").onClick.AddListener(OnChangePwd);
        }

        private void OnEndEditOldPwdInputField(string arg0)
        {
            var res = GameHelper.VerifyPasswordText(arg0, out var verifyMsg);
            _oldPwdTipsText.text = res ? "" : "原" + verifyMsg;
            _oldPwdVertify = res;
        }

        private void OnEndEditNewPwdInputField(string arg0)
        {
            var res = GameHelper.VerifyPasswordText(arg0, out var verifyMsg);
            _newPwdTipsText.text = res ? "" : "新" + verifyMsg;
            if (res)
            {
                if (_newPwdInput.text.Equals(_oldPwdInput.text))
                {
                    res = false;
                    _newPwdTipsText.text = "新密码不能与原密码相同!";
                }
            }
            _newPwdVertify = res;
        }

        private void OnEndEditConfirmPwdInputField(string arg0)
        {
            if (string.IsNullOrEmpty(arg0))
            {
                _confirmPwdTipsText.text = "确认密码不能为空!";
                _confirmPwdVertify = false;
            }
            else if (!arg0.Equals(_newPwdInput.text))
            {
                _confirmPwdTipsText.text = "确认密码与新密码不同,请重新输入!";
                _confirmPwdVertify = false;
            }
            else
            {
                _confirmPwdTipsText.text = "";
                _confirmPwdVertify = true;
            }
        }

        public void Start()
        {
            ComponentFactory.CreateWithParent<MobileInputFieldAdaptionCpt, InputField>(this, _oldPwdInput);
            ComponentFactory.CreateWithParent<MobileInputFieldAdaptionCpt, InputField>(this, _newPwdInput);
            ComponentFactory.CreateWithParent<MobileInputFieldAdaptionCpt, InputField>(this, _confirmPwdInput);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        private async void OnChangePwd()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            if (_isRequest)
            {
                Game.PopupComponent.ShowTips(DataCenterComponent.Instance.tipInfo.ModifyPasswordNotRepeatTip);
                return;
            }

            OnEndEditOldPwdInputField(_oldPwdInput.text);
            OnEndEditNewPwdInputField(_newPwdInput.text);
            OnEndEditConfirmPwdInputField(_confirmPwdInput.text);

            if (!_oldPwdVertify || !_newPwdVertify || !_confirmPwdVertify) return;

            try
            {
                Game.PopupComponent.ShowLoadingLockUI();
                _isRequest = true;

                var response = (G2C_ResetPassword_Res)await SessionComponent.Instance.Session.Call(
                    new C2G_ResetPassword_Req()
                    {
                        UserId = GamePrefs.GetUserId(),
                        OldPassword = _oldPwdInput.text,
                        NewPassword = _newPwdInput.text
                    });

                if (response.Error != 0)
                {
                    Game.PopupComponent.ShowMessageBox(response.Message);
                    _isRequest = false;
                    Game.PopupComponent.CloseLoadingLockUI();
                    return;
                }

                GamePrefs.SetUserPwd(_confirmPwdInput.text);

                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.ModifyPasswordSuccTip);

                OnCloseMask();
                Game.PopupComponent.CloseLoadingLockUI();
            }
            catch (Exception e)
            {
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.ModifyPasswordFailTip);
                Debug.LogError("修改密码失败: " + e.Message);
                Game.PopupComponent.CloseLoadingLockUI();
                throw;
            }
            finally
            {
                _isRequest = false;
            }
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

            _nickNameText.text = "";
            _oldPwdInput.text = "";
            _newPwdInput.text = "";
            _confirmPwdInput.text = "";

            _oldPwdTipsText.text = "";
            _newPwdTipsText.text = "";
            _confirmPwdTipsText.text = "";

            panelGo?.SetActive(false);
        }

        public void ShowPanel()
        {
            _nickNameText.text = UserDataHelper.UserInfo.Nickname;
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