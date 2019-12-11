using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class GamePrefs
    {
        private const string SoundState = "SoundState";

        private const string Operation = "Operation";

        private const string Tablecloth = "Tablecloth";

        private const string UserId = "UserId";

        private const string UserAccount = "AssociatedKey";

        private const string UserPwd = "AssociatedPwd";

        private const string GuestAccount = "GuestAccount";

        private const string GuestPwd = "GuestPwd";

        #region 公开函数

        #region 声音状态

        /// <summary>
        /// 得到音乐开关状态
        /// </summary>
        /// <returns></returns>
        public static int GetMusicState()
        {
            return GetPrefsState(SoundState, 1);
        }

        /// <summary>
        /// 设置音乐开关
        /// </summary>
        /// <param name="state"></param>
        public static void SetMusicState(float state)
        {
            SetPrefsState(SoundState, 1, state);
        }

        /// <summary>
        /// 得到音效开关状态
        /// </summary>
        /// <returns></returns>
        public static int GetSoundEffectState()
        {
            return GetPrefsState(SoundState, 1 << 1);
        }

        /// <summary>
        /// 设置音效开关
        /// </summary>
        /// <param name="state"></param>
        public static void SetSoundEffectState(float state)
        {
            SetPrefsState(SoundState, 1 << 1, state);
        }

        /// <summary>
        /// 得到方言的开关状态
        /// </summary>
        /// <returns></returns>
        public static int GetLocalismState()
        {
            return GetPrefsState(SoundState, 1 << 2);
        }

        /// <summary>
        /// 设置方言的开关状态
        /// </summary>
        /// <param name="state"></param>
        public static void SetLocalismState(float state)
        {
            SetPrefsState(SoundState, 1 << 2, state);
        }

        #endregion

        #region 操作状态

        /// <summary>
        /// 得到单击出牌的开关状态
        /// </summary>
        /// <returns></returns>
        public static int GetClickShowCardState()
        {
            return GetPrefsState(Operation, 1);
        }

        /// <summary>
        /// 设置单机出牌的开关状态
        /// </summary>
        /// <param name="state"></param>
        public static void SetClickShowCardState(float state)
        {
            SetPrefsState(Operation, 1, state);
        }

        #endregion

        #region 其他状态

        /// <summary>
        /// 得到桌布的状态
        /// </summary>
        /// <returns></returns>
        public static int GetTableclothState()
        {
            return PlayerPrefs.GetInt(Tablecloth, 1);
        }

        /// <summary>
        /// 设置桌布的状态
        /// </summary>
        /// <param name="index">桌布标记</param>
        public static void SetTableclothState(int index)
        {
            PlayerPrefs.SetInt(Tablecloth, index);
        }

        #endregion

        #region 用户ID

        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <returns></returns>
        public static long GetUserId()
        {
            return long.Parse(PlayerPrefs.GetString(UserId, ""));
        }

        /// <summary>
        /// 保存用户ID
        /// </summary>
        /// <param name="value"></param>
        public static void SetUserId(long value)
        {
            PlayerPrefs.SetString(UserId, value.ToString());
        }

        /// <summary>
        /// 获取最后一次登录的账号
        /// </summary>
        /// <returns></returns>
        public static string GetUserAccount()
        {
            return PlayerPrefs.GetString(UserAccount, "");
        }

        /// <summary>
        /// 保存用户账号
        /// </summary>
        /// <param name="value"></param>
        public static void SetUserAccount(string value)
        {
            PlayerPrefs.SetString(UserAccount, value);
        }

        /// <summary>
        /// 获取用户密码
        /// </summary>
        /// <returns></returns>
        public static string GetUserPwd()
        {
            return PlayerPrefs.GetString(UserPwd, "");
        }

        /// <summary>
        /// 是否是VIP账户
        /// </summary>
        /// <returns></returns>
        public static bool HasVipAccount()
        {
            return PlayerPrefs.HasKey(UserAccount);
        }

        /// <summary>
        /// 保存用户密码
        /// </summary>
        /// <param name="value"></param>
        public static void SetUserPwd(string value)
        {
            PlayerPrefs.SetString(UserPwd, value);
        }

        /// <summary>
        /// 获取游客账号
        /// </summary>
        public static string GetGuestAccount()
        {
            return PlayerPrefs.GetString(GuestAccount, "");
        }

        /// <summary>
        /// 获取游客密码
        /// </summary>
        /// <returns></returns>
        public static string GetGuestPwd()
        {
            return PlayerPrefs.GetString(GuestPwd, "");
        }

        /// <summary>
        /// 设置游客账号
        /// </summary>
        /// <param name="value"></param>
        public static void SetGuestAccount(string value)
        {
            PlayerPrefs.SetString(GuestAccount, value);
        }

        /// <summary>
        /// 设置游客密码
        /// </summary>
        /// <param name="value"></param>
        public static void SetGuestPwd(string value)
        {
            PlayerPrefs.SetString(GuestPwd, value);
        }

        /// <summary>
        /// 清除游客信息
        /// </summary>
        public static void ClearGuestInfo()
        {
            if (PlayerPrefs.HasKey(GuestAccount)) PlayerPrefs.DeleteKey(GuestAccount);
            if (PlayerPrefs.HasKey(GuestPwd)) PlayerPrefs.DeleteKey(GuestPwd);
        }

        #endregion

        #endregion

        #region 私有函数

        /// <summary>
        /// 得到开关状态
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index">存储位置</param>
        /// <returns></returns>
        private static int GetPrefsState(string key, int index)
        {
            return (PlayerPrefs.GetInt(key, 1 | (1 << 1)) & index) != 0 ? 1 : 0;
        }

        /// <summary>
        /// 设置开关状态
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index">存储位置</param>
        /// <param name="state">状态</param>
        private static void SetPrefsState(string key, int index, float state)
        {
            if (state - 0.1f > 0)
            {
                PlayerPrefs.SetInt(key, PlayerPrefs.GetInt(key, 1 | (1 << 1)) | index);

                //                SceneHelperComponent.Instance.AudioSound.SoundMuteByType(index == 1 ? AudioSoundType.Background : AudioSoundType.Game, false);

                Debug.Log("开");
            }
            else
            {
                PlayerPrefs.SetInt(key, (PlayerPrefs.GetInt(key, 1 | (1 << 1)) & index) != 0
                    ? PlayerPrefs.GetInt(key, 1 | (1 << 1)) ^ index
                    : PlayerPrefs.GetInt(key, 1 | (1 << 1)));

                //                SceneHelperComponent.Instance.AudioSound.SoundMuteByType(index == 1 ? AudioSoundType.Background : AudioSoundType.Game, true);

                Debug.Log("关");
            }
        }

        #endregion
    }
}