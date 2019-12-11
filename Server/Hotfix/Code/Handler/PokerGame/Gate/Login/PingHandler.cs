using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class PingHandler : AMRpcHandler<PingRequest, PingResponse>
    {
        protected override void Run(Session session, PingRequest message, Action<PingResponse> reply)
        {
            PingResponse response = new PingResponse();
            try
            {
                    Game.Scene.GetComponent<PingComponent>().UpdateSession(session.Id);

                    reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
