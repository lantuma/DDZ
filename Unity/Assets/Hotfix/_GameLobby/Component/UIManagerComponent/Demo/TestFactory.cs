using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ETModel;

namespace ETHotfix
{
    [UIFactory(UIType.Login)]
    public  class TestFactory: IUIFactoryEx
    {
        public  UIEx Create(Scene scene, string type, GameObject parent)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle(type.StringToAB());
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(type.StringToAB(), type);
                GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
                gameObject.layer = LayerMask.NameToLayer(LayerNames.UI);

                UIEx ui = ComponentFactory.Create<UIEx,GameObject>(gameObject);

                ui.AddUiComponent<TestComponent>();
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;

            }
        }

        public  void Remove(string type)
        {
            //Game.Scene.GetComponent<UIComponent_Z>().Remove(type);
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(type.StringToAB());
        }
    }
}
