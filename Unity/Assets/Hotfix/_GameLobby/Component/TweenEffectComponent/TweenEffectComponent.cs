/******************************************************************************************
*         【模块】{ Tween动画组件 }                                                                                                                      
*         【功能】{ 统一管理Tween动画 }
*         【修改日期】{ 2019年5月9日 }                                                                                                                        
*         【贡献者】{ 周瑜 ｝                                                                                                               
*                                                                                                                                        
 ******************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class TweenEffectComponentAwakeSystem : AwakeSystem<TweenEffectComponent>
    {
        public override void Awake(TweenEffectComponent self)
        {
            self.Awake();
        }
    }

    public  class TweenEffectComponent: Component
    {
        /// <summary>
        /// Tween动画数据
        /// </summary>
        private class TweenAnimationData
        {
            /// <summary>
            /// 动画时长
            /// </summary>
            public float playTime;

            /// <summary>
            /// 动画目标
            /// </summary>
            public Transform target;

            /// <summary>
            /// 缓动效果
            /// </summary>
            public Ease ease = Ease.OutBack;

            /// <summary>
            /// Tweener 对象
            /// </summary>
            public Tweener tweener;

            /// <summary>
            /// 动画效果类型
            /// </summary>
            public TweenEffectType effectType;

            /// <summary>
            /// 是否在播放
            /// </summary>
            public bool IsPlaying;

            /// <summary>
            /// 是否在暂停
            /// </summary>
            public bool IsPauseing;

            /// <summary>
            /// 滚动数字
            /// </summary>
            public RollNumData rollNumData = null;
        }

        /// <summary>
        /// 滚动数字
        /// </summary>
        public class RollNumData
        {
            /// <summary>
            /// 组
            /// </summary>
            public Sequence sequence;

            /// <summary>
            /// 新数字
            /// </summary>
            public float NewNumber = 0f;

            /// <summary>
            /// 旧数字
            /// </summary>
            public float OldNumber = 0f;
        }

        /// <summary>
        /// Tween动画类型
        /// </summary>
        public enum TweenEffectType
        {
            /// <summary>
            /// 无
            /// </summary>
            None = 0,

            /// <summary>
            /// Bound缩放动画
            /// </summary>
            BoundScale = 1,

            /// <summary>
            /// 由大到小缩放动画
            /// </summary>
            Max2ToMin = 2,

            /// <summary>
            /// 数字滚动动画
            /// </summary>
            DigitalRoll = 3,

            /// <summary>
            /// 物体抖动
            /// </summary>
            ObjectShake = 4
        }

        /// <summary>
        /// 播放的Tween动画
        /// </summary>
        private Dictionary<string, TweenAnimationData> tweenAnis;

        /// <summary>
        /// Tween动画结束，回调方法
        /// </summary>
        private Dictionary<string, Action> AniTypeEnd;

        public void Awake()
        {
            tweenAnis = new Dictionary<string, TweenAnimationData>();

            AniTypeEnd = new Dictionary<string, Action>();
        }

        /// <summary>
        /// 添加Tween动画需要播放的参数
        /// </summary>
        /// <param name="key">Tween动画KEY</param>
        /// <param name="tweenEffectType">Tween动画类型</param>
        /// <param name="target">动画目标对象</param>
        /// <param name="playTime">动画时长</param>
        /// <param name="ease">缓动类型</param>
        /// <param name="endAction">结束回调</param>
        public void Add(string key, Transform target = null,TweenEffectType tweenEffectType= TweenEffectType.BoundScale,float playTime = 0.3f,Ease ease = Ease.OutBack, Action endAction = null)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此TweenEffectComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (this.tweenAnis.TryGetValue(key, out TweenAnimationData data))
            {
                Log.Error("已经添加过");

                return;
            }

            if (key == TweenAnimationIdType.None || key == "" || key == null)
            {
                Log.Error("key 需要值定义在TweenAnimationIdType里面");

                return;
            }

            if (target == null)
            {
                throw new Exception("target参数不能为空");
            }

            this.tweenAnis[key] = new TweenAnimationData()
            {
                playTime = playTime,

                target = target,

                ease = ease,

                effectType = tweenEffectType
            };

            if (endAction != null)
            {
                this.AniTypeEnd[key] = endAction;
            }
        }

        /// <summary>
        /// 添加滚动数字参数
        /// 【不支持回放，暂停，重置】
        /// </summary>
        /// <param name="key"></param>
        /// <param name="target"></param>
        /// <param name="oldNum"></param>
        /// <param name="newNum"></param>
        /// <param name="playTime"></param>
        public void AddRoll(string key, Transform target = null, float oldNum=0f, float newNum=0f,float playTime = 0.3f,Action endAction = null)
        {
            //构建滚动数据
            var rollNumData = new RollNumData
            {
                OldNumber = oldNum,

                NewNumber = newNum,

                sequence = DOTween.Sequence().SetAutoKill(false)
            };

            this.tweenAnis[key] = new TweenAnimationData()
            {
                target = target,

                playTime = playTime,

                rollNumData = rollNumData,

                effectType = TweenEffectType.DigitalRoll
            };

            if (endAction != null)
            {
                this.AniTypeEnd[key] = endAction;
            }
        }

        /// <summary>
        /// 播放指定名称Tween动画
        /// </summary>
        /// <param name="key"></param>
        public void Play(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此TweenEffectComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.tweenAnis.TryGetValue(key, out TweenAnimationData data) || data.target == null)
            {
                Log.Error($"播放传入的key:{key} 没有对应的播放数据");

                return;
            }

            data.IsPlaying = true;

            data.IsPauseing = false;

            this.BeginPlayAni(key);
        }

        /// <summary>
        /// 播放新的滚动数字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="RollNum"></param>
        public void Play(string key, float RollNum)
        {
            if (!this.tweenAnis.TryGetValue(key, out TweenAnimationData item))
            {
                Log.Error($"播放传入的key:{key} 没有对应的播放数据");

                return;
            }

            if (item.effectType == TweenEffectType.DigitalRoll) { PlayDigitalRoll(key, item, RollNum); }
        }

        /// <summary>
        /// 回放指定名称Tween动画
        /// </summary>
        /// <param name="key"></param>
        public void PlayBackwards(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此TweenEffectComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.tweenAnis.TryGetValue(key, out TweenAnimationData item))
            {
                Log.Error($"播放传入的key:{key} 没有对应的播放数据");

                return;
            }

            if (item.tweener != null)
            {
                item.tweener.PlayBackwards();
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="key"></param>
        public void Pause(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此TweenEffectComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.tweenAnis.TryGetValue(key, out TweenAnimationData item))
            {
                Log.Error($"播放传入的key:{key} 没有对应的播放数据");

                return;
            }

            if (item.tweener != null)
            {
                item.tweener.Pause();

                item.IsPauseing = true;

                item.IsPlaying = false;
            }
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="key"></param>
        public void Recover(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此TweenEffectComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.tweenAnis.TryGetValue(key, out TweenAnimationData item))
            {
                Log.Error($"播放传入的key:{key} 没有对应的播放数据");

                return;
            }

            item.IsPauseing = false;

            item.IsPlaying = true;

            this.BeginPlayAni(key);
        }

        /// <summary>
        /// 停止动画
        /// </summary>
        /// <param name="key"></param>
        public void Stop(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此TweenEffectComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.tweenAnis.TryGetValue(key, out TweenAnimationData item))
            {
                Log.Error($"播放传入的key:{key} 没有对应的播放数据");

                return;
            }
            
            this.tweenAnis.Remove(key);

            if (this.AniTypeEnd.TryGetValue(key, out Action action))
            {
                action();

                this.AniTypeEnd.Remove(key);
            }
        }

        /// <summary>
        /// 遍历整个Tween动画播放序列，进行播放
        /// 可以多组播放
        /// </summary>
        /// <param name="key"></param>
        private void BeginPlayAni(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此TweenEffectComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.tweenAnis.TryGetValue(key, out TweenAnimationData item))
            {
                Log.Error($"播放传入的key:{key} 没有对应的播放数据");

                return;
            }

            if (item.effectType == TweenEffectType.BoundScale) { PlayBoundScale(key, item); }

            if (item.effectType == TweenEffectType.Max2ToMin) { PlayMax2ToMin(key, item); }

            if (item.effectType == TweenEffectType.ObjectShake) { PlayObjectShake(key, item); }
        }

        /// <summary>
        /// 播放 BoundScale
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        private async void PlayBoundScale(string key,TweenAnimationData item)
        {
            item.target.localScale = Vector3.zero;

            item.target.gameObject.SetActive(true);

            Tweener _tweener = item.target.DOScale(Vector3.one, item.playTime).Pause().SetEase(item.ease).SetAutoKill(false);

            item.tweener = _tweener;

            _tweener.PlayForward();

            await Task.Delay((int)item.playTime * 1000);

            if (AniTypeEnd.TryGetValue(key, out Action endAction))
            {
                endAction();
            }
        }

        /// <summary>
        /// 由大到小动画
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        private async void PlayMax2ToMin(string key, TweenAnimationData item)
        {
            item.target.localScale = Vector3.one * 2;

            item.target.gameObject.SetActive(true);

            Tweener _tweener = item.target.DOScale(Vector3.one, item.playTime).Pause().SetAutoKill(false);

            item.tweener = _tweener;

            _tweener.PlayForward();

            await Task.Delay((int)item.playTime * 1000);

            if (AniTypeEnd.TryGetValue(key, out Action endAction))
            {
                endAction();
            }
        }

        /// <summary>
        /// 数字滚动动画
        /// 应用场景：玩家分数变化
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        private async void PlayDigitalRoll(string key, TweenAnimationData item,float newNum)
        {
            var text = item.target.GetComponent<Text>();

            var rollData = item.rollNumData;

            rollData.NewNumber = newNum;

            rollData.sequence.Append(DOTween.To(delegate (float value)
            {
                var temp = Math.Floor(value);

                text.text = temp + "";

            }, rollData.OldNumber, rollData.NewNumber, item.playTime));

            rollData.OldNumber = rollData.NewNumber;

            await Task.Delay((int)item.playTime * 1000);

            if (AniTypeEnd.TryGetValue(key, out Action endAction))
            {
                endAction();
            }
        }

        /// <summary>
        /// 物体抖动
        /// 应用场景：斗地主：（炸弹，王炸时，场景根结点振动）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        private async void PlayObjectShake(string key, TweenAnimationData item)
        {
            item.target.DOShakePosition(0.5f, new Vector3(0, 10, 0), 20).SetEase(Ease.Linear);

            await Task.Delay(500);

            item.target.localPosition = Vector3.zero;

            if (AniTypeEnd.TryGetValue(key, out Action endAction))
            {
                endAction();
            }
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

            AniTypeEnd.Clear();

            AniTypeEnd = null;

            tweenAnis.Clear();

            tweenAnis = null;
        }
    }
}
