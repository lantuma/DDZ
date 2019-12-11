/******************************************************************************************
*         【模块】{ 基础模块 }                                                                                                                      
*         【功能】{ 点击播放特效帮助类 }                                                                                                                   
*         【修改日期】{ 2019年11月12日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    public class ClickEffectHelper : MonoBehaviour
    {
        /// <summary>
        /// 特效预置体
        /// </summary>
        private GameObject _clickEffect;

        /// <summary>
        /// 特效生成层级对象
        /// </summary>
        private Transform _parentLayer;

        /// <summary>
        /// 特效缓存
        /// </summary>
        private List<GameObject> list = new List<GameObject>();

        /// <summary>
        /// 消失时间
        /// </summary>
        private float _fadeOutTime = 1f;

        private GameObject clickEffect
        {
            get
            {
                if (_clickEffect == null)
                {
                    _clickEffect = (GameObject)ResourcesHelper.Load("Effects/ClickEffect");

                    _clickEffect.SetActive(false);
                }

                return _clickEffect;
            }

        }

        private Transform parentLayer
        {
            get
            {
                if (_parentLayer == null)
                {
                    _parentLayer = GameObject.Find("Topmost").transform;
                }

                return _parentLayer;
            }
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Play(Input.mousePosition);
            }
        }

        private void Play(Vector3 pos)
        {
            var go = Rent();

            if (go != null)
            {
                go.transform.localPosition = CurrMousePosition(go.transform, pos);

                go.SetActive(true);
                
                //Return(go);
            }
            
        }

        /// <summary>
        /// 获取真正坐标
        /// </summary>
        /// <param name="go"></param>
        /// <param name="mousePosition"></param>
        /// <returns></returns>
        private Vector2 CurrMousePosition(Transform go,Vector3 mousePosition)
        {
            Vector2 vecMouse;

            RectTransform parentRectTrans = go.GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTrans, mousePosition, Camera.main, out vecMouse);

            return vecMouse;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        private GameObject Rent()
        {
            if (list.Count <= 0)
            {
                GameObject effect = GameObject.Instantiate(clickEffect,this.parentLayer) as GameObject;
                
                return effect;
            }
            else
            {
                var effect = list[0];

                effect.SetActive(true);

                list.RemoveAt(0);

                return effect;
            }
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="go"></param>
        private void Return(GameObject go)
        {
            go.SetActive(false);

            list.Add(go);
        }

    }
}
