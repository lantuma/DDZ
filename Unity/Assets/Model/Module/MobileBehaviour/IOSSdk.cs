using System.Runtime.InteropServices;
using UnityEngine;

namespace ETModel
{
    public class IOSSdk : IBaseSdk
    {
        //[DllImport("__Internal")]
        //public static extern void IOSWeChatLogin();


        public void Alipay(string info)
        {
            Log.Debug("IOSSdk---Alipay");
        }

        public void GetLocation()
        {
            Log.Debug("IOSSdk---GetLocation");
        }

        public void InstallApk(string fileFullPath)
        {
            Log.Debug("IOSSdk---InstallApk");
        }

        public void OpenApp(string packageName, string appName, string versionUrl)
        {
            Log.Debug("IOSSdk---OpenApp");
        }

        public void WeChatLogin()
        {
            Log.Debug("IOSSdk---WeChatLogin");
        }

        public void WeChatpay(string prepayId, string nonceStr)
        {
            Log.Debug("IOSSdk---WeChatpay");
        }

        public void WeChatShareImage(string path, string title, string desc, int shareType)
        {
            Log.Debug("IOSSdk---WeChatShareImage");
        }

        public void WeChatShareUrl(string url, string title, string description, int shareType)
        {
            Log.Debug("IOSSdk---WeChatShareUrl");
        }

        public float GetKeyboardHeight()
        {
            return TouchScreenKeyboard.area.height;
        }

        public void ShowToast(string text)
        {
            Log.Debug("IOSSdk---ShowToast");
        }

        public void UnityCallAndroidToast(string toast)
        {
            Log.Debug("IOSSdk---UnityCallAndroidToast");
        }

        public void CreateToast(string toast)
        {
            Log.Debug("IOSSdk---CreateToast");
        }

        public void RestartApplication()
        {
            Log.Debug("IOSSdk---RestartApplication");
        }

        public void CallThirdApp(string packName)
        {
            Log.Debug("IOSSdk---CallThirdApp");
        }

        public void SendNotification(string[] contents)
        {
            Log.Debug("IOSSdk---SendNotification");
        }

        public int GetBatteryLevel()
        {
            return 3;
        }

        public int GetWifiLevel()
        {
            return 3;
        }

        public string CheckSIM()
        {
            return "";
        }
        
        public void ChangeAppIcon()
        {
            
        }
    }
}
