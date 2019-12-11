using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    [ObjectSystem]
    public class UiBundleDownloaderComponentAwakeSystem : AwakeSystem<BundleDownloaderComponent>
    {
        public override void Awake(BundleDownloaderComponent self)
        {
            self.bundles = new Queue<string>();
            self.downloadedBundles = new HashSet<string>();
            self.downloadingBundle = "";
        }
    }

    /// <summary>
    /// 用来对比web端的资源，比较md5，对比下载资源
    /// </summary>
    public class BundleDownloaderComponent : Component
    {
        private VersionConfig remoteVersionConfig;

        public Queue<string> bundles;
        /// <summary>
        /// 当前已经下载的数据大小
        /// </summary>
        public long alreadyDownloadBytes;
        /// <summary>
        /// 需要下载的资源总大小
        /// </summary>
		public long TotalSize;

        public long CurrVersionBytes;

        public HashSet<string> downloadedBundles;

        public string downloadingBundle;

        public UnityWebRequestAsync webRequest;

        /// <summary>
        /// 初始时过滤掉资源[临时]
        /// </summary>
        private  string[] FilterResNames =
            {
                "uizjhgamepanel","uizjhhelppanel","zjhatlas",

                "uiniuniugamepanel","uiniuniuhelppanel","uiniuniusettingpanel","uiniuniutrendpanel","niuniuatlas",//bairenniuniuatlas

                "bjlgame",// 大厅为毛为依赖这玩意 baijialeatlas

                "uitexaspokerpanel", "texaspokeratlas", "texaspokerplayeritem","uitexaspokerhelppanel",//dezhoupukeatlas

                "nhdgame",//"longhudouatlas",

                "hongheitableatlas","hongheiroadatlas","hongheiroad","hongheipanel","hongheihelp",//hongheidazhanatlas

                "ddzgame","ddzfx",

                "uifruitmachinepanel","uifruitmachinehelppanel","fruitmachineatlas",

                "qznngame",

                "uiblackjackpanel","uiblackjackhelppanel"//21点
            };

        public async ETTask StartAsync()
        {
            alreadyDownloadBytes = 0;

            TotalSize = 0;

            CurrVersionBytes = 0;
            
            string versionUrl = "";

            versionUrl = GlobalConfigComponent.Instance.GlobalProto.GetUrl() + "StreamingAssets/" + "Version.txt";

            Log.Debug($"当前资源为AB模式,远程版本文件路径:" + versionUrl);

            try
            {
                using (UnityWebRequestAsync webRequestAsync = ComponentFactory.Create<UnityWebRequestAsync>())
                {
                    await webRequestAsync.DownloadAsync(versionUrl);

                    CurrVersionBytes += (long)webRequestAsync.Request.downloadedBytes;

                    // 获取远程的Version.txt
                    remoteVersionConfig = JsonHelper.FromJson<VersionConfig>(webRequestAsync.Request.downloadHandler.text);
                }
            }
            catch (Exception e)
            {
                Log.Error($"download version.txt error: {e}");
                return;
            }

            // 获取streaming目录的Version.txt
            VersionConfig streamingVersionConfig;

            string versionPath = Path.Combine(PathHelper.AppResPath4Web, "Version.txt");

            using (UnityWebRequestAsync request = ComponentFactory.Create<UnityWebRequestAsync>())
            {
                Log.Debug("本地版本文件路径:" + versionPath);

                await request.DownloadAsync(versionPath);

                streamingVersionConfig = JsonHelper.FromJson<VersionConfig>(request.Request.downloadHandler.text);
            }

            // 删掉远程不存在的文件
            DirectoryInfo directoryInfo = new DirectoryInfo(PathHelper.AppHotfixResPath);
            if (directoryInfo.Exists)
            {
                FileInfo[] fileInfos = directoryInfo.GetFiles();
                foreach (FileInfo fileInfo in fileInfos)
                {
                    if (remoteVersionConfig.FileInfoDict.ContainsKey(fileInfo.Name))
                    {
                        continue;
                    }

                    if (fileInfo.Name == "Version.txt")
                    {
                        continue;
                    }

                    fileInfo.Delete();
                }
            }
            else
            {
                directoryInfo.Create();
            }
            // 对比MD5
            foreach (FileVersionInfo fileVersionInfo in remoteVersionConfig.FileInfoDict.Values)
            {
                // 对比md5
                string localFileMD5 = BundleHelper.GetBundleMD5(streamingVersionConfig, fileVersionInfo.File);
                //如果MD5相同，且当前项目不含有这个文件
                if (fileVersionInfo.MD5 == localFileMD5)
                {
                    continue;
                }

                if (IsFilterRes(fileVersionInfo.File))
                {
                    continue;
                }
                
                this.bundles.Enqueue(fileVersionInfo.File);

                this.TotalSize += fileVersionInfo.Size;

            }
        }

        /// <summary>
        /// 是否是过滤资源
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool IsFilterRes(string fileName)
        {
            bool isFilter = false;

            foreach (var item in FilterResNames)
            {
                //增加:后续需要动态加载的资源包名全部采用‘_’结尾
                if (item.StringToAB() == fileName || item.StartsWith("_"))
                {
                    isFilter = true;

                    break;
                }
            }

            return isFilter;
        }

        public int Progress
        {
            get
            {
                if (this.TotalSize == 0)
                {
                    return 0;
                }

                #region 不建议这么改
                /*
                long alreadyDownloadBytes = 0;
                foreach (string downloadedBundle in this.downloadedBundles)
                {
                    long size = this.remoteVersionConfig.FileInfoDict[downloadedBundle].Size;
                    alreadyDownloadBytes += size;
                }
                if (this.webRequest != null)
                {
                    alreadyDownloadBytes += (long)this.webRequest.Request.downloadedBytes;
                }
                */
                #endregion
                return (int)(alreadyDownloadBytes * 100f / this.TotalSize);
            }
        }

        public async ETTask DownloadAsync()
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

                                alreadyDownloadBytes += (long)this.webRequest.Request.downloadedBytes;

                                string path = Path.Combine(PathHelper.AppHotfixResPath, this.downloadingBundle);

                                using (FileStream fs = new FileStream(path, FileMode.Create))
                                {
                                    fs.Write(data, 0, data.Length);
                                }

                                Log.Debug($"<color=green>current downloadingBundle ------->>> {this.downloadingBundle}</color>");
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Error($"download bundle error: {this.downloadingBundle}\n{e}");
                            continue;
                        }

                        break;
                    }
                    this.downloadedBundles.Add(this.downloadingBundle);

                    this.downloadingBundle = "";

                    this.webRequest = null;
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
