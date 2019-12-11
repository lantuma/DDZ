/******************************************************************************************
*         【模块】{ 特效播放组件 }                                                                                                                      
*         【功能】{ 统一管理游戏特效播放 }
*         【修改日期】{ 2019年6月20日 }                                                                                                                        
*         【贡献者】{ 周瑜 ｝                                                                                                               
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class FXAnimationComponentAwakeSystem : AwakeSystem<FXAnimationComponent>
    {
        public override void Awake(FXAnimationComponent self)
        {
            self.Awake();
        }
    }

    public class FXAnimationComponent:Component
    {
        private class FXAnimationData
        {
            //动画时长,单位:秒
            public float PlayTime;

            //延迟几秒之后播放,单位:秒
            public float Delay = 0.0f;

            //是否循环播放 -1 表示循环播放，0和1都表示播放1次
            public int Loop = 0;

            //记录播放次数
            public int RecordLoopCount = 0;

            //是否在播放
            public bool IsPlaying;

            //特效目标父级
            public Transform ParentTarget;

            //特效对象
            public Transform FXTarget;

            //图集名称
            public string AtlasName;

            //是否缓存特效
            public bool CacheFX = true;
        }

        //播放的特效动画
        private Dictionary<string, FXAnimationData> FXAnis;

        //动画播放结束,调用Stop方法，都会触发
        private Dictionary<string, Action> AniTypeEnd;

        public void Awake()
        {
            FXAnis = new Dictionary<string, FXAnimationData>();

            AniTypeEnd = new Dictionary<string, Action>();
        }

        public void Add(string key,string atlasName,float playTime, Transform parentTarget, Action endAction = null, float delay = 0.0f, int loop = 0,bool cacheFX = true)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此FXComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (this.FXAnis.TryGetValue(key, out FXAnimationData data))
            {
                Log.Error("已经添加过，如果需要修改参数，使用ModifyData方法");

                return;
            }

            if (key == FXAnimationIdType.None || key == "" || key == null)
            {
                Log.Error("key需要值定义在FXNameIdType里面");

                return;
            }

            if (parentTarget == null)
            {
                throw new Exception("ParentTarget参数不能为空");
            }

            if (atlasName == "")
            {
                Log.Error("AtlasName 不能为空");

                return;
            }

            if (playTime <= 0)
            {
                Log.Error("播放时长必须大于0");

                return;
            }

            this.FXAnis[key] = new FXAnimationData()
            {
                Delay = delay,

                Loop = loop,

                ParentTarget = parentTarget,

                FXTarget = null,

                AtlasName = atlasName,

                PlayTime = playTime,

                CacheFX = cacheFX
            };

            if (endAction != null)
            {
                this.AniTypeEnd[key] = endAction;
            }
        }

        public void Play(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此FXComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.FXAnis.TryGetValue(key, out FXAnimationData data) || data.ParentTarget == null)
            {
                Log.Error($"播放传入的 key:{key} 没有对应的播放数据");

                return;
            }

            data.IsPlaying = true;

            this.BeginPlayAni(key);
        }

        public void Stop(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此 FXComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.FXAnis.TryGetValue(key, out FXAnimationData data))
            {
                Log.Error($"暂停播放传入的 key:{key} 没有对应的播放数据");

                return;
            }

            this.FXAnis.Remove(key);

            if (this.AniTypeEnd.TryGetValue(key, out Action action))
            {
                action();

                this.AniTypeEnd.Remove(key);
            }
        }

        private async void BeginPlayAni(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此FXComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.FXAnis.TryGetValue(key, out FXAnimationData item))
            {
                Log.Error($"播放传入的key:{key} 没有对应的播放数据");

                return;
            }
            //如果没有，则加载一次
            if (item.FXTarget == null)
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(item.AtlasName.StringToAB(), key);

                GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, item.ParentTarget);

                gameObject.transform.localPosition = Vector3.zero;

                GameObject.transform.localScale = Vector3.one;

                item.FXTarget = gameObject.transform;
            }

            await Task.Delay((int)item.Delay * 1000);

            item.FXTarget.gameObject.SetActive(true);

            await Task.Delay((int)item.PlayTime * 1000);

            item.IsPlaying = false;

            if (item.CacheFX)
            {
                item.FXTarget.gameObject.SetActive(false);
            }
            else
            {
                //如果不缓存，直接删掉

                GameObject.Destroy(item.FXTarget.gameObject);

                item.FXTarget = null;
            }

            if (AniTypeEnd.TryGetValue(key, out Action endAction))
            {
                endAction();
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

            FXAnis.Clear();

            FXAnis = null;
        }
    }
}
