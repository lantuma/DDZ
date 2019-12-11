using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETHotfix
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
	public class UIComponent: Component
	{
		public GameObject Root; 
		
		public Dictionary<string, UI> uis = new Dictionary<string, UI>();

        public void Awake()
        {
            Root = Component.Global.transform.Find("UI").gameObject;
        }

		public void Add(UI ui)
		{
			//ui.GameObject.GetComponent<Canvas>().worldCamera = this.Camera.GetComponent<Camera>();
			
			this.uis.Add(ui.Name, ui);

            // 设置canvas
            string cavasName = ui.GameObject.GetComponent<CanvasConfig>().CanvasName.ToString();

            ui.GameObject.transform.SetParent(this.Root.Get<GameObject>(cavasName).transform, false);

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