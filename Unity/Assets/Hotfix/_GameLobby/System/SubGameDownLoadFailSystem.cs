/******************************************************************************************
*         【模块】{ 大厅模块 }                                                                                                                      
*         【功能】{ 子游戏加载失败 }                                                                                                                   
*         【修改日期】{ 2019年6月24日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [Event("SubGameDownLoadFail")]
    public class SubGameDownLoadFailSystem : AEvent<string>
    {
        public override void Run(string subGameName)
        {
            Game.PopupComponent.ShowTips(DataCenterComponent.Instance.tipInfo.SubGameLoadFailTip);
        }
    }
}