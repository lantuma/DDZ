/******************************************************************************************
*         【模块】{ 基础模块 }                                                                                                                      
*         【功能】{ 点击播放特效组件 }                                                                                                                   
*         【修改日期】{ 2019年11月12日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class ClickEffectComponentAwakeSystem : AwakeSystem<ClickEffectComponent>
    {
        public override void Awake(ClickEffectComponent self)
        {
            self.Awake();
        }
    }

    public class ClickEffectComponent:Component 
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static ClickEffectComponent instance;
        
        public void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// 是否绑定
        /// </summary>
        public void Bind()
        {
            Camera.main.gameObject.AddComponent<ClickEffectHelper>();
        }

        /// <summary>
        /// 移除绑定
        /// </summary>
        public void Remove()
        {
            GameObject.Destroy(Camera.main.gameObject.GetComponent<ClickEffectHelper>());
        }
        
        /// <summary>
        /// 是否生效
        /// </summary>
        /// <param name="flag"></param>
        public void SetEnable(bool flag = true)
        {
            Camera.main.gameObject.GetComponent<ClickEffectHelper>().enabled = flag;
        }
        
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
        }
    }
}
