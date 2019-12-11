using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameUserCenterCptAwakeSystem : AwakeSystem<GameUserCenterCpt>
    {
        public override void Awake(GameUserCenterCpt self)
        {
            self.Awake();
        }
    }

    public class GameUserCenterCpt : Entity
    {
        private GameObject panelGo;
        private ReferenceCollector ReferenceCollector;

        #region 组件

        private Tweener _panelTweener;

        // 按钮组
        private ButtonGroup ButtonGroup;
        // 个人信息组件
        private UserCenter_UserInfoCpt UserInfoCpt;

        //问题反馈组件
        private Setting_ReportCpt ReportCpt;
        
        #endregion

        public void Awake()
        {
            var res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();

            res.LoadBundle(UIType.UserCenterWin.StringToAB());

            var gameObject = res.GetAsset(UIType.UserCenterWin.StringToAB(), UIType.UserCenterWin);

            this.panelGo = (GameObject)UnityEngine.Object.Instantiate(gameObject, this.Parent.Parent.GameObject.transform);

            UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.UserCenterWin, this.panelGo, false);

            Game.Scene.GetComponent<UIComponent>().Add(ui);

            _panelTweener = panelGo.transform.GetChild(1).DOScale(Vector3.one, 0.3f).Pause().SetEase(Ease.OutBack)
                .SetAutoKill(false);

            this.ReferenceCollector = this.panelGo.GetComponent<ReferenceCollector>();

            this.ReferenceCollector.Get<GameObject>("CloseBtn").Get<Button>().onClick.AddListener(Close);
            this.ReferenceCollector.Get<GameObject>("mask").Get<Button>().onClick.AddListener(Close);
            ButtonGroup = AddComponent<ButtonGroup>();

            UserInfoCpt = AddComponent<UserCenter_UserInfoCpt>();
            var userInfoBtn = ReferenceCollector.Get<GameObject>("PersonInfoToggle");
            UserInfoCpt.Awake(userInfoBtn, ReferenceCollector.Get<GameObject>("PersonView"), ButtonGroup);

            ReportCpt = AddComponent<Setting_ReportCpt>();
            var reportBtn = ReferenceCollector.Get<GameObject>("ReportToggle");
            ReportCpt.Awake(reportBtn, ReferenceCollector.Get<GameObject>("ReportView"), ButtonGroup);


            ButtonGroup.OnButtonClick(UserInfoCpt);
        }

        public void Open()
        {
            panelGo.transform.GetChild(1).localScale = Vector3.zero;
            this.panelGo.SetActive(true);
            _panelTweener.PlayForward();
            this.UserInfoCpt.Initial();
        }

        public async void Close()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            _panelTweener.PlayBackwards();
            await Task.Delay(300);
            this.panelGo.SetActive(false);
        }

        /// <summary>
        /// 重新刷新手机号绑定
        /// </summary>
        public void RefBindPhoneNum()
        {
            if (UserDataHelper.UserInfo.PhoneNumber != 0)
            {
                this.UserInfoCpt.BindPhoneNumLab.text = UserDataHelper.UserInfo.PhoneNumber.ToString();
            }
            else
            {
                this.UserInfoCpt.BindPhoneNumLab.text = DataCenterComponent.Instance.tipInfo.NoBindPhoneNumTip;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();

            _panelTweener.Kill();
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(UIType.UserCenterWin.StringToAB());
        }
    }
}