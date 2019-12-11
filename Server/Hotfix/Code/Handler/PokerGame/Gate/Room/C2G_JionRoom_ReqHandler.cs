using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Map)]
    public class C2G_JionRoom_ReqHandler : AMRpcHandler<C2G_JionRoom_Req, G2C_JionRoom_Res>
    {
        protected async override void Run(Session session, C2G_JionRoom_Req message, Action<G2C_JionRoom_Res> reply)
        {
            G2C_JionRoom_Res response = new G2C_JionRoom_Res();

            try
            {
                if (!GateHelper.SingSession(session))
                {
                    response.Error = ErrorCode.ERR_SessionError;
                    response.Message = " 当前的session已经断开  !!!";
                    reply(response);
                    return;
                }
                
                int RoomId =  RoomHelper.JionRoom(message.GameId, message.AreaId, message.UserId, session.Id,message.RoomId,out string errMsg);

                if (RoomId == -1)
                {
                    response.Error = ErrorCode.ERR_JionRoomError;
                    response.Message = errMsg;
                    reply(response);
                    return;
                }

                var myLastRoomInfo = Game.Scene.GetComponent<PlayerManagerComponent>().Get(message.UserId).GetComponent<MyLastJionRoom>().MyLastJionRoomList;

                myLastRoomInfo.Clear();

                MyLastJionRoomData myLast = new MyLastJionRoomData()
                {
                    GameId = message.GameId,
                    AreaId = message.AreaId,
                    RoomId = RoomId
                };

                myLastRoomInfo.Add(myLast);
                
                response.RoomId = RoomId;

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
