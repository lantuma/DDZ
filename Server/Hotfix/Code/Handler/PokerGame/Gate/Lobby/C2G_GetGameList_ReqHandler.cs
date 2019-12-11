using ETModel;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_GetGameList_ReqHandler : AMRpcHandler<C2G_GetGameList_Req, G2C_GetGameList_Res>
    {
        protected override void Run(Session session, C2G_GetGameList_Req message, Action<G2C_GetGameList_Res> reply)
        {
            G2C_GetGameList_Res response = new G2C_GetGameList_Res();
            try
            {
                foreach (var game in Game.Scene.GetComponent<LobbyManagerCpt>().gameList)
                {
                    GameInfo gameInfo = new GameInfo()
                    {
                        GameId = game.GameId,
                        GameName=game.GameName
                    };
                    response.GameInfo.Add(gameInfo);
                }

                if (response.GameInfo == null || response.GameInfo.count == 0)
                {
                    response.Error = ErrorCode.ERR_GetGameListError;
                    response.Message = "获取游戏列表失败 !!!";
                    reply(response);
                    return;
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
