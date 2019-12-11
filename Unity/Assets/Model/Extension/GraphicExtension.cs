using UnityEngine;
using UnityEngine.UI;

//namespace MMGame.Framework
//{
    public static class GraphicExtension
    {
        public static Vector2 GetSize(this Graphic thiz)
        {
            return thiz.rectTransform.sizeDelta;
        }

        public static Graphic SetSize(this Graphic thiz, int width, int height)
        {
            thiz.rectTransform.sizeDelta = new Vector2(width, height);
            return thiz;
        }
    }
//}

