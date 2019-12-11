/******************************************************************************************
*         【模块】{ 大厅模块 }                                                                                                                      
*         【功能】{ 游戏列表管理插件 }                                                                                                                   
*         【修改日期】{ 2019年7月18日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETModel;
using System.Threading.Tasks;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameLobbyGameListPluginAwakeSystem : AwakeSystem<GameLobbyGameListPlugin, GameObject>
    {
        public override void Awake(GameLobbyGameListPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class GameLobbyGameListPlugin : Entity
    {
        private GameObject panel;

        public ReferenceCollector _rf;

        private GameLobbyCpt lobby;

        #region GameButton
        //public GameObject ZJHButton;

        //public GameObject NiuNiuButton;

        //public GameObject BJLButton;

        //public GameObject DZButton;

        //public GameObject HongHeiButton;

        //public GameObject LongHDButton;

        public GameObject DDZButton;

        //public GameObject SGJButton;

        //public GameObject QZNNButton;

        #endregion

        #region ImageAnimation
       
        private UIImageAnimation _RechargeButtonAnimationCpt;

        #endregion

        public GameObject _GameListsDialog;

        public GameObject _GameType;

        public Dictionary<int, SubGame> SubGameDic;

        public Dictionary<int, SubGameComponent> subGameComponentDic;

        public Dictionary<int, ETModel.SubGame> SubGameModelDic;//Model层

        public Dictionary<int, ETModel.ResGroupLoadUIComponent> SubGameModelLoadUIDic;

        public ResGroupLoadComponent ResGroupLoadComponent;

        public GameLobbyGameListPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            this._GameListsDialog = this.panel;
            
            this._rf = this.panel.GetComponent<ReferenceCollector>();

            this.lobby = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIHallPanel).GetComponent<GameLobbyCpt>();

            this.SubGameDic = new Dictionary<int, SubGame>();

            this.SubGameModelDic = new Dictionary<int, ETModel.SubGame>();

            this.subGameComponentDic = new Dictionary<int, SubGameComponent>();

            this.SubGameModelLoadUIDic = new Dictionary<int, ResGroupLoadUIComponent>();

            var subGameInfoList = DataCenterComponent.Instance.gameInfo.SubGameInfoList;

            for (int i = 0; i < subGameInfoList.Count; i++)
            {
                    SubGameInfo subGameInfo = subGameInfoList[i];

                    SubGame subGame = SubGameFactory.Create(subGameInfo.GameID, subGameInfo.Index, this);

                    SubGameDic[i] = subGame;

                    GameObject subGameIcon = _rf.Get<GameObject>(subGameInfo.PrefabName);

                    subGame.GetComponent<SubGameComponent>().SetPanel(subGameIcon, subGameInfo.Index);

                    subGameComponentDic[i] = subGame.GetComponent<SubGameComponent>();

                    //创建MODEL层子游戏

                    ETModel.SubGame subGameModel = ETModel.SubGameFactory.Create(subGameInfo.GameID, subGameInfo.Index);

                    SubGameModelDic[i] = subGameModel;

                    subGameModel.GetComponent<ResGroupLoadUIComponent>().SetPanel(subGameIcon, subGameModel.Index);

                    SubGameModelLoadUIDic[i] = subGameModel.GetComponent<ResGroupLoadUIComponent>();
              
            }

            #region Find 

            this._GameType = _rf.Get<GameObject>("GameType");

            //this.ZJHButton = _rf.Get<GameObject>("ZJHButton");

            //this.NiuNiuButton = _rf.Get<GameObject>("NiuNiuButton");

            //this.BJLButton = _rf.Get<GameObject>("BJLButton");

            //this.DZButton = _rf.Get<GameObject>("DZButton");

            //this.HongHeiButton = _rf.Get<GameObject>("HongHeiButton");

            //this.LongHDButton = _rf.Get<GameObject>("LHDButton");

            this.DDZButton = _rf.Get<GameObject>("DDZButton");

            //this.SGJButton = _rf.Get<GameObject>("SGJButton");

            //this.QZNNButton = _rf.Get<GameObject>("QZNNButton");
            

            //ButtonHelper.RegisterButtonEvent(_rf, "NiuNiuButton", () => 
            //{
            //    Game.PopupComponent.SetClickLock();

            //    this.OnNiuNiuButton(); }
            //);

            //ButtonHelper.RegisterButtonEvent(_rf, "HongHeiButton", () => 
            //{
            //    Game.PopupComponent.SetClickLock();

            //    //this.OnHongHeiButton();

            //    Game.EventSystem.Run(EventIdType.HongHeiEnterGameModule);
            //}
            //);

            //ButtonHelper.RegisterButtonEvent(_rf, "LHDButton", () =>
            //{
            //    Game.PopupComponent.SetClickLock();

            //    Game.EventSystem.Run(EventIdType.OnEnterNHDGameModule);
            //});

            //ButtonHelper.RegisterButtonEvent(_rf, "BJLButton", () => 
            //{
            //    Game.PopupComponent.SetClickLock();

            //    Game.EventSystem.Run(EventIdType.OnEnterBJLGameModule);
            //});

            //ButtonHelper.RegisterButtonEvent(_rf, "DZButton", () => 
            //{
            //    Game.PopupComponent.SetClickLock();

            //    this.OnDZButton();
            //});

            //ButtonHelper.RegisterButtonEvent(_rf, "QZNNButton", () => 
            //{
            //    Game.PopupComponent.SetClickLock();
               
            //    Game.EventSystem.Run(EventIdType.OnEnterQZNNGameModule, lobby.GameLobbyGameListPlugin._GameType, lobby.GameLobbyGameTypeSelectPlugin._BackGameListsButton);
            //});

            //ButtonHelper.RegisterButtonEvent(_rf, "ZJHButton", () => 
            //{
            //    Game.PopupComponent.SetClickLock();

            //    this.OnZJHButton();
            //});

            //ButtonHelper.RegisterButtonEvent(_rf, "SGJButton", () => 
            //{
            //    Game.PopupComponent.SetClickLock();

            //    this.OnSGJButton();
            //});

            ButtonHelper.RegisterButtonEvent(_rf, "DDZButton", () => 
            {
                Game.PopupComponent.SetClickLock();
                Game.EventSystem.Run(EventIdType.OnEnterDDZGameModule);
            });

            #endregion

            this.SetAnimationEffect();

            this.InitSubGameResGroup();

            this.InitSubGameUpdate();

            return this;
        }

        ///// <summary>
        ///// 牛牛
        ///// </summary>
        //private async void OnNiuNiuButton()
        //{
        //    ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            
        //    if (!lobby._canGetArea) return;

        //    lobby._canGetArea = false;
        //    SoundHelper.FadeInPlaySound(DataCenterComponent.Instance.soundInfo.bairen_nn_enter,3500);
        //    await lobby.GameLobbyGameTypeSelectPlugin.OnSelectedArea(GameType.NiuNiu);

        //    lobby._canGetArea = true;
        //}

        ///// <summary>
        ///// 炸金花
        ///// </summary>
        //private async void OnZJHButton()
        //{
        //    ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);

        //    if (!lobby._canGetArea) return;

        //    lobby._canGetArea = false;
        //    SoundHelper.FadeInPlaySound(DataCenterComponent.Instance.soundInfo.zjh_second_enter);
        //    await lobby.GameLobbyGameTypeSelectPlugin.OnSelectedArea(GameType.ZhaJinHua);

        //    lobby._canGetArea = true;
        //}

        //private async void OnSGJButton()
        //{
        //    ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            
        //    if (!lobby._canGetArea) return;

        //    lobby._canGetArea = false;

        //    await  lobby.GameLobbyGameTypeSelectPlugin.OnSelectedArea(GameType.ShuiGuoJi);

        //    lobby._canGetArea = true;
        //}

        //private async void OnDZButton()
        //{
        //    ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);

        //    Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.DeZhouNotOpenTip);

        //    return;

        //    if (!lobby._canGetArea) return;

        //    lobby._canGetArea = false;

        //    await lobby.GameLobbyGameTypeSelectPlugin.OnSelectedArea(GameType.DeZhouPuKe);

        //    lobby._canGetArea = true;
        //}

        ///// <summary>
        ///// 进入红黑大战游戏
        ///// </summary>
        //private async void OnHongHeiButton()
        //{
        //    ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);

        //    if (!lobby._canGetArea) return;

        //    lobby._canGetArea = false;

        //    var areaResult = await lobby.GameLobbyGameTypeSelectPlugin.OnSelectedArea(GameType.HongHeiGame);

        //    lobby._canGetArea = true;

        //    if (!areaResult) return;

        //    lobby.RequestJoinRoom(0);
        //}

        /// <summary>
        /// 初始化子游戏资源组
        /// </summary>
        private  void InitSubGameResGroup()
        {
            Log.Debug("初始化子游戏资源组========>");

            var subGameInfos = DataCenterComponent.Instance.gameInfo.SubGameInfoList;

            //百人牛牛
            //ResGroupLoadComponent.Instance.Add(subGameInfos[0].PrefabName, ResGroupIdType.NiuNiu, SubGameModelLoadUIDic[0]);

            ////红黑大战
            //ResGroupLoadComponent.Instance.Add(subGameInfos[1].PrefabName, ResGroupIdType.HongHeiGame, SubGameModelLoadUIDic[1]);

            ////龙虎斗
            //ResGroupLoadComponent.Instance.Add(subGameInfos[2].PrefabName, ResGroupIdType.LongHuDou, SubGameModelLoadUIDic[2]);

            ////百家乐
            //ResGroupLoadComponent.Instance.Add(subGameInfos[3].PrefabName, ResGroupIdType.BaiJiaLe, SubGameModelLoadUIDic[3]);

            ////德州扑克
            //ResGroupLoadComponent.Instance.Add(subGameInfos[4].PrefabName, ResGroupIdType.DeZhouPuKe, SubGameModelLoadUIDic[4]);

            ////抢庄牛牛
            //ResGroupLoadComponent.Instance.Add(subGameInfos[5].PrefabName, ResGroupIdType.QiangZhuangNiuNiu, SubGameModelLoadUIDic[5]);

            ////炸金花
            //ResGroupLoadComponent.Instance.Add(subGameInfos[6].PrefabName, ResGroupIdType.ZhaJinHua, SubGameModelLoadUIDic[6]);

            ////水果机
            //ResGroupLoadComponent.Instance.Add(subGameInfos[7].PrefabName, ResGroupIdType.ShuiGuoJi, SubGameModelLoadUIDic[7]);

            //斗地主
            ResGroupLoadComponent.Instance.Add(subGameInfos[0].PrefabName, ResGroupIdType.DouDiZhu, SubGameModelLoadUIDic[0]);
            
        }

        /// <summary>
        /// 初始化子游戏是否热更
        /// </summary>
        private  void InitSubGameUpdate()
        {
            for (int i = 0; i < 1; i++)
            {
                    this.subGameComponentDic[i].Init();
            }
        }

        /// <summary>
        /// 清除子游戏资源组
        /// </summary>
        private void ClearSubGameResGroup()
        {
            Log.Debug("清除子游戏资源组========>");
            ResGroupLoadComponent.Instance.Clear();
        }

        private void SetAnimationEffect()
        {
           
            
        }

        public void Reset()
        {

        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach (var item in SubGameDic)
            {
                item.Value.Dispose();
            }

            this.SubGameDic.Clear();

            foreach (var item in SubGameModelDic)
            {
                item.Value.Dispose();
            }

            this.SubGameModelDic.Clear();

            this.ClearSubGameResGroup();

            _RechargeButtonAnimationCpt?.Dispose();

           // _BJLAnimationCpt?.Dispose();

           // _ZJHAnimationCpt?.Dispose();

            //_DZAnimationCpt?.Dispose();

           // _NNAnimationCpt?.Dispose();

            //_HHDZAnimationCpt?.Dispose();

            //_LHDAnimationCpt?.Dispose();
        }
    }
}
