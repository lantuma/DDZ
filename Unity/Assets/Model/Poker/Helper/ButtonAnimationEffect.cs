using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    public class ButtonAnimationEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private static Vector3 downScale = new Vector3(1.1f, 1.1f, 1.1f);

        private static Vector3 upScale = Vector3.one;

        /// <summary>
        /// 按钮抬起事件
        /// </summary>
        public static Action ButtonPointerUpAction;

        public void OnPointerDown(PointerEventData eventData)
        {
            this.transform.localScale = downScale;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ButtonPointerUpAction?.Invoke();

            this.transform.localScale = upScale;
        }
    }
}
