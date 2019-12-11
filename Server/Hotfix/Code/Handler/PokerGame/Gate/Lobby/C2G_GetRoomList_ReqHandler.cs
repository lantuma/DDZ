using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_GetRoomList_ReqHandler : AMRpcHandler<C2G_GetRoomList_Req, G2C_GetRoomLis_Res>
    {
        protected override void Run(Session session, C2G_GetRoomList_Req message, Action<G2C_GetRoomLis_Res> reply)
        {
            G2C_GetRoomLis_Res response = new G2C_GetRoomLis_Res();
            try
            {
              List<RoomListInfo>  roomList=   RoomHelper.GetRoomList(message.GameId, message.AreaId);

                if (roomList == null || roomList.Count < 1)
                {
                    response.Error = ErrorCode.ERR_GetRoomListError;
                    response.Message = "获取房间列表失败 !!!";
                    reply(response);
                }

                response.RoomList = new RepeatedField<RoomListInfo>();

                roomList.ForEach(u => response.RoomList.Add(u));

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
