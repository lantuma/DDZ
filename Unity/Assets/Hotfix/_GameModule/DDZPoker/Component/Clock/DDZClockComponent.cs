/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 闹钟组件 }                                                                                                                   
*         【修改日期】{ 2019年5月30日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Threading;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class DDZClockComponentAwakeSystem : AwakeSystem<DDZClockComponent, GameObject>
    {
        public override void Awake(DDZClockComponent self, GameObject a)
        {
            self.Awake(a);
        }
    }

    public class DDZClockComponent : Component
    {
        public Text clockText { get; private set; }

        private GameObject clockObj;

        private int currentTime = 0;

        private TimerComponent timer = ETModel.Game.Scene.GetComponent<TimerComponent>();

        private SoundInfo soundInfo = DataCenterComponent.Instance.soundInfo;

        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        private CancellationToken cancelToken;

        private GameObject ClockAni;

        private GameObject bg;


        public DDZClockComponent Awake(GameObject clock)
        {
            this.clockText = clock.GetComponent<Text>();

            this.clockObj = clockText.transform.parent.gameObject;

            this.ClockAni = this.clockObj.transform.Find("ClockAni").gameObject;

            this.bg = this.clockObj.transform.Find("bg").gameObject;

            this.cancelToken = this.tokenSource.Token;

            return this;
        }

        /// <summary>
        /// 开始倒计时
        /// </summary>
        /// <param name="time"></param>
        public async void Play(int time,Action endAction=null)
        {
            this.Stop(time);

            this.currentTime = time;

            this.bg.SetActive(true);

            this.ClockAni.SetActive(false);

            if (this.currentTime == 0)
            {
                this.clockText.text = " ";
                
                return;
            }

            while (this.currentTime > 0)
            {
                await this.timer.WaitAsync(1000, this.cancelToken);

                if (this.currentTime > 0)
                {
                    this.currentTime--;

                    string timeStr = this.currentTime.ToString();

                    if (this.currentTime >= 10)
                    {
                        timeStr = timeStr.Replace("0", "a").Replace("1", "b").Replace("2", "c").Replace("3", "d");
                    }
                    
                    this.clockText.text = timeStr;
                    
                    if (this.currentTime <= 5 && this.clockObj.activeSelf)
                    {
                        SoundComponent.Instance.PlayClip(soundInfo.DDZ_sound_lastTime);

                        this.bg.SetActive(false);

                        this.ClockAni.SetActive(true);
                    }

                    //if (this.currentTime == 0) endAction();
                }
            }
        }

        /// <summary>
        /// 取消倒计时
        /// </summary>
        public void Stop(int time)
        {
            this.tokenSource.Cancel();

            this.tokenSource = new CancellationTokenSource();

            this.cancelToken = this.tokenSource.Token;

            this.currentTime = time;

            string timeStr = this.currentTime.ToString();

            if (this.currentTime >= 10)
            {
                timeStr = timeStr.Replace("0", "a").Replace("1", "b").Replace("2", "c").Replace("3", "d");
            }

            this.clockText.text = timeStr;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            this.clockText.text = "";
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.Reset();
        }
    }
}
