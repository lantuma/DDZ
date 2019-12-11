using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ETModel;

namespace ETHotfix.Code.Handler.PokerGame.Gate.Lobby
{
    [MessageHandler(AppType.Gate)]
    public class C2G_MobileLogin_ReqHandler : AMRpcHandler<C2G_MobileLogin_Req, G2C_MobileLogin_Res>
    {
        G2C_MobileLogin_Res response = new G2C_MobileLogin_Res();
        protected override async void Run(Session session, C2G_MobileLogin_Req message, Action<G2C_MobileLogin_Res> reply)
        {
            try
            {
                response.Error = ErrorCode.ERR_GetGameListError;
                response.Message = "该功能暂未开放。";
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
