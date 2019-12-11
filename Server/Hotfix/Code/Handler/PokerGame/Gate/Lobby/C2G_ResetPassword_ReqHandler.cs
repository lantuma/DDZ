using ETModel;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_ResetPassword_ReqHandler : AMRpcHandler<C2G_ResetPassword_Req, G2C_ResetPassword_Res>
    {
        protected override async void Run(Session session, C2G_ResetPassword_Req message, Action<G2C_ResetPassword_Res> reply)
        {
            G2C_ResetPassword_Res response = new G2C_ResetPassword_Res();
            try
            {
                if (!GateHelper.SingSession(session))
                {
                    response.Error = ErrorCode.ERR_SessionError;
                    response.Message = " 非法的 siesson 链接请求";
                    reply(response);
                    return;
                }
               
                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
               
                List<ComponentWithId> result = await dbProxy.Query<AccountInfo>(_account => _account.Id == message.UserId);

                AccountInfo account = result[0] as AccountInfo;

                if (account.Password!=message.OldPassword)
                {
                    response.Error = ErrorCode.ERR_ResetPasswordPError;
                    response.Message = "您的原始密码有误，请重新输入 ！！！";
                    reply(response);
                    return;
                }
                if (message.NewPassword==account.Password)
                {
                    response.Error = ErrorCode.ERR_ResetPasswordPError;
                    response.Message = "不能与原始密码相同 ！！！";
                    reply(response);
                    return;
                }

                account.Password = message.NewPassword;
                await dbProxy.Save(account);

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
