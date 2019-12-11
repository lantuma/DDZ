using System.Diagnostics;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [MessageHandler]
    public class LobbyNoticeHandler : AMHandler<Actor_Test1_Ntt>
    {
        protected override void Run(ETModel.Session session, Actor_Test1_Ntt message)
        {
            //UnityEngine.Debug.Log("收到广播消息Actor_Test1_Ntt: " + message.Message1);
        }
    }

}