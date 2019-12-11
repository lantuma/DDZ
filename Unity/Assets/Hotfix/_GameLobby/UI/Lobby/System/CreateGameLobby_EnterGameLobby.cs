using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.CreateGameLobby)]
    public class CreateGameLobby_EnterGameLobby : AEvent
    {
        public override void Run()
        {
            UI ui = GameLobbyFactory.Create();

            Game.Scene.GetComponent<UIComponent>().Add(ui);
        }
    }
}