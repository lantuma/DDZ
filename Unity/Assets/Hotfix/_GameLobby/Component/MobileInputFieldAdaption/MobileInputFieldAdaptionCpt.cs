using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class MobileInputFieldAdaptionCptAwakeSystem : AwakeSystem<MobileInputFieldAdaptionCpt, InputField>
    {
        public override void Awake(MobileInputFieldAdaptionCpt self, InputField a)
        {
            self.Awake(a);
        }
    }

    [ObjectSystem]
    public class MobileInputFieldAdaptionCptUpdateSyste : UpdateSystem<MobileInputFieldAdaptionCpt>
    {
        public override void Update(MobileInputFieldAdaptionCpt self)
        {
            self.Update();
        }
    }

    public class MobileInputFieldAdaptionCpt : Component
    {
        private float _heightOffset;
        private float _maxHeight;

        private InputField _inputField;
        private RectTransform _currentCanvas;

        private float _zDepth;

        public void Awake(InputField inputField)
        {
            _inputField = inputField;

            var parents = inputField.GetComponentsInParent<Canvas>();
            foreach (var canvas in parents)
            {
                if (canvas.CompareTag("BasicCanvas"))
                {
                    _currentCanvas = canvas.transform as RectTransform;
                }
            }
            _inputField.onEndEdit.AddListener(OnEndEdit);

            var inputfieldRectTransform = (RectTransform)_inputField.transform;
            var inputfieldHeightHalf = inputfieldRectTransform.sizeDelta.y / 2;

            _zDepth = _currentCanvas.position.z;
            var canvasHeight = _currentCanvas.parent.GetComponent<RectTransform>().sizeDelta.y / 2f;

            _heightOffset = inputfieldHeightHalf - canvasHeight;
            _maxHeight = canvasHeight - inputfieldRectTransform.anchoredPosition.y - inputfieldHeightHalf - 5f;
        }

        public void Update()
        {
            if (_inputField == null || !_inputField.isFocused) return;

            if (Application.platform == RuntimePlatform.Android)
            {
                _currentCanvas.localPosition = new Vector3(0, 1, _zDepth) * CalcHeight(AndroidGetKeyboardHeight());
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _currentCanvas.localPosition = new Vector3(0, 1, _zDepth) * CalcHeight(IOSGetKeyboardHeight());
            }
        }

        /// <summary>
        /// 编辑结束监听
        /// </summary>
        /// <param name="arg0"></param>
        private void OnEndEdit(string arg0)
        {
            if (_currentCanvas == null) return;
            _currentCanvas.localPosition = new Vector3(0, 0, _zDepth);
        }

        private float CalcHeight(float boardHeight)
        {
            var height = boardHeight + _heightOffset + 100f;
            if (height < 0) height = 0;
            return Mathf.Min(height, _maxHeight);
        }

        /// <summary>
        /// 获取IOS平台上键盘的高度
        /// </summary>
        /// <returns></returns>
        public float IOSGetKeyboardHeight()
        {
            return TouchScreenKeyboard.area.height;
        }

        /// <summary>
        /// 获取安卓平台上键盘的高度
        /// </summary>
        /// <returns></returns>
        public int AndroidGetKeyboardHeight()
        {
            using (AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").
                    Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

                using (AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
                {
                    View.Call("getWindowVisibleDisplayFrame", Rct);
                    return Screen.height - Rct.Call<int>("height");
                }
            }
        }
    }
}