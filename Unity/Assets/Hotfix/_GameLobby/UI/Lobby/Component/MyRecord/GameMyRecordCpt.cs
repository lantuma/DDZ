using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameMyRecordCptAwakeSystem : AwakeSystem<GameMyRecordCpt>
    {
        public override void Awake(GameMyRecordCpt self)
        {
            self.Awake();
        }
    }

    public class GameMyRecordCpt : Entity
    {
        private GameObject panelGo;
        private ReferenceCollector ReferenceCollector;
        private GameObject Root;
        private TweenEffectComponent _TEC;
        private Button MyRecordToggle;
        private Button ChenJuToggle;
        private GameObject _Content;
        private List<UIRecordItem> _RecordLists;
        private GameObject _RecordViewTipsText;

        public void Awake()
        {
            var res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            res.LoadBundle(UIType.MyRecordPanel.StringToAB());
            var gameObject = res.GetAsset(UIType.MyRecordPanel.StringToAB(), UIType.MyRecordPanel);
            this.panelGo = (GameObject)UnityEngine.Object.Instantiate(gameObject);
            UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.MyRecordPanel, this.panelGo, false);
            Game.Scene.GetComponent<UIComponent>().Add(ui);
            this.ReferenceCollector = this.panelGo.GetComponent<ReferenceCollector>();
            this.Root = this.ReferenceCollector.Get<GameObject>("Root");
            _TEC = this.AddComponent<TweenEffectComponent>();
            _TEC.Add(TweenAnimationIdType.MyRecordPanel, this.Root.transform);
            this.ReferenceCollector.Get<GameObject>("CloseBtn").GetComponent<Button>().onClick.AddListener(Close);
            this.ReferenceCollector.Get<GameObject>("mask").Get<Button>().onClick.AddListener(Close);
            this.MyRecordToggle = this.ReferenceCollector.Get<GameObject>("MyRecordToggle").GetComponent<Button>();
            this.ChenJuToggle = this.ReferenceCollector.Get<GameObject>("ChenJuToggle").GetComponent<Button>();
            this.MyRecordToggle.onClick.AddListener(() =>
            {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
            });

            this.ChenJuToggle.onClick.AddListener(() => {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.ChengJuNotOpenTip);
            });

            _Content = ReferenceCollector.Get<GameObject>("Content");
            _RecordLists = new List<UIRecordItem>();
            _RecordViewTipsText = ReferenceCollector.Get<GameObject>("RecordViewTipsText");
        }
        
        public async void Start()
        {
            RecyleRecordItem();
            //请求战绩数据
            var resp = (G2C_MyRecord_Res)await SessionComponent.Instance.Session.Call
                     (
                new C2G_MyRecord_Req()
                {
                    UserId = GamePrefs.GetUserId(),
                });
            if (resp.Error == 0)
            {
                _RecordViewTipsText.SetActive(resp.Recordlist.Count == 0);

                for (int i = 0; i < resp.Recordlist.Count; i++)
                {
                    CreateRecordListsItem(resp.Recordlist[i], i);
                }
            }
        }

        /// <summary>
        /// 回收战绩Item对象
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
        /// 创建战绩Item
        /// </summary>
        /// <param name="info"></param>
        private void CreateRecordListsItem(Record info, int index)
        {
            var view = ComponentFactory.CreateWithParent<UIRecordItem>(this);
            view.InitData();
            view.SetRecordItemData(_Content.transform, info, index);
            _RecordLists.Add(view);
        }

        public void Open()
        {
            this.panelGo.SetActive(true);

            SoundHelper.FadeInPlaySound(DataCenterComponent.Instance.soundInfo.paihangbang_enter);

            this._TEC.Play(TweenAnimationIdType.MyRecordPanel);

            this.Start();
        }

        private async void Close()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
            this._TEC.PlayBackwards(TweenAnimationIdType.MyRecordPanel);

            SoundHelper.FadeOutPlaySound();

            await Task.Delay(300);

            this.panelGo.SetActive(false);
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();

            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(UIType.MyRecordPanel.StringToAB());
        }
    }
}