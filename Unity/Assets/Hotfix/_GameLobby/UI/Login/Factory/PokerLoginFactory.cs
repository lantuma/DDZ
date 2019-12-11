namespace ETHotfix
{
    public class PokerLoginFactory
    {
        public static UI Create(bool isLogout)
        {
            UI ui = AUIFactory.Create(UIType.Login);
            ui.AddComponent<UILoginCpt, bool>(isLogout);

            return ui;
        }

        public static void Remove()
        {
            AUIFactory.Remove(UIType.Login);
        }
    }
}
