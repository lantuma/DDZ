using ETModel;
using System;

namespace ETHotfix
{
    public static class UserInfoFactory
    {
        public static ETModel.UserInfo Create(long userId, Session session) 
        {
            ETModel.UserInfo userInfo = ComponentFactory.CreateWithId<ETModel.UserInfo>(userId);
            
            userInfo.AddComponent<MyRecord>();
          
            userInfo.AddComponent<MyMailboxInfo>();
           
            userInfo.RegistrTime = DateTime.Now;
            
            userInfo.AddComponent<UnitGateComponent, long>(session.Id);
           
            userInfo.AddComponent<MyLastJionRoom>();
            
            return userInfo;
        }
    }
}
