/******************************************************************************************
*         【模块】{ 计时器模块管理器 }                                                                                                                      
*         【功能】{ 游戏计时器生命周期管理}                                                                                                                   
*         【修改日期】{ 2019年3月11日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;

namespace ETModel
{
    public class ETimer
    {
        static List<ETimer> timers = new List<ETimer>();
        private Action<float> UpdateEvent;
        private Action EndEvent;

        /// <summary>
        /// 用户设定的定时时长
        /// </summary>
        private float _time = -1;

        /// <summary>
        /// 是否执行循环
        /// </summary>
        private bool _loop;

        /// <summary>
        /// 是否忽略Timescale
        /// </summary>
        private bool _ignorTimescale;

        /// <summary>
        /// 用户指定的定时器标志，便于手动清除，暂停，恢复
        /// </summary>
        private string _flag;

        /// <summary>
        /// 拿驱动器的引用只是为了初始化驱动器
        /// </summary>
        public static ETimerComponent driver = null;

        /// <summary>
        /// 获取当前时间
        /// </summary>
        private float CurrentTime { get { return _ignorTimescale ? UnityEngine.Time.realtimeSinceStartup : UnityEngine.Time.time; } }

        /// <summary>
        /// 缓存时间
        /// </summary>
        private float cachedTime;

        /// <summary>
        /// 已经流逝的时间
        /// </summary>
        float timePassed;

        /// <summary>
        /// 计时器是否结束
        /// </summary>
        private bool _isFinish = false;

        /// <summary>
        /// 计时器是否暂停
        /// </summary>
        private bool _isPause = false;

        private static bool showLog = true;
        /// <summary>
        /// 确认是否输出Debug信息
        /// </summary>
        public static bool ShowLog { set { showLog = value; } }

        /// <summary>
        /// 当前定时器设定的时间
        /// </summary>
        public float Duration { get { return _time; } }

        /// <summary>
        /// 暂停计时器
        /// </summary>
        public bool IsPause
        {
            get { return _isPause; }
            set
            {
                if (value)
                {
                    Pause();
                }
                else
                {
                    Resum();
                }
            }
        }

        /// <summary>
        /// 构造定时器
        /// </summary>
        /// <param name="time">定时时长</param>
        /// <param name="flag">定时器标识符</param>
        /// <param name="loop">是否循环</param>
        /// <param name="ignorTimescale">是否忽略TimeScale</param>
        private ETimer(float time, string flag, bool loop = false, bool ignorTimescale = true)
        {
            if (null == driver) driver = Game.Scene.GetComponent<ETimerComponent>();//初始化Time驱动
            _time = time;
            _loop = loop;
            _ignorTimescale = ignorTimescale;
            cachedTime = CurrentTime;
            if (timers.Exists((v) => { return v._flag == flag; }))
            {
                if (showLog) Debug.LogWarningFormat("【TimerTrigger（容错）】:存在相同的标识符【{0}】！", flag);
            }
            _flag = string.IsNullOrEmpty(flag) ? GetHashCode().ToString() : flag;//设置辨识标志符
        }

        /// <summary>
        /// 暂停计时
        /// </summary>
        private void Pause()
        {
            if (_isFinish)
            {
                if (showLog) Debug.LogWarning("【TimerTrigger（容错）】:计时已经结束！");
            }
            else
            {
                _isPause = true;
            }
        }

        /// <summary>
        /// 继续计时
        /// </summary>
        private void Resum()
        {
            if (_isFinish)
            {
                if (showLog) Debug.LogWarning("【TimerTrigger（容错）】:计时已经结束！");
            }
            else
            {
                if (_isPause)
                {
                    cachedTime = CurrentTime - timePassed;
                    _isPause = false;
                }
                else
                {
                    if (showLog) Debug.LogWarning("【TimerTrigger（容错）】:计时并未处于暂停状态！");
                }
            }
        }

        /// <summary>
        /// 刷新定时器
        /// </summary>
        private void Update()
        {
            if (!_isFinish && !_isPause)
            {
                timePassed = CurrentTime - cachedTime;
                if (null != UpdateEvent) UpdateEvent.Invoke(Mathf.Clamp01(timePassed / _time));
                if (timePassed >= _time)
                {
                    if (null != EndEvent) EndEvent.Invoke();
                    if (_loop)
                    {
                        cachedTime = CurrentTime;
                    }
                    else
                    {
                        Stop();
                    }
                }
            }
        }

        /// <summary>
        /// 回收定时器
        /// </summary>
        private void Stop()
        {
            if (timers.Contains(this))
            {
                timers.Remove(this);
            }
            _time = -1;
            _isFinish = true;
            _isPause = false;
            UpdateEvent = null;
        }

        #region -------------- Static Function Extend---------------------

        /// <summary>
        /// 添加定时触发器
        /// </summary>
        /// <param name="time">定时时长</param>
        /// <param name="flag">定时器标识符</param>
        /// <param name="loop">是否循环</param>
        /// <param name="ignorTimescale">是否忽略TimeScale</param>
        /// <returns></returns>
        public static ETimer AddTimer(float time, string flag = "", bool loop = false, bool ignorTimescale = true)
        {
            ETimer timer = new ETimer(time, flag, loop, ignorTimescale);
            timers.Add(timer);
            return timer;
        }

        /// <summary>
        /// 刷新所有计时器
        /// </summary>
        public static void UpdateAllTimer()
        {
            for (int i = 0; i < timers.Count; i++)
            {
                if (null != timers[i])
                {
                    timers[i].Update();
                }
            }
        }

        /// <summary>
        /// 确认是否存在指定的定时器
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static bool Exist(string flag)
        {
            return timers.Exists((v) => { return v._flag == flag; });
        }

        /// <summary>
        /// 确认是否存在指定的定时器
        /// </summary>
        /// <param name="timer">定时器指定</param>
        /// <returns></returns>
        public static bool Exist(ETimer timer)
        {
            return timers.Contains(timer);
        }

        /// <summary>
        /// 获得指定的定时器
        /// </summary>
        /// <param name="flag">标志指定</param>
        /// <returns></returns>
        public static ETimer GetTimer(string flag)
        {
            return timers.Find((v) => { return v._flag == flag; });
        }

        /// <summary>
        /// 暂停用户指定的计时触发器
        /// </summary>
        /// <param name="flag">指定的标识符</param>
        public static void Pause(string flag)
        {
            ETimer timer = GetTimer(flag);
            if (null != timer)
            {
                timer.Pause();
            }
            else
            {
                if (showLog) Debug.Log("【TimerTrigger（容错）】:定时器已完成触发或无此定时器！---Flag【" + flag + "】。");
            }
        }

        /// <summary>
        /// 恢复用户指定的计时触发器
        /// </summary>
        /// <param name="flag"></param>
        public static void Resum(string flag)
        {
            ETimer timer = GetTimer(flag);
            if (null != timer)
            {
                timer.Resum();
            }
            else
            {
                if (showLog) Debug.Log("【TimerTrigger（容错）】:定时器已完成触发或无此定时器！---Flag【" + flag + "】。");
            }
        }

        /// <summary>
        /// 恢复用户指定的计时触发器
        /// </summary>
        /// <param name="timer"></param>
        public static void Resum(ETimer timer)
        {
            if (Exist(timer))
            {
                timer.Resum();
            }
            else
            {
                if (showLog) Debug.Log("【TimerTrigger（容错）】:此定时器已完成触发或无此定时器！");
            }
        }

        /// <summary>
        /// 删除用户指定的计时触发器
        /// </summary>
        /// <param name="flag">指定的标识符</param>
        public static void DelTimer(string flag)
        {
            ETimer timer = GetTimer(flag);
            if (null != timer)
            {
                timer.Stop();
            }
            else
            {
                if (showLog) Debug.Log("【TimerTrigger（容错）】:此定时器已完成触发或无此定时器！");
            }
        }

        /// <summary>
        /// 删除用户指定的计时触发器
        /// </summary>
        /// <param name="timer"></param>
        public static void DelTimer(ETimer timer)
        {
            if (Exist(timer))
            {
                timer.Stop();
            }
            else
            {
                if (showLog) Debug.Log("【TimerTrigger（容错）】:此定时器已完成触发或无此定时器！");
            }
        }

        /// <summary>
        /// 删除用户指定的计时触发器
        /// </summary>
        /// <param name="completedEvent">指定的完成事件(直接赋值匿名函数无效)</param>
        public static void DelTimer(Action completedEvent)
        {
            ETimer timer = timers.Find((v) => { return v.EndEvent == completedEvent; });
            if (null != timer)
            {
                timer.Stop();
            }
            else
            {
                if (showLog) Debug.Log("【TimerTrigger（容错）】:定时器已完成触发或无此定时器！---方法名：【" + completedEvent.Method.Name + "】。");
            }
        }

        /// <summary>
        /// 删除用户指定的计时触发器
        /// </summary>
        /// <param name="updateEvent">指定的Update事件(直接赋值匿名函数无效)</param>
        public static void DelTimer(Action<float> updateEvent)
        {
            ETimer timer = timers.Find((v) => { return v.UpdateEvent == updateEvent; });
            if (null != timer)
            {
                timer.Stop();
            }
            else
            {
                if (showLog) Debug.Log("【TimerTrigger（容错）】:定时器已完成触发或无此定时器！---方法名：【" + updateEvent.Method.Name + "】。");
            }
        }

        /// <summary>
        /// 删除运行中所有计时触发器
        /// </summary>
        public static void RemoveAll()
        {
            timers.ForEach((v) => { v.Stop(); });
            timers.Clear();
        }

        public void AddEvent(Action completedEvent)
        {
            if (null == EndEvent)
            {
                EndEvent = completedEvent;
            }
            else
            {
                Delegate[] delegates = EndEvent.GetInvocationList();
                if (!Array.Exists(delegates, (v) => { return v == (Delegate)completedEvent; }))
                {
                    EndEvent += completedEvent;
                }
            }
        }

        public void AddEvent(Action<float> updateEvent)
        {
            if (null == UpdateEvent)
            {
                UpdateEvent = updateEvent;
            }
            else
            {
                Delegate[] delegates = UpdateEvent.GetInvocationList();
                if (!Array.Exists(delegates, (v) => { return v == (Delegate)updateEvent; }))
                {
                    UpdateEvent += updateEvent;
                }
            }
        }

        #endregion

        #region -----------------运行中的定时器参数修改-------------------

        /// <summary>
        /// 重新设置运行中的定时器的时间
        /// </summary>
        /// <param name="endTime">定时时长</param>
        public ETimer SetTime(float endTime)
        {
            if (_isFinish)
            {
                if (showLog) Debug.LogWarning("【TimerTrigger（容错）】:计时已经结束！");
            }
            else
            {
                if (endTime == _time)
                {
                    if (showLog) Debug.Log("【TimerTrigger（容错）】:时间已被设置，请勿重复操作！");
                }
                else
                {
                    if (endTime < 0)
                    {
                        if (showLog) Debug.Log("【TimerTrigger（容错）】:时间不支持负数，已自动取正！");
                        endTime = Mathf.Abs(endTime);
                    }
                    if (endTime < timePassed)//如果用户设置时间已错失
                    {
                        if (showLog) Debug.Log(string.Format("【TimerTrigger（容错）】:时间设置过短【passed:set=>{0}:{1}】,事件提前触发！", timePassed, endTime));
                    }
                    _time = endTime;
                }
            }
            return this;
        }

        /// <summary>
        /// 设置运行中的定时器的loop状态
        /// </summary>
        /// <param name="loop"></param>
        public ETimer Setloop(bool loop)
        {
            if (!_isFinish)
            {
                _loop = loop;
            }
            else
            {
                if (showLog) Debug.Log("【TimerTrigger（容错）】:定时器已失效,设置Loop Fail！");
            }
            return this;
        }

        /// <summary>
        /// 设置运行中的定时器的ignoreTimescale状态
        /// </summary>
        /// <param name="loop"></param>
        public ETimer SetIgnoreTimeScale(bool ignoreTimescale)
        {
            if (!_isFinish)
            {
                _ignorTimescale = ignoreTimescale;
            }
            else
            {
                if (showLog) Debug.Log("【TimerTrigger（容错）】:定时器已失效，设置IgnoreTimescale Fail！");
            }
            return this;
        }

        #endregion
    }

   

    public static class TimerExtend
    {
        /// <summary>
        /// 当计时器计数完成时执行的事件链
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="completedEvent"></param>
        /// <returns></returns>
        public static ETimer OnCompleted(this ETimer timer, Action completedEvent)
        {
            if (null == timer)
            {
                return null;
            }
            timer.AddEvent(completedEvent);
            return timer;
        }

        /// <summary>
        /// 当计数器计时进行中执行的事件链
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="updateEvent"></param>
        /// <returns></returns>
        public static ETimer OnUpdated(this ETimer timer, Action<float> updateEvent)
        {
            if (null == timer)
            {
                return null;
            }
            timer.AddEvent(updateEvent);
            return timer;
        }
    }

    [ObjectSystem]
    public class ETimerComponent_AwakeSystem : AwakeSystem<ETimerComponent>
    {
        public override void Awake(ETimerComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class ETimerComponent_UpdateSystem : UpdateSystem<ETimerComponent>
    {
        public override void Update(ETimerComponent self)
        {
            self.Update();
        }
    }

    public class ETimerComponent : Component
    {
        public static ETimerComponent Instance;
        
        public void Awake()
        {
            Instance = this;
        }

        public void Update()
        {
            ETimer.UpdateAllTimer();
        }

        public ETimer OnCompleted(ETimer timer, Action completedEvent)
        {
            return TimerExtend.OnCompleted(timer, completedEvent);
        }

        public ETimer OnUpdated(ETimer timer, Action<float> updateEvent)
        {
            return TimerExtend.OnUpdated(timer, updateEvent);
        }
        
    }
}
