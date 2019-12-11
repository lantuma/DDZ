/******************************************************************************************
*         【模块】{ 通用模块 }                                                                                                                      
*         【功能】{ 玩家列表面板}                                                                                                                   
*         【修改日期】{ 2019年5月5日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIGamePlayerListsPanelAwakeSystem : AwakeSystem<UIGamePlayerListsPanel>
    {
        public override void Awake(UIGamePlayerListsPanel self)
        {
            self.Awake();
        }
    }

    public class UIGamePlayerListsPanel : Component
    {
        private ReferenceCollector _rf;

        private Text countText;

        private ObjPool _pool;

        private Transform Content;

        private List<GameObject> _objList;

        public GameObject Root;

        public void Awake()
        {
            _rf = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            _objList = new List<GameObject>();

            countText = _rf.Get<GameObject>("countText").GetComponent<Text>();

            Content = _rf.Get<GameObject>("Content").transform;

            Root = _rf.Get<GameObject>("Root");

            ButtonHelper.RegisterButtonEvent(_rf, "CloseButton", OnClose);
            ButtonHelper.RegisterButtonEvent(_rf, "mask", OnClose);

            _pool = ETModel.Game.Scene.GetComponent<SimpleObjectPoolComponent>().GreateObjectPool("playerListPool", UIType.CommonUI, UIType.UIPlayerListsItem, 0);
            
        }

        public void OnClose()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

            OnHide();
        }

        public void Reset()
        {

        }

        public void OnShow()
        {
            this.GetParent<UI>().GameObject.SetActive(true);

            var tec = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIGamePlayerListsPanel).GetComponent<TweenEffectComponent>();

            tec.Play(TweenAnimationIdType.GamePlayerListPanel);

            this.InitList();
        }

        public bool IsShow()
        {
            return this.GetParent<UI>().GameObject.activeSelf;
        }

        public void OnHide()
        {
            this.RecycleItem();

            this.GetParent<UI>().GameObject.SetActive(false);
        }

        public void InitList()
        {
            var _list = DataCenterComponent.Instance.userInfo.getNoSeatUsers();

            countText.text = (_list.Count).ToString();

            var myVO = DataCenterComponent.Instance.userInfo.getMyUserVo();

            for (int i = 0; i < _list.Count; i++)
            {
                var userVo = _list[i];

                GameObject item = _pool.Get();

                item.SetActive(true);

                item.transform.SetParent(this.Content);

                item.transform.localPosition = Vector3.zero;

                item.transform.localScale = Vector3.one;

                item.TryGet<Image>("headImg").sprite = SpriteHelper.GetPlayerHeadSpriteName(userVo.headId);

                item.TryGet<Text>("nickNameTxt").text = StringHelper.FormatNickName(userVo.nickName);

                item.TryGet<Text>("coinTxt").text = $"{NumberHelper.FormatMoney(userVo.gold)}";

                item.TryGet<Image>("Light").gameObject.SetActive(userVo.userID == myVO.userID);

                _objList.Add(item);
            }
        }

        public void RecycleItem()
        {
            for (int i = 0; i < _objList.Count; i++)
            {
                _pool.Recycle(_objList[i]);
            }

            _objList.Clear();
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
