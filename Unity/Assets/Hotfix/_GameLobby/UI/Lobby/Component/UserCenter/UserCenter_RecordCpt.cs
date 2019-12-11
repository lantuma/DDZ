using ETModel;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UserCenter_RecordCptAwakeSystem : AwakeSystem<UserCenter_RecordCpt, GameObject, GameObject,ButtonGroup>
    {
        public override void Awake(UserCenter_RecordCpt self, GameObject button, GameObject panel,ButtonGroup group)
        {
            self.Awake(button, panel, group);
        }
    }

    [ObjectSystem]
    public class UserCenter_RecordCptStartSystem : StartSystem<UserCenter_RecordCpt>
    {
        public override void Start(UserCenter_RecordCpt self)
        {
            self.Start();
        }
    }


    public class UserCenter_RecordCpt : SimpleToggle
    {
        private ReferenceCollector _Ref;
        private GameObject _RecordView;
        private GameObject _AchieveView;
        private GameObject _Content;
        private List<UIRecordItem> _RecordLists;
        private GameObject _RecordViewTipsText;
        private GameObject _ChengJuTableToggle;

        public override void Init()
        {
            base.Init();

            _Ref = this.panelGo.GetComponent<ReferenceCollector>();
            _RecordView = _Ref.Get<GameObject>("RecordView");
            _AchieveView = _Ref.Get<GameObject>("AchieveView");
            _Content = _Ref.Get<GameObject>("Content");
            _RecordLists = new List<UIRecordItem>();
            _RecordViewTipsText = _Ref.Get<GameObject>("RecordViewTipsText");
            _Ref.Get<GameObject>("ChengJuTableToggle").Get<Button>().onClick.AddListener(OnChengJuTableToggle);
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
                    CreateRecordListsItem(resp.Recordlist[i],i);
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
        private void CreateRecordListsItem(Record info,int index)
        {
            var view = ComponentFactory.CreateWithParent<UIRecordItem>(this);
            view.InitData();
            view.SetRecordItemData(_Content.transform, info, index);
            _RecordLists.Add(view);
        }

        public override void ChangeToggle(bool state)
        {
            base.ChangeToggle(state);
        }

        #region ------Button

        private void OnChengJuTableToggle()
        {
            Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.ChengJuNotOpenTip);
        }

        #endregion

        public override void Dispose()
        {
            RecyleRecordItem();
        }
    }
}