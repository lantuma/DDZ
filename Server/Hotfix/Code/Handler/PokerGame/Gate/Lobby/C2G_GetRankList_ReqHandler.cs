using System;
using System.Linq;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Map)]
    public class C2G_GetRankList_ReqHandler : AMRpcHandler<C2G_GetRankList_Req, G2C_GetRankList_Res>
    {
        protected override void Run(Session session, C2G_GetRankList_Req message, Action<G2C_GetRankList_Res> reply)
        {
            G2C_GetRankList_Res response = new G2C_GetRankList_Res();
            try
            {
                var IncomeList = Game.Scene.GetComponent<RankingManagerCpt>().IncomeRankingList;
                
                var goldList = Game.Scene.GetComponent<RankingManagerCpt>().GoldRankingList;

                if (IncomeList == null || goldList == null)
                {
                    response.Error = ErrorCode.ERR_GetRankListError;
                    response.Message = " 获取排行榜失败  !!!";
                    reply(response);
                    return;
                }

                response.IncomeList = new RepeatedField<UserInfo>();

                response.GoldList = new RepeatedField<UserInfo>();

                IncomeList.ForEach(u => { if (u.Scroe > 0) response.IncomeList.Add(CoventUserData(u)); });

                goldList.ForEach(u => { if (u.Gold > 0) response.GoldList.Add(CoventUserData(u)); });

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }


        public UserInfo CoventUserData(ETModel.UserInfo _userInfo)
        {
            UserInfo userInfo = new UserInfo()
            {
                Account= _userInfo.Account,
                PlayerId= _userInfo.PlayerId,
                Nickname= _userInfo.Nickname,
                HeadId= _userInfo.HeadId,
                Gold = _userInfo.Gold,
                Gender= _userInfo.Gender,
                Score= _userInfo.Scroe
            };

            return userInfo;
        }
    }

}
