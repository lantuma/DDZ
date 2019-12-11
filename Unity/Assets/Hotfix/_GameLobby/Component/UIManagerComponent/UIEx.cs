using System.Collections.Generic;
using UnityEngine;
using ETModel;

namespace ETHotfix
{
	[ObjectSystem]
	public class UIEx_AwakeSystem : AwakeSystem<UIEx, GameObject>
	{
		public override void Awake(UIEx self, GameObject gameObject)
		{
			self.Awake(gameObject);
		}
	}
	
    /// <summary>
    /// UI实体类
    /// </summary>
	public sealed class UIEx: Entity
	{
		public string Name
		{
			get
			{
				return this.GameObject.name;
			}
		}

		public GameObject GameObject { get; private set; }

		public Dictionary<string, UIEx> children = new Dictionary<string, UIEx>();

        /// <summary>
        /// UI窗体主控组件
        /// </summary>
	    public UIBaseComponent UiComponent { get; private set; }

	    /// <summary>
	    /// 添加主UI组件，继承自UIBaseComponent
	    /// </summary>
	    /// <typeparam name="K"></typeparam>
	    /// <returns></returns>
	    public K AddUiComponent<K>() where K : UIBaseComponent, new()
	    {
	        UiComponent = this.AddComponent<K>();
	        return (K)UiComponent;
	    }

	    public K AddUiComponent<K, P1>(P1 p1) where K : UIBaseComponent, new()
	    {
	        UiComponent = this.AddComponent<K, P1>(p1);
	        return (K)UiComponent;
	    }

	    public K AddUiComponent<K, P1, P2>(P1 p1, P2 p2) where K : UIBaseComponent, new()
	    {
	        UiComponent = this.AddComponent<K, P1, P2>(p1, p2);
	        return (K)UiComponent;
	    }

	    public K AddUiComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where K : UIBaseComponent, new()
	    {
	        UiComponent = this.AddComponent<K, P1, P2, P3>(p1, p2, p3);
	        return (K)UiComponent;
	    }

        public void Awake(GameObject gameObject)
		{
			this.children.Clear();
			this.GameObject = gameObject;
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
			base.Dispose();

			foreach (UIEx ui in this.children.Values)
			{
				ui.Dispose();
			}
			
			UnityEngine.Object.Destroy(GameObject);
			children.Clear();
		}

		public void SetAsFirstSibling()
		{
			this.GameObject.transform.SetAsFirstSibling();
		}

		public void Add(UIEx ui)
		{
			this.children.Add(ui.Name, ui);
			ui.Parent = this;
		}

		public void Remove(string name)
		{
            UIEx ui;
			if (!this.children.TryGetValue(name, out ui))
			{
				return;
			}
			this.children.Remove(name);
			ui.Dispose();
		}

		public UIEx Get(string name)
		{
			UIEx child;
			if (this.children.TryGetValue(name, out child))
			{
				return child;
			}
			GameObject childGameObject = this.GameObject.transform.Find(name)?.gameObject;
			if (childGameObject == null)
			{
				return null;
			}
			child = ComponentFactory.Create<UIEx, GameObject>(childGameObject);
			this.Add(child);
			return child;
		}
	}
}