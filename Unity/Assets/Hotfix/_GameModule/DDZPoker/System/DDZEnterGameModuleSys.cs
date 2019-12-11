
using System;
using System.Linq;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [Event(EventIdType.OnEnterDDZGameModule)]
    public class DDZEnterGameModuleSys : AEvent
    {
        public async override void Run()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);
            
            DDZGameHelper.CurrentGameInfo = GameHelper.GetGameInfo(GameType.DouDiZhu);

            GameHelper.CurrentGameInfo = GameHelper.GetGameInfo(GameType.DouDiZhu);

            GameHelper.CurrentGameInfo.GameId = 1;

            var ares = await GameHelper.GetGameAreaList(DDZGameHelper.CurrentGameInfo.GameId);

            DDZUIFactory.Init();

            //OnDDZ_LevelButton();
            GameLobbyFactory.Remove();

            var ui = Game.Scene.GetComponent<UIComponent>()?.Get(UIType.UIDDZFieldScene);

            if (ui == null)
            {
                DDZUIFactory.fieldScene.Create();
            }
            else
            {
                ui.GetComponent<UIDDZFieldScene>().OnShow();
            }
        }

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

                DDZUIFactory.gameScene.Create();
            }
            catch (Exception e)
            {
                Game.PopupComponent.ShowMessageBox(e.Message);
                throw;
            }
        }
    }
}
