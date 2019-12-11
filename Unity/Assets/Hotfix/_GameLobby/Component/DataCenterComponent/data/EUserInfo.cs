////////////////////////////////////////////////////////
///用户信息
///zhouyu 2019.3.12
///////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;


namespace ETHotfix
{
    public class EUserInfo
    {
        //[userID][userVO]
        public Dictionary<long, UserVO> userList = new Dictionary<long, UserVO>();
        
        //用户http登录时,保存自己数据
        public UserVO httpUserInfo = null;

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userVo">用户数据Vo</param>
        public void addUser(UserVO userVo)
        {
            if (userList.ContainsKey(userVo.userID))
            {
                 UnityEngine.Debug.Log("用户重复添加:" + userVo.userID);
            }
            else
            {
                userList[userVo.userID] = userVo;
            }
        }

        /// <summary>
        /// 获取自己用户信息
        /// </summary>
        /// <returns></returns>
        public UserVO getMyUserVo()
        {
            if (this.httpUserInfo != null)
            {
                return this.getUser(this.httpUserInfo.userID);
            }
            return null;
        }

        /// <summary>
        /// 修改自己的名字
        /// </summary>
        /// <param name="str"></param>
        public void changeName(string str)
        {
            UserVO user = this.getUser(this.httpUserInfo.userID);
            user.nickName = str;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userID">用户userId</param>
        /// <returns>返回用户信息</returns>
        public UserVO getUser(long userID)
        {
            if (userList.ContainsKey(userID))
            {
                return userList[userID];
            }
            return null;
            
        }

        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool isExist(long userID)
        {
            if (this.getUser(userID) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据座位号获取用户信息
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public UserVO getUserBySeatID(long seatID)
        {
            foreach (var key in this.userList.Keys)
            {
                if (this.userList[key].seatID == seatID)
                {
                    return this.userList[key];
                }
            }
            return null;
        }

        /// <summary>
        /// 根据userID获取用户信息
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns>返回用户信息</returns>
        public UserVO getUserByUserID(long userID)
        {
            foreach (var key in this.userList.Keys)
            {
                if (this.userList[key].userID == userID)
                {
                    return this.userList[key];
                }
            }
            return null;
        }

        /// <summary>
        /// 得到无座玩家列表
        /// </summary>
        /// <returns></returns>
        public List<UserVO> getNoSeatUsers()
        {
            List<UserVO> noSeatList = new List<UserVO>();

            foreach (var key in this.userList.Keys)
            {
                if (this.userList[key].seatID == -1)
                {
                    noSeatList.Add(this.userList[key]);
                }
            }

            return noSeatList;
        }

        /// <summary>
        /// 得到有座玩家列表
        /// </summary>
        /// <returns></returns>
        public List<UserVO> getOnSeatUsers()
        {
            List<UserVO> onSeatList = new List<UserVO>();

            foreach (var key in this.userList.Keys)
            {
                if (this.userList[key].seatID > -1)
                {
                    onSeatList.Add(this.userList[key]);
                }
            }

            return onSeatList;
        }
        
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        public void deleteUser(long userID)
        {
            if (this.userList.ContainsKey(userID))
            {
                this.userList.Remove(userID);
            }
        }

        /// <summary>
        /// 删除所有用户信息,除了自己
        /// </summary>
        public void deleteAllUserExcptMe()
        {
            foreach (var item in this.userList.ToList())
            {
                if (!item.Key.Equals(this.httpUserInfo.userID))
                {
                    this.userList.Remove(item.Key);
                }
            }
        }

        /// <summary>
        /// 删除所有用户信息
        /// </summary>
        public void deleteAllUser()
        {
            this.userList.Clear();
        }

        

    }
}
