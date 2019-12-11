/******************************************************************************************
*         【模块】{ 序列帧动画组件 }                                                                                                                      
*         【功能】{ 统一管理序列帧动画 }
*         【修改日期】{ 2019年5月9日 }                                                                                                                        
*         【贡献者】{ 周瑜 整理 ｝                                                                                                               
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class FrameAnimationComponentAwakeSystem : AwakeSystem<FrameAnimationComponent>
    {
        public override void Awake(FrameAnimationComponent self)
        {
            self.Awake();
        }
    }

    public class FrameAnimationComponent : Component
    {
        private class FrameAnimationData
        {
            //需要播放的精灵图集
            public Sprite[] Sprites;

            //延迟几秒之后播放，单位：秒
            public float Delay = 0.0f;

            //是否循环播放 -1 表示循环播放，0和1都表示播放1次
            public int Loop = 0;

            //每张图片间隔多少时间播放，每一帧播放的时间数组,默认播放时间为0.1s
            public float[] FrameSeconds;

            //从图集里面的第几个开始播放
            public int First = 0;

            //记录播放次数
            public int RecordLoopCount = 0;

            //是否在播放
            public bool IsPlaying;

            //是否在暂停
            public bool IsPauseing;

            //当你暂停时，需要记录当前播放到哪一个帧动画的精灵了，等回复时，需要置空，默认为-1
            public int PauseCount = -1;

            //帧动画的精灵图片承载对象
            public Image TargetImage;

            //播放的第几帧，并且回调当前切换的sprite
            public Action<int, Sprite> ChangeUpdate;
        }

        //播放的帧动画
        private Dictionary<string, FrameAnimationData> FrameAnis;

        //动画播放结束，调用Stop方法，都会触发
        private Dictionary<string, Action<string>> AniTypeEnd;

        private TimerComponent TC;

        public void Awake()
        {
            FrameAnis = new Dictionary<string, FrameAnimationData>();

            AniTypeEnd = new Dictionary<string, Action<string>>();

            TC = ETModel.Game.Scene.GetComponent<TimerComponent>();
        }

        /// <summary>
        /// 加载精灵序列
        /// </summary>
        /// <returns></returns>
        public Sprite[] LoadSpriteSheets(string atlasName, string spriteName, int spriteCount)
        {
            List<Sprite> loadList = new List<Sprite>();

            for (int i = 0; i < spriteCount; i++)
            {
                var sprite = SpriteHelper.GetSprite(atlasName, string.Format("{0}{1}", spriteName, i));

                loadList.Add(sprite);
            }

            return loadList.ToArray();
        }

        /// <summary>
        /// 添加帧动画需要播放的参数
        /// </summary>
        /// <param name="key">记录此帧动画</param>
        /// <param name="sprites">帧动画切换的图片</param>
        /// <param name="targetImage">帧动画承载的UI容器</param>
        /// <param name="frameSeconds">每帧需要的秒数</param>
        /// <param name="endAction">结束调用的方法</param>
        /// <param name="changeUpdate">每一帧都调用的方法</param>
        /// <param name="delay">延迟加载</param>
        /// <param name="loop">是否循环</param>
        /// <param name="first">第一帧加载的图片</param>
        public void Add(string key, Sprite[] sprites, Image targetImage, float[] frameSeconds = null, Action<string> endAction = null, Action<int, Sprite> changeUpdate = null, float delay = 0.0f, int loop = 0, int first = 0)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此FrameAnimationComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (this.FrameAnis.TryGetValue(key, out FrameAnimationData data))
            {
                Log.Error("已经添加过，如果需要修改参数，使用ModifyData方法");

                return;
            }

            if (key == AnimationIdType.None || key == "" || key == null)
            {
                Log.Error("key需要值定义在AnimationIdType里面");

                return;
            }

            if (first < 0 || first > sprites.Length - 1 || sprites?.Length <= 0)
            {
                throw new Exception("传入First或者sprites参数有误");
            }

            if (targetImage == null)
            {
                throw new Exception("Image参数不能为空");
            }

            if (frameSeconds == null || frameSeconds?.Length <= 0)
            {
                //每帧需要播放的时间，默认每帧播放时间为0.1s

                List<float> temp = new List<float>();

                for (int i = 0; i < sprites.Length; i++)
                {
                    temp.Add(0.1f);
                }

                frameSeconds = temp.ToArray();
            }

            this.FrameAnis[key] = new FrameAnimationData()
            {
                Sprites = sprites,

                First = first,

                Delay = delay,

                Loop = loop,

                FrameSeconds = frameSeconds,

                TargetImage = targetImage,

                ChangeUpdate = changeUpdate
            };

            if (endAction != null)
            {
                this.AniTypeEnd[key] = endAction;
            }
        }
        
        public void Modify(string key, Sprite[] sprites = null, Image targetImage = null, float[] frameSeconds = null, Action<string> endAction = null, Action<int, Sprite> changeUpdate = null, float delay = 0.0f, int loop = 0, int first = 0)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此 FrameAnimationComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.FrameAnis.TryGetValue(key, out FrameAnimationData data))
            {
                Log.Debug("这个还没有被添加过，请先添加，再修改");

                return;
            }

            data.Sprites = sprites;

            data.TargetImage = targetImage;

            data.FrameSeconds = frameSeconds;

            data.ChangeUpdate = changeUpdate;

            data.First = first;

            data.Delay = delay;

            data.Loop = loop;

            if (endAction != null)
            {
                this.AniTypeEnd[key] = endAction;
            }

        }

        /// <summary>
        /// 播放某一个帧动画，不能调用2次
        /// </summary>
        /// <param name="key"></param>
        public void Play(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此FrameAnimationComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.FrameAnis.TryGetValue(key, out FrameAnimationData data) || data.TargetImage == null)
            {
                Log.Error($"播放传入的 key:{key} 没有对应的播放数据");

                return;
            }

            data.PauseCount = -1;

            data.IsPlaying = true;

            data.IsPauseing = false;

            data.TargetImage.sprite = data.Sprites[0];

            this.BeginPlayAni(key).Coroutine();
        }

        /// <summary>
        /// 暂停某一个帧动画
        /// </summary>
        /// <param name="key"></param>
        public void Pause(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此 FrameAnimationComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.FrameAnis.TryGetValue(key, out FrameAnimationData data))
            {
                Log.Error($"暂停播放传入的 key:{key} 没有对应的播放数据");

                return;
            }

            data.IsPauseing = true;

            data.IsPlaying = false;
        }

        /// <summary>
        /// 恢复某一个帧动画
        /// </summary>
        /// <param name="key"></param>
        public void Recover(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此 FrameAnimationComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.FrameAnis.TryGetValue(key, out FrameAnimationData data))
            {
                Log.Error($"暂停播放传入的 key:{key} 没有对应的播放数据");

                return;
            }

            data.IsPauseing = false;

            data.IsPlaying = true;

            this.BeginPlayAni(key).Coroutine();
        }

        /// <summary>
        /// 停止某一个帧动画
        /// </summary>
        /// <param name="key"></param>
        public void Stop(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此 FrameAnimationComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.FrameAnis.TryGetValue(key, out FrameAnimationData data))
            {
                Log.Error($"暂停播放传入的 key:{key} 没有对应的播放数据");

                return;
            }

            this.FrameAnis.Remove(key);

            if (this.AniTypeEnd.TryGetValue(key, out Action<string> action))
            {
                action(key);

                this.AniTypeEnd.Remove(key);
            }
        }

        /// <summary>
        /// 开始遍历整个帧动画播放序列，进行播放。
        /// 可以多组播放精灵
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private async ETVoid BeginPlayAni(string key)
        {
            while (true)
            {
                if (this == null || this.InstanceId == 0)
                {
                    return;
                }

                if (!this.FrameAnis.TryGetValue(key, out FrameAnimationData item))
                {
                    Log.Error($"无法播放此帧动画，key:{key} 对应的帧动画数据没有");

                    return;
                }

                if (!item.IsPlaying)
                {
                    continue;
                }

                if (item.Delay > 0 && item.RecordLoopCount == 0)
                {
                    //只有当有延迟播放和播放次数为0的时候，才执行延迟调用

                    float time = item.Delay * 1000;

                    await this.TC.WaitAsync((long)time);
                }

                item.RecordLoopCount++;//开始加载图片

                int oCount = (item.PauseCount != -1) ? item.PauseCount : item.First;//从第几张开始播放图片

                int sCount = oCount;

                while (true)
                {
                    if (sCount > item.Sprites.Length - 1)
                    {
                        sCount = 0;
                    }

                    float time = item.FrameSeconds[sCount] * 1000;

                    await this.TC.WaitAsync((long)time);

                    if (item.TargetImage == null)
                    {
                        return;
                    }

                    if (sCount > item.Sprites.Length - 1)
                    {
                        sCount = 0;
                    }

                    item.TargetImage.sprite = item.Sprites[sCount];

                    item.ChangeUpdate?.Invoke(sCount, item.Sprites[sCount]);

                    sCount++;

                    if (sCount > item.Sprites.Length - 1)
                    {
                        sCount = 0;
                    }

                    if (sCount == oCount)
                    {
                        item.PauseCount = -1;

                        break;
                    }

                    if (item.IsPauseing)
                    {
                        item.PauseCount = sCount;

                        return;
                    }
                }

                if ((item.Loop == 0 || item.Loop == 1) || (item.Loop > 1 && item.RecordLoopCount == item.Loop))
                {
                    float time = item.FrameSeconds[sCount] * 1000;

                    await this.TC.WaitAsync((long)time);

                    item.TargetImage.sprite = item.Sprites[item.First];

                    item.IsPlaying = false;

                    item.RecordLoopCount = 0;

                    item.PauseCount = -1;

                    item.IsPauseing = true;

                    if (this.AniTypeEnd.TryGetValue(key, out Action<string> action))
                    {
                        action(key);
                    }

                    return;
                }
            }
        }


        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            AniTypeEnd.Clear();

            AniTypeEnd = null;

            FrameAnis.Clear();

            FrameAnis = null;
        }

    }
}
