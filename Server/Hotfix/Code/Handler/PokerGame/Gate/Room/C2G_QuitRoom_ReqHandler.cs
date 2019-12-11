using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_QuitRoom_ReqHandler : AMRpcHandler<C2G_QuitRoom_Req, G2C_QuitRoom_Res>
    {
        protected async override void Run(Session session, C2G_QuitRoom_Req message, Action<G2C_QuitRoom_Res> reply)
        {
            G2C_QuitRoom_Res response = new G2C_QuitRoom_Res();

            try
            {
                if (!GateHelper.SingSession(session))
                {
                    response.Error = ErrorCode.ERR_SessionError;
                    response.Message = " 当前的session已经断开  !!!";
                    reply(response);
                    return;
                }

               var Result= await RoomHelper.QuitRoom(message.GameId, message.AreaId, message.UserId,message.RoomId);

                if (Result != "0" && Result != "1")
                {
                    response.Error = ErrorCode.ERR_QuitRoomError;
                    response.Message = Result;
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
