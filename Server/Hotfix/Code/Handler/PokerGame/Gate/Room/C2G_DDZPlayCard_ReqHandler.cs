using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_DDZPlayCard_ReqHandler : AMRpcHandler<C2G_DDZPlayCard_Req, G2C_DDZPlayCard_Res>
    {
        protected override void Run(Session session, C2G_DDZPlayCard_Req message, Action<G2C_DDZPlayCard_Res> reply)
        {
            G2C_DDZPlayCard_Res response = new G2C_DDZPlayCard_Res();
            try
            {
                if (!GateHelper.SingSession(session))
                {
                    response.Error = ErrorCode.ERR_SessionError;
                    response.Message = " 当前的session已经断开  !!!";
                    reply(response);
                    return;
                }

                response = (G2C_DDZPlayCard_Res)RoomHelper.PlayCard(message.GameId, message.AreaId, message.RoomId, message);

                response.UserId = message.UserId;
              
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}