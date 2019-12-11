using System;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class UiLoadingComponentAwakeSystem : AwakeSystem<UILoadingComponent>
    {
        public override void Awake(UILoadingComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UiLoadingComponentStartSystem : StartSystem<UILoadingComponent>
    {
        public override void Start(UILoadingComponent self)
        {
            self.Start();
        }

        
    }

    [ObjectSystem]
    public class UILoadingComponentUpdateSystem : UpdateSystem<UILoadingComponent>
    {
        public override void Update(UILoadingComponent self)
        {
            self.Update();
        }
    }

    public class UILoadingComponent : Component
    {
        public Text UpNumText;

        public Slider updateSlider;

        public float TargetValue;
        
        private BundleDownloaderComponent bundleDownloaderComponent;

        /// <summary>
        /// 是否开始倒计时
        /// </summary>
        private bool _IsCountDown;

        /// <summary>
        /// 是否需要下载
        /// </summary>
        private bool _IsDownload;

        private Text _AppVersionText;
        private Text _ResouceVersionText;

        public void Awake()
        {
            bundleDownloaderComponent = Game.Scene.GetComponent<BundleDownloaderComponent>();
            ReferenceCollector referenceCtrl = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            UpNumText = GetParent<UI>().GameObject.TryGetInChilds<Text>("UpNumText");
            updateSlider = GetParent<UI>().GameObject.TryGetInChilds<Slider>("UpdateSlider");
            updateSlider.value = 0f;
            
            _AppVersionText = referenceCtrl.Get<GameObject>("AppVersionText").Get<Text>();
            _ResouceVersionText = referenceCtrl.Get<GameObject>("ResouceVersionText").Get<Text>();
        }

        public async void Start()
        {
            _AppVersionText.text = "";

            _ResouceVersionText.text = "";

            updateSlider.value = 0f;

            _IsCountDown = false;

            Log.Debug("首先获取后台配置，成功后再进入更新流程");
            
            var isSuccess = await RequestWebConfigURL("http://39.98.214.224:666/syskuma/api/getGameConfig");
            
            if (!isSuccess)
            {
                Log.Error("获取配置失败(可能是超时，可能是解析异常)!!");

                return;
            }

            var webConfig = ClientComponent.Instance.webConfig;

            if (webConfig != null)
            {
                this._ResouceVersionText.text = $"资源版本号：{webConfig.zybbh}";

                this._AppVersionText.text = $"APP版本号：{webConfig.dbbh}";
            }

            BundleDownloaderComponent bundleDownloaderComponent = Game.Scene.GetComponent<BundleDownloaderComponent>();
            if (bundleDownloaderComponent == null) { return; }
            
            UpNumText.text = "";

            _IsDownload = bundleDownloaderComponent.TotalSize - bundleDownloaderComponent.CurrVersionBytes > 0;
            
            if (!_IsDownload)
            {
                TargetValue = 1f;
            }
            else
            {
                OnOkButton();
            }
        }

        /// <summary>
        /// 获取后台配置
        /// </summary>
        /// <param name="url"></param>
        private async ETTask<bool> RequestWebConfigURL(string url)
        {
            try
            {
                var response = await HttpRequestHelper.SendPostRequestAsync<WebConfigResponse>(url, "");

                ClientComponent.Instance.webConfig = response;

                Log.Debug("获取后台配置成功");

                return true;

            }
            catch (Exception e)
            {
                Log.Error("获取后台配置失败");

                return false;
            }
        }

        public async ETVoid StartAsync()
        {
            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();
            while (_IsCountDown)
            {
                await timerComponent.WaitAsync(50);

                if (TargetValue >= 1)
                {
                    _IsCountDown = false;
                    return;
                }

                
                if (bundleDownloaderComponent == null)
                {
                    continue;
                }

                var temp = (bundleDownloaderComponent.Progress / 100f).ToString("f2");

                TargetValue = float.Parse(temp);

                if (UpNumText != null)
                {
                    updateSlider.value = TargetValue;
                    UpNumText.text = $"正在下载（{bundleDownloaderComponent.alreadyDownloadBytes.ConvertToHumanReadSize()}/{bundleDownloaderComponent.TotalSize.ConvertToHumanReadSize()}）{bundleDownloaderComponent.Progress}%";
                }
            }
        }

        public void Update()
        {
            if (!_IsDownload)
            {
                if (updateSlider != null)
                {
                    UpNumText.text = $"正在加载（{updateSlider.value*100:##0}%）";
                }
                if (updateSlider?.value >= 1)
                {
                    OnOkButton();

                    return;
                }

                if (TargetValue >= 1)
                {
                    if (updateSlider != null) updateSlider.value += 2e-2f;
                }
                else if (updateSlider?.value < TargetValue)
                {
                    if (updateSlider != null) updateSlider.value += 2e-3f;
                }
            }
        }

        private async void OnOkButton()
        {
            _IsDownload = true;
            _IsCountDown = true;
            StartAsync().Coroutine();
            await BundleHelper.StartDownloadBundle();
            _IsCountDown = false;
            Game.EventSystem.Run(EventIdType.LoadingFinish);
        }
    }
}
