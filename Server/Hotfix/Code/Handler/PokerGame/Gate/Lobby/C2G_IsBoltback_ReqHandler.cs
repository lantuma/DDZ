using System.Collections.Generic;
using ETModel;
using System;
using System.Linq;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_IsBoltback_ReqHandler : AMRpcHandler<C2G_IsBoltback_Req, G2C_IsBoltback_Res>
    {
        protected override void Run(Session session, C2G_IsBoltback_Req message, Action<G2C_IsBoltback_Res> reply)
        {
            G2C_IsBoltback_Res response = new G2C_IsBoltback_Res();
            try
            {
                if (!GateHelper.SingSession(session))
                {
                    response.Error = ErrorCode.ERR_SessionError;
                    response.Message = " 非法的 siesson 链接请求";
                    return;
                }

                var userInfo = Game.Scene.GetComponent<PlayerManagerComponent>().Get(message.UserId);

                if (userInfo == null)
                {
                    response.Error = ErrorCode.ERR_IsBoltBackError;
                    response.Message = "玩家不在线。";
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