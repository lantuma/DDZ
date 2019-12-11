/******************************************************************************************
*         【模块】{ 大厅模块 }                                                                                                                      
*         【功能】{ 子游戏对象}                                                                                                                   
*         【修改日期】{ 2019年7月22日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using ETModel;

namespace ETHotfix
{
    public sealed class SubGame : Entity
    {
        public long GameID { get; set; }

        public int Index { get; set; }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.GameID = 0;

            this.Index = -1;
        }
    }
}
