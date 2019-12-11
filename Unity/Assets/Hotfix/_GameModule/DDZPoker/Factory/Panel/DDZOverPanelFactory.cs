/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 结算面板工厂 }                                                                                                                   
*         【修改日期】{ 2019年4月3日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

namespace ETHotfix
{

    public class DDZOverPanelFactory
    {

        public  UI Create()
        {
            UI ui = DDZUIFactory.Create(UIType.UIDDZOverPanel);

            ui.AddComponent<UIDDZOverPanel>();

            return ui;
        }

        public  void Remove()
        {
            DDZUIFactory.Remove(UIType.UIDDZOverPanel);
        }
    }
}