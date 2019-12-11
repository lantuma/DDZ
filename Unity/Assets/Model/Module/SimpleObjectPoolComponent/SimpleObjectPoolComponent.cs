/******************************************************************************************
*         【模块】{ 基础组件 }                                                                                                                      
*         【功能】{ 简单对象池组件，减少GC }                                                                                                                   
*         【修改日期】{3月29日 }                                                                                                                        
*         【贡献者】{ BigDong,周瑜（修改） }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETModel
{
    public class ObjPool
    {
        public Queue<GameObject> poolQuque;

        public string _poolName;

        public string _abName;

        public string _assetName;

        public GameObject assetObject;

        private Transform PoolParent;

        /// <summary>
        /// 初始化对象池容器
        /// </summary>
        /// <param name="poolName"></param>
        /// <param name="abName"></param>
        /// <param name="assetName"></param>
        /// <param name="capacity"></param>
        public ObjPool(string poolName, string abName, string assetName, int capacity)
        {
            _poolName = poolName;

            _abName = abName;

            _assetName = assetName;

            poolQuque = new Queue<GameObject>(capacity);

            PoolParent = GameObject.Find("ObjectPool").transform;

            ResourcesComponent res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();

            res.LoadBundle(abName.StringToAB());

            assetObject = (GameObject)res.GetAsset(abName.StringToAB(), assetName);

            for (int i = 0; i < capacity; i++)
            {
                GameObject objClone = (GameObject)UnityEngine.Object.Instantiate(assetObject);

                objClone.SetActive(false);

                objClone.transform.SetParent(PoolParent);

                poolQuque.Enqueue(objClone);
            }

        }

        /// <summary>
        /// 获得游戏资源对象
        /// </summary>
        /// <returns></returns>
        public GameObject Get()
        {
            if (poolQuque.Count <= 0)
                poolQuque.Enqueue((GameObject)UnityEngine.Object.Instantiate(assetObject));

            return poolQuque.Dequeue();
        }

        /// <summary>
        /// 回收游戏资源
        /// </summary>
        /// <param name="assetObject"></param>
        public void Recycle(GameObject assetObject)
        {
            assetObject.SetActive(false);

            assetObject.transform.SetParent(PoolParent);

            poolQuque.Enqueue(assetObject);
        }

        public void CleanThePool()
        {
            for (int i = poolQuque.Count - 1; i > 0; i--)
            {
                GameObject.Destroy(poolQuque.Dequeue());
            }

            poolQuque.Clear();

            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(_abName);
        }
    }

    [ObjectSystem]
    public class SimpleObjectPoolComponentAwakeSystem : AwakeSystem<SimpleObjectPoolComponent>
    {
        public override void Awake(SimpleObjectPoolComponent self)
        {
            self.Awake();
        }
    }

    public class SimpleObjectPoolComponent : Component
    {
        private Dictionary<string, ObjPool> objectPoolDic;

        public void Awake()
        {
            objectPoolDic = new Dictionary<string, ObjPool>();

            var poolParent = new GameObject("ObjectPool");

            GameObject.DontDestroyOnLoad(poolParent);
        }

        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <param name="poolName"></param>
        /// <param name="abName"></param>
        /// <param name="assetName"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public ObjPool GreateObjectPool(string poolName, string abName, string assetName, int capacity)
        {
            if (!objectPoolDic.ContainsKey(poolName))
            {
                ObjPool objectPool = new ObjPool(poolName, abName, assetName, capacity);

                objectPoolDic.Add(poolName, objectPool);
            }

            return objectPoolDic[poolName];
        }

        /// <summary>
        /// 移除指定对象池
        /// </summary>
        /// <param name="poolName"></param>
        public void RemoveObjectPool(string poolName)
        {
            if (!objectPoolDic.ContainsKey(poolName)) return;

            objectPoolDic[poolName].CleanThePool();

            objectPoolDic.Remove(poolName);
        }

        public void CleanTheObjectPoolDic()
        {
            List<string> tempList = new List<string>();

            foreach (var pool in objectPoolDic)
            {
                tempList.Add(pool.Key);
            }

            for (int i = objectPoolDic.Count - 1; i > 0; i--)
            {
                var poolName = tempList[i];

                objectPoolDic[poolName].CleanThePool();

                objectPoolDic.Remove(poolName);
            }

            objectPoolDic.Clear();

            tempList = null;
        }
    }
}
