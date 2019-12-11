using ETModel;

namespace ETHotfix
{

    [ObjectSystem]
    public class LobbyManagerCptDestroySystem : DestroySystem<LobbyManagerCpt>
    {
        public override void Destroy(LobbyManagerCpt self)
        {
            self.gameList.Clear();
        }
    }

    public static class LobbyManagerCptEx
    {

    }
}
