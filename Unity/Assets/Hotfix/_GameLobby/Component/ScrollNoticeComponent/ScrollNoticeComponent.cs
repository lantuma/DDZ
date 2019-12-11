
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class ScrollNoticeComponentAwakeSystem : AwakeSystem<ScrollNoticeComponent>
    {
        public override void Awake(ScrollNoticeComponent self)
        {
            self.Awake();
        }
    }

    public class ScrollNoticeComponent : Component
    {
        private ReferenceCollector _rf;

        private GameObject Root;

        private GameObject panel;

        private Text NoticeText;

        private Queue<string> NoticeQueue;

        private TaskCompletionSource<string> Tcs;

        private NoticeData data = null;

        public static ScrollNoticeComponent Instance;

        public void Awake()
        {
            Instance = this;

            this.panel = this.GetParent<UI>().GameObject;

            _rf = this.panel.GetComponent<ReferenceCollector>();

            this.Root = _rf.Get<GameObject>("Root");

            this.NoticeText = _rf.Get<GameObject>("NoticeText").GetComponent<Text>();

            this.panel.SetActive(false);

            NoticeQueue = new Queue<string>();
        }

        /// <summary>
        /// 消息入队
        /// </summary>
        /// <param name="msg"></param>
        public void Add(string msg)
        {
            this.NoticeQueue.Enqueue(msg);

            if (this.Tcs == null)
            {
                return;
            }

            var t = this.Tcs;

            this.Tcs = null;

            t.SetResult(this.NoticeQueue.Dequeue());
        }

        /// <summary>
        /// 获取一条公告
        /// </summary>
        /// <returns></returns>
        private Task<string> GetAsync()
        {
            if (this.NoticeQueue.Count > 0)
            {
                return Task.FromResult(this.NoticeQueue.Dequeue());
            }

            this.Tcs = new TaskCompletionSource<string>();

            return this.Tcs.Task;
        }

        /// <summary>
        /// 依次处理消息
        /// </summary>
        public async void Process()
        {
            SemaphoreSlim mutex = new SemaphoreSlim(1);

            long instanceId = this.InstanceId;

            while (true)
            {
                if (this.InstanceId != instanceId)
                {
                    return;
                }

                await mutex.WaitAsync().ConfigureAwait(false);

                try
                {
                    string msg = await this.GetAsync();

                    if (msg == null)
                    {
                        return;
                    }

                    await HandleNotice(msg);
                }
                catch (System.Exception e)
                {

                    Log.Error(e);
                }
                finally
                {
                    mutex.Release();
                }
            }
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private async Task HandleNotice(string msg)
        {
            this.panel.SetActive(true);

            this.NoticeText.text = msg;

            this.NoticeText.transform.localPosition = new Vector2(0, 0f);

            Vector2 pos = new Vector2(-this.NoticeText.preferredWidth-610, 0f);

            float time = Mathf.Max(8, (int)(this.NoticeText.preferredWidth / 60));
            
            this.NoticeText.transform.DOLocalMove(pos, time).SetEase(Ease.Linear);

            await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync((long)time*1000);
            
            if (this.NoticeQueue.Count == 0)
            {
                this.Add(DataCenterComponent.Instance.MarqueeInfo.SubGameDefault);
            }
            else
            {
                this.panel.SetActive(false);
            }
        }
        
        /// <summary>
        /// 打开跑马灯
        /// </summary>
        /// <param name="arg"></param>
        public void OnOpen(object arg = null)
        {
            if (arg != null)
            {
                data = null;

                data = arg as NoticeData;
                //设置公告显示位置
                this.panel.transform.localPosition = data.Pos;

                this.Add(data.mes);

                this.Process();
            }
        }

        /// <summary>
        /// 重置参数
        /// </summary>
        public void Reset()
        {
            this.NoticeQueue.Clear();

            var t = this.Tcs;

            this.Tcs = null;

            t?.SetResult(null);
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
