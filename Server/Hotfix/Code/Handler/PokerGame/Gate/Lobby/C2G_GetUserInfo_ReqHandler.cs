using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_GetUserInfo_ReqHandler : AMRpcHandler<C2G_GetUserInfo_Req, G2C_GetUserInfo_Res>
    {
        protected override async void Run(Session session, C2G_GetUserInfo_Req message, Action<G2C_GetUserInfo_Res> reply)
        {
            G2C_GetUserInfo_Res response = new G2C_GetUserInfo_Res();
            try
            {
                if (!GateHelper.SingSession(session))
                {
                    response.Error = ErrorCode.ERR_SessionError;
                    response.Message = " 非法的 siesson 链接请求";
                    return;
                }

                ETModel.UserInfo userInfo = Game.Scene.GetComponent<PlayerManagerComponent>().Get(message.UserId);

                if (userInfo == null)
                {
                    response.Error = ErrorCode.ERR_GetUserInfoError;
                    response.Message = $"查询用户信息失败，用户名{message.UserId}";
                    reply(response);
                    return;
                }
                
                response.UserInfo = new UserInfo()
                {
                    Account = userInfo.Account,
                    PlayerId = userInfo.PlayerId,
                    Nickname = userInfo.Nickname,
                    HeadId = userInfo.HeadId,
                    Gold = userInfo.Gold,
                    Gender = userInfo.Gender,
                    IsTourist = userInfo.IsTourist,
                };

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
