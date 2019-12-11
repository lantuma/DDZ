using ETModel;
using System;
using System.Collections.Generic;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class C2R_Register_ReqHandler : AMRpcHandler<C2R_Register_Req, R2C_Register_Res>
    {
        protected override async void Run(Session session, C2R_Register_Req message, Action<R2C_Register_Res> reply)
        {
            R2C_Register_Res response = new R2C_Register_Res();
            try
            {
                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
             
                List<ComponentWithId> result= await dbProxy.Query<AccountInfo>(_account => _account.Account == message.Account);
                if (result.Count > 0)
                {
                    response.Error = ErrorCode.ERR_AccountAlreadyRegister;
                    response.Message = "当前账号已经被注册 !!!";
                    reply(response);
                    return;
                }

                AccountInfo newAccount = ComponentFactory.CreateWithId<AccountInfo>(IdGenerater.GenerateId());
                newAccount.Account = message.Account;
                newAccount.Password = message.Password;

                Log.Info($"注册新账号：{MongoHelper.ToJson(newAccount)}");

                ETModel.UserInfo newUser = UserInfoFactory.Create(newAccount.Id, session);

                if (newUser.GetComponent<MailBoxComponent>() != null) newUser.RemoveComponent<MailBoxComponent>();
               await newUser.AddComponent<MailBoxComponent>().AddLocation();

                newUser.PlayerId = RandomHelper.GenerateRandomPlayerId(6);
                newUser.Account = message.Account;
                newUser.Nickname = $"{ RandomHelper.GenerateRandomCode(4):0000}";
                newUser.HeadId = RandomHelper.GetRandom().Next(1,11);

                if (newUser.HeadId < 6)
                    newUser.Gender = 1;
                else
                    newUser.Gender = 2;

                newUser.Gold = 100000;
                newUser.IsTourist = false;
                await dbProxy.Save(newAccount);
                await dbProxy.Save(newUser);
             
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
