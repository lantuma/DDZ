/******************************************************************************************
*         【模块】{ 大厅模块 }                                                                                                                      
*         【功能】{ 子游戏组件 }                                                                                                                   
*         【修改日期】{ 2019年7月22日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class SubGameComponentStartSystem : StartSystem<SubGameComponent>
    {
        public override void Start(SubGameComponent self)
        {
            self.Start();
        }
    }

    public class SubGameComponent:Entity
    {
        public GameObject Panel { get; private set; }

        private ReferenceCollector _rf;

        public int index = 0;

        public int gameId = 0;

        /// <summary>
        /// 是否需要动态下载
        /// </summary>
        public bool NeddLoad = true;

        /// <summary>
        /// 是否已经下载
        /// </summary>
        public bool HasLoaded = false;

        /// <summary>
        /// 是否可玩
        /// </summary>
        public bool IsOpen = true;

        /// <summary>
        /// 当前下载量
        /// </summary>
        private float CurrentLoadValue = 0;
        

        public GameObject LoadSlider;

        private Button UpdateFlag;

        private Image sliderFg;

        private Text sliderValue;

        private Button enter;

        private SubGameInfo subGameInfo;

        public void Start()
        {

        }

        public void Reset()
        {
            this.LoadSlider.SetActive(false);

            this.UpdateFlag.gameObject.SetActive(false);

            this.sliderFg.fillAmount = 0;

            this.sliderValue.text = "";
            
        }


        public async void SetPanel(GameObject panel, int index)
        {
            this.Panel = panel;

            _rf = this.Panel.GetComponent<ReferenceCollector>();

            this.index = index;

            this.subGameInfo = DataCenterComponent.Instance.gameInfo.SubGameInfoList[index];

            this.gameId = subGameInfo.GameID;

            //是否已经下载 0:未下载 1:已下载
            //this.HasLoaded = PlayerPrefs.GetInt("subGame" + this.gameId, 0) == 0 ? false : true;
            
            //是否需要下载
            this.NeddLoad = subGameInfo.NeedLoad;

            //是否可玩
            this.IsOpen = subGameInfo.IsOpen;

            this.LoadSlider = _rf.Get<GameObject>("LoadSlider");

            this.UpdateFlag = _rf.Get<GameObject>("UpdateFlag").GetComponent<Button>();

            this.sliderFg = _rf.Get<GameObject>("sliderFg").GetComponent<Image>();

            this.sliderValue = _rf.Get<GameObject>("sliderValue").GetComponent<Text>();

            this.enter = this.Panel.transform.GetChild(0).GetComponent<Button>();

            this.UpdateFlag.onClick.AddListener(() =>
            {
                ResGroupLoadComponent.Instance.Load(subGameInfo.PrefabName);
            });

           // this.Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public async void Init()
        {
            this.HasLoaded = await ResGroupLoadComponent.Instance.CheckModuleNeedLoad(subGameInfo.PrefabName);

            if (this.NeddLoad && this.HasLoaded)
            {
                this.UpdateFlag.gameObject.SetActive(true);
            }
            else
            {
                this.UpdateFlag.gameObject.SetActive(false);
            }
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
