
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class PlayerOfflineHandler : AMHandler<Actor_PlayerOffline_Ntt>
    {
        protected override void Run(ETModel.Session session, Actor_PlayerOffline_Ntt message)
        {
            switch (message.PlayerOfflineTypes)
            {
                case 0:
                    Game.EventSystem.Run(EventIdType.NoPlayForLongTime);
                    break;
                case 1:
                    Game.EventSystem.Run(EventIdType.SamePlayerLogin);
                    break;
            }
        }
    }
}