using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    public class PointerEvent : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public Action OnPointerDownAction;

        public Action OnPointerUpAction;

        public Action OnPointerEnterAction;

        public Action OnPointerExitAction;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownAction?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterAction?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitAction?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpAction?.Invoke();
        }
    }
}
