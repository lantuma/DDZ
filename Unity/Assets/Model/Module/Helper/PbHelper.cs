/******************************************************************************************
*         【模块】{ 基础模块 }                                                                                                                      
*         【功能】{ Pb帮助类 }                                                                                                                   
*         【修改日期】{ 2019年6月2日 }                                                                                                                        
*         【贡献者】{ 周瑜(整合) }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections.Generic;
using Google.Protobuf;

namespace ETModel
{
    public static class PbHelper
    {
        public static ByteString CopyFrom(List<byte> list)
        {
           return  ByteString.CopyFrom(list.ToArray());
        }
        
    }
}