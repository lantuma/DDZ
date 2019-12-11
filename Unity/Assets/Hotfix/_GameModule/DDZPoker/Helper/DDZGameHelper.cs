/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 游戏帮助类 }                                                                                                                   
*         【修改日期】{ 2019年4月3日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class DDZGameHelper
    {
        /// <summary>
        /// 当前游戏信息
        /// </summary>
        public static GameInfo CurrentGameInfo;
        
        /// <summary>
        /// 是否获得房间详情
        /// </summary>
        public static bool IsGetRoomDetails = false;

        /// <summary>
        /// 是否已加入房间
        /// </summary>
        public static bool IsJoinRoom = false;
        
        /// <summary>
        /// 当前场ID
        /// </summary>
        public static int CurrentFieldId = 0;
        
        /// <summary>
        /// 当前房间ID
        /// </summary>
        public static int RoomId = 0;

        /// <summary>
        /// 结算数据
        /// </summary>
        public static Actor_DDZhSettlement_Req_Ntt settle;

        /// <summary>
        /// 是否游戏已局
        /// </summary>
        public static bool IsStartGame = false;

        /// <summary>
        /// 重置
        /// </summary>
        public static void Reset()
        {
            CurrentFieldId = 0;

            IsJoinRoom = false;
            
            settle = null;
        }

        /// <summary>
        /// 转换座位号,将seatID转变成桌子固定的位置：逆时针计算，自己的位置是0,右1，左2
        /// </summary>
        /// <param name="seatID">座位号</param>
        /// <returns>固定位置</returns>
        public static int ChangeSeat(int seatID)
        {
            if (seatID < 0) { return seatID; }

            var myVO = DataCenterComponent.Instance.userInfo.getMyUserVo();

            if (myVO == null) { return seatID; }

            var _mySeatID = myVO.seatID;//假如我是2

            int _pos = seatID - _mySeatID;

            if (_pos < 0)
            {
                int _pos2 = Math.Abs(Math.Abs(_pos) - 3);

                return _pos2;
            }
            else
            {
                return _pos;
            }
        }

        /// <summary>
        /// int 比较
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int CompareTo(int a, int b)
        {
            int result;

            if (a > b)
            {
                result = 1;
            }
            else if (a < b)
            {
                result = -1;
            }
            else
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// byte列表转int列表(牌的Value)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<int> CardFromByteToValue(List<byte> list)
        {
            List<int> intList = new List<int>();

            for (int i = 0; i < list.Count; i++)
            {
                intList.Add(DDZGameHelper.GetPokerNum(list[i]));
            }

            return intList;
        }

        /// <summary>
        /// 出牌排序
        /// </summary>
        /// <param name="outCards"></param>

        public static void SortOutCards(byte[] outCards)
        {
            

        }

        /// <summary>
        /// 获取斗地主真实的牌值
        /// </summary>
        /// <param name="iPoker"></param>
        /// <returns></returns>
        public static int GetPokerNum(byte iPoker)
        {
            //A 14 //2是2
            byte Num = PokerCardsHelper.GetPokerNum(iPoker);

            if (Num == 2) { return 15; }

            if (Num == 15) { return 16; };

            if (Num == 16) { return 17; };

            return Num;
        }
        
    }

    
}
