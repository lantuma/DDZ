using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine.UI;

namespace ETHotfix
{
    public static class LoginHelper
    {

        /// <summary>
        /// 普通账号游客登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <param name="isGuest"></param>
        /// <returns></returns>
        public static async Task<int> OnLoginAsync(string account, string pwd, bool isGuest = false)
        {
            int errcode = -1;
            try
            {
                // 创建一个ETModel层的Session
                ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(GlobalConfigComponent.Instance.GlobalProto.Address);

                // 创建一个ETHotfix层的Session, ETHotfix的Session会通过ETModel层的Session发送消息
                Session realmSession = ComponentFactory.Create<Session, ETModel.Session>(session);

                //向服务器申请了一个Key
                R2C_Login r2CLogin = (R2C_Login)await realmSession.Call(new C2R_Login() { Account = account, Password = pwd });
                errcode = r2CLogin.Error;
                realmSession.Dispose();

                if (r2CLogin.Error != 0)
                {
                    if (isGuest && r2CLogin.Error == ErrorCode.ERR_AccountDoesnExist)
                    {
                        return r2CLogin.Error;
                    }
                    Game.PopupComponent.ShowMessageBox(r2CLogin.Message);
                    return r2CLogin.Error;
                }

                GamePrefs.SetUserId(r2CLogin.UserId);

                // 创建一个ETModel层的Session,并且保存到ETModel.SessionComponent中
                ETModel.Session gateSession = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(r2CLogin.Address);
                ETModel.Game.Scene.AddComponent<ETModel.SessionComponent>().Session = gateSession;

                // 创建一个ETHotfix层的Session, 并且保存到ETHotfix.SessionComponent中
                Game.Scene.AddComponent<SessionComponent>().Session = ComponentFactory.Create<Session, ETModel.Session>(gateSession);

                //增加客户端断线处理组件
                Game.Scene.GetComponent<SessionComponent>().Session.AddComponent<SessionOfflineComponent>();


                G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await SessionComponent.Instance.Session.Call(new C2G_LoginGate() { Key = r2CLogin.Key, UserId = r2CLogin.UserId });
                errcode = g2CLoginGate.Error;
                if (g2CLoginGate.Error != 0)
                {
                    Game.PopupComponent.ShowMessageBox(g2CLoginGate.Message);

                    Game.Scene.GetComponent<SessionComponent>().Session.Dispose();
                    return g2CLoginGate.Error;
                }

                if (Game.Scene.GetComponent<PingComponent>() == null)
                    Game.Scene.AddComponent<PingComponent, long, Session, Action>(3000, Game.Scene.GetComponent<SessionComponent>().Session, null);

                return g2CLoginGate.Error;
            }
            catch (Exception e)
            {
                //                if (e.Message.Equals("ERR_AccountDoesnExist")) throw;
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.NotConnectGateTip);
                Log.Error("无法连接到网关服务器: " + e.Message);
                return errcode;
            }
        }

        /// <summary>
        /// token 登录-手机号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<int> OnLoginAsyncToken(string account, string token)
        {
            int errcode = -1;
            try
            {
                ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(GlobalConfigComponent.Instance.GlobalProto.Address);
                
                Session realmSession = ComponentFactory.Create<Session, ETModel.Session>(session);
                
                G2C_TokenLogin_Res r2CLogin = (G2C_TokenLogin_Res)await realmSession.Call(new C2G_TokenLogin_Req() { Account = account, Token = token });
                errcode = r2CLogin.Error;
                realmSession.Dispose();

                if (r2CLogin.Error != 0)
                {
                    Game.PopupComponent.ShowMessageBox(r2CLogin.Message);
                    return r2CLogin.Error;
                }

                GamePrefs.SetUserId(r2CLogin.UserId);
                ETModel.Session gateSession = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(r2CLogin.Address);
                ETModel.Game.Scene.AddComponent<ETModel.SessionComponent>().Session = gateSession;
                
                Game.Scene.AddComponent<SessionComponent>().Session = ComponentFactory.Create<Session, ETModel.Session>(gateSession);
                
                Game.Scene.GetComponent<SessionComponent>().Session.AddComponent<SessionOfflineComponent>();
                
                G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await SessionComponent.Instance.Session.Call(new C2G_LoginGate() { Key = r2CLogin.Key, UserId = r2CLogin.UserId});
                errcode = g2CLoginGate.Error;
                if (g2CLoginGate.Error != 0)
                {
                    Game.PopupComponent.ShowMessageBox(g2CLoginGate.Message);

                    Game.Scene.GetComponent<SessionComponent>().Session.Dispose();
                    return g2CLoginGate.Error;
                }

                if (Game.Scene.GetComponent<PingComponent>() == null)
                    Game.Scene.AddComponent<PingComponent, long, Session, Action>(3000, Game.Scene.GetComponent<SessionComponent>().Session, null);

                return g2CLoginGate.Error;
            }
            catch (Exception e)
            {
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.NotConnectGateTip);
                Log.Error("无法连接到网关服务器: " + e.Message);
                return errcode;
            }
        }
    }

    public class GetCode
    {
        public string mobile;
    }
}