/******************************************************************************************
*         【模块】{ 大厅模块 }                                                                                                                      
*         【功能】{ 创建子游戏对象工厂}                                                                                                                   
*         【修改日期】{ 2019年7月22日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using ETModel;

namespace ETHotfix
{
    public class SubGameFactory
    {
        public static SubGame Create(long GameID, int Index, Component parent)
        {
            SubGame subGame = ComponentFactory.CreateWithParent<SubGame>(parent);

            subGame.GameID = GameID;

            subGame.Index = Index;

            subGame.AddComponent<SubGameComponent>();

            return subGame;
        }
    }
}
