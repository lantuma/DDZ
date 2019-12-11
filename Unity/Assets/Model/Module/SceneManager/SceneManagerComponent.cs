/******************************************************************************************
*         【模块】{ 场景管理模块 }                                                                                                                      
*         【功能】{ 管理场景类，用于维护场景生命周期 }                                                                                                                   
*         【修改日期】{ 2019年8月5日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class SceneManagerComponentAwakeSystem : AwakeSystem<SceneManagerComponent>
    {
        public override void Awake(SceneManagerComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class SceneManagerComponentUpdateSystem : UpdateSystem<SceneManagerComponent>
    {
        public override void Update(SceneManagerComponent self)
        {
            self.Update();
        }
    }

    public class SceneManagerComponent : Component
    {
        /// <summary>
        /// 当前场景
        /// </summary>
        public SceneBase currScene;

        /// <summary>
        /// 背景场景
        /// </summary>
        public SceneBase bgScene;

        /// <summary>
        /// 场景堆栈
        /// </summary>
        public List<int> sceneBackStack;

        /// <summary>
        /// 上一个场景ID
        /// </summary>
        public int lastSceenID = -1;

        /// <summary>
        /// 单例
        /// </summary>
        public static SceneManagerComponent instance;

        public void Awake()
        {
            this.currScene = null;

            sceneBackStack = new List<int>();

            //监听各种事件消息，触发各类方法，如重回成功 GAME_RELOGIN_FINISH

            //this.OnRelogin();

            instance = this;
        }

        /// <summary>
        /// 正在加载场景
        /// </summary>
        /// <returns></returns>
        public bool IsLoading()
        {
            if (this.currScene == null)
            {
                return false;
            }

            return this.currScene.IsLoading();
        }

        /// <summary>
        /// 获取场景类型
        /// </summary>
        /// <returns></returns>
        public int GetSceneID()
        {
            if (this.currScene == null)
            {
                return -1;
            }

            return this.currScene.GetSceneID();
        }

        /// <summary>
        /// 场景循环
        /// </summary>
        public void Update()
        {
            if (this.currScene != null && this.currScene.IsEnter())
            {
                this.currScene.Update();
            }
        }

        /// <summary>
        /// 重回
        /// </summary>
        public void OnRelogin()
        {
            if (this.currScene != null)
            {
                this.currScene.OnRelogin();
            }
        }

        /// <summary>
        /// 返回上个场景
        /// </summary>
        /// <param name="param"></param>
        /// <param name="onloaded"></param>
        public void Back(SceneParam param, Action onloaded)
        {
            if (this.lastSceenID != -1)
            {
                this.ChangeScene(this.lastSceenID, param, onloaded, false, true);
            }
        }

        /// <summary>
        /// 返回上次记录的场景，如果上次记录为空，则返回上个场景
        /// </summary>
        /// <param name="param"></param>
        /// <param name="onloaded"></param>
        public void PopSceneStack(SceneParam param, Action onloaded)
        {
            int count = this.sceneBackStack.Count;

            if (count > 0)
            {
                int sceneId = this.sceneBackStack[count - 1];

                this.sceneBackStack.RemoveAt(count - 1);

                this.ChangeScene(sceneId, param, onloaded, false, true);
            }
            else
            {
                this.Back(param, onloaded);
            }
        }

        /// <summary>
        /// 清空场景记录
        /// </summary>
        public void ClearSceneStack()
        {
            this.sceneBackStack.Clear();
        }

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="sceneID"></param>
        /// <param name="param"></param>
        /// <param name="onloaded"></param>
        /// <param name="isback"></param>
        /// <param name="addSceneBackStack"></param>
        public void ChangeScene(int sceneID, SceneParam param, Action onloaded, bool sceneUIPush, bool isback, bool addSceneBackStack = false)
        {
            int currSceneID = this.GetSceneID();

            if (currSceneID == sceneID)
            {
                Debug.Log("ChangeScene current scene is the target....sceneID:" + sceneID);

                return;
            }

            if (this.IsLoading())
            {
                Debug.Log("ChangeScene current scene is loading...." + currSceneID + "  " + sceneID);

                return;
            }

            if (currSceneID != -1)
            {
                Debug.Log("StopMusic  " + currSceneID);

                SoundComponent.Instance.Stop("bgSound");
            }

            this.LoadScene(sceneID, param, onloaded, sceneUIPush, isback, addSceneBackStack);
        }

        /// <summary>
        /// 叠加场景
        /// </summary>
        /// <param name="sceneID"></param>
        /// <param name="param"></param>
        public void LoadAdditiveScene(int sceneID, SceneParam param)
        {
            SceneBase newScene = this.CreateScene(sceneID);

            if (newScene == null)
            {
                return;
            }

            newScene.Load(param);

            this.bgScene = newScene;
        }

        /// <summary>
        /// 聚焦新场景
        /// </summary>
        public void FocusBgScene()
        {
            this.ExitCurrent();

            this.currScene = this.bgScene;

            SceneUIHelper.MainScene = this.currScene;
        }

        /// <summary>
        /// 根据场景ID加载新的场景
        /// </summary>
        /// <param name="sceneID"></param>
        /// <param name="param"></param>
        /// <param name="onloaded"></param>
        /// <param name="isback"></param>
        /// <param name="sceneBackStackPush"></param>
        private void LoadScene(int sceneID, SceneParam param, Action onloaded, bool sceneUIPush, bool isback, bool sceneBackStackPush)
        {
            //新建下一个场景
            SceneBase newScene = this.CreateScene(sceneID);

            if (newScene == null)
            {
                return;
            }

            int lastSceneId = this.GetSceneID();

            if (sceneBackStackPush)
            {
                this.sceneBackStack.Add(lastSceneId);
            }

            //缷载旧的场景

            this.lastSceenID = lastSceneId;

            SceneBase lastScene = this.currScene;

            if (lastScene != null)
            {
                string lastSceneName = lastScene.GetSceneName();

                int lastSceneType = lastScene.GetSceneType();

                //退出当前场景并通知UI关闭当前场景UI
                this.ExitCurrent(sceneUIPush);

                //清理当前场景缓存的对象，终止正在加载的队列
                if (lastSceneType != newScene.GetSceneType())
                {
                    SceneUIHelper.UnloadSceneAB(lastSceneName, false);
                }

                SceneUIHelper.MainScene = null;

                SceneUIHelper.MainCamera = null;

                SceneUIHelper.SceneUIRoot = null;

                //除了登陆场景，其他场景切换均有场景过渡

                if (lastSceneType != (int)SceneDefine.SceneType.LOGIN)
                {
                    //如果参数里标记了使用CUTSCENE过渡，那么不要打开这个普通过渡 界面

                    bool useNormalTransition = true;

                    if (param != null)
                    {
                        if (param.UseCutSceneTransition)
                        {
                            useNormalTransition = false;
                        }
                        else if (param.useCivSceneTransition)
                        {
                            useNormalTransition = false;

                            //打开一个加载界面
                        }
                    }

                    if (useNormalTransition)
                    {
                        //打开正常的加载过渡界面
                    }
                }
            }

            this.currScene = newScene;

            //通知开始加载新场景

            SceneUIHelper.MainScene = newScene;

            newScene.Load(param, onloaded, isback);

            //发送场景切换事件
        }

        /// <summary>
        /// 生成一个SceneBase
        /// </summary>
        /// <param name="sceneID"></param>
        /// <returns></returns>
        public SceneBase CreateScene(int sceneID)
        {
            SceneConfig sceneConfig = SceneDefine.SceneConfig[sceneID];

            if (sceneConfig == null)
            {
                Debug.Log("can not find scene config...SceneID:" + sceneID);

                return null;
            }

            //新场景加载前的准备（应该根据类型实例化对应的场景类，需要反射调用）
            SceneBase newScene = new SceneBase(sceneConfig);

            return newScene;
        }

        /// <summary>
        /// 退出当前场景
        /// </summary>
        /// <param name="sceneUIPush"></param>
        public void ExitCurrent(bool sceneUIPush = false)
        {
            if (this.currScene == null)
            {
                return;
            }

            int sceneID = this.currScene.GetSceneID();

            SceneUIHelper.SceneExit(sceneID, sceneUIPush);

            this.currScene.Exit();

            this.currScene.Dispose();

            this.currScene = null;
        }

        /// <summary>
        /// 停止当前逻辑
        /// </summary>
        public void OnBeforeRelogin()
        {
            if (this.currScene == null)
            {
                return;
            }

            this.currScene.OnBeforeRelogin();
        }

        #region Helper

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="DoSceneLoaded"></param>
        public async void ChangeSceneAsync(string sceneName, Action DoSceneLoaded)
        {
            using (SceneChangeComponent sceneChangeComponent = ETModel.Game.Scene.AddComponent<SceneChangeComponent>())
            {
                await sceneChangeComponent.ChangeSceneAsync(sceneName);

                if (DoSceneLoaded != null)
                {
                    DoSceneLoaded();
                }
            }
        }

        #endregion
    }
}
