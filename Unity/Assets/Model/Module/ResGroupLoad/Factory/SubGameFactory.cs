namespace ETModel
{
    public static class SubGameFactory
    {
        public static SubGame Create(long GameID, int Index)
        {
            SubGame subGame = ComponentFactory.CreateWithParent<SubGame>(Game.Scene.GetComponent<UIComponent>());

            subGame.GameID = GameID;

            subGame.Index = Index;

            subGame.AddComponent<ResGroupLoadUIComponent>();

            return subGame;
        }
    }
}
