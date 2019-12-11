using ETModel;
using System;
using System.Collections.Generic;
using System.Net;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class C2R_LoginReqHandler : AMRpcHandler<C2R_Login, R2C_Login>
    {
        protected override void Run(Session session, C2R_Login message, Action<R2C_Login> reply)
        {
            RunAsync(session, message, reply).Coroutine();
        }

        private async ETVoid RunAsync(Session session, C2R_Login message, Action<R2C_Login> reply)
        {
            R2C_Login response = new R2C_Login();
            try
            {
                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();

                Log.Info($"登录请求：{{Account:'{message.Account}',Password:'{message.Password}'}}");
                
                List<ComponentWithId> result = await dbProxy.Query<AccountInfo>(_account => _account.Account == message.Account);
                if (result.Count == 0)
                {
                    response.Error = ErrorCode.ERR_AccountDoesnExist;
                    response.Message = "账号不存在";
                    reply(response);
                    return;
                }
                AccountInfo accountInfo = result[0] as AccountInfo;
                if (accountInfo.Password != message.Password)
                {
                    response.Error = ErrorCode.ERR_LoginError;
                    response.Message = "密码错误";
                    reply(response);
                    return;
                }

                AccountInfo account = result[0] as AccountInfo;

                Log.Info($"账号登录成功{MongoHelper.ToJson(account)}");
                
                StartConfig config = Game.Scene.GetComponent<RealmGateAddressComponent>().GetAddress();

                IPEndPoint innerAddress = config.GetComponent<InnerConfig>().IPEndPoint;

                Session gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(innerAddress);
               
                G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey)await gateSession.Call(new R2G_GetLoginKey() { UserId= account.Id });

                string outerAddress = config.GetComponent<OuterConfig>().Address2;

                response.Address = outerAddress;

                response.Key = g2RGetLoginKey.Key;

                response.UserId = account.Id;

                reply(response);
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
