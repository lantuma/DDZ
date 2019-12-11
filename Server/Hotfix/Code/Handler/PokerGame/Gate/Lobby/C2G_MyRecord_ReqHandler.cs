using System.Collections.Generic;
using ETModel;
using System;
using System.Linq;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_MyRecord_ReqHandler : AMRpcHandler<C2G_MyRecord_Req, G2C_MyRecord_Res>
    {
        protected override void Run(Session session, C2G_MyRecord_Req message, Action<G2C_MyRecord_Res> reply)
        {
            G2C_MyRecord_Res response = new G2C_MyRecord_Res();
            try
            {
                var userInfo = Game.Scene.GetComponent<PlayerManagerComponent>().Get(message.UserId);

                if (userInfo == null)
                {
                    response.Error = ErrorCode.ERR_GetUserInfoError;
                    response.Message = "用户不在线。";
                    reply(response);
                    return;
                }

                var recordList = (from a in userInfo?.GetComponent<MyRecord>().recordsList orderby a.JionTime descending select a).Take(10).ToList();

                response.Recordlist = new RepeatedField<Record>();

                foreach (var record in recordList)
                {
                    response.Recordlist.Add(new Record()
                    {
                        GameId = record.GameId,
                        JionTime = record.JionTime.ToString(),
                        QuitTime = record.QuitTime.ToString(),
                        Income = record.Income
                    });
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
