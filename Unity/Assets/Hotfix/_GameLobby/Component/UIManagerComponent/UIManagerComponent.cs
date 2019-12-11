/******************************************************************************************
*         【模块】{ UI管理组件改进版 }                                                                                                                      
*         【功能】{ UI管理器}
*         
*         【1】关闭界面不直接销毁物体，避免重复加载和实例化物体，减轻运行压力
*         【2】只需进行一次获取引用操作,减少GetComponent,以及RC中取出物体调用,节省性能开销
*         【3】扩展方便<传递参数+动画事件>
*         
*         【修改日期】{ 2019年3月28日 }                                                                                                                        
*         【贡献者】{ 周瑜(整合改进) }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIManagerComponent_AwakeSystem : AwakeSystem<UIManagerComponent>
    {
        public override void Awake(UIManagerComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIManagerComponent_LoadSystem : LoadSystem<UIManagerComponent>
    {
        public override void Load(UIManagerComponent self)
        {
            self.Load();
        }
    }

    /// <summary>
    /// 管理所有UI
    /// </summary>
    public class UIManagerComponent : Component
    {
        private GameObject Root;
        private Dictionary<string, IUIFactoryEx> UiTypes;
        private readonly Dictionary<string, UIEx> uis = new Dictionary<string, UIEx>();
        
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            foreach (string type in uis.Keys.ToArray())
            {
                UIEx ui;
                if (!uis.TryGetValue(type, out ui))
                {
                    continue;
                }
                uis.Remove(type);
                ui.Dispose();
            }

            base.Dispose();
        }

        public void Awake()
        {
            this.Root = Component.Global.transform.Find("UI").gameObject;
            this.Load();
        }


        public void Load()
        {
            UiTypes = new Dictionary<string, IUIFactoryEx>();

            foreach (Type type in ETModel.Game.Hotfix.GetHotfixTypes())
            {
                object[] attrs = type.GetCustomAttributes(typeof(UIFactoryAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                UIFactoryAttribute attribute = attrs[0] as UIFactoryAttribute;
                if (UiTypes.ContainsKey(attribute.Type))
                {
                    Log.Debug($"已经存在同类UI Factory: {attribute.Type}");
                    throw new Exception($"已经存在同类UI Factory: {attribute.Type}");
                }
                object o = Activator.CreateInstance(type);
                IUIFactoryEx factory = o as IUIFactoryEx;
                if (factory == null)
                {
                    Log.Error($"{o.GetType().FullName} 没有继承 IUIFactory");
                    continue;
                }
                this.UiTypes.Add(attribute.Type, factory);
            }
        }

        /// <summary>
        /// 创建ui
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public UIEx Create(string type)
        {
            try
            {
                UIEx ui;

                if (uis.ContainsKey(type))
                {
                    ui = uis[type];
                }
                else
                {
                    ui = UiTypes[type].Create(this.GetParent<Scene>(), type, Root);
                    uis.Add(type, ui);
                }

                // 设置canvas
                SetViewParent(ui, ui.GameObject.GetComponent<CanvasConfig_Z>().CanvasName);
                ui.UiComponent.Show();

                return ui;
            }
            catch (Exception e)
            {
                throw new Exception($"{type} UI 错误: {e.ToStr()}");
            }
        }

        /// <summary>
        /// 关闭ui
        /// </summary>
        /// <param name="type"></param>
        public void Close(string type)
        {
            UIEx ui;
            if (!uis.TryGetValue(type, out ui))
            {
                return;
            }
            uis[type].UiComponent.Close();
            SetViewParent(uis[type], ZCanvasType.UIHiden);
        }

        /// <summary>
        /// 设置ui显示层级
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="layer"></param>
        private void SetViewParent(UIEx ui, ZCanvasType layer)
        {
            RectTransform _rt = ui.GameObject.GetComponent<RectTransform>();
            _rt.SetParent(this.Root.Get<GameObject>(layer.ToString()).transform, false);

            ui.UiComponent.Layer = layer;
        }

        /// <summary>
        /// 移除窗体
        /// </summary>
        /// <param name="type"></param>
        public void Remove(string type)
        {
            UIEx ui;
            if (!uis.TryGetValue(type, out ui))
            {
                return;
            }
            //如果没有关闭，先关闭再移除
            if (ui.UiComponent.InShow)
            {
                Close(type);
            }

            UiTypes[type].Remove(type);
            uis.Remove(type);
            ui.Dispose();
        }

        public void RemoveAll()
        {
            foreach (string type in this.uis.Keys.ToArray())
            {
                Remove(type);
            }
        }

        public UIEx Get(string type)
        {
            UIEx ui;
            this.uis.TryGetValue(type, out ui);
            return ui;
        }

        public List<string> GetUITypeList()
        {
            return new List<string>(this.uis.Keys);
        }
    }
}