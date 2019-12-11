/******************************************************************************************
*         【模块】{ 框架扩展 }                                                                                                                      
*         【功能】{ Object扩展 }                                                                                                                   
*         【修改日期】{ 10月30日 }                                                                                                                        
*         【贡献者】{ BigDong }                                                                                                                
*                                                                                                                                        
******************************************************************************************/
using System;
using UnityEngine;

namespace EGCore
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 对象如果为Null，抛出异常
        /// </summary>
        /// <param name="o"></param>
        /// <param name="message">异常消息</param>
        public static void ThrowIfNull(this object o, string message)
        {
            if (o == null) throw new NullReferenceException(message);
        }

        public static T Get<T>(this GameObject gameObject, string key) where T : class
        {
            try
            {
                return gameObject.GetComponent<ReferenceCollector>().Get<T>(key);
            }
            catch (Exception e)
            {
                throw new Exception($"获取{gameObject.name}的ReferenceCollector key失败, key: {key}", e);
            }
        }
    }
}
