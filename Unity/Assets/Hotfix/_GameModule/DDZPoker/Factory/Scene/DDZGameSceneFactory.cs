/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 游戏场景工厂 }                                                                                                                   
*         【修改日期】{ 2019年4月3日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

namespace ETHotfix
{

    public class DDZGameSceneFactory
    {

        public  UI Create()
        {
            UI ui = DDZUIFactory.Create(UIType.UIDDZGameScene);
            
            ui.AddComponent<UIDDZGameScene>();
            
            return ui;
        }
        
        public  void Remove()
        {
            DDZUIFactory.Remove(UIType.UIDDZGameScene);

            DDZUIFactory.Clear();
        }
    }
}
