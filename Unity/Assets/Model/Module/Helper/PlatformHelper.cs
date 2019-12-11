/******************************************************************************************
*         【模块】{ 基础模块 }                                                                                                                      
*         【功能】{ 平台帮助类 }                                                                                                                   
*         【修改日期】{ 2019年11月5日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

namespace ETModel
{
    public static class PlatformHelper
    {
        /// <summary>
        /// 获取平台类型
        /// </summary>
        /// <returns></returns>
        public static string GetPlatformType()
        {
#if UNITY_ANDROID 
            return "Android";
#elif UNITY_IOS || UNITY_IPHONE
			return "IOS";
#elif UNITY_WEBGL
			return "WebGL";
#elif UNITY_STANDALONE_OSX
			return "MacOS";
#else
            return "PC";
#endif

        }

    }
}