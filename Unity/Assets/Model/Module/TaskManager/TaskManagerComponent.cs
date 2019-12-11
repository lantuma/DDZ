/******************************************************************************************
*         【模块】{ 任务管理模块 }                                                                                                                      
*         【功能】{ 替代协程管理 }                                                                                                                   
*         【修改日期】{ 2019年8月5日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class TaskManagerComponentAwakeSystem : AwakeSystem<TaskManagerComponent>
    {
        public override void Awake(TaskManagerComponent self)
        {
            self.Awake();
        }
    }

    public class TaskManagerComponent : Component
    {
        public static TaskManagerComponent instance;

        public void Awake()
        {
            TaskManager.singleton = Camera.main.gameObject.AddComponent<TaskManager>();

            instance = this;
        }

        public ETask CreateTask(IEnumerator c, bool autoStart = true)
        {
            ETask task = new ETask(c, autoStart);

            return task;
        }

        ///////////////////////////////////示例///////////////////////////////////////////////

        IEnumerator MyAwesomeTask()
        {
            while (true)
            {
                Debug.Log("Logcat is in console");

                yield return null;
            }
        }

        IEnumerator TaskKiller(float delay, ETask t)
        {
            yield return new WaitForSeconds(delay);

            t.Stop();
        }

        public void Test()
        {
            ETask spam = TaskManagerComponent.instance.CreateTask(MyAwesomeTask());

            new ETask(TaskKiller(5, spam));
        }
    }
}
