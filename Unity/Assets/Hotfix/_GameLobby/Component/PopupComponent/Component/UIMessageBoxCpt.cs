using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMessageBoxCptAwakeSystem : AwakeSystem<UIMessageBoxCpt, object>
    {
        public override void Awake(UIMessageBoxCpt self, object arg = null)
        {
            self.Awake(arg);
        }
    }

    public class UIMessageBoxCpt : Component
    {
        private ReferenceCollector rc;

        private Button okBtn;

        private Button _okBtn;

        private Button _noBtn;

        private Text msg;

        private Text Title;

        public GameObject Root;

        private MessageData data = null;

        public void Awake(object arg = null)
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            okBtn = rc.Get<GameObject>("okBtn").GetComponent<Button>();

            _noBtn = rc.Get<GameObject>("_noBtn").GetComponent<Button>();

            _okBtn = rc.Get<GameObject>("_okBtn").GetComponent<Button>();

            Root = rc.Get<GameObject>("Root");

            msg = rc.Get<GameObject>("msg").GetComponent<Text>();

            Title = rc.Get<GameObject>("Title").GetComponent<Text>();

            ButtonHelper.RegisterButtonEvent(rc, "closeBtn", OnnoBtn);

            ButtonHelper.RegisterButtonEvent(rc, "mask", OnnoBtn);

            okBtn.onClick.AddListener(OnokBtn);

            _noBtn.onClick.AddListener(OnnoBtn);

            _okBtn.onClick.AddListener(OnokBtn);

            this.OnOpen(arg);
        }

        protected void OnOpen(object arg = null)
        {
            if (arg != null)
            {
                data = null;

                data = arg as MessageData;

                msg.text = data.mes;

                SetOneButton();

                SetBtnText();

                SetTitleText();
            }
        }
        
        private void OnClose()
        {
            Game.PopupComponent.isShowBox = false;

            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UIMessageBoxPanel);
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
        }

        #region Button events
        private async void OnCloseButtonClicked()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
            //大版本更新时，只提示但不能被关闭
            if (data != null && !data.canClose) { return; }

            //某些情况，立即关掉窗口
            if (data != null && !data.fastClose)
            {
                var ui = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIMessageBoxPanel);

                var tec = ui.GetComponent<TweenEffectComponent>();

                tec.PlayBackwards(TweenAnimationIdType.MessageBox);

                await Task.Delay(200);
            }

            OnClose();
        }

        private void OnokBtn()
        {
            if (data != null && data.ok != null) data.ok();

            OnCloseButtonClicked();
        }

        private void OnnoBtn()
        {
            if (data != null && data.cancel != null) data.cancel();

            OnCloseButtonClicked();
        }

        private void OnmaskBtn()
        {
            if (data != null && data.click) OnCloseButtonClicked();
        }

        #endregion

        #region setting

        /// <summary>
        /// 是否显示取消按钮
        /// </summary>
        private void SetOneButton()
        {
            if (data != null)
            {
                okBtn.gameObject.SetActive(data.onlyOk);

                _okBtn.gameObject.SetActive(!data.onlyOk);

                _noBtn.gameObject.SetActive(!data.onlyOk);
            }
        }

        /// <summary>
        /// 设置按钮文字
        /// </summary>
        public void SetBtnText()
        {
            if (data != null)
            {
                okBtn.transform.GetComponentInChildren<Text>().text = data.okStr;

                _okBtn.transform.GetComponentInChildren<Text>().text = data.okStr;

                _noBtn.transform.GetComponentInChildren<Text>().text = data.noStr;
            }
        }

        /// <summary>
        /// 设置弹窗标题
        /// </summary>
        public void SetTitleText()
        {
            if (data != null)
            {
                Title.text = data.titleStr;
            }
        }

        #endregion
    }
}
