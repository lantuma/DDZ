
using System;
using System.Linq;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [Event(EventIdType.CheckGameReBack)]
    public class CheckGameRebackSystem : AEvent
    {
        private GameReBackData backData = null;

        public override void Run()
        {

            GameHelper.ApplicationIsPause = false;

            if (DataCenterComponent.Instance.GameReBackInfo.backData != null)
            {
                this.backData = DataCenterComponent.Instance.GameReBackInfo.backData;

                this.ShowMessageBox();
            }
            else
            {
                //从持久化数据中查询
                string _str = PlayerPrefs.GetString(DataCenterComponent.Instance.GameReBackInfo.reBackDataKey, "");

                if (!string.IsNullOrEmpty(_str))
                {
                    GameReBackData _backData = JsonHelper.FromJson<GameReBackData>(_str);

                    this.backData = _backData;

                    this.ShowMessageBox();
                }
            }
        }

        /// <summary>
        /// 清除重回数据
        /// </summary>
        public void ClearReBackData()
        {
            this.backData = null;

            DataCenterComponent.Instance.GameReBackInfo.backData = null;

            DataCenterComponent.Instance.GameReBackInfo.isBackSuccess = false;

            PlayerPrefs.DeleteKey(DataCenterComponent.Instance.GameReBackInfo.reBackDataKey);
        }

        public async void ShowMessageBox()
        {
            if (this.backData == null) { return; }

            //增加：先向服务器验证，如果通过再弹窗口

            var response = (G2C_IsBoltback_Res)await SessionComponent.Instance.Session.Call(
                    new C2G_IsBoltback_Req()
                    {
                        AreaId = this.backData.AreaId,
                        GameId = this.backData.GameId,
                        UserId = GamePrefs.GetUserId(),
                        RoomId = this.backData.RoomId
                    });

            if (!response.IsBoltback)
            {
                Log.Debug("服务器验证失败，不弹重回窗口!!");

                return;
            }

            MessageData messData = new MessageData();

            messData.onlyOk = false;

            messData.ok = OkAction;

            messData.cancel = NoAction;

            messData.titleStr = DataCenterComponent.Instance.tipInfo.Tips;

            messData.mes = DataCenterComponent.Instance.tipInfo.HasGameRebackTip;

            messData.okStr = DataCenterComponent.Instance.tipInfo.OkStrTip;

            Game.PopupComponent.ShowMessageBox(messData.mes, messData);
        }
        
        /// <summary>
        /// 确认回调
        /// </summary>
        public async void OkAction()
        {
            await Task.Delay(200);

            Game.PopupComponent.ShowLoadingLockUI(DataCenterComponent.Instance.tipInfo.TryRebackGameTip);

            await GameHelper.GetGameList();//获取一下游戏列表

            this.SaveGameInfo(this.backData.GameId);

            var ares = await GameHelper.GetGameAreaList(GameHelper.CurrentGameInfo.GameId);//获取场列表

            var roomList = await GameHelper.GetRoomList(GameHelper.CurrentGameInfo.GameId, ares[0].AreaId);//获取房间列表

            this.RequestJoinRoom(backData.GameId, backData.AreaId, backData.RoomId,backData.index);//重新加入房间
        }

        /// <summary>
        /// 保存游戏信息
        /// </summary>
        /// <param name="GameId"></param>
        private void SaveGameInfo(int GameId)
        {
           GameHelper.CurrentGameInfo = GameHelper.GetGameInfo((GameType)GameId);

           GameHelper.CurrentGameInfo.GameId = GameId;
        }

        /// <summary>
        /// 重新加入房间
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="AreaId"></param>
        /// <param name="RoomId"></param>
        private async void RequestJoinRoom(int GameId, int AreaId, int RoomId = 0,int Index=0)
        {
            try
            {
                var response = (G2C_JionRoom_Res)await SessionComponent.Instance.Session.Call(
                    new C2G_JionRoom_Req()
                    {
                        AreaId = AreaId,
                        GameId = GameId,
                        UserId = GamePrefs.GetUserId(),
                        RoomId = RoomId
                    });

                //去掉小菊花,并清掉缓存 
                Game.PopupComponent.CloseLoadingLockUI();
                
                if (response.Error != 0)
                {
                    Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.GameRebackErrorTip);

                    this.ClearReBackData();

                    return;
                }

                //将子游戏重回处理，分发到各子游戏模块
                Game.EventSystem.Run(EventIdType.HandleSubGameReBack, GameId, response.RoomId, backData);
                
            }
            catch (Exception e)
            {
                Game.PopupComponent.CloseLoadingLockUI();

                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.GameRebackErrorTip);

                throw;
            }
        }

        public void HandleGame(int GameId, int RoomId,int Index)
        {
            GameHelper.CurrentAreaInfo = GameHelper.AreaList[GameHelper.CurrentGameInfo.GameId][Index];
            GameHelper.CurrentRoomId = RoomId;
            GameHelper.CurrentFieldId = Index;

            // 加入成功, 跳转场景
            
            Game.EventSystem.Run(EventIdType.RemoveGameLobby);

            this.ClearReBackData();
        }
        
        /// <summary>
        /// 取消回调
        /// </summary>
        public void NoAction()
        {
            this.ClearReBackData();
        }
    }
}