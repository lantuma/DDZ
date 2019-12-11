/******************************************************************************************
*         【模块】{ Animator动画控制组件 }                                                                                                                      
*         【功能】{ Animator行为控制 }
*         【修改日期】{ 2019年7月31日 }                                                                                                                        
*         【贡献者】{ 周瑜 整理 ｝                                                                                                               
*                                                                                                                                        
 ******************************************************************************************/

using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class AnimatorControlComponentAwakeSystem : AwakeSystem<AnimatorControlComponent, GameObject>
    {
        public override void Awake(AnimatorControlComponent self, GameObject target)
        {
            self.Awake(target);
        }
    }

    [ObjectSystem]
    public class AnimatorControlComponentUpdateSystem : UpdateSystem<AnimatorControlComponent>
    {
        public override void Update(AnimatorControlComponent self)
        {
            self.Update();
        }
    }

    public class AnimatorControlComponent : Component
    {
        public GameObject target;

        public Dictionary<string, AnimationClip> animationClips = new Dictionary<string, AnimationClip>();

        public HashSet<string> Parameter = new HashSet<string>();

        public Animator Animator;

        public AnimatorMotionType MotionType;
        
        public bool isStop;

        public float stopSpeed;

        public float MontionSpeed;

        public void Awake(GameObject target)
        {
            this.target = target;

            Animator animator = this.target.GetComponent<Animator>();

            if (animator == null)
            {
                return;
            }

            if (animator.runtimeAnimatorController == null)
            {
                return;
            }

            if (animator.runtimeAnimatorController.animationClips == null)
            {
                return;
            }

            this.Animator = animator;

            foreach (AnimationClip animationClip in animator.runtimeAnimatorController.animationClips)
            {
                this.animationClips[animationClip.name] = animationClip;
            }

            foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
            {
                this.Parameter.Add(animatorControllerParameter.name);
            }
        }

        #region Update Check

        public void Update()
        {
            if (this.isStop)
            {
                return;
            }

            if (this.MotionType == AnimatorMotionType.None)
            {
                return;
            }

            try
            {
                this.Animator.SetFloat("MotionSpeed", this.MontionSpeed);

                //设置触发器
                this.Animator.SetTrigger(this.MotionType.ToString());

                this.MontionSpeed = 1;

                this.MotionType = AnimatorMotionType.None;
            }
            catch (Exception ex)
            {

                throw new Exception($"动作播放失败:{this.MotionType}", ex);
            }
        }

        #endregion

        #region Set
        
        public void SetInTime(AnimatorMotionType motionType, float time = 1)
        {
            AnimationClip animationClip;

            if (this.animationClips.TryGetValue(motionType.ToString(), out animationClip))
            {
                throw new Exception($"找不到该动作:{motionType}");
            }

            float motionSpeed = animationClip.length / time;

            //检测是否合理

            if (motionSpeed < 0.01f || motionSpeed > 1000f)
            {
                Log.Error($"motionSpeed数值异常,{motionSpeed},此动作跳过");

                return;
            }

            this.MotionType = motionType;

            this.MontionSpeed = motionSpeed;
        }

        public void Set(AnimatorMotionType motionType, float motionSpeed = 1f)
        {
            if (!this.HasParameter(MotionType.ToString()))
            {
                return;
            }

            this.MotionType = motionType;

            this.MontionSpeed = motionSpeed;
        }

        #endregion 

        #region Pause/Run

        /// <summary>
        /// 暂停动画
        /// </summary>
        public void Pause()
        {
            if (this.isStop)
            {
                return;
            }

            this.isStop = true;

            if (this.Animator == null)
            {
                return;
            }

            this.stopSpeed = this.Animator.speed;

            this.Animator.speed = 0;
        }

        /// <summary>
        /// 运行动画
        /// </summary>
        public void Run()
        {
            if (!this.isStop)
            {
                return;
            }

            this.isStop = false;

            if (this.Animator == null)
            {
                return;
            }

            this.Animator.speed = this.stopSpeed;
        }

        #endregion

        #region Help

        /// <summary>
        /// 获取动画时间
        /// </summary>
        /// <param name="motionType"></param>
        /// <returns></returns>
        public float GetAnimationTime(AnimatorMotionType motionType)
        {
            AnimationClip animationClip;

            if (this.animationClips.TryGetValue(motionType.ToString(), out animationClip))
            {
                throw new Exception($"找不到该动作:{motionType}");
            }

            return animationClip.length;
        }

        /// <summary>
        /// 是否包含参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool HasParameter(string parameter)
        {
            return this.Parameter.Contains(parameter);
        }

        /// <summary>
        /// 获取动画数量
        /// </summary>
        /// <returns></returns>
        public int GetAniClipCount()
        {
            return this.animationClips.Count;
        }

        #endregion

        #region Set Parmaeter

        public void SetBoolValue(string name, bool state)
        {
            if (!this.HasParameter(name))
            {
                return;
            }

            this.Animator.SetBool(name, state);
        }

        public void SetFloatValue(string name, float state)
        {
            if (!this.HasParameter(name))
            {
                return;
            }

            this.Animator.SetFloat(name, state);
        }

        public void SetIntValue(string name, int value)
        {
            if (!this.HasParameter(name))
            {
                return;
            }

            this.Animator.SetInteger(name, value);
        }

        public void SetTrigger(string name)
        {
            if (!this.HasParameter(name))
            {
                return;
            }

            this.Animator.SetTrigger(name);
        }

        public void SetAnimatorSpeed(float speed)
        {
            this.stopSpeed = this.Animator.speed;

            this.Animator.speed = speed;
        }

        public void ResetAnimatorSpeed()
        {
            this.Animator.speed = this.stopSpeed;
        }

        #endregion
        
        #region Dispose

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.animationClips = null;

            this.Parameter = null;

            this.Animator = null;
        }

        #endregion
    }
}
