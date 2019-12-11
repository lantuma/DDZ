using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace ETEditor
{
    public class LocalInfoEditor : Editor
    {
        private const string SoundState = "SoundState";

        private const string Operation = "Operation";

        private const string Tablecloth = "Tablecloth";

        private const string UserId = "UserId";

        private const string UserAccount = "AssociatedKey";

        private const string UserPwd = "AssociatedPwd";

        private const string GuestAccount = "GuestAccount";

        private const string GuestPwd = "GuestPwd";

        #region VIP Account

        [MenuItem("Local/VIP Account/Get VIP Account", false, 1000)]
        public static void GetVipAccount()
        {
            var res = "";
            if (PlayerPrefs.HasKey(UserAccount)) res += "Vip账号: " + PlayerPrefs.GetString(UserAccount);
            if (PlayerPrefs.HasKey(UserPwd)) res += ", Vip密码: " + PlayerPrefs.GetString(UserPwd);
            Debug.Log(!string.IsNullOrEmpty(res) ? res : "没有本地Vip账号!");
        }

        [MenuItem("Local/VIP Account/Clear VIP Account", false, 1001)]
        public static void ClearVipAccount()
        {
            if (PlayerPrefs.HasKey(UserAccount)) PlayerPrefs.DeleteKey(UserAccount);
            if (PlayerPrefs.HasKey(UserPwd)) PlayerPrefs.DeleteKey(UserPwd);
            Debug.Log("Clear VIP Account");
        }

        #endregion

        #region Guest Account

        [MenuItem("Local/Guest Account/Get Guets Account", false, 1000)]
        public static void GetGusetAccount()
        {
            var res = "";
            if (PlayerPrefs.HasKey(GuestAccount)) res += "游客账号: " + PlayerPrefs.GetString(GuestAccount);
            if (PlayerPrefs.HasKey(GuestPwd)) res += ", 游客密码: " + PlayerPrefs.GetString(GuestPwd);
            Debug.Log(!string.IsNullOrEmpty(res) ? res : "没有本地游客账号!");
        }

        [MenuItem("Local/Guest Account/Clear Guest Account", false, 1001)]
        public static void ClearGuestAccount()
        {
            if (PlayerPrefs.HasKey(GuestAccount)) PlayerPrefs.DeleteKey(GuestAccount);
            if (PlayerPrefs.HasKey(GuestPwd)) PlayerPrefs.DeleteKey(GuestPwd);
            Debug.Log("Clear Guest Account");
        }

        #endregion

        [MenuItem("Local/Get UserId", false, 1100)]
        public static void GetUserId()
        {
            var res = "";
            if (PlayerPrefs.HasKey(UserId)) res = PlayerPrefs.GetString(UserId);
            Debug.Log(!string.IsNullOrEmpty(res) ? res : "没有本地UserId!");
        }

        [MenuItem("Local/Clear All Local Info", false, 10000)]
        public static void ClearAll()
        {
            if (PlayerPrefs.HasKey(UserAccount)) PlayerPrefs.DeleteKey(UserAccount);
            if (PlayerPrefs.HasKey(UserPwd)) PlayerPrefs.DeleteKey(UserPwd);
            if (PlayerPrefs.HasKey(GuestAccount)) PlayerPrefs.DeleteKey(GuestAccount);
            if (PlayerPrefs.HasKey(GuestPwd)) PlayerPrefs.DeleteKey(GuestPwd);
            Debug.Log("Clear All Local Info");
        }

        [MenuItem("Local/Clear All SubGame Info",false,10001)]
        public static void ClearSubGame()
        {
            for (int i = 1; i < 10; i++)
            {
                if (PlayerPrefs.HasKey("subGame" + i)) PlayerPrefs.DeleteKey("subGame" + i);
            }

            Debug.Log("Clear All SubGame Info");
        }
    }
}