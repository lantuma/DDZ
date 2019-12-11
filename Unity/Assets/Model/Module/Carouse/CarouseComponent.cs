/******************************************************************************************
*         【模块】{ 图片轮播模块 }                                                                                                                      
*         【功能】{ 轮播控制组件 }                                                                                                                   
*         【修改日期】{ 2019年8月1日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class CarouseComponentAwakeSystem : AwakeSystem<CarouseComponent>
    {
        public override void Awake(CarouseComponent self)
        {
            self.Awake();
        }
    }

    public class CarouseComponent:Component
    {
        public static CarouseComponent instance;

        public void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="mCellSize">子物体size</param>
        /// <param name="mSpacing">子物体间隔</param>
        /// <param name="MMoveAxisType">方向</param>
        /// <param name="mLoopDirType">轮播方向-- 1为向左移动，-1为向右移动</param>
        /// <param name="mTweenStepNum">Tween时的步数</param>
        /// <param name="mAutoLoop">自动轮播</param>
        /// <param name="mDrag">可否拖动</param>
        /// <param name="mLoopSpaceTime">mLoopSpaceTime</param>
        public void SetConfig(GameObject target, Vector2 mCellSize,Vector2 mSpacing, AxisType MMoveAxisType, LoopDirType mLoopDirType,int mTweenStepNum,bool mAutoLoop,bool mDrag,float mLoopSpaceTime)
        {
            CarouseImageComponent CarouseImageComponent = target.AddComponent<CarouseImageComponent>();

            GameObject page = target.GetComponent<ReferenceCollector>().Get<GameObject>("Page");//约定

            var pagePoint = page.AddComponent<CarousePagePointComponent>();

            CarouseImageComponent.CarousePagePointComponent = pagePoint;

            CarouseImageComponent.enabled = false;

            CarouseImageComponent.mCellSize = mCellSize;

            CarouseImageComponent.mSpacing = mSpacing;

            CarouseImageComponent.MMoveAxisType = MMoveAxisType;

            CarouseImageComponent.mLoopDirType = mLoopDirType;

            CarouseImageComponent.mTweenStepNum = mTweenStepNum;

            CarouseImageComponent.mAutoLoop = mAutoLoop;

            CarouseImageComponent.mDrag = mDrag;

            CarouseImageComponent.mLoopSpaceTime = mLoopSpaceTime;

            CarouseImageComponent.enabled = true;
        }

        /// <summary>
        /// 设置大厅默认
        /// </summary>
        public void SetHallDefault(GameObject target, int mTweenStepNum=10, float mLoopSpaceTime=8f)
        {
            this.SetConfig(target,

                           new Vector2(281, 430),

                           new Vector2(10, 10),

                           AxisType.Horizontal,

                           LoopDirType.LeftOrDown,

                           mTweenStepNum,

                           true,

                           true,

                           mLoopSpaceTime

                           );
        }
    }
}
