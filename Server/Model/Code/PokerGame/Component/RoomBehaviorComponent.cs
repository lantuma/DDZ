using System.Threading.Tasks;

namespace ETModel
{
    public abstract class RoomBehaviorComponent : Entity
    {
        public abstract void Init(object rule, RoomComponent _parent);                             

        public abstract void ResetGame();                                                                         
        
        public abstract Task<string> JionRoom(long userId, long sessionId);                     

        public abstract ETTask<string> QuitRoom(long userId);                                        

        public abstract object GetRoomInfo(long userId);                                                    
        
        public virtual int Prepare(long userId) { return -1; }                                                 

        public virtual object AskScore(object parameter) { return null; }                         

        public virtual object PlayCard(object parameter) { return null; }                               

        public abstract void BroadcastAll(IActorMessage message);                                     

        public abstract void BroadcastOtherPlayer(IActorMessage message, long userId);  
    }
}
