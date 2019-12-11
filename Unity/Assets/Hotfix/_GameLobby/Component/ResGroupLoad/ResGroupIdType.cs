/******************************************************************************************
*         【模块】{ 资源组下载组件 }                                                                                                                      
*         【功能】{ 资源名称 }
*         【修改日期】{ 2019年7月22日 }                                                                                                                        
*         【贡献者】{ 周瑜｝                                                                                                               
*                                                                                                                                        
 ******************************************************************************************/

namespace ETHotfix
{
    public static partial class ResGroupIdType
    {
        /// <summary>
        /// 炸金花
        /// </summary>
        public static string[] ZhaJinHua = {"uizjhgamepanel","uizjhhelppanel","zjhatlas", "zhajinhuaatlas" };

        /// <summary>
        /// 百人牛牛
        /// </summary>
        public static string[] NiuNiu = { "uiniuniugamepanel","uiniuniuhelppanel","uiniuniusettingpanel","uiniuniutrendpanel","niuniuatlas" };//依赖了红黑,百家乐 "hongheitableatlas", "bjlgame",

        /// <summary>
        /// 百家乐
        /// </summary>
        public static string[] BaiJiaLe = { "bjlgame"};//没问题:木有依赖

        /// <summary>
        /// 德州扑克
        /// </summary>
        public static string[] DeZhouPuKe = {"uitexaspokerpanel","texaspokeratlas","texaspokerplayeritem", "uitexaspokerhelppanel" };

        /// <summary>
        /// 龙虎斗
        /// </summary>
        public static string[] LongHuDou = { "nhdgame", "longhudouatlas" };//依赖红黑  hongheitableatlas

        /// <summary>
        /// 红黑
        /// </summary>
        public static string[] HongHeiGame = { "hongheitableatlas","hongheiroadatlas","hongheiroad","hongheipanel","hongheihelp"};

        /// <summary>
        /// 斗地主
        /// </summary>
        public static string[] DouDiZhu = { "ddzgame", "ddzfx" };//没有依赖

        /// <summary>
        /// 水果机
        /// </summary>
        public static string[] ShuiGuoJi = { "uifruitmachinepanel","uifruitmachinehelppanel", "fruitmachineatlas" };//没有依赖

        /// <summary>
        /// 抢庄牛牛
        /// </summary>
        public static string[] QiangZhuangNiuNiu = { "qznngame" };//依赖了斗地主  ddzgame

    }
}
