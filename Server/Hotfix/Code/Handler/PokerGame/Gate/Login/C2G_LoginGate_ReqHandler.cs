using System;
using System.Net;
using ETModel;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_LoginGate_ReqHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
    {
        protected override async void Run(Session session, C2G_LoginGate message, Action<G2C_LoginGate> reply)
        {
            G2C_LoginGate response = new G2C_LoginGate();
            try
            {
                PokerGateSessionKeyCpt pokerGateSessionKey = Game.Scene.GetComponent<PokerGateSessionKeyCpt>();

                long key = pokerGateSessionKey.Get(message.Key);

                if (key == 0)
                {
                    response.Error = ErrorCode.ERR_ConnectGateKeyError;
                    response.Message = "登录网关服务器失败。";
                    reply(response);
                    return;
                }
                pokerGateSessionKey.Remove(message.Key);
                
                DBProxyComponent dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();

                ETModel.UserInfo userInfo = await dbProxyComponent.Query<ETModel.UserInfo>(message.UserId);
                userInfo.GetComponent<UnitGateComponent>().GateSessionActorId = session.Id;
                
                if (session.GetComponent<SessionUserComponent>() == null)
                {
                    session.AddComponent<SessionUserComponent>().userInfo = userInfo;
                }

                session.GetComponent<SessionUserComponent>().sessionId = session.Id;

                await session.AddComponent<MailBoxComponent, string>(ActorInterceptType.GateSession).AddLocation();

                StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
                IPEndPoint realmIPEndPoint = config.RealmConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session realmSession = Game.Scene.GetComponent<NetInnerComponent>().Get(realmIPEndPoint);
              
                await realmSession.Call(new G2R_PlayerOnline_Req() { UserID = message.UserId, GateAppID = config.StartConfig.AppId });

                Game.Scene.GetComponent<PlayerManagerComponent>().Add(message.UserId, session.GetComponent<SessionUserComponent>(), session.Id);

                userInfo.IsOnline = true;
              
                await dbProxyComponent.Save(userInfo);
                
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}

