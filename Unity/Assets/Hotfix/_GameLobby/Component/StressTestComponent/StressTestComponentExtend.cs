using System;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public partial class StressTestComponent : Component
    {
        /// <summary>
        /// 登陆用例
        /// </summary>
        private async Task StressTest_Login()
        {
            try
            {
                // 创建一个ETModel层的Session
                ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>()
                    .Create(GlobalConfigComponent.Instance.GlobalProto.Address);

                // 创建一个ETHotfix层的Session, ETHotfix的Session会通过ETModel层的Session发送消息
                Session realmSession = ComponentFactory.Create<Session, ETModel.Session>(session);

                var response = (G2C_TouristsLogin_Res)await realmSession.Call(new C2G_TouristsLogin_Req());

                if (response.Error != 0)
                {
                    return;
                }

                Debug.Log("account: " + response.Account);
                Debug.Log("pwd: " + response.Password);

                await LoginRequest(response.Account, response.Password);

            }
            catch (Exception e)
            {
                Debug.Log("游客登录失败: " + e.Message);

                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 登录请求
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="isGuest"></param>
        private async Task<int> LoginRequest(string account, string password, bool isGuest = false)
        {
            int isScuess = -1;

            try
            {
                //登录网关服务器
                isScuess = await LoginHelper.OnLoginAsync(account, password, isGuest);
                if (isScuess != 0)
                {
                    Log.Debug("登陆失败!!!");

                    return isScuess;
                }

                Log.Debug("登陆成功===========>");

                return isScuess;
            }
            catch (Exception e)
            {

                Log.Error("无法连接到服务器: " + e.Message);

                return isScuess;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 登出用例
        /// </summary>
        private async Task StressTest_LoginOut()
        {
            try
            {
                Game.Scene.RemoveComponent<PingComponent>();

                Game.Scene.GetComponent<SessionComponent>()?.Session?.Dispose();

                Game.Scene.RemoveComponent<SessionComponent>();

                Log.Debug("登出成功===========>");
            }
            catch (Exception e)
            {
                Log.Debug("出现未知错误:" + e.Message);
            }
        }

        /// <summary>
        /// 战绩请求用例
        /// </summary>
        /// <returns></returns>
        private async Task StressTest_MyRecordReq()
        {
            var resp = (G2C_MyRecord_Res)await SessionComponent.Instance.Session.Call
                    (
               new C2G_MyRecord_Req()
               {
                   UserId = GamePrefs.GetUserId(),
               });
            if (resp.Error == 0)
            {
                Log.Debug("获取战绩成功=>数量:" + resp.Recordlist.Count);
            }
            else
            {
                Log.Debug("获取战绩失败" + resp.Message);
            }
        }

        /// <summary>
        /// 登入登出100次用例
        /// </summary>
        /// <returns></returns>
        private async Task StressTest_LoginInAndOut100(StressTestData data)
        {
            for (int i = 0; i < data.TestCount; i++)
            {
                Log.Debug("当前次数:" + i);

                data.CurrentTestCount++;

                await Task.Delay(500);

                await this.StressTest_Login();

                await Task.Delay(100);

                await this.StressTest_LoginOut();

            }
        }

        /// <summary>
        /// 登入登出1000次用例
        /// </summary>
        /// <returns></returns>
        private async Task StressTest_LoginInAndOut1000(StressTestData data)
        {
            for (int i = 0; i < data.TestCount; i++)
            {
                Log.Debug("当前次数:" + i);

                data.CurrentTestCount++;

                await Task.Delay(500);

                await this.StressTest_Login();

                await Task.Delay(100);

                await this.StressTest_LoginOut();

            }
        }

        /// <summary>
        /// 请求战绩100次
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task StressTest_MyRecordReqTest100(StressTestData data)
        {
            for (int i = 0; i < data.TestCount; i++)
            {
                Log.Debug("当前次数:" + i);

                data.CurrentTestCount++;

                await Task.Delay(500);

                await this.StressTest_MyRecordReq();
            }
        }

        /// <summary>
        /// 请求战绩1000次
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task StressTest_MyRecordReqTest1000(StressTestData data)
        {
            for (int i = 0; i < data.TestCount; i++)
            {
                Log.Debug("当前次数:" + i);

                data.CurrentTestCount++;

                await Task.Delay(500);

                await this.StressTest_MyRecordReq();
            }
        }
    }
}
