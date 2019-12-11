using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class PingAwakeSystem : AwakeSystem<PingComponent, long, Session, Action>
    {
        public override void Awake(PingComponent self, long a, Session s, Action b)
        {
            self.Awake(a, s, b);
        }
    }

    public class PingComponent:Component
    {
        #region 成员变量

        /// <summary>
        /// 发送时间
        /// </summary>
        private Session _session;


        /// <summary>
        /// 发送时间
        /// </summary>
        private long _sendTimer;

        /// <summary>
        /// 接收时间
        /// </summary>
        private long _receiveTimer;

        /// <summary>
        /// 延时
        /// </summary>
        public long Ping = 0;

        private bool IsRun = false;

        /// <summary>
        /// 心跳协议包
        /// </summary>
        private readonly PingRequest _request = new PingRequest();

        public Action PingBackCall;

        #endregion

        #region Awake

        public async void Awake(long waitTime, Session _sessionWrap, Action action)
        {
            this.PingBackCall = action;

            var timerComponent = ETModel.Game.Scene.GetComponent<TimerComponent>();

            this._session = _sessionWrap;

            IsRun = true;

            while (IsRun)
            {
                try
                {
                    if (this._session == null)
                    {
                        Game.Scene.RemoveComponent<PingComponent>();

                        this.PingBackCall?.Invoke();

                        Debug.Log("超时断线了");

                        Game.EventSystem.Run(EventIdType.HeartBeatTimeOut);

                        break;
                    }

                    _sendTimer = TimeHelper.ClientNowSeconds();

                    await _session.Call(_request);

                    _receiveTimer = TimeHelper.ClientNowSeconds();

                    // 计算延时

                    Ping = ((_receiveTimer - _sendTimer) / 2) < 0 ? 0 : (_receiveTimer - _sendTimer) / 2;
                }
                catch (Exception)
                {
                    this._session = null;

                    Game.Scene.RemoveComponent<PingComponent>();

                    this.PingBackCall?.Invoke();

                    Debug.Log("异常断线了");

                    Game.EventSystem.Run(EventIdType.HeartBeatTimeOut);
                }

                await timerComponent.WaitAsync(waitTime);
            }
        }

        #endregion

        public override void Dispose()
        {
            if (this.IsDisposed) return;

            base.Dispose();

            IsRun = false;

            _session?.Dispose();

            _session = null;
        }
    }
}
