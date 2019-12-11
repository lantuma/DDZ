using ETModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Linq;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class G2G_QuitSave_ReqHandler : AMRpcHandler<G2G_QuitSave_Req, G2G_QuitSave_Res>
    {
        protected async override void Run(Session session, G2G_QuitSave_Req message, Action<G2G_QuitSave_Res> reply)
        {
            G2G_QuitSave_Res response = new G2G_QuitSave_Res();
            try
            {
                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();

                var sessionUser = Game.Scene.GetComponent<PlayerManagerComponent>().GetSessionUser(message.UserId);
                
                await dbProxy.Save(sessionUser.userInfo);

                StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();

                IPEndPoint realmIPEndPoint = config.RealmConfig.GetComponent<InnerConfig>().IPEndPoint;

                Session realmSession = Game.Scene.GetComponent<NetInnerComponent>().Get(realmIPEndPoint);

                await realmSession.Call(new G2R_PlayerOffline_Req() { UserID = message.UserId });
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
