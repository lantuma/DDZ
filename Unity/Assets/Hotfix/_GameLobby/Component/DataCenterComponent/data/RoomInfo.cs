///////////////////////////////////////////
///房间信息
///zhouyu 2019.3.12
///////////////////////////////////////////

using System.Collections.Generic;

namespace ETHotfix
{
    public class RoomInfo
    {
        /// <summary>
        /// 桌子列表
        /// </summary>
        public List<DeskInfo> deskList;

        /// <summary>
        /// 当前桌子
        /// </summary>
        public DeskInfo curDesk;

        /// <summary>
        /// 房间类型
        /// </summary>
        public RoomType roomType = RoomType.MatchRoom;

        public RoomInfo()
        {
            this.init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            this.deskList = new List<DeskInfo>();

            this.curDesk = null;
        }

        /// <summary>
        /// 清除房间信息
        /// </summary>
        public void clean()
        {
            this.init();
        }

        /// <summary>
        /// 获得当前的房间信息
        /// </summary>
        /// <returns></returns>
        public DeskInfo getCurDesk()
        {
            return this.curDesk;
        }

        /// <summary>
        /// 初始化桌子列表
        /// </summary>
        public void InitDeskList()
        {
            this.clean();

            for (int i = 0; i < 5; i++)
            {
                DeskInfo desk = new DeskInfo();

                desk.deskNo = i;

                this.deskList.Add(desk);
            }
            this.sortDesk(this.deskList);
        }

        /// <summary>
        /// 房间列表排序
        /// </summary>
        /// <param name="deskList"></param>
        public void sortDesk(List<DeskInfo> deskList)
        {
            int len = deskList.Count;

            for (int i = 0; i < len; i++)
            {
                for (int j = i + 1; j < len; j++)
                {
                    if (deskList[i].deskID > deskList[j].deskID)
                    {
                        var temp = deskList[i];

                        deskList[i] = deskList[j];

                        deskList[j] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// 根据桌子ID获得房间信息
        /// </summary>
        /// <param name="deskId"></param>
        /// <returns></returns>
        public DeskInfo getDeskById(int deskId)
        {
            DeskInfo findDesk=null;

            for (int i = 0; i < this.deskList.Count; i++)
            {
                var desk = this.deskList[i];

                if (deskId == desk.deskID)
                {
                    findDesk = desk;

                    return findDesk;
                }
            }

            Log.Debug("no find deskId" + deskId);

            return null;
        }

        /// <summary>
        /// 根据房间号获得房间信息
        /// </summary>
        /// <param name="deskNo"></param>
        /// <returns></returns>
        public DeskInfo getDeskByNo(int deskNo)
        {
            DeskInfo findDesk = null;

            for (int i = 0; i < this.deskList.Count; i++)
            {
                var desk = this.deskList[i];

                if (deskNo == desk.deskNo)
                {
                    findDesk = desk;

                    return findDesk;
                }
            }

            Log.Debug("no find deskNo" + deskNo);

            return null;
        }
    }

    /// <summary>
    /// 房间类型
    /// </summary>
    public enum RoomType
    {
        FriendRoom = 1,//好友房

        MatchRoom = 2 //匹配房
    }
}
