using UnityEngine;

namespace ETModel
{
    public enum CanvasType
    {
        Background, Popup, Popup2, Top, Topmost
    }

    public class CanvasConfig : MonoBehaviour
    {
        public CanvasType CanvasName;
    }
}
