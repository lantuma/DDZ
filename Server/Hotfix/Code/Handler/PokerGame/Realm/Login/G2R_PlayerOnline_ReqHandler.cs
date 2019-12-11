using ETModel;
using System;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class G2R_PlayerOnline_ReqHandler: AMRpcHandler<G2R_PlayerOnline_Req, R2G_PlayerOnline_Res>
    {
        protected override async void Run(Session session, G2R_PlayerOnline_Req message, Action<R2G_PlayerOnline_Res> reply)
        {
            R2G_PlayerOnline_Res response = new R2G_PlayerOnline_Res();
            try
            {
                OnlineComponent onlineComponent = Game.Scene.GetComponent<OnlineComponent>();
                
                await RealmHelper.KickOutPlayer(message.UserID);
               
                onlineComponent.Add(message.UserID, message.GateAppID);
                
                Log.Info($"玩家{message.UserID}上线");

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }

}

