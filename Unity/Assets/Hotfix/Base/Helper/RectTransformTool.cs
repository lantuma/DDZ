namespace ETHotfix
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class RectTransformTool
    {
        /// <summary>
        /// UI子对象的坐标转换为已Canvas为父对象的坐标
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public static Vector2 ChildToCanvasCoord(Transform child)
        {
            RectTransform canvasRectTrans = GameObject.Find("UI").GetComponent<RectTransform>();
            Vector2 _screenPoint = Vector2.zero;
            Camera camera = canvasRectTrans.GetComponent<Canvas>().worldCamera;

            Vector2 local;
            if (canvasRectTrans.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay)
            {
                _screenPoint = child.position;
                camera = null;
            }
            else
            {
                _screenPoint = Camera.main.WorldToScreenPoint(child.position);
                camera = canvasRectTrans.GetComponent<Canvas>().worldCamera;
            }
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTrans, _screenPoint, camera, out local))
            {
                return local;
            }

            return new Vector2(0, 0);
        }
    }
}