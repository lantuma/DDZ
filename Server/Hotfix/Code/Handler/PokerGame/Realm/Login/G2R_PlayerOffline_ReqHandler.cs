using ETModel;
using System;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class G2R_PlayerOffline_ReqHandler : AMRpcHandler<G2R_PlayerOffline_Req, R2G_PlayerOffline_Res>
    {
        protected override void Run(Session session, G2R_PlayerOffline_Req message, Action<R2G_PlayerOffline_Res> reply)
        {
            R2G_PlayerOffline_Res response = new R2G_PlayerOffline_Res();
            try
            {
                Game.Scene.GetComponent<OnlineComponent>().Remove(message.UserID);
                
                Log.Info($"玩家{message.UserID}下线");

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
