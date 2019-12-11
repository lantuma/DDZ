/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 帮助面板工厂 }                                                                                                                   
*         【修改日期】{ 2019年4月3日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

namespace ETHotfix
{

    public class DDZHelpPanelFactory
    {

        public  UI Create()
        {
            UI ui = DDZUIFactory.Create(UIType.UIDDZHelpPanel);
            
            ui.AddComponent<UIDDZHelpPanel>();

            return ui;
        }

        public  void Remove()
        {
            DDZUIFactory.Remove(UIType.UIDDZHelpPanel);
        }
    }
}