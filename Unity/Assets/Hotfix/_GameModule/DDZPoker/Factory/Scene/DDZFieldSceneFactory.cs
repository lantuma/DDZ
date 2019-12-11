/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 游戏选场场景工厂 }                                                                                                                   
*         【修改日期】{ 2019年9月7日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

namespace ETHotfix
{

    public class DDZFieldSceneFactory
    {

        public UI Create()
        {
            UI ui = DDZUIFactory.Create(UIType.UIDDZFieldScene);

            ui.AddComponent<UIDDZFieldScene>();

            return ui;
        }

        public void Remove()
        {
            DDZUIFactory.Remove(UIType.UIDDZFieldScene);

            DDZUIFactory.Clear();
        }
    }
}
