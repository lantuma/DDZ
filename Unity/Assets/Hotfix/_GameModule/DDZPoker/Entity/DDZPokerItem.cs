/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 扑克 实体 }                                                                                                                   
*         【修改日期】{ 2019年4月3日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using ETModel;

namespace ETHotfix
{
    public sealed class DDZPokerItem : Entity
    {
        public int index { get; set; }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.index = 0;
        }
    }
}
