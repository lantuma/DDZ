using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.RemoveGameLobby)]
    public class RemoveGameLobby : AEvent
    {
        public override void Run()
        {
            GameLobbyFactory.Remove();
        }
    }
}