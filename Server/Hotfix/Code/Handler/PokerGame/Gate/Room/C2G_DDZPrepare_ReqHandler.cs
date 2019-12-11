using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_DDZPrepare_ReqHandler : AMRpcHandler<C2G_DDZPrepare_Req, G2C_DDZPrepare_Res>
    {
        protected override void Run(Session session, C2G_DDZPrepare_Req message, Action<G2C_DDZPrepare_Res> reply)
        {
            G2C_DDZPrepare_Res response = new G2C_DDZPrepare_Res();
            try
            {
                if (!GateHelper.SingSession(session))
                {
                    response.Error = ErrorCode.ERR_SessionError;
                    response.Message = " 当前的session已经断开  !!!";
                    reply(response);
                    return;
                }

                int restul = RoomHelper.Prepare(message.GameId, message.AreaId, message.RoomId,message.UserId);
                if (restul == -1)
                {
                    response.Error = ErrorCode.ERR_DDZPrepareError;
                    response.Message = " 准备失败  !!!";
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
