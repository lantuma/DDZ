using System;

namespace ETHotfix
{
    public class EventCenterObject
    {
        public event Action MsgCallback;

        public event Action<object> MsgP1Callback;

        public event Action<object, object> MsgP2Callback;


        public void SendMsg()
        {
            MsgCallback?.Invoke();
        }

        public void SendMsg<T>(T obj)
        {
            MsgP1Callback?.Invoke(obj);
        }

        public void SendMsg<T, K>(T obj, K obj1)
        {
            MsgP2Callback?.Invoke(obj, obj1);
        }
    }
}
