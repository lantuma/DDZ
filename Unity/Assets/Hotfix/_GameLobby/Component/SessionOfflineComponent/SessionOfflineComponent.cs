/******************************************************************************************
*         【模块】{ 客户端断线处理组件 }                                                                                                                      
*         【功能】{ 用于Session断开时触发下线 }
*         【修改日期】{ 2019年5月21日 }                                                                                                                        
*         【贡献者】{ 周瑜                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

namespace ETHotfix
{
    public class SessionOfflineComponent : Component
    {
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
            Game.Scene.RemoveComponent<SessionComponent>();
            ETModel.Game.Scene.RemoveComponent<ETModel.SessionComponent>();
        }
    }
}