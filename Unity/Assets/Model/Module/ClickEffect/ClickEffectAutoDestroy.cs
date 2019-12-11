using UnityEngine;

namespace ETModel
{
    public class ClickEffectAutoDestroy : MonoBehaviour
    {
        void Start()
        {
            GameObject.Destroy(gameObject, 1f);
        }
        
    }
}
