using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.InitPokerSceneStart)]
    public class InitSceneStart_CreatePokerLogin : AEvent
    {
        public override void Run()
        {
            UI ui = PokerLoginFactory.Create(false);

            Game.Scene.GetComponent<UIComponent>().Add(ui);
        }
    }

}
