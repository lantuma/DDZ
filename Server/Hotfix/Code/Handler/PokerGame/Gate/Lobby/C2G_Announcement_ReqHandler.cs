using System;
using System.IO;
using ETModel;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_Announcement_ReqHandler : AMRpcHandler<C2G_Announcement_Req, G2C_Announcement_Res>
    {
        protected override  void Run(Session session, C2G_Announcement_Req message, Action<G2C_Announcement_Res> reply)
        {
            G2C_Announcement_Res response = new G2C_Announcement_Res();
            try
            {
                if (!GateHelper.SingSession(session))
                {
                    response.Error = ErrorCode.ERR_SessionError;
                    response.Message = " 当前的session已经断开  !!!";
                    reply(response);
                    return;
                }

                response.Info = new Google.Protobuf.Collections.RepeatedField<AnnounceInfo>();
                
                reply(response);

            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
