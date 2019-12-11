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
    public class C2G_MailAsk_ReqHandler : AMRpcHandler<C2G_MailAsk_Req, G2C_MailReturn_Res>
    {
        protected override void Run(Session session, C2G_MailAsk_Req message, Action<G2C_MailReturn_Res> reply)
        {
            G2C_MailReturn_Res response = new G2C_MailReturn_Res();
            try
            {
                if (!GateHelper.SingSession(session))
                {
                    response.Error = ErrorCode.ERR_SessionError;
                    response.Message = " 当前的session已经断开  !!!";
                    reply(response);
                    return;
                }
                
                ETModel.UserInfo userInfo = Game.Scene.GetComponent<PlayerManagerComponent>().Get(message.UserId);

                if (userInfo == null)
                {
                    response.Error = ErrorCode.ERR_BindBankCardAskError;
                    response.Message = "用户不在线。";
                    reply(response);
                    return;
                }

                response.Info = new Google.Protobuf.Collections.RepeatedField<MailInfo>();

                userInfo.GetComponent<MyMailboxInfo>().MyMail.ForEach(u => response.Info.Add(new MailInfo() { Content = u.Content, Timestamp = u.SendTime.ToString("yy-MM-dd HH:mm"), Title = u.Title }));

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
