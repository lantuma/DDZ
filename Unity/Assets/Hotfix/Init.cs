using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class Init
    {
        public static void Start()
        {
#if ILRuntime
			if (!Define.IsILRuntime)
			{
				Log.Error("mono层是mono模式, 但是Hotfix层是ILRuntime模式");
			}
#else
            if (Define.IsILRuntime)
            {
                Log.Error("mono层是ILRuntime模式, Hotfix层是mono模式");
            }
#endif
            try
            {
                // 注册热更层回调
                ETModel.Game.Hotfix.Update = () => { Update(); };
                ETModel.Game.Hotfix.LateUpdate = () => { LateUpdate(); };
                ETModel.Game.Hotfix.OnApplicationQuit = () => { OnApplicationQuit(); };
                ETModel.Game.Hotfix.OnApplicationResume = () => { OnApplicationResume(); };
                ETModel.Game.Hotfix.OnApplicationPause = () => { OnApplicationPause(); };

                Game.Scene.AddComponent<UIComponent>();
                Game.Scene.AddComponent<OpcodeTypeComponent>();
                Game.Scene.AddComponent<MessageDispatcherComponent>();
                Game.Scene.AddComponent<DataCenterComponent>();
                Game.Scene.AddComponent<PopupComponent>();
                Game.Scene.AddComponent<TweenCommonAniComponent>();
                Game.Scene.AddComponent<MonoEventComponent>();
                Game.Scene.AddComponent<StressTestComponent>();
                Game.Scene.AddComponent<EventCenterController>();
                // 加载热更配置
                ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
                Game.Scene.AddComponent<ConfigComponent>();
                ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");
                UnitConfig unitConfig = (UnitConfig)Game.Scene.GetComponent<ConfigComponent>().Get(typeof(UnitConfig), 1001);
                Game.EventSystem.Run(EventIdType.InitPokerSceneStart);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void Update()
        {
            try
            {
                Game.EventSystem.Update();

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    var data = new MessageData();
                    data.onlyOk = false;
                    data.click = true;
                    data.ok = QuitGame;
                    Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.ExitAppTip, data);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private static void QuitGame()
        {
            Application.Quit();
        }

        public static void LateUpdate()
        {
            try
            {
                Game.EventSystem.LateUpdate();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void OnApplicationQuit()
        {
            Game.Close();
        }

        public static void OnApplicationResume()
        {
            Game.EventSystem.Run(EventIdType.OnApplicationResume);
        }

        public static void OnApplicationPause()
        {
            GameHelper.ApplicationIsPause = true;
        }

    }
}