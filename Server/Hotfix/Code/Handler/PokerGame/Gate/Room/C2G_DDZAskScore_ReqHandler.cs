using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_DDZAskScore_ReqHandler : AMRpcHandler<C2G_DDZAskScore_Req, G2C_DDZAskScore_Res>
    {
        protected override void Run(Session session, C2G_DDZAskScore_Req message, Action<G2C_DDZAskScore_Res> reply)
        {
            G2C_DDZAskScore_Res response = new G2C_DDZAskScore_Res();
            try
            {
                if (!GateHelper.SingSession(session))
                {
                    response.Error = ErrorCode.ERR_SessionError;
                    response.Message = " 当前的session已经断开  !!!";
                    reply(response);
                    return;
                }

                response = (G2C_DDZAskScore_Res)RoomHelper.AskSocre(message.GameId, message.AreaId, message.RoomId,message);
                
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
