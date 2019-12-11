using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_GetAreaList_ReqHandler : AMRpcHandler<C2G_GetAreaList_Req, G2C_GetAreaList_Res>
    {
        protected override void Run(Session session, C2G_GetAreaList_Req message, Action<G2C_GetAreaList_Res> reply)
        {
            G2C_GetAreaList_Res response = new G2C_GetAreaList_Res();
            try
            {
                if (!GateHelper.SingSession(session))
                {
                    response.Error = ErrorCode.ERR_SessionError;
                    response.Message = " 当前的session已经断开  !!!";
                    reply(response);
                    return;
                }

                foreach (var Area in Game.Scene.GetComponent<LobbyManagerCpt>().GetComponent<AreaManagerCpt>().areaList)
                {
                    if (Area.GameId != message.GameId) continue;

                    AreaInfo areaInfo = new AreaInfo()
                    {
                        GameId = Area.GameId,
                        AreaId = Area.AreaId,
                        Score = Area.Score,
                        AreaType = Area.AreaType
                    };

                    response.AreaInfo.Add(areaInfo);
                }

                if (response.AreaInfo.count == 0)
                {
                    response.Error = ErrorCode.ERR_GetAreaError;
                    response.Message = "获取场信息失败 !!!";
                    reply(response);
                }
                
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
