//using EGCore;
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
    [ObjectSystem]
    public class UIRankPanelCptAwakeSystem : AwakeSystem<UIRankPanelCpt>
    {
        public override void Awake(UIRankPanelCpt self)
        {
            self.Awake();
        }
    }

    public class UIRankPanelCpt : Component
    {
        private ReferenceCollector _ReferenceCollector;

        private Transform _rankContent;
        private GameObject RankListsItem;
        private Tweener windowAnim;

        private List<UIRankListsItem> RankingList;

        private Image richBtnImg;
        private Image profitBtnImg;

        private GameObject TableBtn;
        private GameObject _NoData;

        private Text MeNameText;
        private Text MeRankNumberText;
        private Text MeGoldText;
        private Image MeHeadImage;
        private Image RankLevelImg;
        private Text TipsText;

        public void Awake()
        {
            var res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            res.LoadBundle(UIType.UIRankPanel.StringToAB());
            var gameObject = res.GetAsset(UIType.UIRankPanel.StringToAB(), UIType.UIRankPanel);
            this.GameObject = (GameObject)UnityEngine.Object.Instantiate(gameObject);
            UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.UIRankPanel, this.GameObject, false);
            Game.Scene.GetComponent<UIComponent>().Add(ui);

            _ReferenceCollector = this.GameObject.Get<ReferenceCollector>();
            _rankContent = _ReferenceCollector.Get<Transform>("RankContent");
            _ReferenceCollector.Get<GameObject>("CloseButton").Get<Button>().onClick.AddListener(OnCloseButtonClicked);
            _ReferenceCollector.Get<GameObject>("CloseBg").Get<Button>().onClick.AddListener(OnCloseButtonClicked);
            var profitBtn = _ReferenceCollector.Get<GameObject>("ProfitBtn");
            profitBtn.Get<Button>().onClick.AddListener(OnClickProfitBtn);
            profitBtnImg = profitBtn.Get<Image>();

            var richBtn = _ReferenceCollector.Get<GameObject>("RichBtn");
            richBtn.Get<Button>().onClick.AddListener(OnClickRichBtn);
            richBtnImg = richBtn.Get<Image>();
            windowAnim = _ReferenceCollector.Get<GameObject>("Root").GetComponent<RectTransform>().DOScale(1, 0.4f).SetEase(Ease.OutBack).Pause().SetAutoKill(false);
            RankingList = new List<UIRankListsItem>();

            TableBtn = _ReferenceCollector.Get<GameObject>("TableBtn");
            _NoData = _ReferenceCollector.Get<GameObject>("NOData");

            MeNameText = _ReferenceCollector.Get<GameObject>("MeNameText").Get<Text>();
            MeRankNumberText = _ReferenceCollector.Get<GameObject>("MeRankNumberText").Get<Text>();
            MeGoldText = _ReferenceCollector.Get<GameObject>("MeGoldText").Get<Text>();
            MeHeadImage = _ReferenceCollector.Get<GameObject>("MeHeadImage").Get<Image>();
            RankLevelImg= _ReferenceCollector.Get<GameObject>("RankLevelImg").Get<Image>();
            TipsText = _ReferenceCollector.Get<GameObject>("TipsText").Get<Text>();
        }

        /// <summary>
        /// 设置我自己 的数据显示
        /// </summary>
        /// <param name="level"></param>
        /// <param name="data"></param>
        /// <param name="type"></param>
        private void SetMyUIData(int level, UserInfo data, int type)
        {
            MeNameText.text = data.Nickname;
            MeHeadImage.sprite = SpriteHelper.GetPlayerHeadSpriteName(data.HeadId);
            if (level < 4)
            {
                MeRankNumberText.text = "";
                RankLevelImg.gameObject.SetActive(true);
                RankLevelImg.sprite = SpriteHelper.GetSprite("rankatlas", $"rank2_{level}");
                RankLevelImg.SetNativeSize();
            }
            else
            {
                MeRankNumberText.gameObject.SetActive(true);
                MeRankNumberText.text = level.ToString();
            }
            if (type == 1)
            {
                TipsText.text = "拥有金币";
                MeGoldText.text = GameHelper.ConvertCoinToString(data.Gold);
            }
            else
            {
                TipsText.text = "今日赢金";
                MeGoldText.text = GameHelper.ConvertCoinToString(data.Score);
            }
        }

        /// <summary>
        /// 今日富豪按钮
        /// </summary>
        private void OnClickRichBtn()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
            TableBtn.SetActive(true);
            RankLevelImg.gameObject.SetActive(false);
            RecyleRankItem();
            bool isMe = false;
            if (_resp != null)
            {
                _NoData.SetActive(_resp.GoldList.Count == 0);
                for (int i = 0; i < _resp.GoldList.Count; i++)
                {
                    CreateRrankListsItem(_resp.GoldList[i], 1);
                    if (_resp.GoldList[i].PlayerId == UserDataHelper.UserInfo.PlayerId)
                    {
                        SetMyUIData(i + 1, _resp.GoldList[i], 1);
                        isMe = true;
                    }
                }
            }
            if (!isMe)
            {
                MeRankNumberText.text = "";
                RankLevelImg.gameObject.SetActive(true);
                RankLevelImg.sprite = SpriteHelper.GetSprite("rankatlas", $"rank2_weishangbang");
                RankLevelImg.SetNativeSize();
                MeNameText.text = UserDataHelper.UserInfo.Nickname;
                MeGoldText.text = GameHelper.ConvertCoinToString(UserDataHelper.UserInfo.Gold);
                TipsText.text = "拥有金币";
                MeHeadImage.sprite = SpriteHelper.GetPlayerHeadSpriteName(UserDataHelper.UserInfo.HeadId);
            }
        }

        /// <summary>
        /// 今日盈利按钮
        /// </summary>
        private async void OnClickProfitBtn()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
            TableBtn.SetActive(false);
            RankLevelImg.gameObject.SetActive(false);
            RecyleRankItem();
            bool isMe = false;
            await Task.Delay(1);
            if (_resp != null)
            {
                _NoData.SetActive(_resp.IncomeList.Count == 0);
                for (int i = 0; i < _resp.IncomeList.Count; i++)
                {
                    CreateRrankListsItem(_resp.IncomeList[i],2);
                    if (_resp.IncomeList[i].PlayerId == UserDataHelper.UserInfo.PlayerId)
                    {
                        SetMyUIData(i + 1, _resp.IncomeList[i], 2);
                        isMe = true;
                    }
                }
            }
            if (!isMe)
            {
                MeRankNumberText.text = "";
                RankLevelImg.gameObject.SetActive(true);
                RankLevelImg.sprite = SpriteHelper.GetSprite("rankatlas", $"rank2_weishangbang");
                RankLevelImg.SetNativeSize();
                MeNameText.text = UserDataHelper.UserInfo.Nickname;
                MeGoldText.text = "0";
                TipsText.text = "今日赢金";
                MeHeadImage.sprite = SpriteHelper.GetPlayerHeadSpriteName(UserDataHelper.UserInfo.HeadId);
            }
        }

        private G2C_GetRankList_Res _resp;

        public async void OnOpen()
        {
            _NoData.SetActive(false);
            this.GameObject.SetActive(true);
            _ReferenceCollector.Get<GameObject>("Root").Get<RectTransform>().localScale = Vector3.zero;
            windowAnim.Restart();
            var resp = (G2C_GetRankList_Res)await SessionComponent.Instance.Session.Call
                     (
                new C2G_GetRankList_Req()
                {
                });
            _resp = resp;
            OnClickProfitBtn();
        }

        /// <summary>
        /// 回收子Item
        /// </summary>
        private void RecyleRankItem()
        {
            for (int i = 0; i < RankingList.Count; i++)
            {
                RankingList[i].Dispose();
            }
            if (RankingList != null)
            {
                RankingList.Clear();
            }
        }

        /// <summary>
        /// 请求排行榜
        /// </summary>
        private void CreateRrankListsItem(UserInfo info,int type)
        {
            var view = ComponentFactory.CreateWithParent<UIRankListsItem>(this);
            view.InitData();
            view.SetRankItemData(_rankContent, RankingList.Count + 1,info,type);
            RankingList.Add(view);
        }

        private void OnClose()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);
            this.GameObject.SetActive(false);
        }

        #region Button events

        private void OnCloseButtonClicked()
        {
            OnClose();
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
            this.GameObject.SetActive(false);
            
            RecyleRankItem();
        }
    }

}
