using UnityEngine;

namespace ETModel
{
    public enum ZCanvasType
    {
        /// <summary>
        /// 隐藏层(当调用Close时候,实际上是把UI物体移到该层中进行隐藏)
        /// </summary>
        UIHiden,

        /// <summary>
        /// 底层(一般用来放置最底层的UI)
        /// </summary>
        Bottom,

        /// <summary>
        /// 中间层(常用,大部分界面均放在此层)
        /// </summary>
        Medium,

        /// <summary>
        /// 上层(放置各种弹窗,小窗口之类)
        /// </summary>
        Top,

        /// <summary>
        /// 最上层,一般用来做各种遮罩层,屏蔽输入，或者切换动画等
        /// </summary>
        TopMost
    }

    /// <summary>
    /// UI层级配置
    /// </summary>
    public class CanvasConfig_Z : MonoBehaviour
    {
        public ZCanvasType CanvasName = ZCanvasType.Medium;
    }
}
