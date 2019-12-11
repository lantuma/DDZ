using System;
using System.IO;
using System.Linq;
using ETModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    [MessageHandler(AppType.Map)]
    public class C2G_ChangerRoom_ReqHandler : AMRpcHandler<C2G_ChangerRoom_Req, G2C_ChangerRoom_Res>
    {
        protected async override void Run(Session session, C2G_ChangerRoom_Req message, Action<G2C_ChangerRoom_Res> reply)
        {
            G2C_ChangerRoom_Res response = new G2C_ChangerRoom_Res();

            try
            {
                if (!GateHelper.SingSession(session))
                {
                    response.Error = ErrorCode.ERR_SessionError;
                    response.Message = " 当前的session已经断开  !!!";
                    reply(response);
                    return;
                }
                
                int[]restul = RoomHelper.ChangerRoom(message.GameId, message.AreaId, message.RoomId, session.Id);
                
                if (restul[0] == -1 )
                {
                    response.Error = ErrorCode.ERR_ChangerRoomError;
                    response.Message = "换桌失败";
                    reply(response);
                    return;
                }
                
                var myLastRoomInfo = Game.Scene.GetComponent<PlayerManagerComponent>().Get(message.UserId).GetComponent<MyLastJionRoom>().MyLastJionRoomList;

                if (myLastRoomInfo.Count == 0) myLastRoomInfo.Add(new MyLastJionRoomData());
                
                myLastRoomInfo.First().GameId = message.GameId;
                myLastRoomInfo.First().AreaId = message.AreaId;
                myLastRoomInfo.First().RoomId = restul[0];


                response.GameId = message.GameId;
                response.AreaId = message.AreaId;
                response.RoomId = restul[0];
                response.Index = restul[1];
                
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }

    
}
