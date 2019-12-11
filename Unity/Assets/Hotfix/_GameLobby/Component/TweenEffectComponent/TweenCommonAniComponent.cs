/******************************************************************************************
*         【模块】{ Tween常用动画组件 }                                                                                                                      
*         【功能】{ 封装常用点击及显示效果 
*         【应用场景】{按钮\图片的点击或者默认显示动画效果}
*         【修改日期】{ 2019年6月27日 }                                                                                                                        
*         【贡献者】{ 周瑜 ｝                                                                                                               
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Collections.Generic;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class TweenCommonAniComponentAwakeSystem : AwakeSystem<TweenCommonAniComponent>
    {
        public override void Awake(TweenCommonAniComponent self)
        {
            self.Awake();
        }
    }
    
    public class TweenCommonAniComponent : Component
    {
        #region 动画枚举

        /// <summary>
        /// 点击效果动画
        /// </summary>
        public enum TweenCommonPressAniType
        {
            /// <summary>
            /// 无
            /// </summary>
            None = 0,

            /// <summary>
            /// 左右缩放
            /// </summary>
            PRESS_1 = 1,

            /// <summary>
            /// 上下缩放
            /// </summary>
            PRESS_2 = 2,

            /// <summary>
            /// 整体缩放
            /// </summary>
            PRESS_3 = 3,

            /// <summary>
            /// 整体缩放2
            /// </summary>
            PRESS_4 = 4,

            /// <summary>
            /// 上跳
            /// </summary>
            PRESS_5 = 5,

            /// <summary>
            /// 右上旋转
            /// </summary>
            PRESS_6 = 6,

            /// <summary>
            /// 左上旋转
            /// </summary>
            PRESS_7 = 7,

            /// <summary>
            /// 左上旋转2
            /// </summary>
            PRESS_8 = 8,

            /// <summary>
            /// 左上旋转3
            /// </summary>
            PRESS_9 = 9,

            /// <summary>
            /// 上跳2
            /// </summary>
            PRESS_10 = 10,

            /// <summary>
            /// 上跳并左右缩放
            /// </summary>
            PRESS_11 = 11,

            /// <summary>
            /// 上跳并上下缩放
            /// </summary>
            PRESS_12 = 12,

            /// <summary>
            /// 上跳并上下缩放2（快）
            /// </summary>
            PRESS_13 = 13,

            /// <summary>
            /// 上跳并向后缩放
            /// </summary>
            PRESS_14 = 14,

            /// <summary>
            /// 右上旋转并左移动
            /// </summary>
            PRESS_15 = 15,

            /// <summary>
            /// 左移动并缩放归位
            /// </summary>
            PRESS_16 = 16,

            /// <summary>
            /// 右移动并缩放归位
            /// </summary>
            PRESS_17 = 17,

            /// <summary>
            /// 右下移动并缩放归位
            /// </summary>
            PRESS_18 = 18,

            /// <summary>
            /// 左旋转并归位（快）
            /// </summary>
            PRESS_19 = 19,

            /// <summary>
            /// 右旋转并归位（快）
            /// </summary>
            PRESS_20 = 20,

            /// <summary>
            /// 从大到小缩放归位
            /// </summary>
            PRESS_21 = 21,

            /// <summary>
            /// 向下移动不归位
            /// </summary>
            PRESS_22 = 22,

            /// <summary>
            /// 向下移动不归位2
            /// </summary>
            PRESS_23 = 23,

            /// <summary>
            /// 向下移动不归位3
            /// </summary>
            PRESS_24 = 24,

            /// <summary>
            /// 向下移动不归位4
            /// </summary>
            PRESS_25 = 25,

            /// <summary>
            /// 向下移动不归位5
            /// </summary>
            PRESS_26 = 26,

            /// <summary>
            /// 向下移动不归位6
            /// </summary>
            PRESS_27 = 27,

            /// <summary>
            /// 向下移动不归位7
            /// </summary>
            PRESS_28 = 28,

            /// <summary>
            /// 向下移动不归位8
            /// </summary>
            PRESS_29 = 29,

            /// <summary>
            /// 向下移动不归位9
            /// </summary>
            PRESS_30 = 30,
        }

        /// <summary>
        /// 显示效果动画
        /// </summary>
        public enum TweenCommonShowAniType
        {
            /// <summary>
            /// 无
            /// </summary>
            None = 0,

            /// <summary>
            /// 上下浮动（缓慢）
            /// </summary>
            SHOW_1 = 1,

            /// <summary>
            /// 色彩渐隐渐现
            /// </summary>
            SHOW_2 = 2,

            /// <summary>
            /// 循环缩放
            /// </summary>
            SHOW_3 = 3,

            /// <summary>
            /// 缩放并左右来回偏移
            /// </summary>
            SHOW_4 = 4,

            /// <summary>
            /// 缩放并上下来回偏移
            /// </summary>
            SHOW_5 = 5,

            /// <summary>
            /// 左右轻微缓动
            /// </summary>
            SHOW_6 = 6,

            /// <summary>
            /// 上下缓动中途有延迟
            /// </summary>
            SHOW_7 = 7,

            /// <summary>
            /// 左右缓动中途有延迟
            /// </summary>
            SHOW_8 = 8,

            /// <summary>
            /// 循环左右带角度旋转
            /// </summary>
            SHOW_9 = 9,

            /// <summary>
            /// 左右旋转Z轴方向
            /// </summary>
            SHOW_10 = 10,

            /// <summary>
            /// 左右来回（急）
            /// </summary>
            SHOW_11 = 11,

            /// <summary>
            /// 上下来回 (急)
            /// </summary>
            SHOW_12 = 12,
        }

        #endregion

        /// <summary>
        /// 点击动画效果字典
        /// </summary>
        private Dictionary<int, Action<Transform>> pressEffectDic;

        /// <summary>
        /// 显示动画效果字典
        /// </summary>
        private Dictionary<int, Action<Transform>> showEffectDic;

        /// <summary>
        /// 单例模式
        /// </summary>
        public static TweenCommonAniComponent Instance { get; private set; }

        public void Awake()
        {
            Instance = this;

            this.RegisterPressEffect();

            this.RegisterShowEffect();
        }

        /// <summary>
        /// 绑定按下动画效果
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        public void BindPressEffect(Transform target, TweenCommonPressAniType type = TweenCommonPressAniType.PRESS_1)
        {
            if (!this.pressEffectDic.TryGetValue((int)type, out Action<Transform> item))
            {
                Log.Error($"播放传入的key:{type} 没有对应的按下效果方法");

                return;
            }

            item(target);
        }

        /// <summary>
        /// 绑定显示动画效果
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        public void BindShowEffect(Transform target, TweenCommonShowAniType type = TweenCommonShowAniType.SHOW_1)
        {
            if (!this.showEffectDic.TryGetValue((int)type, out Action<Transform> item))
            {
                Log.Error($"播放传入的key:{type} 没有对应的显示效果方法");

                return;
            }

            item(target);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.pressEffectDic.Clear();

            this.showEffectDic.Clear();

            this.pressEffectDic = null;

            this.showEffectDic = null;
        }

        #region ClickEffect

        private void Press_1(Transform target)
        {
            target.DOPunchScale(new Vector3(-0.2f, 0, 0), 0.4f, 12, 0.5f);
        }

        private void Press_2(Transform target)
        {
            target.DOPunchScale(new Vector3(0f, -0.2f, 0), 0.4f, 12, 0.5f);
        }

        private void Press_3(Transform target)
        {
            target.DOPunchScale(new Vector3(-0.2f, -0.2f, 0), 0.4f, 12, 0.5f);
        }

        private void Press_4(Transform target)
        {
            target.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.4f, 12, 0.5f);
        }

        private void Press_5(Transform target)
        {
            target.DOPunchPosition(new Vector3(0, 20, 0), 0.2f, 4, 0.5f);
        }

        private void Press_6(Transform target)
        {
            target.DOPunchPosition(new Vector3(0, 40, 0), 0.4f, 12, 0.5f);

            target.DOPunchRotation(new Vector3(0, 0, 30), 0.4f, 12, 0.5f);
        }

        private void Press_7(Transform target)
        {
            target.DOPunchPosition(new Vector3(0, 40, 0), 0.4f, 12, 0.5f);

            target.DOPunchRotation(new Vector3(0, 0, -30), 0.4f, 12, 0.5f);
        }

        private void Press_8(Transform target)
        {
            target.DOPunchPosition(new Vector3(0, 20, 0), 0.2f, 4, 0.5f);

            target.DOPunchRotation(new Vector3(0, 0, 30), 0.4f, 4, 0.5f);
        }

        private void Press_9(Transform target)
        {
            target.DOPunchPosition(new Vector3(0, 20, 0), 0.2f, 4, 0.5f);

            target.DOPunchRotation(new Vector3(0, 0, -30), 0.4f, 4, 0.5f);
        }

        private void Press_10(Transform target)
        {
            target.DOPunchPosition(new Vector3(0, 20, 0), 0.2f, 4, 0.5f);
        }

        private void Press_11(Transform target)
        {
            target.DOPunchPosition(new Vector3(0, 40, 0), 0.4f, 4, 0.5f);

            target.DOPunchScale(new Vector3(0.4f, 0, 0), 0.3f, 4, 0.5f).SetDelay(0.1f);
        }

        private void Press_12(Transform target)
        {
            target.DOPunchPosition(new Vector3(0, 40, 0), 0.4f, 4, 0.5f);

            target.DOPunchScale(new Vector3(0f, 0.2f, 0), 0.3f, 4, 0.5f).SetDelay(0.1f);
        }

        private void Press_13(Transform target)
        {
            target.DOPunchPosition(new Vector3(0, 40, 0), 0.4f, 8, 0.5f);

            target.DOPunchScale(new Vector3(0.1f, -0.2f, 0), 0.4f, 12, 0.5f);
        }

        private void Press_14(Transform target)
        {
            target.DOPunchPosition(new Vector3(0, 20, 0), 0.2f, 4, 0.5f);

            target.DOPunchScale(new Vector3(0.1f, 0.2f, 0), 0.4f, 12, 0.5f);
        }

        private void Press_15(Transform target)
        {
            target.DOPunchPosition(new Vector3(-80, 0, 0), 0.4f, 4, 1f);

            target.DOPunchRotation(new Vector3(0, 0, 30), 0.3f, 4, 0.5f).SetDelay(0.1f);

            target.DOPunchScale(new Vector3(-0.2f, 0.2f, 0), 0.4f, 16, 0.5f);
        }

        private void Press_16(Transform target)
        {
            target.DOPunchPosition(new Vector3(-40, 0, 0), 0.4f, 4, 1f);

            target.DOPunchScale(new Vector3(-0.2f, 0.2f, 0), 0.4f, 16, 0.5f);
        }

        private void Press_17(Transform target)
        {
            target.DOPunchPosition(new Vector3(40, 0, 0), 0.4f, 4, 1f);

            target.DOPunchScale(new Vector3(-0.2f, 0.2f, 0), 0.4f, 16, 0.5f);
        }

        private void Press_18(Transform target)
        {
            target.DOPunchPosition(new Vector3(80, 0, 0), 0.4f, 4, 1f);

            target.DOPunchRotation(new Vector3(0, 0, -30), 0.3f, 4, 0.5f).SetDelay(0.1f);

            target.DOPunchScale(new Vector3(-0.2f, 0.2f, 0), 0.4f, 16, 0.5f);
        }

        private void Press_19(Transform target)
        {
            target.DOPunchRotation(new Vector3(0, 0, 30), 0.2f, 4, 0.5f);
        }

        private void Press_20(Transform target)
        {
            target.DOPunchRotation(new Vector3(0, 0, -30), 0.2f, 4, 0.5f);
        }

        private void Press_21(Transform target)
        {
            target.DOPunchScale(new Vector3(-0.2f, -0.2f, 0), 0.2f, 4, 0.5f);
        }

        private void Press_22(Transform target)
        {
            target.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.2f, 4, 0.5f);
        }

        private void Press_23(Transform target)
        {
            target.DOPunchPosition(new Vector3(-20, 0, 0), 0.2f, 4, 0.5f);

            target.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.2f, 4, 0.5f);
        }

        private void Press_24(Transform target)
        {
            target.DOPunchPosition(new Vector3(0, 20, 0), 0.2f, 4, 0.5f);

            target.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.2f, 4, 0.5f);
        }

        private void Press_25(Transform target)
        {
            target.DOPunchPosition(new Vector3(0, -20, 0), 0.2f, 4, 0.5f);

            target.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.2f, 4, 0.5f);
        }

        private void Press_26(Transform target)
        {
            target.DOPunchPosition(new Vector3(20, 0, 0), 0.2f, 4, 0.5f);

            target.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.2f, 4, 0.5f);
        }

        private void Press_27(Transform target)
        {
            target.DOPunchRotation(new Vector3(0, 0, 30), 0.2f, 4, 0.5f);

            target.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.2f, 4, 0.5f);
        }

        private void Press_28(Transform target)
        {
            target.DOPunchRotation(new Vector3(0, 0, -30), 0.2f, 4, 0.5f);

            target.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.2f, 4, 0.5f);
        }

        private void Press_29(Transform target)
        {
            target.DOPunchPosition(new Vector3(-40, 0, 0), 0.4f, 4, 0.5f);

            target.DOPunchRotation(new Vector3(0, 0, 30), 0.4f, 4, 0.5f);
        }

        private void Press_30(Transform target)
        {
            target.DOPunchPosition(new Vector3(40, 0, 0), 0.4f, 4, 0.5f);

            target.DOPunchRotation(new Vector3(0, 0, -30), 0.4f, 4, 0.5f);
        }


        #endregion

        #region ShowEffect

        private void Show_1(Transform target)
        {
            target.DOMove(target.position + new Vector3(0, 1, 0), 0.5f)

            .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        private void Show_2(Transform target)
        {
            Image image = target.GetComponent<Image>();

            image.DOFade(0.4f, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        private void Show_3(Transform target)
        {
            target.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            target.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        private void Show_4(Transform target)
        {
            target.DOMove(target.position + new Vector3(5, 0, 0), 0.5f).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);

            target.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            target.DOScale(new Vector3(1, 1, 1), 0.25f).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        }

        private void Show_5(Transform target)
        {
            target.DOMove(target.position + new Vector3(0, 5, 0), 0.5f).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);

            target.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            target.DOScale(new Vector3(1, 1, 1), 0.25f).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        }

        private void Show_6(Transform target)
        {
            target.DOMove(target.position + new Vector3(10, 0, 0), 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        private void Show_7(Transform target)
        {
            target.DOMove(target.position + new Vector3(0, 10, 0), 1f).SetEase(Ease.InOutElastic).SetLoops(-1, LoopType.Yoyo);
        }

        private void Show_8(Transform target)
        {
            target.DOMove(target.position + new Vector3(10, 0, 0), 1f).SetEase(Ease.InOutElastic).SetLoops(-1, LoopType.Yoyo);
        }

        private void Show_9(Transform target)
        {
            target.eulerAngles = new Vector3(0, 0, -10);

            target.DORotate(new Vector3(0, 0, 10), 1f).SetEase(Ease.InOutElastic).SetLoops(-1, LoopType.Yoyo);
        }

        private void Show_10(Transform target)
        {
            target.eulerAngles = new Vector3(0, 0, -10);

            target.DORotate(new Vector3(0, 0, 10), 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        private void Show_11(Transform target)
        {
            target.DOMove(target.position + new Vector3(10, 0, 0), 0.2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        private void Show_12(Transform target)
        {
            target.DOMove(target.position + new Vector3(0, 10, 0), 0.2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        #endregion

        #region Register

        /// <summary>
        /// 注册点击效果
        /// </summary>
        private void RegisterPressEffect()
        {
            this.pressEffectDic = new Dictionary<int, Action<Transform>>
            {
                {1,this.Press_1},{2,this.Press_2},{3,this.Press_3},{4,this.Press_4},{5,this.Press_5},

                {6,this.Press_6},{7,this.Press_7},{8,this.Press_8},{9,this.Press_9},{10,this.Press_10},

                {11,this.Press_11},{12,this.Press_12},{13,this.Press_13},{14,this.Press_14},{15,this.Press_15},

                {16,this.Press_16},{17,this.Press_17},{18,this.Press_18},{19,this.Press_19},{20,this.Press_20},

                {21,this.Press_21},{22,this.Press_22},{23,this.Press_23},{24,this.Press_24},{25,this.Press_25},

                {26,this.Press_26},{27,this.Press_27},{28,this.Press_28},{29,this.Press_29},{30,this.Press_30},
            };
        }

        /// <summary>
        /// 注册显示效果
        /// </summary>
        private void RegisterShowEffect()
        {
            this.showEffectDic = new Dictionary<int, Action<Transform>>
            {
                {1,this.Show_1},{2,this.Show_2},{3,this.Show_3},{4,this.Show_4},{5,this.Show_5},

                {6, this.Show_6},{7,this.Show_7},{8,this.Show_8},{9,this.Show_9},{10,this.Show_10},

                {11, this.Show_11},{12,this.Show_12}
            };
        }

        #endregion

    }
}
