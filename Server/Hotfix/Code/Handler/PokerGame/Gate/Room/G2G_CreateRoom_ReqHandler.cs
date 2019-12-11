using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class G2G_CreateRoom_ReqHandler : AMRpcHandler<G2G_CreateRoom_Req, G2G_CreateRoom_Res>
    {
        protected override void Run(Session session, G2G_CreateRoom_Req message, Action<G2G_CreateRoom_Res> reply)
        {
            G2G_CreateRoom_Res response = new G2G_CreateRoom_Res();
            try
            {
                RoomHelper.CreateRoom();

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
