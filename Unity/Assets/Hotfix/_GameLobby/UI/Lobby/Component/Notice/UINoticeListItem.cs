using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ETModel;

namespace ETHotfix
{
    public class UINoticeListItem : Component
    {
        private ReferenceCollector ReferenceCollector;

        private Text TimeText;
        private Text TitleText;

        private bool _IsInit;

        private AnnounceInfo _AnnounceInfo;
        private MailInfo _MailInfo;

        private GameNoticeCpt _GameNoticeCpt;

        public void InitData()
        {
            if (this.GameObject != null)
                this.GameObject.SetActive(true);

            if (_IsInit)
                return;

            var res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            res.LoadBundle(UIType.NoticeListItem.StringToAB());
            var gameObject = res.GetAsset(UIType.NoticeListItem.StringToAB(), UIType.NoticeListItem);
            this.GameObject = (GameObject)UnityEngine.Object.Instantiate(gameObject, this.Parent.Parent.GameObject.transform);

            ReferenceCollector = this.GameObject.Get<ReferenceCollector>();
            TitleText = ReferenceCollector.Get<GameObject>("TitleText").Get<Text>();
            TimeText = ReferenceCollector.Get<GameObject>("TimeText").Get<Text>();
            _IsInit = true;

            this.GameObject.Get<Button>().onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
            _GameNoticeCpt.OpenMsgDialog(_AnnounceInfo, _MailInfo);
        }

        /// <summary>
        /// 设置界面数据
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="data"></param>
        public void SetRecordItemData(Transform parent, AnnounceInfo data, MailInfo mail, GameNoticeCpt cpt)
        {
            this.GameObject.transform.SetParent(parent);
            this.GameObject.transform.localPosition = Vector3.zero;
            this.GameObject.transform.localScale = Vector3.one;
            _MailInfo = mail;
            _AnnounceInfo = data;
            _GameNoticeCpt = cpt;
            TitleText.text = data == null ? mail.Title : data.Title;
            TimeText.text = data == null ? mail.Timestamp : data.Timestamp;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.GameObject.SetActive(false);
        }
    }
}