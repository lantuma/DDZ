 using System;
    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using DG.Tweening;
    using System.Runtime.CompilerServices;

public static class GameObjectExtension
{
    public static T GetComponentInChildrenEx<T>(this GameObject thiz)
    {
        if (thiz.activeInHierarchy)
        {
            var component = thiz.GetComponent<T>();
            if (component != null)
            {
                return component;
            }
        }
        var thizTransform = thiz.transform;
        if (thizTransform != null)
        {
            for (int i = 0; i < thizTransform.childCount; ++i)
            {
                var subTrans = thizTransform.GetChild(i);
                var componentInChildren = subTrans.gameObject.GetComponentInChildrenEx<T>();
                if (componentInChildren != null)
                {
                    return componentInChildren;
                }
            }
        }
        return default(T);
    }

    public static T GetComponentInParentEx<T>(this GameObject thiz)
    {
        if (thiz.activeInHierarchy)
        {
            var component = thiz.GetComponent<T>();
            if (component != null)
            {
                return component;
            }
        }
        Transform parent = thiz.transform.parent;
        if (parent != null)
        {
            while (parent != null)
            {
                if (parent.gameObject.activeInHierarchy)
                {
                    var component2 = parent.gameObject.GetComponent<T>();
                    if (component2 != null)
                    {
                        return component2;
                    }
                }
                parent = parent.parent;
            }
        }
        return default(T);
    }

    public static GameObject SetActiveEx(this GameObject thiz, bool value)
    {
        if (thiz.activeSelf != value)
        {
            thiz.SetActive(value);
        }

        return thiz;
    }

    public static T Get<T>(this GameObject thiz) where T : Component
    {
        T t = thiz.GetComponent<T>();
        if (t == default(T))
            t = thiz.AddComponent<T>();

        return t;
    }



    public static T Get<T>(this GameObject thiz, int childIdx) where T : Component
    {
        return thiz.gameObject.transform.GetChild(childIdx).Get<T>();
    }

    public static T Get<T>(this Transform thiz) where T : Component
    {
        return thiz.gameObject.Get<T>();
    }

    /// <summary>
    /// 搜索子物体组件-Transform版
    /// </summary>
    public static T TryGet<T>(this GameObject thiz, string subnode = null) where T : Component
    {
        if (thiz == null)
            return null;

        if (subnode == null)
            return thiz.GetComponent<T>();

        Transform sub = thiz.transform.Find(subnode);
        if (sub != null)
            return sub.GetComponent<T>();
        return null;
    }

    //public static T TryGetBeginWith<T>(this GameObject thiz, string name) where T : Component
    //{
    //    T t = thiz.TryGet<T>();
    //    if (t != default(T) && t.name.StartsWith(name))
    //    {
    //        return t;
    //    }

    //    var num = thiz.transform.childCount;
    //    Transform sub = null;
    //    for (int i = 0; i < num; ++i)
    //    {
    //        sub = thiz.transform.GetChild(i);
    //        if (sub.name.StartsWith(name))
    //            return sub.GetComponent<T>();
    //    }

    //    return t;
    //}

    /// <summary>
    /// 深度查找子级对象---深层次
    /// </summary>
    /// <returns>The component by name in child.</returns>
    /// <param name="thiz">Thiz.</param>
    /// <param name="name">Name.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static T TryGetInChilds<T>(this GameObject thiz, string name) where T : Component
    {
        T[] comps = thiz.GetComponentsInChildren<T>(true);
        int count = comps.Length;
        for (int i = 0; i < count; i++)
        {
            if (comps[i].gameObject.name.Equals(name))
            {
                return comps[i];
            }
        }
        Debug.LogError($"{thiz.name} 子级下面未找到名称为 {name} 的对象。");
        return null;
    }

    public static GameObject SetSubActive(this GameObject thiz, string subnode, bool active)
    {
        var node = thiz.TryGet<Transform>(subnode);
        if (node != null)
        {
            node.gameObject.SetActive(active);
        }
        return thiz;
    }

    public static void RemoveCollider(this GameObject thiz)
    {
        var c = thiz.TryGet<Collider>();
        if (c != null)
        {
            GameObject.DestroyImmediate(c);
            c = null;
        }
    }

    /// <summary>
    /// Parent the specified thiz.
    /// </summary>
    /// <param name="thiz">Thiz.</param>
    public static GameObject Parent(this GameObject thiz)
    {
        return thiz.transform.parent.gameObject;
    }

    /// <summary>
    /// Optimizes the animator.
    /// </summary>
    /// <returns>The animator.</returns>
    /// <param name="thiz">Thiz.</param>
    public static GameObject OptimizeAnimator(this GameObject thiz)
    {
        var animators = thiz.GetComponentsInChildren<Animator>();
        //            Debug.LogError("animators--->" + animators.Length);
        for (int i = 0; i < animators.Length; ++i)
        {
            var ani = animators[i];
            if (ani != null && ani.isOptimizable)
            {
                //                    Debug.LogError("xxxxxxxxx");
                ani.logWarnings = false;
                AnimatorUtility.OptimizeTransformHierarchy(ani.gameObject, null);
            }
        }

        return thiz;
    }

    /// <summary>
    /// Removes the animator.
    /// </summary>
    /// <returns>The animator.</returns>
    /// <param name="thiz">Thiz.</param>
    public static GameObject RemoveAnimator(this GameObject thiz)
    {
        var animators = thiz.GetComponentsInChildren<Animator>();
        for (int i = 0; i < animators.Length; ++i)
        {
            var ani = animators[i];
            if (ani != null)
            {
                GameObject.DestroyImmediate(ani);
            }
        }
        return thiz;
    }


