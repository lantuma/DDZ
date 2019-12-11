using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public static class BundleHelper
    {
        public static async ETTask DownloadBundle()
        {
            if (!Define.UseEditorResouces)
            {
                try
                {
                    using (BundleDownloaderComponent bundleDownloaderComponent = Game.Scene.AddComponent<BundleDownloaderComponent>())
                    {
                        await bundleDownloaderComponent.StartAsync();

                        Game.EventSystem.Run(EventIdType.LoadingBegin);


                        return;
                        await bundleDownloaderComponent.DownloadAsync();

                        var loadingCpt = Game.Scene.GetComponent<UIComponent>().Get(UIType.InitLoadingUI)?.GetComponent<UILoadingComponent>();
                        if (loadingCpt != null) loadingCpt.TargetValue = 1;
                    }
                    //                    Game.EventSystem.Run(EventIdType.LoadingFinish);
                    //Debug.LogError("xxx");
                    Game.Scene.GetComponent<ResourcesComponent>().LoadOneBundle("StreamingAssets");
                    ResourcesComponent.AssetBundleManifestObject = (AssetBundleManifest)Game.Scene.GetComponent<ResourcesComponent>().GetAsset("StreamingAssets", "AssetBundleManifest");
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

            }
        }

        public static async ETTask StartDownloadBundle()
        {
            if (!Define.UseEditorResouces)
            {
                try
                {
                    await Game.Scene.GetComponent<BundleDownloaderComponent>().DownloadAsync();

                    var loadingCpt = Game.Scene.GetComponent<UIComponent>().Get(UIType.InitLoadingUI)?.GetComponent<UILoadingComponent>();
                    if (loadingCpt != null) loadingCpt.TargetValue = 1;

                    //                    Game.EventSystem.Run(EventIdType.LoadingFinish);
                    //Debug.LogError("xxx");
                    Game.Scene.GetComponent<ResourcesComponent>().LoadOneBundle("StreamingAssets");
                    ResourcesComponent.AssetBundleManifestObject = (AssetBundleManifest)Game.Scene.GetComponent<ResourcesComponent>().GetAsset("StreamingAssets", "AssetBundleManifest");

                    Game.Hotfix.LoadHotfixAssembly();
                    // 加载配置
                    Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
                    Game.Scene.AddComponent<ConfigComponent>();
                    Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");
                    Game.Scene.AddComponent<OpcodeTypeComponent>();
                    Game.Scene.AddComponent<MessageDispatcherComponent>();
                    Game.Scene.AddComponent<SoundComponent>();
                    Game.Scene.AddComponent<ETimerComponent>();
                    Game.Scene.AddComponent<SimpleObjectPoolComponent>();
                    Game.Scene.AddComponent<OpenInstallComponent>();
                    Game.Scene.AddComponent<QRCodeComponent>();
                    Game.Scene.AddComponent<StageScaleModeComponent>();
                    Game.Scene.AddComponent<LocalizationComponent>();
                    StageScaleModeComponent.Instance.ChangeScaleMode(StageScaleMode.FULL_SCREEN);
                    Game.Hotfix.GotoHotfix();
                    //Game.EventSystem.Run(EventIdType.TestHotfixSubscribMonoEvent, "TestHotfixSubscribMonoEvent");
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public static string GetBundleMD5(VersionConfig streamingVersionConfig, string bundleName)
        {
            string path = Path.Combine(PathHelper.AppHotfixResPath, bundleName);
            if (File.Exists(path))
            {
                return MD5Helper.FileMD5(path);
            }

            if (streamingVersionConfig.FileInfoDict.ContainsKey(bundleName))
            {
                return streamingVersionConfig.FileInfoDict[bundleName].MD5;
            }

            return "";
        }

        /// <summary>
        /// 获取远程版本文件
        /// </summary>
        /// <returns></returns>
        public static async ETTask<VersionConfig> GetRemoteVersion()
        {
            VersionConfig remoteVersionConfig;

            string versionUrl = GlobalConfigComponent.Instance.GlobalProto.GetUrl() + "StreamingAssets/" + "Version.txt";

            try
            {
                using (UnityWebRequestAsync webRequestAsync = ComponentFactory.Create<UnityWebRequestAsync>())
                {
                    await webRequestAsync.DownloadAsync(versionUrl);

                    remoteVersionConfig = JsonHelper.FromJson<VersionConfig>(webRequestAsync.Request.downloadHandler.text);
                }
            }
            catch (Exception e)
            {
                Log.Error($"download version.txt error:{e}");

                throw;
            }

            return remoteVersionConfig;
        }

        /// <summary>
        /// 获取本地版本文件[暂时取大版本号]
        /// </summary>
        /// <returns></returns>
        public static async ETTask<VersionConfig> GetLocalVersion()
        {
            VersionConfig streamingVersionConfig = new VersionConfig();

            streamingVersionConfig.Version = LargeVersion;

            return streamingVersionConfig;
        }

        /// <summary>
        /// 获取本地版本文件
        /// </summary>
        /// <returns></returns>
        public static async ETTask<VersionConfig> GetLocalVersionConfig()
        {
            VersionConfig streamingVersionConfig = new VersionConfig();
            string versionPath = Path.Combine(PathHelper.AppResPath4Web, "Version.txt");
            try
            {
                using (UnityWebRequestAsync request = ComponentFactory.Create<UnityWebRequestAsync>())
                {
                    //Log.Debug("本地版本文件路径:" + versionPath);
                    await request.DownloadAsync(versionPath);
                    streamingVersionConfig = JsonHelper.FromJson<VersionConfig>(request.Request.downloadHandler.text);
                }
            }
            catch(Exception e)
            {
                Log.Error($"获取本地版本文件 version.txt error:{e}");
            }
            return streamingVersionConfig;
        }

        /// <summary>
        /// 游戏大版本号
        /// </summary>
        public static int LargeVersion = 1;
    }
}
