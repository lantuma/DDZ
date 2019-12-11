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
    /// <summary>
    /// 战绩 Item Cpt
    /// </summary>
    public class UIRecordItem : Component
    {
        private ReferenceCollector ReferenceCollector;
        
        private Text GetGoldText;
        private Text TimeText;
        private Text GameTypeText;
        
        private bool _IsInit;

        public void InitData()
        {
            if (this.GameObject != null)
                this.GameObject.SetActive(true);

            if (_IsInit)
                return;

            var res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            res.LoadBundle(UIType.RecordItem.StringToAB());
            var gameObject = res.GetAsset(UIType.RecordItem.StringToAB(), UIType.RecordItem);
            this.GameObject = (GameObject)UnityEngine.Object.Instantiate(gameObject, this.Parent.Parent.GameObject.transform);

            ReferenceCollector = this.GameObject.Get<ReferenceCollector>();
            GetGoldText = ReferenceCollector.Get<GameObject>("GetGoldText").Get<Text>();
            TimeText = ReferenceCollector.Get<GameObject>("TimeText").Get<Text>();
            GameTypeText = ReferenceCollector.Get<GameObject>("GameTypeText").Get<Text>();
            _IsInit = true;
        }

        /// <summary>
        /// 设置战绩界面数据
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="data"></param>
        public void SetRecordItemData(Transform parent, Record data,int index)
        {
            this.GameObject.transform.SetParent(parent);
            this.GameObject.transform.localPosition = Vector3.zero;
            this.GameObject.transform.localScale = Vector3.one;
            //this.GameObject.Get<Image>().enabled = index % 2 == 0;
            if (data.Income >= 0)
            {
                GetGoldText.color = new Color(255f / 255f, 216f / 255f, 44f / 255f, 255f / 255f);
                if (data.Income > 0)
                {
                    GetGoldText.text = $"+{GameHelper.ConvertCoinToString(data.Income, true)}";
                }
                else
                {
                    GetGoldText.text = $"+{GameHelper.ConvertCoinToString(data.Income, true)}";
                }
            }
            else
            {
                GetGoldText.color = new Color(0f / 255f, 204f / 255f, 0f / 255f, 255f / 255f);
                GetGoldText.text = GameHelper.ConvertCoinToString(data.Income, true);
            }
            TimeText.text = data.JionTime;
            GameTypeText.text = GameHelper.GetGameTypeName(data.GameId);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.GameObject.SetActive(false);
        }
    }
}