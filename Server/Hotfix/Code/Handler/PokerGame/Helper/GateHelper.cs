
using ETModel;

namespace ETHotfix
{
    public static class GateHelper
    {
        /// <summary>
        /// 验证session是否合法
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static bool SingSession(Session session)
        {
            SessionUserComponent sessionUseer = session.GetComponent<SessionUserComponent>();

           if(sessionUseer == null || Game.Scene.GetComponent<PlayerManagerComponent>().Get(sessionUseer.userInfo.Id)==null)
                return false;

            return true;
        }
    }

}
