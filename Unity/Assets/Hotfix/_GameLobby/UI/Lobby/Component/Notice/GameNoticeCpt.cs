using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameNoticeCptAwakeSystem : AwakeSystem<GameNoticeCpt>
    {
        public override void Awake(GameNoticeCpt self)
        {
            self.Awake();
        }
    }

    public class GameNoticeCpt : Entity
    {
        private GameObject panelGo;
        private ReferenceCollector ReferenceCollector;

        private GameObject Content;

        private GameObject Root;
        private TweenEffectComponent _TEC;
        private Transform GZBtn;

        private Transform FangPianBtn;
        private List<UINoticeListItem> _RecordLists;
        private GameObject NoMsgTips;
        private GameObject Title01;
        private GameObject Title02;

        private GameObject MsgDialog;
        private Text MsgTitleText;
        private Text MsgTxt;
        private GameObject MsgTitleImage01;
        private GameObject MsgTitleImage02;

        public void Awake()
        {
            _RecordLists = new List<UINoticeListItem>();

            var res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            res.LoadBundle(UIType.UINoticePanel.StringToAB());
            var gameObject = res.GetAsset(UIType.UINoticePanel.StringToAB(), UIType.UINoticePanel);
            this.panelGo = (GameObject)UnityEngine.Object.Instantiate(gameObject);
            UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.UINoticePanel, this.panelGo, false);
            Game.Scene.GetComponent<UIComponent>().Add(ui);
            this.ReferenceCollector = this.panelGo.GetComponent<ReferenceCollector>();
            this.Root = this.ReferenceCollector.Get<GameObject>("Root");

            this.GZBtn = this.ReferenceCollector.Get<GameObject>("GZBtn").transform;
            this.FangPianBtn = this.ReferenceCollector.Get<GameObject>("EmailBtn").transform;
            _TEC = this.AddComponent<TweenEffectComponent>();

            _TEC.Add(TweenAnimationIdType.NoticePanel, this.Root.transform);
            this.ReferenceCollector.Get<GameObject>("CloseButton").GetComponent<Button>().onClick.AddListener(Close);
            this.ReferenceCollector.Get<GameObject>("CloseBg").Get<Button>().onClick.AddListener(Close);

            this.Content = this.ReferenceCollector.Get<GameObject>("Content");
            NoMsgTips = ReferenceCollector.Get<GameObject>("NoMsgTips");
            Title01 = ReferenceCollector.Get<GameObject>("Title01");
            Title02 = ReferenceCollector.Get<GameObject>("Title02");

            MsgDialog = ReferenceCollector.Get<GameObject>("MsgDialog");
            MsgTitleText = ReferenceCollector.Get<GameObject>("MsgTitleText").Get<Text>();
            MsgTxt = ReferenceCollector.Get<GameObject>("MsgTxt").Get<Text>();
            MsgTitleImage01 = ReferenceCollector.Get<GameObject>("MsgTitleImage01");
            MsgTitleImage02 = ReferenceCollector.Get<GameObject>("MsgTitleImage02");

            ButtonHelper.RegisterButtonEvent(this.ReferenceCollector, "MsgCloseBtn", () =>
            {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
                MsgDialog.SetActive(false);
            });

            ButtonHelper.RegisterButtonEvent(this.ReferenceCollector, "Closebg", () =>
            {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
                MsgDialog.SetActive(false);
            });

            ButtonHelper.RegisterButtonEvent(this.ReferenceCollector, "GZBtn", () =>
            {
                this.GZBtn.Find("an").gameObject.SetActive(false);
                this.FangPianBtn.Find("an").gameObject.SetActive(true);
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
                GetNotice();

            });

            ButtonHelper.RegisterButtonEvent(this.ReferenceCollector, "EmailBtn", () =>
            {
                this.GZBtn.Find("an").gameObject.SetActive(true);
                this.FangPianBtn.Find("an").gameObject.SetActive(false);
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
                GetMail();

            });
        }

        public void Open()
        {
            this.panelGo.SetActive(true);
            this._TEC.Play(TweenAnimationIdType.NoticePanel);
            NoMsgTips.SetActive(false);
            MsgDialog.SetActive(false);
            GetNotice();
        }

        /// <summary>
        /// 打开公告详情界面
        /// </summary>
        /// <param name="info"></param>
        /// <param name="mail"></param>
        public void OpenMsgDialog(AnnounceInfo info, MailInfo mail)
        {
            MsgDialog.SetActive(true);
            MsgTitleImage01.SetActive(info != null);
            MsgTitleImage02.SetActive(mail != null);
            MsgTitleText.text = info == null ? mail.Title : info.Title;
            MsgTxt.text = info == null ? mail.Content : info.Content;
        }

        /// <summary>
        /// 点击获取公告
        /// </summary>
        private async void GetNotice()
        {
            Title01.SetActive(true);
            Title02.SetActive(false);
            RecyleRecordItem();
            //请求公告信息数据
            var resp = (G2C_Announcement_Res)await SessionComponent.Instance.Session.Call(
                    new C2G_Announcement_Req());

            if (resp.Error == 0)
            {
                NoMsgTips.SetActive(resp.Info.count == 0);
                for (int i = 0; i < resp.Info.Count; i++)
                {
                    CreateRecordListsItem(resp.Info[i], null,1);
                }
            }
            else
            {
                Game.PopupComponent.ShowTips(resp.Message);
                return;
            }
        }

        /// <summary>
        /// 点击获取邮件
        /// </summary>
        private async void GetMail()
        {
            Title01.SetActive(false);
            Title02.SetActive(true);
            RecyleRecordItem();
            //请求公告信息数据
            var resp = (G2C_MailReturn_Res)await SessionComponent.Instance.Session.Call(
                new C2G_MailAsk_Req()
                {
                    UserId = GamePrefs.GetUserId()
                });

            if (resp.Error == 0)
            {
                NoMsgTips.SetActive(resp.Info.count == 0);
                for (int i = 0; i < resp.Info.Count; i++)
                {
                    CreateRecordListsItem(null, resp.Info[i], 2);
                }
            }
            else
            {
                Game.PopupComponent.ShowTips(resp.Message);
                return;
            }
        }

        /// <summary>
        /// 回收Item对象
        /// </summary>
        private void RecyleRecordItem()
        {
            for (int i = 0; i < _RecordLists.Count; i++)
            {
                _RecordLists[i].Dispose();
            }
            if (_RecordLists != null)
            {
                _RecordLists.Clear();
            }
        }

        /// <summary>
        /// 创建Item  type=1 公告，type=2 邮件
        /// </summary>
        /// <param name="info"></param>
        private void CreateRecordListsItem(AnnounceInfo info, MailInfo mail,int type)
        {
            var view = ComponentFactory.CreateWithParent<UINoticeListItem>(this);
            view.InitData();
            if (type == 1)
            {
                view.SetRecordItemData(Content.transform, info,null,this);
            }
            else
            {
                view.SetRecordItemData(Content.transform, null,mail,this);
            }
            _RecordLists.Add(view);
        }

        private async void Close()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
            this._TEC.PlayBackwards(TweenAnimationIdType.NoticePanel);

            await Task.Delay(300);

            this.panelGo.SetActive(false);
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();

            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(UIType.UINoticePanel.StringToAB());
        }
    }
}