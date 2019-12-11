/******************************************************************************************
*         【模块】{ 场景管理模块 }                                                                                                                      
*         【功能】{ 场景基类（Actor-Load-DoSceneLoaded-OnLoaded-onStart-Update-Exit-OnEnd-Dispose） }                                                                                                                   
*         【修改日期】{ 2019年8月5日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 场景配置
    /// </summary>
    public class SceneConfig
    {
        /// <summary>
        /// 场景类型
        /// </summary>
        public int SceneType;

        /// <summary>
        /// 场景ID
        /// </summary>
        public int SceneId;

        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName;

        /// <summary>
        /// 场景目录
        /// </summary>
        public string SceneFolder;

        /// <summary>
        /// 加载状态
        /// </summary>
        public int SceneLoadState;

        /// <summary>
        /// 场景音乐
        /// </summary>
        public string SceneMusic;
    }

    /// <summary>
    /// 场景参数
    /// </summary>
    public class SceneParam
    {
        /// <summary>
        /// 过渡1
        /// </summary>
        public bool UseCutSceneTransition = false;

        /// <summary>
        /// 过渡2
        /// </summary>
        public bool useCivSceneTransition = false;
    }

    /// <summary>
    /// 场景基类
    /// </summary>
    public class SceneBase
    {
        private int SceneType;

        private int SceneId;

        private string SceneName;

        private string SceneFolder;

        private int SceneLoadState;

        private string SceneMusic;

        //场景根结点
        private Transform sceneRoot = null;

        //是否进入
        private bool isEnter = false;

        //场景参数
        private SceneParam sceneParam = null;

        //加载完成回调
        private Action onLoadComplete;

        //是否返回
        private bool isBack = false;

        public SceneBase(SceneConfig sceneConfig)
        {
            this.SceneType = sceneConfig.SceneType;

            this.SceneId = sceneConfig.SceneId;

            this.SceneName = sceneConfig.SceneName;

            this.SceneFolder = sceneConfig.SceneFolder;

            this.SceneLoadState = sceneConfig.SceneLoadState;

            this.SceneMusic = sceneConfig.SceneMusic;

            this.sceneRoot = null;//根节hko

            this.isEnter = false;

            this.sceneParam = null;//切换场景，外部传入的参数
        }

        /// <summary>
        ///加载场景
        /// </summary>
        /// <param name="param"></param>
        /// <param name="onComplete"></param>
        /// <param name="isback"></param>
        public void Load(SceneParam param, Action onComplete = null, bool isback = false)
        {
            this.sceneParam = param;

            this.onLoadComplete = onComplete;

            this.SceneLoadState = (int)SceneDefine.LoadState.None;

            this.isBack = isback;

            //调用加载
            SceneManagerComponent.instance.ChangeSceneAsync(this.SceneName, DoSceneLoader);
        }

        public void DoSceneLoader()
        {
            this.SceneLoadState = (int)SceneDefine.LoadState.LOADED;

            //场景UI进入
            SceneUIHelper.SceneEnter(this.SceneId, this.isBack);

            if (this.SceneMusic != "")
            {
                SoundComponent.Instance.PlayMusic(this.SceneMusic, 0, 1, true);
            }

            //场景加载完成，通知业务可以实例化对象了
            this.OnLoaded();

            //标记为进入
            this.Enter();

            if (this.onLoadComplete != null)
            {
                this.onLoadComplete();
            }
        }

        /// <summary>
        /// 场景加载完成，通知业务可以实例化对象了
        /// </summary>
        public void OnLoaded(SceneParam param = null)
        {
            Transform root = GameObject.Find("SceneRoot").transform;

            if (root != null)
            {
                //找到场景节点
                this.sceneRoot = root;

                //初始化UI信息，并保持相关引用
                SceneUIHelper.MainCamera = null;

                SceneUIHelper.SceneUIRoot = null;

                if (SceneUIHelper.SceneUIRoot != null)
                {
                    SceneUIHelper.SceneCanvas = SceneUIHelper.SceneUIRoot.GetComponent<Canvas>();
                }
            }
        }

        /// <summary>
        /// 进入
        /// </summary>
        public void Enter()
        {
            if (!this.isEnter)
            {
                this.isEnter = true;

                this.OnStart();
            }
        }

        /// <summary>
        /// 通知业务开始监听事件，打开界面等等
        /// </summary>
        public virtual void OnStart()
        {

        }

        public virtual void Update()
        {

        }

        /// <summary>
        /// 通知业务取消监听事件，关闭界面等
        /// </summary>
        public void Exit()
        {
            if (this.isEnter)
            {
                this.isEnter = false;

                this.OnEnd();

                this.OnExit();
            }
        }

        public virtual void OnEnd()
        {

        }

        /// <summary>
        /// 退出场景
        /// </summary>
        public virtual void OnExit()
        {

        }

        /// <summary>
        /// 场景销毁前调用，通知业务移除事件，删除对象
        /// </summary>
        public void Dispose()
        {
            this.SceneLoadState = (int)SceneDefine.LoadState.None;
        }

        /// <summary>
        /// 是否进入
        /// </summary>
        /// <returns></returns>
        public bool IsEnter() { return this.isEnter; }

        /// <summary>
        /// 重回前
        /// </summary>
        public void OnBeforeRelogin()
        {
            this.Exit();
        }

        /// <summary>
        /// 断线重联
        /// </summary>
        public void OnRelogin()
        {
            this.Enter();
        }

        /// <summary>
        /// 是否加载中
        /// </summary>
        /// <returns></returns>
        public bool IsLoading()
        {
            return this.SceneLoadState == (int)SceneDefine.LoadState.LOADING;
        }

        /// <summary>
        /// 获取场景ID
        /// </summary>
        /// <returns></returns>
        public int GetSceneID()
        {
            return this.SceneId;
        }

        /// <summary>
        /// 获取场景名称
        /// </summary>
        /// <returns></returns>
        public string GetSceneName()
        {
            return this.SceneName;
        }

        /// <summary>
        /// 获取场景类型
        /// </summary>
        /// <returns></returns>
        public int GetSceneType()
        {
            return this.SceneType;
        }
    }
}
