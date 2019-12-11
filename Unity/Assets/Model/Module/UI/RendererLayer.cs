using UnityEngine;

namespace ETModel
{
    public class RendererLayer : MonoBehaviour
    {
        public string SortingLayer
        {
            get
            {
                if (GetComponent<Renderer>() != null)
                {
                    return GetComponent<Renderer>().sortingLayerName;
                }

                return UiSortLayer.Default.ToString();
            }
            set { GetComponent<Renderer>().sortingLayerName = value; }
        }

        public int OrderInLayer
        {
            get
            {
                if (GetComponent<Renderer>() != null)
                {
                    return GetComponent<Renderer>().sortingOrder;
                }
                else
                {
                    return 0;
                }

            }
            set { GetComponent<Renderer>().sortingOrder = value; }
        }

        public bool IsTransparentQueue()
        {
            var render = GetComponent<Renderer>();
            if (render != null)
            {
                return render.material.renderQueue == 3000;
            }
            return false;
        }

        public enum UiSortLayer
        {
            Default, Background, Popup, Popup2, Top, Topmost
        }
    }
}