/******************************************************************************************
*         【模块】{ Mono事件组件 }                                                                                                                      
*         【功能】{ 动态添加事件、触发等事件 }
*         【修改日期】{ 2019年7月17日 }                                                                                                                        
*         【贡献者】{ 周瑜  整理 }                                                                                                           
*                                                                                                                                        
 ******************************************************************************************/
using ETModel;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class MonoEventComponentAwakeSystem : AwakeSystem<MonoEventComponent>
    {

        public override void Awake(MonoEventComponent self)
        {
            self.Awake();
        }
    }

    public class MonoEventComponent : Component
    {
        public static MonoEventComponent Instance;

        public void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// 添加按钮Click事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        public void AddButtonClick(Button obj, Action action)
        {
            AddButtonClick(obj, true, action);
        }

        /// <summary>
        /// 添加按钮Click事件
        /// </summary>
        /// <param name="obj">Button组件</param>
        /// <param name="IsRemoveAllListeners">是否清除以前注册的事件</param>
        /// <param name="action">回调函数</param>
        public void AddButtonClick(Button obj, bool IsRemoveAllListeners, Action action)
        {
            if (IsRemoveAllListeners) obj.onClick.RemoveAllListeners();

            obj.onClick.Add(action);
        }

        /// <summary>
        /// 添加触发事件
        /// </summary>
        /// <param name="obj">绑定对象</param>
        /// <param name="eventTriggerType">事件类型</param>
        /// <param name="action">回调函数</param>
        /// <returns></returns>
        public EventTrigger AddEventTrigger(GameObject obj, EventTriggerType eventTriggerType, UnityAction<BaseEventData> action)
        {
            return AddEventTrigger(obj, eventTriggerType, true, action);
        }
        
        /// <summary>
        /// 添加触发事件
        /// </summary>
        /// <param name="obj">绑定对象</param>
        /// <param name="eventTriggerType">事件类型</param>
        /// <param name="IsRemoveAllListeners">添加事件是否清空以前添加的事件</param>
        /// <param name="action">回调函数</param>
        /// <returns></returns>
        public EventTrigger AddEventTrigger(GameObject obj, EventTriggerType eventTriggerType, bool IsRemoveAllListeners, UnityAction<BaseEventData> action)
        {
            return AddEventTrigger(obj, eventTriggerType, IsRemoveAllListeners, false, action);
        }

        /// <summary>
        /// 添加触发事件
        /// </summary>
        /// <param name="obj">绑定对象</param>
        /// <param name="eventTriggerType">事件类型</param>
        /// <param name="IsRemoveAllListeners">添加事件是否清空以前添加的事件</param>
        /// <param name="IsClearTriggers">是否清除对象上所有触发事件</param>
        /// <param name="action">回调函数</param>
        /// <returns></returns>
        public EventTrigger AddEventTrigger(GameObject obj, EventTriggerType eventTriggerType, bool IsRemoveAllListeners, bool IsClearTriggers, UnityAction<BaseEventData> action)
        {
            if (obj == null) return null;

            EventTrigger eventTrigger = obj.GetComponent<EventTrigger>() ?? obj.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventTriggerType };

            //UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(action);

            entry.callback.AddListener(action);

            if (IsClearTriggers) eventTrigger.triggers.Clear();

            eventTrigger.triggers.Add(entry);

            return eventTrigger;
        }
    }
}