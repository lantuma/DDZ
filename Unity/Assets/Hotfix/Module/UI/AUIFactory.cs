using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public abstract class AUIFactory
    {
        /// <summary>
        /// 创建UI
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static UI Create(string type)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle(type.StringToAB());
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(type.StringToAB(), type);
                GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);

                UI ui = ComponentFactory.Create<UI, string, GameObject>(type, gameObject, false);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        /// <summary>
        /// 移除UI
        /// </summary>
        /// <param name="type"></param>
        public static void Remove(string type)
        {
            if (Game.Scene.GetComponent<UIComponent>().Get(type) == null) return;
            Game.Scene.GetComponent<UIComponent>().Remove(type);
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(type.StringToAB());
        }
    }
}