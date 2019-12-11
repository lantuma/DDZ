//namespace MMGame.Framework
//{

    using UnityEngine;
    public static class Texture2DExtension
    {
        public static Sprite CreateSprite(this Texture2D thiz, Rect rect)
        {
            var sprite = Sprite.Create(thiz, rect, new Vector2(0.5f, 0.5f));

            return sprite;
        }

        public static Sprite ToSprite(this Texture2D thiz)
        {
            return CreateSprite(thiz, new Rect(0f, 0f, thiz.width, thiz.height));
        }

    }
//}

