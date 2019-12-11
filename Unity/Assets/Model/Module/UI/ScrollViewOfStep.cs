using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETModel
{
    public class ScrollViewOfStep : ScrollRect
    {
        public Action OnDragAction;
        public Action OnBeginDragAction;
        public Action OnEndDragAction;
        public Action<int> OnScrollEndAction;

        public int _currentIndex;
        //        private bool _isDraging;
        private Transform CenterPoint;
        private float _stepSize;

        protected override void Awake()
        {
            base.Awake();
            CenterPoint = content.parent.GetChild(0);
            _stepSize = content.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            OnDragAction?.Invoke();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            _currentAsyncCount++;
            _lastPosition = Vector3.zero;
            _isScrolling = false;
            _scrollTweener?.Kill();
            inertia = true;
            //            _isDraging = true;
            base.OnBeginDrag(eventData);
            OnBeginDragAction?.Invoke();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            //            _isDraging = false;
            base.OnEndDrag(eventData);
            OnEndDragAction?.Invoke();
            _isScrolling = true;
        }

        private Vector3 _lastPosition;
        private bool _isScrolling;
        private Tweener _scrollTweener;

        protected override void LateUpdate()
        {
            base.LateUpdate();
            SetItemSize();

            if (_isScrolling && Mathf.Abs(content.localPosition.x - _lastPosition.x) < 1f)
            {
                _isScrolling = false;
                StepContentPos(_currentAsyncCount);
            }
            _lastPosition = content.localPosition;
        }

        /// <summary>
        /// 设置元素大小
        /// </summary>
        private void SetItemSize()
        {
            foreach (Transform item in content)
            {
                if (!item.gameObject.activeSelf) continue;
                var offset2 = Mathf.Abs(Mathf.Abs(CenterPoint.parent.worldToLocalMatrix.MultiplyPoint(item.position).x)
                                        - CenterPoint.localPosition.x);
                float rate;
                if (offset2 > _stepSize * 2) continue;
                if (offset2 > _stepSize) rate = 0;
                else rate = 1 - (offset2 / _stepSize);
                item.localScale = Vector3.one * (0.8f + rate * 0.7f);
            }
        }

        private int _currentAsyncCount;
        private async ETVoid StepContentPos(int count)
        {
            inertia = false;
            var near = Mathf.Round(content.localPosition.x / (_stepSize + 0.001f));
            near = ReviseNearIndex(near);
            _scrollTweener = content.DOLocalMoveX(_stepSize * near, 0.15f).SetAutoKill(true);
            await Task.Delay(150);
            if (_currentAsyncCount != count) return;
            var index = Mathf.Abs(near);
            if (index > content.childCount - 5) index = content.childCount - 5;
            OnScrollEndAction?.Invoke((int)index);
            _currentAsyncCount = 0;
            _currentIndex = (int)index;
        }

        /// <summary>
        /// 校正最近的索引
        /// </summary>
        /// <param name="near"></param>
        private float ReviseNearIndex(float near)
        {
            if (near > 0) near = 0;
            else if (Mathf.Abs(near) > content.childCount - 5)
            {
                near = -(content.childCount - 5);
            }
            return near;
        }

        /// <summary>
        /// 切换位置(页数)
        /// </summary>
        /// <param name="i"></param>
        public async ETVoid ChangeStep(int i)
        {
            if (i > 0 && _currentIndex + 1 > content.childCount - 4) return;
            if (i < 0 && _currentIndex - 1 < 0) return;
            var temp = content.localPosition.x;
            temp = i > 0 ? temp - _stepSize : temp + _stepSize;
            content.DOLocalMoveX(temp, 0.1f).SetAutoKill(true);
            await Task.Delay(100);
            StepContentPos(_currentAsyncCount);
        }
    }
}