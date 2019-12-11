using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.DB)]
    public class DBSaveRequestHandler : AMRpcHandler<DBSaveRequest, DBSaveResponse>
    {
        protected override void Run(Session session, DBSaveRequest message, Action<DBSaveResponse> reply)
        {
            RunAsync(session, message, reply).Coroutine();
        }

        protected async ETVoid RunAsync(Session session, DBSaveRequest message, Action<DBSaveResponse> reply)
        {
            DBSaveResponse response = new DBSaveResponse();
            try
            {
                DBComponent dbComponent = Game.Scene.GetComponent<DBComponent>();
                if (string.IsNullOrEmpty(message.CollectionName))
                {
                    // myDebugInfo(session, message);//[MyAppend]
                    if (message.Component != null)//[MyAppend]
                    {//[MyAppend]
                        message.CollectionName = message.Component.GetType().Name;
                    }//[MyAppend]
                }

                await dbComponent.Add(message.Component, message.CollectionName);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }


        //------------//[MyAppend begin]------------
        private void myDebugInfo(Session session, DBSaveRequest message)
        {
            if (message == null)
            {
                Log.Error("[MyDebug - message==null, " + session.RemoteAddress.Address + "]\n\n");
            }
            else if (message.Component == null)
            {
                Log.Error("[MyDebug - message.Component==null, " + session.RemoteAddress.Address + "]\n\n");
            }
        }

        //------------//[MyAppend end]------------
    }
}