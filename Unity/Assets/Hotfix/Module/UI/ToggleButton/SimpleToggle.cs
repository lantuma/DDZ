using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class SimpleToggleAwakeSystem : AwakeSystem<SimpleToggle, GameObject, GameObject, ButtonGroup>
    {
        public override void Awake(SimpleToggle self, GameObject a, GameObject b, ButtonGroup c)
        {
            self.Awake(a, b, c);
        }
    }

    public class SimpleToggle : Entity, IToggleButton
    {
        protected GameObject panelGo;
        protected ReferenceCollector ReferenceCollector;

        private GameObject onGo;
        private GameObject offGo;

        public void Awake(GameObject button, GameObject panel, ButtonGroup group)
        {
            panelGo = panel;
            onGo = button.transform.GetChild(0).gameObject;
            offGo = button.transform.GetChild(1).gameObject;

            // 将自己加入group组,且不自动选中
            group.AddButton(this, false);

            // 注册自己的点击事件, 这里是通知Button组自己的变化,请不要手动调用
            button.GetComponent<Button>().onClick.AddListener(OnClick);

            ReferenceCollector = panelGo.GetComponent<ReferenceCollector>();

            Init();
        }

        public void OnClick()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            ButtonGroup.OnButtonClick(this);
        }

        public ButtonGroup ButtonGroup { get; set; }

        public void IsButtonClick(bool click)
        {
            if (click)
            {
                onGo.SetActive(true);
                offGo.SetActive(false);
                panelGo.SetActive(true);
                ChangeToggle(true);
            }
            else
            {
                onGo.SetActive(false);
                offGo.SetActive(true);
                panelGo.SetActive(false);
                ChangeToggle(false);
            }
        }

        /// <summary>
        /// 在这里进行初始化工作
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// 切换Toggle时调用
        /// </summary>
        /// <param name="state">切换Toggle的状态为True或False</param>
        public virtual void ChangeToggle(bool state)
        {
        }
    }
}