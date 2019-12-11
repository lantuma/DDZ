using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class EasyToggleAwakeSystem : AwakeSystem<EasyToggle, GameObject, GameObject, Action<bool>>
    {
        public override void Awake(EasyToggle self, GameObject button, GameObject panel, Action<bool> action)
        {
            self.Awake(button, panel, action);
        }
    }

    public class EasyToggle : Component, IToggleButton
    {
        private GameObject panelGo;

        private GameObject onGo;
        private GameObject offGo;

        private Action<bool> _callAction;

        public void Awake(GameObject button, GameObject panel, Action<bool> action)
        {
            panelGo = panel;
            onGo = button.transform.GetChild(0).gameObject;
            offGo = button.transform.GetChild(1).gameObject;
            _callAction = action;

            button.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            ButtonGroup.OnButtonClick(this);
        }

        public ButtonGroup ButtonGroup { get; set; }
        public void IsButtonClick(bool click)
        {
            if (click)
            {
                onGo.SetActive(true);
                offGo.SetActive(false);
                panelGo.SetActive(true);
                _callAction?.Invoke(true);
            }
            else
            {
                onGo.SetActive(false);
                offGo.SetActive(true);
                panelGo.SetActive(false);
                _callAction?.Invoke(false);
            }
        }

        public override void Dispose()
        {
            if (IsDisposed) return;
            base.Dispose();

            _callAction = null;
        }
    }
}