using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.DB)]
    public class DBSaveBatchRequestHandler : AMRpcHandler<DBSaveBatchRequest, DBSaveBatchResponse>
    {
        protected override void Run(Session session, DBSaveBatchRequest message, Action<DBSaveBatchResponse> reply)
        {
            RunAsync(session, message, reply).Coroutine();
        }

        protected async ETVoid RunAsync(Session session, DBSaveBatchRequest message, Action<DBSaveBatchResponse> reply)
        {
            DBSaveBatchResponse response = new DBSaveBatchResponse();
            try
            {
                DBComponent dbComponent = Game.Scene.GetComponent<DBComponent>();

                if (string.IsNullOrEmpty(message.CollectionName))
                {
                    // myDebugInfo(session, message);//[MyAppend]
                    if (message.Components != null)//[MyAppend]
                    {//[MyAppend]
                        message.CollectionName = message.Components[0].GetType().Name;
                    }//[MyAppend]
                }

                await dbComponent.AddBatch(message.Components, message.CollectionName);

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

        //------------//[MyAppend begin]------------
        private void myDebugInfo(Session session, DBSaveBatchRequest message)
        {
            if (message == null)
            {
                Log.Error("[MyDebug - message==null, "+ session.RemoteAddress.Address + "]\n\n");
            }
            else if (message.Components == null)
            {
                Log.Error("[MyDebug - message.Components==null, " + session.RemoteAddress.Address + "]\n\n");
            }
            else if (message.Components.Count == 0)
            {
                Log.Error("[MyDebug - message.Components.Count == 0, " + session.RemoteAddress.Address + "]\n\n");
            }
        }
        
        //------------//[MyAppend end]------------
    }
}
