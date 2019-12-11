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

    public class UIRankListsItem : Component
    {
        private ReferenceCollector ReferenceCollector;

        private Image PortraitImg;
        private Image RankLevelImg;
        private Text RankLevelTxt;
        private Text PlayerNameTxt;
        private Text ProfitTxt;
        private Text TipsText;

        private Image BgImage;
        private GameObject IsMeObj;

        private bool _IsInit;

        public void InitData()
        {
            if (this.GameObject != null)
                this.GameObject.SetActive(true);

            if (_IsInit)
                return;

            var res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            res.LoadBundle(UIType.RankListsItem.StringToAB());
            var gameObject = res.GetAsset(UIType.RankListsItem.StringToAB(), UIType.RankListsItem);
            this.GameObject = (GameObject)UnityEngine.Object.Instantiate(gameObject, this.Parent.Parent.GameObject.transform);

            ReferenceCollector = this.GameObject.Get<ReferenceCollector>();
            BgImage = ReferenceCollector.Get<GameObject>("RankListsItem").Get<Image>();
            PortraitImg = ReferenceCollector.Get<GameObject>("PortraitImg").Get<Image>();
            RankLevelImg = ReferenceCollector.Get<GameObject>("RankLevelImg").Get<Image>();
            RankLevelTxt = ReferenceCollector.Get<GameObject>("RankLevelTxt").Get<Text>();
            PlayerNameTxt = ReferenceCollector.Get<GameObject>("PlayerNameTxt").Get<Text>();
            ProfitTxt = ReferenceCollector.Get<GameObject>("ProfitTxt").Get<Text>();
            IsMeObj = ReferenceCollector.Get<GameObject>("IsMeObj");
            TipsText= ReferenceCollector.Get<GameObject>("TipsText").Get<Text>();
            _IsInit = true;
        }

        /// <summary>
        /// 设置数据   type 1-金币，2-收益
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="level"></param>
        /// <param name="info"></param>
        /// <param name="type"></param>
        public void SetRankItemData(Transform parent, int level, UserInfo info, int type)
        {
            this.GameObject.transform.SetParent(parent);
            this.GameObject.transform.localScale = Vector3.one;
            if (level < 4)
            {
                RankLevelImg.gameObject.SetActive(true);
                RankLevelImg.sprite = SpriteHelper.GetSprite("rankatlas", $"rank2_{level}");
                RankLevelImg.SetNativeSize();
            }
            else
            {
                RankLevelImg.gameObject.SetActive(false);
            }
            RankLevelTxt.text = level > 3 ? level.ToString() : "";

            PlayerNameTxt.text = info.Nickname;
            PortraitImg.sprite = SpriteHelper.GetPlayerHeadSpriteName(info.HeadId);
            if (type == 1)
            {
                TipsText.text = "拥有金币";
                ProfitTxt.text = GameHelper.ConvertCoinToString(info.Gold);
            }
            else
            {
                TipsText.text = "今日赢金";
                ProfitTxt.text = GameHelper.ConvertCoinToString(info.Score);
            }
            IsMeObj.SetActive(info.PlayerId == UserDataHelper.UserInfo.PlayerId);
            BgImage.enabled = info.PlayerId != UserDataHelper.UserInfo.PlayerId;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.GameObject.SetActive(false);
        }
    }
}