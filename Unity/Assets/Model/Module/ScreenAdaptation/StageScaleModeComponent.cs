/******************************************************************************************
*         【模块】{ UI适配模块 }                                                                                                                      
*         【功能】{ 指定UI适配方案 }                                                                                                                   
*         【修改日期】{ 2019年7月4日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class StageScaleModeComponentAwakeSystem : AwakeSystem<StageScaleModeComponent>
    {
        public override void Awake(StageScaleModeComponent self)
        {
            self.Awake();
        }
    }

    public class StageScaleModeComponent : Component
    {
        /// <summary>
        /// 当前适配方案
        /// </summary>
        public StageScaleMode ScaleModel { get; set; }

        /// <summary>
        /// 单例
        /// </summary>
        public static StageScaleModeComponent Instance;

        public void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// 改变适配模式
        /// </summary>
        /// <param name="scaleModel"></param>
        public void ChangeScaleMode(StageScaleMode scaleModel = StageScaleMode.FULL_SCREEN)
        {
            this.ScaleModel = scaleModel;

            this.Handle_SHOWALLMode();

            this.Handle_FullScreenMode();
        }

        /// <summary>
        /// ShowAll适配
        /// </summary>
        private void Handle_SHOWALLMode()
        {
            if (ScaleModel == StageScaleMode.SHOW_ALL)
            {
                //Log.Debug("当前适配模式:SHOW_ALL");

                if (Global != null)
                {
                    var canvasScaler = Global.GetComponentInChildren<CanvasScaler>();

                    canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

                    canvasScaler.referenceResolution = new Vector2(1334, 750);

                    canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

                    canvasScaler.referencePixelsPerUnit = 100;

                }
            }
        }

        /// <summary>
        /// 全面屏适配
        /// </summary>
        private void Handle_FullScreenMode()
        {
            if (ScaleModel == StageScaleMode.FULL_SCREEN)
            {
                //Log.Debug("当前适配模式:FULL_SCREEN");

                if (Global != null)
                {
                    var canvasScaler = Global.GetComponentInChildren<CanvasScaler>();

                    canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

                    canvasScaler.referenceResolution = new Vector2(1334, 750);

                    canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

                    canvasScaler.matchWidthOrHeight = 1;

                    canvasScaler.referencePixelsPerUnit = 100;

                    //场景6层：是否是宽度适配
                    
                    //设置界面：Aspect Ratio Model = Custom->Up To 2.1

                    //各预置体根据情况进行宽度适配，部分UI需进行美术调整。

                }
                
            }
        }
    }

    /// <summary>
    /// 屏幕适配模式
    /// </summary>
    public enum StageScaleMode
    {
        /// <summary>
        /// 保持宽高比，显示全部内容，缩放后应用程序内容向较宽方向填满窗口，另一个方向的两侧可能会不够宽而留有黑边。
        /// 16:9 的手机屏幕，指定一个宽高尺寸，可以在大部分移动设备有相近的体验。
        /// </summary>
        SHOW_ALL = 0,

        /// <summary>
        /// 保持原始宽高比，缩放后在水平和竖直方向填满窗口，但只保持原始宽度不变，高度可能会改变。
        /// 主要适用于竖屏适配
        /// </summary>
        FIXED_WIDTH = 1,

        /// <summary>
        /// 保持原始宽高比，缩放后在水平和竖直方向填满窗口，但只保持原始高度不变，宽度可能改变。
        /// 主要适用于横屏适配
        /// </summary>
        FIXED_HEIGHT = 2,

        /// <summary>
        /// 异形屏/全面屏适配
        /// </summary>
        FULL_SCREEN = 3
    }
}