    /// <summary>
    /// 播放动作
    /// </summary>
    /// <returns>The animator.</returns>
    /// <param name="thiz">Thiz.</param>
    /// <param name="statename">Statename.</param>
    public static GameObject PlayAnimator(this GameObject thiz, string statename)
    {
        var animators = thiz.GetComponentsInChildren<Animator>();
        //            float currTime = animators [0].GetCurrentAnimatorStateInfo;
        //            AnimatorStateInfo xx=animators [0].GetCurrentAnimatorStateInfo(0);
        //            float currTime = xx.normalizedTime;
        //            Debug.LogError("currTime--->" + currTime);
        for (int i = 0; i < animators.Length; ++i)
        {
            var ani = animators[i];
            if (ani != null)
            {
                ani.Play(statename, 0, 0f);
                ani.Update(0f);


                // animator to start
                //                    if(ani.GetCurrentAnimationClipState(0).)

                //                    ani.GetCurrentAnimatorStateInfo("0").IsName("statename");
                //                    MMLog.Debug("Object:{0} Will Play Animator:{1}", ani.name, statename);

                //                    ani.CrossFade(statename,0f,0,currTime);
            }
        }
        return thiz;
    }

    public static AnimationClip GetCurrentPlayingAnimation(this Animation thiz)
    {
        foreach (AnimationState state in thiz)
        {
            if (state != null)
            {
                if (thiz.IsPlaying(state.name))
                    return state.clip;
            }
        }

        return null;
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <returns>The Animation.</returns>
    /// <param name="thiz">Thiz.</param>
    /// <param name="statename">Statename.</param>
    public static GameObject PlaySubAnimation(this GameObject thiz, string animation, float speed = 1f)
    {
#if !UNITY_EDITOR
            if (thiz == null)
                return thiz;
#endif

        var animations = thiz.GetComponentsInChildren<Animation>();
        for (int i = 0; i < animations.Length; ++i)
        {
            var ani = animations[i];
            if (ani != null)
            {
                // animator to start
                var state = ani[animation];
                if (state != null)
                {
                    var clip = ani.GetCurrentPlayingAnimation();
                    if (clip != null && clip.name == state.name)
                        break;

                    state.speed = speed;
                    ani.Play(animation, PlayMode.StopAll);

                    if (clip != null && clip.wrapMode == WrapMode.Loop)
                    {
                        var queuestate = ani.PlayQueued(clip.name, QueueMode.CompleteOthers, PlayMode.StopSameLayer);
                        queuestate.speed = ani[clip.name].speed;
                    }
                    break;
                }

            }
        }
        return thiz;
    }

    public static GameObject StopSubAnimation(this GameObject thiz)
    {
        if (thiz == null)
            return null;

        var animations = thiz.GetComponentsInChildren<Animation>();
        for (int i = 0; i < animations.Length; ++i)
        {
            var ani = animations[i];
            if (ani != null)
            {
                // animator to start
                ani.Rewind();
                ani.Sample();
                ani.Stop();
                break;
            }
        }
        return thiz;
    }


    /// <summary>
    /// 设置层级
    /// </summary>
    /// <param name="thiz">Thiz.</param>
    /// <param name="layer">Layer.</param>
    public static void SetLayerRecursive(this GameObject thiz, int layer, int depth = -1)
    {
        if (depth == 0)
            return;

        thiz.layer = layer;
        Transform trans = thiz.transform;

        var num = trans.childCount;
        Transform child = null;
        for (int i = 0; i < num; ++i)
        {
            child = trans.GetChild(i);
            SetLayerRecursive(child.gameObject, layer, depth - 1);
        }
    }

    /// <summary>
    /// Sets the tag recursive.
    /// </summary>
    /// <param name="thiz">Thiz.</param>
    /// <param name="tag">Tag.</param>
    public static void SetTagRecursive(this GameObject thiz, string tag)
    {
        thiz.tag = tag;
        Transform trans = thiz.transform;

        var num = trans.childCount;
        Transform child = null;
        for (int i = 0; i < num; ++i)
        {
            child = trans.GetChild(i);
            SetTagRecursive(child.gameObject, tag);
        }
    }

    /// <summary>
    /// 射线检查按下的点是否在对象上
    /// </summary>
    /// <returns><c>true</c> if is point on game object the specified thiz; otherwise, <c>false</c>.</returns>
    /// <param name="thiz">Thiz.</param>
    public static bool IsPointOnGameObject(this GameObject thiz)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
        {
            return hit.transform.gameObject == thiz;
        }
        return false;
    }

    /// <summary>
    /// Sets the sub renders visible.
    /// </summary>
    /// <returns>The sub renders visible.</returns>
    /// <param name="thiz">Thiz.</param>
    /// <param name="visible">If set to <c>true</c> visible.</param>
    public static GameObject SetSubRendersVisible(this GameObject thiz, bool visible)
    {
        if (thiz == null)
            return null;

        var renderers = thiz.GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < renderers.Length; ++i)
        {
            if (renderers[i] != null)
                renderers[i].enabled = visible;
        }
        return thiz;
    }

    public static bool IsSubRenderVisible(this GameObject thiz)
    {
        var render = thiz.GetComponentInChildren<Renderer>();
        if (render != null)
            return render.isVisible;
        return false;
    }

}

