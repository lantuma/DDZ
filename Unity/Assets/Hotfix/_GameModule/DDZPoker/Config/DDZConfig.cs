/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 游戏配置类 }                                                                                                                   
*         【修改日期】{ 2019年4月2日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

using System.Collections.Generic;

namespace ETHotfix
{
    public static class DDZConfig
    {
        /// <summary>
        /// 斗地主游戏场景
        /// </summary>
        public static UIDDZGameScene GameScene { get; set; }

        /// <summary>
        /// 黑，红，梅，方(1-54)
        /// 暂时无用
        /// </summary>
        public static Dictionary<byte, int> PokerValueArray = new Dictionary<byte, int>
        {
            [0x32] = 1,//黑3

            [0x33] = 2,//黑4

            [0x34] = 3,//黑5

            [0x35] = 4,//黑6

            [0x36] = 5,//黑7

            [0x37] = 6,//黑8

            [0x38] = 7,//黑9

            [0x39] = 8,//黑10

            [0x3A] = 9,//黑J

            [0x3B] = 10,//黑Q

            [0x3C] = 11,//黑K

            [0x3D] = 12,//黑A

            [0x31] = 13,//黑2


            [0x22] = 14,//红3

            [0x23] = 15,//红4

            [0x24] = 16,//红5

            [0x25] = 17,//红6

            [0x26] = 18,//红7

            [0x27] = 19,//红8

            [0x28] = 20,//红9

            [0x29] = 21,//红10

            [0x2A] = 22,//红J

            [0x2B] = 23,//红Q

            [0x2C] = 24,//红K

            [0x2D] = 25,//红A

            [0x21] = 26,//红2


            [0x12] = 27,//梅3

            [0x13] = 28,//梅4

            [0x14] = 29,//梅5

            [0x15] = 30,//梅6

            [0x16] = 31,//梅7

            [0x17] = 32,//梅8

            [0x18] = 33,//梅9

            [0x19] = 34,//梅10

            [0x1A] = 35,//梅J

            [0x1B] = 36,//梅Q

            [0x1C] = 37,//梅K

            [0x1D] = 38,//梅A

            [0x11] = 39,//梅2



            [0x02] = 40,//方3

            [0x03] = 41,//方4

            [0x04] = 42,//方5

            [0x05] = 43,//方6

            [0x06] = 44,//方7

            [0x07] = 45,//方8

            [0x08] = 46,//方9

            [0x09] = 47,//方10

            [0x0A] = 48,//方J

            [0x0B] = 49,//方Q

            [0x0C] = 50,//方K

            [0x0D] = 51,//方A

            [0x01] = 52,//方2



            [0x4E] = 53,//小王

            [0x4F] = 54,//大王
        };
    }

    /// <summary>
    /// 斗地主游戏状态
    /// </summary>
    public enum DDZ_GameState
    {
        /// <summary>
        /// //未开局
        /// </summary>
        NoStart = 0,

        /// <summary>
        /// 准备
        /// </summary>
        Ready = 1,

        /// <summary>
        /// 发牌
        /// </summary>
        FaPai = 2,

        /// <summary>
        /// 叫地主
        /// </summary>
        CallScore = 3,

        /// <summary>
        /// 打牌
        /// </summary>
        DaPai = 4,

        /// <summary>
        /// 结算
        /// </summary>
        JieSuan = 5
    }

    /// <summary>
    /// 斗地主牌型
    /// </summary>
    public enum DDZ_POKER_TYPE
    {
        /// <summary>
        /// 过牌，不出
        /// </summary>
        DDZ_PASS = 0,
        /// <summary>
        /// //单张
        /// </summary>
        Single = 1,
        /// <summary>
        /// 对儿
        /// </summary>
        TWIN = 2,
        /// <summary>
        /// 数值相同的三张牌（如三个J）
        /// </summary>
        TRIPLE = 3,
        /// <summary>
        /// 数值相同的三张牌 + 一张单牌或一对牌。例如：333+6 或 444+99
        /// </summary>
        TRIPLE_WITH_SINGLE = 4,
        /// <summary>
        /// 三带二
        /// </summary>
        TRIPLE_WITH_TWIN = 5,
        /// <summary>
        /// 单顺   五张或更多的连续单牌（如：45678 或 78910JQK）。不包括 2 点和双王
        /// </summary>
        STRAIGHT_SINGLE = 6,
        /// <summary>
        /// 双顺  三对或更多的连续对牌（如：334455 、77 88 99 1010 JJ）。不包括 2 点和双王
        /// </summary>
        STRAIGHT_TWIN = 7,
        /// <summary>
        /// 飞机   二个或更多的连续三张牌（如：333444 、 555 666 777 888）。不包括 2 点和双王。
        /// </summary>
        PLANE_PURE = 8,
        /// <summary>
        /// 飞机带单
        /// </summary>
        PLANE_WITH_SINGLE = 9,
        /// <summary>
        /// 飞机带双
        /// </summary>
        PLANE_WITH_TWIN = 10,
        /// <summary>
        /// 四带两单
        /// </summary>
        FOUR_WITH_SINGLE = 11,
        /// <summary>
        /// 四带对
        /// </summary>
        FOUR_WITH_TWIN = 12,
        /// <summary>
        /// 炸弹
        /// </summary>
        FOUR_BOMB = 13,
        /// <summary>
        /// 火箭
        /// </summary>
        KING_BOMB = 14,               
    }
    /// <summary>
    /// 斗地主特效类型
    /// </summary>
    public enum DDZ_FX_TYPE
    {
        /// <summary>
        /// 炸弹
        /// </summary>
        Bomb = 0,
        /// <summary>
        /// 连队
        /// </summary>
        LianDui = 1,
        /// <summary>
        /// 飞机
        /// </summary>
        Plane = 2,
        /// <summary>
        /// 顺子
        /// </summary>
        ShunZi = 3,
        /// <summary>
        /// 春天
        /// </summary>
        Spring = 4,
        /// <summary>
        /// 王炸
        /// </summary>
        WangFire = 5,
        
        /// <summary>
        /// 地主赢
        /// </summary>
        LordWin = 6,

        /// <summary>
        /// 地主输
        /// </summary>
        LordLost = 7,

        /// <summary>
        /// 农民赢
        /// </summary>
        NMWin = 8,

        /// <summary>
        /// 农民输
        /// </summary>
        NMLost = 9
    }

}
