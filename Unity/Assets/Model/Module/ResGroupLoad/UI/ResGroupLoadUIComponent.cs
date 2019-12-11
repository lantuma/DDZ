/******************************************************************************************
*         【模块】{ 资源组加载模块 }                                                                                                                      
*         【功能】{ 组加载UI面板 }                                                                                                                   
*         【修改日期】{ 2019年7月30日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class ResGroupLoadUIComponentAwakeSystem : AwakeSystem<ResGroupLoadUIComponent>
    {
        public override void Awake(ResGroupLoadUIComponent self)
        {
            self.Awake();
        }
    }

    public class ResGroupLoadUIComponent : Component
    {
        private GameObject panel;

        private ReferenceCollector _rf;

        public int index = 0;

        /// <summary>
        /// 需要加载的资源
        /// </summary>
        private Queue<string> bundles;

        /// <summary>
        /// 当前正在加载的AB
        /// </summary>
        private string downloadingBundle;

        /// <summary>
        /// web请求
        /// </summary>
        private UnityWebRequestAsync webRequest;

        /// <summary>
        /// 当前已经下载的数据大小
        /// </summary>
        public long alreadyDownloadBytes;

        /// <summary>
        /// 已经加载的AB包
        /// </summary>
        private HashSet<string> downloadedBundles;

        /// <summary>
        /// 总的大小
        /// </summary>
        private long TotalSize;


        public GameObject LoadSlider;

        private Button UpdateFlag;

        private Image sliderFg;

        private Text sliderValue;

        /// <summary>
        /// 下载超时限制
        /// </summary>
        public long downLoadTimeOut { get; set; } = 60;

        private CancellationTokenSource tokenSource = null;

        private CancellationToken cancelToken;

        public void Awake()
        {
            
        }

        public void SetPanel(GameObject panel, int index)
        {
            this.panel = panel;

            this._rf = this.panel.GetComponent<ReferenceCollector>();

            this.index = index;

            this.LoadSlider = _rf.Get<GameObject>("LoadSlider");

            this.UpdateFlag = _rf.Get<GameObject>("UpdateFlag").GetComponent<Button>();

            this.sliderFg = _rf.Get<GameObject>("sliderFg").GetComponent<Image>();

            this.sliderValue = _rf.Get<GameObject>("sliderValue").GetComponent<Text>();

            this.Reset();
        }

        public void Show()
        {

        }

        public void Hide()
        {

        }

        public void Reset()
        {
            this.TotalSize = 0;

            this.downloadingBundle = "";

            this.downloadedBundles = new HashSet<string>();

            this.bundles = new Queue<string>();

            this.alreadyDownloadBytes = 0;
        }

        /// <summary>
        /// 异步加载AB包
        /// </summary>
        /// <returns></returns>
        private async ETTask DownloadAsync()
        {
            if (this.bundles.Count == 0 && this.downloadingBundle == "")
            {
                return;
            }

            try
            {
                while (true)
                {
                    if (this.bundles.Count == 0)
                    {
                        break;
                    }
                    this.downloadingBundle = this.bundles.Dequeue();

                    while (true)
                    {
                        try
                        {
                            using (this.webRequest = ComponentFactory.Create<UnityWebRequestAsync>())
                            {
                                await this.webRequest.DownloadAsync(GlobalConfigComponent.Instance.GlobalProto.GetUrl() + "StreamingAssets/" + this.downloadingBundle);

                                byte[] data = this.webRequest.Request.downloadHandler.data;

                                this.alreadyDownloadBytes += (long)this.webRequest.Request.downloadedBytes;

                                string path = Path.Combine(PathHelper.AppHotfixResPath, this.downloadingBundle);

                                using (FileStream fs = new FileStream(path, FileMode.Create))
                                {
                                    fs.Write(data, 0, data.Length);
                                }

                                Log.Debug($"current downloadingBundle ------->>>{this.downloadingBundle}");
                            }
                        }
                        catch (Exception e)
                        {
                           // Log.Debug($"download bundle error:{this.downloadingBundle}\n{e}");

                            continue;
                        }

                        break;
                    }
                }

                this.downloadedBundles.Add(this.downloadingBundle);

                this.downloadingBundle = "";

                this.webRequest = null;

            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }


        /// <summary>
        /// 检测当前AB是否需要加载
        /// </summary>
        /// <param name="AbName"></param>
        /// <returns></returns>
        private void CheckABLoad(string AbName, VersionConfig remoteVersionConfig, VersionConfig streamingVersionConfig)
        {
            //则将本地MD5与远程MD5对比
            foreach (FileVersionInfo fileVersionInfo in remoteVersionConfig.FileInfoDict.Values)
            {
                if (fileVersionInfo.File == AbName.StringToAB())
                {
                    string localFileMD5 = BundleHelper.GetBundleMD5(streamingVersionConfig, fileVersionInfo.File);

                    if (fileVersionInfo.MD5 != localFileMD5)
                    {
                        this.bundles.Enqueue(fileVersionInfo.File);

                        this.TotalSize += fileVersionInfo.Size;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// 获取下载进度
        /// </summary>
        public int Progress
        {
            get
            {
                if (this.TotalSize == 0)
                {
                    return 0;
                }

                return (int)(this.alreadyDownloadBytes * 100f / this.TotalSize);
            }

        }

        /// <summary>
        /// 开启下载界面
        /// </summary>
        /// <returns></returns>
        public async ETTask StartAsync()
        {
            this.LoadSlider.SetActive(true);

            this.UpdateFlag.gameObject.SetActive(false);

            this.sliderFg.fillAmount = 0;

            this.sliderValue.text = "0%";

            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();

            while (true)
            {
                await timerComponent.WaitAsync(50);

                //Debug.Log("this.Progress:" + this.Progress);

                //根据Progress 进行相关表现
                var temp = (this.Progress / 100f).ToString("f2");

                float value = float.Parse(temp);

                this.sliderValue.text = this.Progress + "%";

                this.sliderFg.fillAmount = value;

                if (this.Progress >= 100)
                {
                    sliderValue.text = "100%";

                    sliderFg.fillAmount = 1;

                    await Task.Delay(500);

                    this.LoadSlider.SetActive(false);

                    break;

                    //结束回调方法
                }
            }
        }

        /// <summary>
        /// 下载超时检测
        /// </summary>
        /// <returns></returns>
        public async ETTask CheckDownLoadTimeOut()
        {
            if (this.tokenSource != null) this.tokenSource.Cancel();

            this.tokenSource = new CancellationTokenSource();

            this.cancelToken = this.tokenSource.Token;

            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();

            while (true)
            {
                await timerComponent.WaitAsync(this.downLoadTimeOut * 1000,this.cancelToken);

                if (this.Progress < 100)
                {
                    if (this.webRequest != null)
                    {
                        this.webRequest.isCancel = true;
                        
                        this.webRequest = null;
                    }

                    if (this.LoadSlider != null) this.LoadSlider.SetActive(false);

                    if(this.UpdateFlag != null) this.UpdateFlag.gameObject.SetActive(true);
                    
                    Game.EventSystem.Run("SubGameDownLoadFail", this.downloadingBundle);

                    break;
                }
            }
        }

        /// <summary>
        /// 取消超时检测
        /// </summary>
        public void StopCheckTimeOut()
        {
            if(this.tokenSource != null) this.tokenSource.Cancel();
        }

        /// <summary>
        /// 检测更新
        /// </summary>
        /// <param name="item"></param>
        /// <param name="remoteVersionConfig"></param>
        /// <param name="streamingVersionConfig"></param>
        public async void CheckUpdate(ResGroupData item, VersionConfig remoteVersionConfig, VersionConfig streamingVersionConfig)
        {
            Log.Debug("检查更新.....");

            this.Reset();//重置

            if (item.ResNames.Length == 0) { return; }

            foreach (string AbName in item.ResNames)
            {
                CheckABLoad(AbName, remoteVersionConfig, streamingVersionConfig);
            }

            Log.Debug("需要加载的AB资源数量:" + this.bundles.Count);

            if (this.bundles.Count > 0)
            {
                StartAsync().Coroutine();

                CheckDownLoadTimeOut().Coroutine();

                await this.DownloadAsync();
            }
            else
            {
                Log.Debug("不需要下载任何资源!!!!!");

                this.UpdateFlag.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 检测是否需要热更新
        /// </summary>
        /// <param name="item"></param>
        /// <param name="remoteVersionConfig"></param>
        /// <param name="streamingVersionConfig"></param>
        /// <returns></returns>
        public bool CheckModuleNeedLoad(ResGroupData item, VersionConfig remoteVersionConfig, VersionConfig streamingVersionConfig)
        {
            Log.Debug("检查是否需要热更新.....");

            this.Reset();//重置

            if (item.ResNames.Length == 0) { return false; }

            foreach (string AbName in item.ResNames)
            {
                CheckABLoad(AbName, remoteVersionConfig, streamingVersionConfig);
            }

            Log.Debug("需要加载的AB资源数量:" + this.bundles.Count);

            if (this.bundles.Count > 0)
            {
                return true;
            }

            return false;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.StopCheckTimeOut();

            base.Dispose();

            this.Reset();
        }
    }
}
