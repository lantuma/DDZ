using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.LoginPokerFinish)]
    public class LoginPokerFinish_RemovePokerLogin : AEvent
    {
        public override void Run()
        {
            PokerLoginFactory.Remove();
        }
    }
}