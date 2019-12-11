/******************************************************************************************
*         【模块】{ 事件控制中心 }                                                                                                                      
*         【应用场景】每个事件消息，有一个接收控制对象，对象里有3种事件
*         【修改日期】{ 2019年8月6日 }                                                                                                                        
*         【贡献者】{ 官方 ｝                                                                                                               
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class EventCenterControllerSystem : AwakeSystem<EventCenterController>
    {
        public override void Awake(EventCenterController self)
        {
            self.Awake();
        }
    }

    public class EventCenterController:Component
    {
        private Dictionary<string, EventCenterObject> msgCallbackList;

        public static EventCenterController instance;

        public void Awake()
        {
            msgCallbackList = new Dictionary<string, EventCenterObject>();

            instance = this;
        }
        
        public void AddMsg(string evtName, Action fun)
        {
            if (msgCallbackList.ContainsKey(evtName))
            {
                msgCallbackList[evtName].MsgCallback += fun;
            }
            else
            {
                msgCallbackList[evtName] = new EventCenterObject();

                msgCallbackList[evtName].MsgCallback += fun;
            }
        }

        public void AddMsg(string evtName, Action<object> fun)
        {
            if (msgCallbackList.ContainsKey(evtName))
            {
                msgCallbackList[evtName].MsgP1Callback += fun;
            }
            else
            {
                msgCallbackList[evtName] = new EventCenterObject();

                msgCallbackList[evtName].MsgP1Callback += fun;
            }
        }

        public void AddMsg(string evtName, Action<object,object> fun)
        {
            if (msgCallbackList.ContainsKey(evtName))
            {
                msgCallbackList[evtName].MsgP2Callback += fun;
            }
            else
            {
                msgCallbackList[evtName] = new EventCenterObject();

                msgCallbackList[evtName].MsgP2Callback += fun;
            }
        }

        public void RemoveMsg(string evtName, Action fun)
        {
            if (msgCallbackList.ContainsKey(evtName))
            {
                msgCallbackList[evtName].MsgCallback -= fun;
            }
        }

        public void RemoveMsg(string evtName, Action<object> fun)
        {
            if (msgCallbackList.ContainsKey(evtName))
            {
                msgCallbackList[evtName].MsgP1Callback -= fun;
            }
        }

        public void RemoveMsg(string evtName, Action<object,object> fun)
        {
            if (msgCallbackList.ContainsKey(evtName))
            {
                msgCallbackList[evtName].MsgP2Callback -= fun;
            }
        }

        public void SendMsg(string evtName)
        {
            if (msgCallbackList.ContainsKey(evtName))
            {
                msgCallbackList[evtName].SendMsg();
            }
        }

        public void SendMsg(string evtName,object objData)
        {
            if (msgCallbackList.ContainsKey(evtName))
            {
                msgCallbackList[evtName].SendMsg(objData);
            }
        }

        public void SendMsg(string evtName, object objData,object objData1)
        {
            if (msgCallbackList.ContainsKey(evtName))
            {
                msgCallbackList[evtName].SendMsg(objData,objData1);
            }
        }
    }
}
