/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 选择场-场景}                                                                                                                   
*         【修改日期】{ 2019年9月7日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

using System;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIDDZFieldSceneAwakeSystem : AwakeSystem<UIDDZFieldScene>
    {
        public override void Awake(UIDDZFieldScene self)
        {
            self.Awake();
        }
    }

    public class UIDDZFieldScene : Entity
    {
        private ReferenceCollector _rf;

        private Tweener _GameTypeTweener;

        private GameObject GameLevelPanel;

        private GameObject panel;

        private Text GoldNumberText;

        private Text PlayerIDText;

        private Image playerHeadImg;



        private GameObject LevelButton_low;

        private GameObject LevelButton_high;

        private GameObject LevelButton_middle;

        public void Awake()
        {
            _rf = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            panel = this.GetParent<UI>().GameObject;

            this.GameLevelPanel = _rf.Get<GameObject>("GameLevelPanel");

            this._GameTypeTweener = this.GameLevelPanel.GetComponent<RectTransform>().DOLocalMoveX(0, 0.3f).Pause().SetAutoKill(false);

            this.GoldNumberText = _rf.Get<GameObject>("GoldNumberText").GetComponent<Text>();

            this.PlayerIDText = _rf.Get<GameObject>("PlayerIDText").GetComponent<Text>();

            this.playerHeadImg = _rf.Get<GameObject>("playerHeadImg").GetComponent<Image>();

            this.LevelButton_low = _rf.Get<GameObject>("LevelButton_low");

            this.LevelButton_high = _rf.Get<GameObject>("LevelButton_high");

            this.LevelButton_middle = _rf.Get<GameObject>("LevelButton_middle");

            ButtonHelper.RegisterButtonEvent(_rf, "LevelButton_middle", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                OnDDZ_LevelButton();

               // QZNNGameHelper.CurrentFieldId = 0;
            });
            

            ButtonHelper.RegisterButtonEvent(_rf, "BackGameListsButton", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                this.OnClose();
            });
            
            this.OnShow();
        }

        public void OnShow()
        {
            SoundHelper.FadeInPlaySound(DataCenterComponent.Instance.soundInfo.ddz_second_enter);

            this.panel.SetActive(true);

            this.GoldNumberText.text = UserDataHelper.UserInfo.Gold.ToString("F2");

            this.PlayerIDText.text = "ID:" + UserDataHelper.UserInfo.PlayerId;

            this.playerHeadImg.sprite = SpriteHelper.GetPlayerHeadSpriteName(UserDataHelper.UserInfo.HeadId);

            var currentAreaInfo = GameHelper.AreaList[DDZGameHelper.CurrentGameInfo.GameId];

            //this.LevelButton_middle.TryGetInChilds<Text>("ZhunRuText").text = "1"; //currentAreaInfo[0].Score.ToString();

            //this.LevelButton_middle.TryGetInChilds<Text>("BottomScoreText").text = "1"; //currentAreaInfo[0].Score.ToString();
            
        }

        public void OnClose()
        {
            SoundHelper.FadeOutPlaySound();

            DDZUIFactory.fieldScene.Remove();

            Game.EventSystem.Run(EventIdType.SubGameReBackLobby);
        }

        /// <summary>
        /// 进入斗地主
        /// </summary>
        private async void OnDDZ_LevelButton()
        {
            try
            {
                Game.PopupComponent.ShowLoadingLockUI(DataCenterComponent.Instance.tipInfo.EnterSubGameTip);

                var response = (G2C_JionRoom_Res)await SessionComponent.Instance.Session.Call(
                    new C2G_JionRoom_Req()
                    {
                        AreaId = GameHelper.AreaList[DDZGameHelper.CurrentGameInfo.GameId][0].AreaId,
                        GameId = DDZGameHelper.CurrentGameInfo.GameId,
                        UserId = GamePrefs.GetUserId(),
                    });

                Game.PopupComponent.CloseLoadingLockUI();

                if (response.Error != 0)
                {
                    Game.PopupComponent.ShowMessageBox(response.Message);
                    return;
                }

                DDZGameHelper.RoomId = response.RoomId;

                GameLobbyFactory.Remove();

                DDZUIFactory.fieldScene.Remove();

                SoundHelper.FadeOutPlaySound();

                DDZUIFactory.gameScene.Create();
            }
            catch (Exception e)
            {
                Game.PopupComponent.ShowMessageBox(e.Message);
                throw;
            }
        }
        
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            var uc = Game.Scene.GetComponent<UIComponent>();
            

            base.Dispose();
        }
    }
}
