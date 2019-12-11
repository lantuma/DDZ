using System;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace ETModel
{
    public class Init : MonoBehaviour
    {
        //private const string BuglyAppIDForiOS = "ff29ef5321";
        //private const string BuglyAppIDForAndroid = "b4f8470805";

        /// <summary>
        /// 是否使用编辑器资源
        /// </summary>
        public bool UseEditorResouces;
        public bool IsOutPutServerMsg;
        
        private void Start()
        {
            //InitBuglySDK();

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.runInBackground = true;
            Application.targetFrameRate = 30;

#if UNITY_EDITOR
            Define.UseEditorResouces = UseEditorResouces;
            Define.IsOutPutServerMsg = IsOutPutServerMsg;
#else
		    Define.UseEditorResouces = false;
            Define.IsOutPutServerMsg = false;
#endif
            this.StartAsync().Coroutine();

        }

        private async ETVoid StartAsync()
        {
            try
            {
                SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

                DontDestroyOnLoad(gameObject);
                Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);
                Game.Scene.AddComponent<TimerComponent>();
                Game.Scene.AddComponent<GlobalConfigComponent>();
                Game.Scene.AddComponent<NetOuterComponent>();
                Game.Scene.AddComponent<ResourcesComponent>();
                Game.Scene.AddComponent<PlayerComponent>();
                Game.Scene.AddComponent<UnitComponent>();
                Game.Scene.AddComponent<UIComponent>();
                
                Game.Scene.AddComponent<ResGroupLoadComponent>();
                Game.Scene.AddComponent<MobileSdkComponent>();
                Game.Scene.AddComponent<CarouseComponent>();
                Game.Scene.AddComponent<TaskManagerComponent>();
                Game.Scene.AddComponent<ClickEffectComponent>();
                Game.Scene.AddComponent<AsyncImageDownloadComponent>();
                
                //棋牌游戏类型
                Game.Scene.AddComponent<ClientComponent>();
                // 下载ab包
                await BundleHelper.DownloadBundle();

                if (Define.UseEditorResouces)
                {
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
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void Update()
        {
            OneThreadSynchronizationContext.Instance.Update();
            Game.Hotfix.Update?.Invoke();
            Game.EventSystem.Update();
        }
    

        private void LateUpdate()
        {
            Game.Hotfix.LateUpdate?.Invoke();
            Game.EventSystem.LateUpdate();
        }

        private void OnApplicationQuit()
        {
            Game.Hotfix.OnApplicationQuit?.Invoke();
            Game.Close();
        }

        private void OnApplicationPause(bool bPause)
        {
            if (bPause)
            {
                Game.Hotfix.OnApplicationPause?.Invoke();
                Game.EventSystem.Run(EventIdType.ApplicationPause);
            }
            else
            {
                Game.Hotfix.OnApplicationResume?.Invoke();
                Game.EventSystem.Run(EventIdType.ApplicationResume);
            }
        }

        /// <summary>
        /// 添加白名单---针对IOS平台特殊处理
        /// </summary>
        private void AddWhiteList()
        {
            var name = this.gameObject.name;

            Queue<int> queue = new Queue<int>();
            transform.SetAsLastSibling();
            var vect = UnityEngine.Random.insideUnitCircle;

            var poolparent = new GameObject("test");

            Queue<GameObject> poolquque = new Queue<GameObject>();

            poolparent.gameObject.SetActive(false);

            var findParent = GameObject.Find("test").transform;

            poolquque.Enqueue(new GameObject("test"));

            var get = poolquque.Dequeue();

            var str = new string(new char[3]);
            var color = UnityEngine.Random.ColorHSV();
        }
    }
}