using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Map)]
    public class C2G_GetRoomInfo_ReqHandler : AMRpcHandler<C2G_GetRoomInfo_Req, G2C_GetRoomInfo_Res>
    {
        protected override void Run(Session session, C2G_GetRoomInfo_Req message, Action<G2C_GetRoomInfo_Res> reply)
        {
            G2C_GetRoomInfo_Res response = new G2C_GetRoomInfo_Res();
            try
            {
                if (!GateHelper.SingSession(session))
                {
                    response.Error = ErrorCode.ERR_SessionError;
                    response.Message = " 当前的session已经断开  !!!";
                    reply(response);
                    return;
                }

                response.RoomData = (RoomData)RoomHelper.GetRoomInfo(message.GameId, message.AreaId, message.UserId,message.RoomId);

                if (response.RoomData == null)
                {
                    response.Error = ErrorCode.ERR_GetRoomInfoError;
                    response.Message = " 获取房间信息失败  !!!";
                    reply(response);
                    return;
                }

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
