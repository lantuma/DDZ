using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_ChangerUserInfo_ReqHandler : AMRpcHandler<C2G_ChangerUserInfo_Req, G2C_ChangerUserInfo_Res>
    {
        protected async override  void Run(Session session, C2G_ChangerUserInfo_Req message, Action<G2C_ChangerUserInfo_Res> reply)
        {
            G2C_ChangerUserInfo_Res response = new G2C_ChangerUserInfo_Res();
            try
            {
                var userInfo = Game.Scene.GetComponent<PlayerManagerComponent>().Get(message.UserId);

                if (userInfo == null)
                {
                    response.Error = ErrorCode.ERR_ChangerUserError;
                    response.Message = $"修改用户信息失败 ";
                    reply(response);
                    return;
                }

                Regex reg = new Regex("[^/[/]/?/*]+");
                Match m = reg.Match(message.NickName);

                switch (message.Type)
                {
                    case 0: 
                        userInfo.HeadId =message.HeadId;
                        ChanagerGender(userInfo.HeadId, userInfo);
                        break;
                    case 1:  
                        bool IsLegal = await VerifyNickName(message.NickName);
                        if (!IsLegal)
                        {
                            response.Error = ErrorCode.ERR_NickNameError;
                            response.Message = $"玩家昵称已存在。";
                            reply(response);
                            return;
                        }
                        else if (message.NickName=="")
                        {
                            response.Error = ErrorCode.ERR_NickNameError;
                            response.Message = $"玩家昵称不可为空。";
                            reply(response);
                            return;
                        }
                        else if (m.Success)
                        {
                            response.Error = ErrorCode.ERR_NickNameError;
                            response.Message = $"玩家昵称不支持特殊符号。";
                            reply(response);
                            return;
                        }
                        else if (message.NickName.ToCharArray().Length > 6)
                        {
                            response.Error = ErrorCode.ERR_NickNameError;
                            response.Message = $"最多可输入6位字符。";
                            reply(response);
                            return;
                        }

                        else
                            userInfo.Nickname = message.NickName;

                        break;
                }
                
                await Game.Scene.GetComponent<DBProxyComponent>().Save(userInfo);

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

        private void ChanagerGender(int headId,ETModel.UserInfo userInfo)
        {
            if (headId < 6)
                userInfo.Gender = 1;
            else
                userInfo.Gender = 2;
        }
        
        private async ETTask<bool> VerifyNickName(string nickeName)
        {
            List<ComponentWithId> result = await Game.Scene.GetComponent<DBProxyComponent>().Query<ETModel.UserInfo>("{Nickname:\"" +nickeName + "\"}");
            if (result.Count > 0)
                return false;
            else return true;
        }

    }
}
