using UnityEngine;

namespace ETModel
{
    public class AndroidSdk : IBaseSdk
    {
        private AndroidJavaClass jc;

        private AndroidJavaObject jo;

        private AndroidJavaObject Jc
        {
            get
            {
                if (jc == null)
                {
                    jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                }
                return jc;
            }
        }

        private AndroidJavaObject Jo
        {
            get
            {
                if (jo == null)
                {
                    jo = Jc.GetStatic<AndroidJavaObject>("currentActivity");
                }
                return jo;
            }
        }

        #region 安卓独有的

        /// <summary>
        /// 原生提示信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="activity"></param>
        public void ShowToast(string text)
        {
            Log.Debug(text);

            AndroidJavaObject activity = null;

            if (activity == null)
            {
                AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                activity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }

            AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");

            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {

                AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", text);

                Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT")).Call("show");

            }));
        }

        /// <summary>
        /// 获取安卓平台上键盘的高度
        /// </summary>
        /// <returns></returns>
        public float GetKeyboardHeight()
        {
            using (AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").

                    Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

                using (AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
                {
                    View.Call("getWindowVisibleDisplayFrame", Rct);

                    return Screen.height - Rct.Call<int>("height");
                }
            }
        }

        /// <summary>
        /// Unity本地推送
        /// </summary>
        /// <param name="contents"></param>
        public  void SendNotification(string[] contents)
        {
            jo.Call("SendNotification", contents);
        }

        /// <summary>
        /// 获取运营商名称
        /// </summary>
        /// <returns></returns>
        public string CheckSIM()
        {
            return jo.Call<string>("CheckSIM");
        }

        /// <summary>
        /// 立即重启应用
        /// </summary>
        public  void RestartApplication()
        {
            jo.Call("RestartApplication");
        }

        /// <summary>
        /// 调用APP
        /// </summary>
        public void CallThirdApp(string packName = "com.tencent.mm")
        {
            jo.Call("CallThirdApp", packName);
        }

        /// <summary>
        /// 安卓原生提示(短)
        /// </summary>
        /// <param name="toast"></param>
        public void UnityCallAndroidToast(string toast)
        {
            jo.Call("UnityCallAndroidToast", toast);
        }

        /// <summary>
        /// 安卓原生提示(长)
        /// </summary>
        /// <param name="toast"></param>
        public void CreateToast(string toast)
        {
            jo.Call("CreateToast", toast);
        }

        /// <summary>
        /// 分析WIFI信号强度 0:连接不上或者掉线 1:信号偏号 2:信号良好  1：信号最好
        /// </summary>
        /// <returns></returns>
        public int GetWifiLevel()
        {
            string wifiData = jo.Call<string>("ObtainWifiInfo");

            string[] args = wifiData.Split('|');

            int value = int.Parse(args[0]);

            if (value > -50 && value < 0)
            {
                return 3;
            }
            else if (value > -70 && value < -50)
            {
                return 2;
            }
            else if (value > -150 && value < -70)
            {
                return 1;
            }
            else if (value < -200)
            {
                return 0;
            }

            return 0;
        }

        /// <summary>
        /// 分析电池强度 1:低  2：中  3：高
        /// </summary>
        /// <returns></returns>
        public  int GetBatteryLevel()
        {
            string batteryData = jo.Call<string>("MonitorBatteryState");

            string[] args = batteryData.Split('|');

            float percent = int.Parse(args[0]) / float.Parse(args[1]);

            if (percent > 70)
            {
                return 3;
            }
            else if (percent > 30 && percent <= 70)
            {
                return 2;
            }
            else if (percent < 30)
            {
                return 1;
            }

            return 1;
        }

        /// <summary>
        /// 动态更换APP ICON
        /// </summary>
        public void ChangeAppIcon()
        {
            jo.Call("ChangeAppIcon");
        }

        #endregion

        public void Alipay(string info)
        {
            Log.Debug("AndroidSdk---Alipay");
        }

        public void GetLocation()
        {
            Log.Debug("AndroidSdk---GetLocation");
        }

        public void InstallApk(string fileFullPath)
        {
            Log.Debug("AndroidSdk---InstallApk");
        }

        public void OpenApp(string packageName, string appName, string versionUrl)
        {
            Log.Debug("AndroidSdk---OpenApp");
        }

        public void WeChatLogin()
        {
            Log.Debug("AndroidSdk---WeChatLogin");
        }

        public void WeChatpay(string prepayId, string nonceStr)
        {
            Log.Debug("AndroidSdk---WeChatpay");
        }

        public void WeChatShareImage(string path, string title, string desc, int shareType)
        {
            Log.Debug("AndroidSdk---WeChatShareImage");
        }

        public void WeChatShareUrl(string url, string title, string description, int shareType)
        {
            Log.Debug("AndroidSdk---WeChatShareUrl");
        }
    }
}
