using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public static class UserHeadHelper
    {
        private static bool _isLoad;

        public static void SetHeadImage(Image img, int headid)
        {
            img.sprite = SpriteHelper.GetPlayerHeadSpriteName(headid);
        }
    }
}