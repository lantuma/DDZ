namespace ETHotfix
{
    public class CardsTypeHelper
    {
        
    }

    public enum NnCardsType
    {
        None = 0,
        WuNiu = 1 << 0,
        Niu1 = 1 << 1,
        Niu2 = 1 << 2,
        Niu3 = 1 << 3,
        Niu4 = 1 << 4,
        Niu5 = 1 << 5,
        Niu6 = 1 << 6,
        Niu7 = 1 << 7,
        Niu8 = 1 << 8,
        Niu9 = 1 << 9,
        NiuNiu = 1 << 10,
        ZhadanNiu = 1 << 11,
        WuhuaNiu = 1 << 12,
        WuxiaoNiu = 1 << 13
    }

    /// <summary>
    /// 炸金花牌型
    /// </summary>
    public enum ZjhCardType
    {   //单张   对子     顺子       同花          同花顺        豹子      特殊
        Normal, Double, Sequence, EntireHua, EntireHuaSequence, Leopard, Special
    }

    /// <summary>
    /// 德州扑克牌型
    /// </summary>
    public enum TexasCardType
    {
        /// <summary>
        /// 高牌
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 一对
        /// </summary>
        Double = 1,
        /// <summary>
        /// 两对
        /// </summary>
        Double2 = 2,
        /// <summary>
        /// 三条
        /// </summary>
        Three = 3,
        /// <summary>
        /// 顺子
        /// </summary>
        Sequence = 4,
        /// <summary>
        /// 同花
        /// </summary>
        EntireHua = 5,
        /// <summary>
        /// 葫芦
        /// </summary>
        Gourd = 6,
        /// <summary>
        /// 四条
        /// </summary>
        Four = 7,
        /// <summary>
        /// 同花顺
        /// </summary>
        EntireHuaSequence = 8,
        /// <summary>
        /// 皇家同花顺
        /// </summary>
        RoyalEhSequence = 9
    }
}