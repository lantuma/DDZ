/******************************************************************************************
*         【模块】{ 资源组加载模块 }                                                                                                                      
*         【功能】{ 按组加载指定资源 }                                                                                                                   
*         【修改日期】{ 2019年7月8日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class ResGroupLoadComponentAwakeSystem : AwakeSystem<ResGroupLoadComponent>
    {
        public override void Awake(ResGroupLoadComponent self)
        {
            self.Awake();
        }
    }
    
    public class ResGroupLoadComponent : Component
    {
        
        /// <summary>
        /// 待加载资源组
        /// </summary>
        private Dictionary<string, ResGroupData> resGroups; 

        /// <summary>
        /// 下载结束回调方法
        /// </summary>
        private Dictionary<string, Action> AniTypeEnd;

        /// <summary>
        /// 本地版本文件
        /// </summary>
        private VersionConfig streamingVersionConfig;

        /// <summary>
        /// 远程版本文件
        /// </summary>
        private VersionConfig remoteVersionConfig;
        
        /// <summary>
        /// 单例
        /// </summary>
        public static ResGroupLoadComponent Instance;

        public void Awake()
        {
            Instance = this;

            this.resGroups = new Dictionary<string, ResGroupData>();

            this.AniTypeEnd = new Dictionary<string, Action>();

            this.Reset();
        }
        
        private void Reset()
        {
            
        }

        /// <summary>
        /// 增加资源组
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ResNames"></param>
        /// <param name="LoadingPanel"></param>
        /// <param name="endAction"></param>
        public void Add(string key, string[] ResNames, ResGroupLoadUIComponent LoadingPanel, Action endAction = null)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此ResGroupLoadComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (this.resGroups.TryGetValue(key, out ResGroupData data))
            {
                Log.Error("已经添加过");

                return;
            }

            if (key == "" || key == null)
            {
                Log.Error("key不能为空");

                return;
            }

            if (ResNames.Length == 0 || ResNames == null)
            {
                Log.Error("ResNames 不能为空");

                return;
            }

            if (LoadingPanel == null)
            {
                Log.Error("资源组加载面面板对象不能为空");

                return;
            }

            this.resGroups[key] = new ResGroupData()
            {
                ResNames = ResNames,

                LoadingPanel = LoadingPanel
            };

            if (endAction != null)
            {
                this.AniTypeEnd[key] = endAction;
            }
        }

        /// <summary>
        /// 加载资源组
        /// </summary>
        /// <param name="key"></param>
        public async void Load(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此ResGroupLoadComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.resGroups.TryGetValue(key, out ResGroupData data))
            {
                Log.Error($"播放传入的 key:{key} 没有对应的资源组数据");

                return;
            }

            //重置
            this.Reset();

            if(this.remoteVersionConfig == null) this.remoteVersionConfig = await BundleHelper.GetRemoteVersion();

            if(this.streamingVersionConfig == null) this.streamingVersionConfig = await BundleHelper.GetLocalVersionConfig();

            this.BeginLoad(key);
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            AniTypeEnd.Clear();
            
            resGroups.Clear();
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        /// <param name="key"></param>
        private async void BeginLoad(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此ResGroupLoadComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.resGroups.TryGetValue(key, out ResGroupData item))
            {
                Log.Error($"播放传入的 key:{key} 没有对应的资源组数据");

                return;
            }

            item.LoadingPanel.CheckUpdate(item,remoteVersionConfig,streamingVersionConfig);
        }

        /// <summary>
        /// 检测模块是否需要热更新
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> CheckModuleNeedLoad(string key)
        {
            if (Define.UseEditorResouces)
            {
                return false;
            }
            
            if (!this.resGroups.TryGetValue(key, out ResGroupData item))
            {
                Log.Error($"播放传入的 key:{key} 没有对应的资源组数据");

                return false;
            }
            if (this.remoteVersionConfig == null) this.remoteVersionConfig = await BundleHelper.GetRemoteVersion();

            if (this.streamingVersionConfig == null) this.streamingVersionConfig = await BundleHelper.GetLocalVersionConfig();

           return  item.LoadingPanel.CheckModuleNeedLoad(item, remoteVersionConfig, streamingVersionConfig);
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

            resGroups.Clear();

            resGroups = null;
        }
    }

    /// <summary>
    /// 资源组数据
    /// </summary>
    public class ResGroupData
    {
        //该组需要加载的资源名
        public string[] ResNames;

        //加载面板对象
        public ResGroupLoadUIComponent LoadingPanel;
    }
}
