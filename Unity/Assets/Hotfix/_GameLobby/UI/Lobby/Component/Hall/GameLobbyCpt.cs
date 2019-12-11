/******************************************************************************************
*         【模块】{ 大厅场景组件 }                                                                                                                      
*         【功能】{ 管理大厅业务 }
*         【修改日期】{ 2019年7月18日 }                                                                                                                        
*         【贡献者】{｝                                                                                                             
*                                                                                                                                        
 ******************************************************************************************/
using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameLobbyCptStartSystem : StartSystem<GameLobbyCpt>
    {
        public override void Start(GameLobbyCpt self)
        {
            self.Start();
        }
    }

    public class GameLobbyCpt : Entity
    {
        /// <summary>
        /// 大厅引用组件
        /// </summary>
        private ReferenceCollector _rf;

        /// <summary>
        ///  是否获取场
        /// </summary>
        public bool _canGetArea;

        #region 插件

        /// <summary>
        /// 大厅-底部插件
        /// </summary>
        public GameLobbyBottomPlugin GameLobbyBottomPlugin;

        /// <summary>
        /// 大厅-顶部插件
        /// </summary>
        public GameLobbyTopPlugin GameLobbyTopPlugin;

        /// <summary>
        /// 大厅-游戏列表插件
        /// </summary>
        public GameLobbyGameListPlugin GameLobbyGameListPlugin;

        /// <summary>
        /// 大厅-轮插插件
        /// </summary>
        public GameLobbyCarousePlugin GameLobbyCarousePlugin;

        /// <summary>
        /// 大厅-场选择插件
        /// </summary>
        public GameLobbyGameTypeSelectPlugin GameLobbyGameTypeSelectPlugin;

        #endregion

        #region 组件
        
        

        /// <summary>
        /// 公告组件
        /// </summary>
        public GameNoticeCpt GameNoticeCpt;
        

        /// <summary>
        /// 用户中心
        /// </summary>
        public GameUserCenterCpt GameUserCenterCpt;
        

        /// <summary>
        /// 个人设置
        /// </summary>
        public GamePersonSettingCpt GamePersonSettingCpt;

        /// <summary>
        /// 战绩组件
        /// </summary>
        public GameMyRecordCpt GameMyRecordCpt;

        /// <summary>
        /// 排行榜组件
        /// </summary>
        public UIRankPanelCpt UIRankPanelCpt;
        

        #endregion

        public void Start()
        {
            _rf = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            //绑定大厅插件
            this.GameLobbyBottomPlugin = AddComponent<GameLobbyBottomPlugin>().Awake(_rf.Get<GameObject>("BottomDialog"));

            this.GameLobbyCarousePlugin = AddComponent<GameLobbyCarousePlugin>().Awake(_rf.Get<GameObject>("ShareBg"));

            this.GameLobbyGameListPlugin = AddComponent<GameLobbyGameListPlugin>().Awake(_rf.Get<GameObject>("GameListsDialog"));

            this.GameLobbyGameTypeSelectPlugin = AddComponent<GameLobbyGameTypeSelectPlugin>().Awake(_rf.Get<GameObject>("GameTypeBg"));

            this.GameLobbyTopPlugin = AddComponent<GameLobbyTopPlugin>().Awake(_rf.Get<GameObject>("L_TopDialog"));

            //检测是否需要动态加载图片
            //Game.EventSystem.Run(EventIdType.AsyncImageDownload);

            //检测模块是否开启或关闭
            //Game.EventSystem.Run(EventIdType.ModuleEnable);

            //检测游戏回重
            Game.EventSystem.Run(EventIdType.CheckGameReBack);

            //初始化
            Initail();

            //播放大厅背景音乐
            var _soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1);
            SoundComponent.Instance.PlayMusic(DataCenterComponent.Instance.soundInfo.bg_hall, 0, _soundVolume, true);
            
            //显示大厅跑马灯

            var noticData = new NoticeData();

            noticData.Pos = new Vector3(0, 60, 0);

            Game.PopupComponent.ShowNoticeUI(DataCenterComponent.Instance.MarqueeInfo.SubGameDefault,noticData);

            //大厅增加点击特效
            ClickEffectComponent.instance.Bind();
        }

        /// <summary>
        /// 初始化大厅
        /// </summary>
        private async void Initail()
        {
            GameLobbyTopPlugin.SetAccountInfo();

            GameHelper.CurrentAreaInfo = null;

            GameHelper.CurrentGameInfo = null;

            GameHelper.AreaList?.Clear();

            _canGetArea = true;

            //获取游戏列表
            await GameHelper.GetGameList();
        }

        /// <summary>
        /// 通用加入房间方法
        /// </summary>
        /// <param name="areaIndex"></param>
        public async void RequestJoinRoom(int areaIndex)
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            var index = GameHelper.AreaList[GameHelper.CurrentGameInfo.GameId].Count;
            if (index > areaIndex) index = areaIndex;
            else index -= 1;
            try
            {
                Game.PopupComponent.ShowLoadingLockUI(DataCenterComponent.Instance.tipInfo.EnterSubGameTip);

                var response = (G2C_JionRoom_Res)await SessionComponent.Instance.Session.Call(
                    new C2G_JionRoom_Req()
                    {
                        AreaId = GameHelper.AreaList[GameHelper.CurrentGameInfo.GameId][index].AreaId,

                        GameId = GameHelper.CurrentGameInfo.GameId,

                        UserId = GamePrefs.GetUserId(),
                    });

                Game.PopupComponent.CloseLoadingLockUI();

                if (response.Error != 0)
                {
                    Game.PopupComponent.ShowMessageBox(response.Message);

                    return;
                }

                GameHelper.CurrentAreaInfo = GameHelper.AreaList[GameHelper.CurrentGameInfo.GameId][index];

                GameHelper.CurrentRoomId = response.RoomId;

                GameHelper.CurrentFieldId = index;

                this.EnterSubGame();

                Game.EventSystem.Run(EventIdType.RemoveGameLobby);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);

                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.JoinRoomFailTip);
            }
        }

        /// <summary>
        /// 进入子游戏
        /// </summary>
        private void EnterSubGame()
        {
        }

        /// <summary>
        /// 释放资源
        /// </summary>

        public override void Dispose()
        {
            if (this.IsDisposed) return;

            var uc = Game.Scene.GetComponent<UIComponent>();
            

            if (this.GameNoticeCpt != null) uc.Remove(UIType.UINoticePanel);

            if (this.GameUserCenterCpt != null) uc.Remove(UIType.UserCenterWin);
            

            if (this.GamePersonSettingCpt != null) uc.Remove(UIType.PersonSettingPanel);

            if (this.GameMyRecordCpt != null) uc.Remove(UIType.MyRecordPanel);
            

            if (this.UIRankPanelCpt != null) uc.Remove(UIType.UIRankPanel);

            Game.PopupComponent.CloseNoticeUI();

            //移除点击特效
            ClickEffectComponent.instance.Remove();

            base.Dispose();

        }
    }
}