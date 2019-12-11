using System.Collections.Generic;

namespace ETModel
{
    public class OnlineComponent : Component
    {
        private readonly Dictionary<long, int> dictionary = new Dictionary<long, int>();
        
        public void Add(long userId, int gateAppId)
        {
            dictionary.Add(userId, gateAppId);
        }
        
        public int Get(long userId)
        {
            int gateAppId;
            dictionary.TryGetValue(userId, out gateAppId);
            return gateAppId;
        }
        
        public void Remove(long userId)
        {
            dictionary.Remove(userId);
        }
    }
}
