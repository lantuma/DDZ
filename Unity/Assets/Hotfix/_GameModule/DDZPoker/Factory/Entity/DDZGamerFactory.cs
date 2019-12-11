/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 创建玩家对象工厂}                                                                                                                   
*         【修改日期】{ 2019年4月3日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using ETModel;

namespace ETHotfix
{
   public  class DDZGamerFactory
    {
        public DDZGamer Create(long userId,int seatId,Component parent)
        {
            DDZGamer gamer = ComponentFactory.CreateWithParent<DDZGamer>(parent);

            gamer.UserID = userId;

            gamer.SeatID = seatId;

            gamer.AddComponent<DDZGamerUIComponent>();

            return gamer;
        }
    }
}
