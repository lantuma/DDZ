/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 扑克 对象工厂}                                                                                                                   
*         【修改日期】{ 2019年4月8日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using ETModel;

namespace ETHotfix
{
    public class DDZPokerItemFactory
    {
        public DDZPokerItem Create(int index,Component parent)
        {
            DDZPokerItem item = ComponentFactory.CreateWithParent<DDZPokerItem>(parent);

            item.index = index;

            item.AddComponent<DDZPokerItemUIComponent>();

            return item;
        }
    }
}
