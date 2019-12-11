using ETModel;
using System;

namespace ETHotfix
{
    /// <summary>
    /// UI窗体主控组件需要继承此类
    /// </summary>
    public abstract class UIBaseComponent : Component
    {
        public event Action OnCloseOneTime;
        public event Action OnShow;
        public event Action OnClose;


        public event Action OnShowAnimation;

        public event Action OnHideAnimation;

        public bool InShow { get { return Layer != ZCanvasType.UIHiden; } }
        public ZCanvasType Layer { get; set; } = ZCanvasType.UIHiden;

        public virtual void Show()
        {
            GetParent<UIEx>().GameObject.SetActive(true);
            OnShow?.Invoke();
        }

        public virtual void Close()
        {
            GetParent<UIEx>().GameObject.SetActive(false);

            if (OnCloseOneTime != null)
            {
                OnCloseOneTime.Invoke();
                OnCloseOneTime = null;
            }
            OnClose?.Invoke();
        }

        public override void Dispose()
        {
            base.Dispose();

            OnCloseOneTime = null;
            OnShow = null;
            OnClose = null;
            Layer = ZCanvasType.UIHiden;
        }
    }
}

