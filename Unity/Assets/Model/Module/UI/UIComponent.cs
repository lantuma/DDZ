using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class UIComponentAwakeSystem : AwakeSystem<UIComponent>
    {
        public override void Awake(UIComponent self)
        {
            self.Awake();

        }
    }

    /// <summary>
    /// 管理所有UI
    /// </summary>
    public class UIComponent : Component
    {
        public GameObject Root;

        public Dictionary<string, UI> uis = new Dictionary<string, UI>();

        public void Awake()
        {
            Root = Component.Global.transform.Find("UI").gameObject;
           // Root.Get<GameObject>("BlackMask").SetActive(!(Mathf.Abs((float)Screen.width / (float)Screen.height - 16f / 9f) < 0.05f));
        }

        public void Add(UI ui)
        {
            //ui.GameObject.GetComponent<Canvas>().worldCamera = this.Camera.GetComponent<Camera>();
            var canvasConfig = ui.GameObject.GetComponent<CanvasConfig>();
            if (canvasConfig != null)
            {
                var parent = Root.Get<GameObject>(canvasConfig.CanvasName.ToString()).transform;
                ui.GameObject.transform.SetParent(parent, false);
            }
            else
                ui.GameObject.transform.SetParent(Root.Get<GameObject>("Background").transform, false);
            this.uis.Add(ui.Name, ui);
            //ui.Parent = this;
        }

        public void Remove(string name)
        {
            if (!this.uis.TryGetValue(name, out UI ui))
            {
                return;
            }
            this.uis.Remove(name);
            ui.Dispose();
        }

        public UI Get(string name)
        {
            UI ui = null;
            this.uis.TryGetValue(name, out ui);
            return ui;
        }
    }
}