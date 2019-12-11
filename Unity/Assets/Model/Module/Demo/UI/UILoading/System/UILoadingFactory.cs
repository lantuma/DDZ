using System;
using UnityEngine;

namespace ETModel
{
    public static class UILoadingFactory
    {
        public static UI Create()
        {
            try
            {
                //GameObject bundleGameObject = ((GameObject)ResourcesHelper.Load("KV")).Get<GameObject>(UIType.UILoading);
                GameObject bundleGameObject = ((GameObject)ResourcesHelper.Load("InitLoading/InitLoadingUI"));
                GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);
                go.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.InitLoadingUI, go, false);

                ui.AddComponent<UILoadingComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public static void Remove(string type)
        {
        }
    }
}