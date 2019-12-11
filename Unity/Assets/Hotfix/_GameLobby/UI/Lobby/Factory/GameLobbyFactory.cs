namespace ETHotfix
{
    public class GameLobbyFactory
    {
        public static UI Create()
        {
            UI ui = AUIFactory.Create(UIType.UIHallPanel);
            ui.AddComponent<GameLobbyCpt>();

            return ui;
        }

        public static void Remove()
        {
            AUIFactory.Remove(UIType.UIHallPanel);
        }
    }
}