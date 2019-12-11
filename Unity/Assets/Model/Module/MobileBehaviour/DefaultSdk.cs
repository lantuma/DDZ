namespace ETModel
{
    public class DefaultSdk : IBaseSdk
    {
        public void Alipay(string info)
        {
            Log.Debug("DefaultSdk---Alipay");
        }

        public void GetLocation()
        {
            Log.Debug("DefaultSdk---GetLocation");
        }

        public void InstallApk(string fileFullPath)
        {
            Log.Debug("DefaultSdk---InstallApk");
        }

        public void OpenApp(string packageName, string appName, string versionUrl)
        {
            Log.Debug("DefaultSdk---OpenApp");
        }

        public void WeChatLogin()
        {
            Log.Debug("DefaultSdk---WeChatLogin");
        }

        public void WeChatpay(string prepayId, string nonceStr)
        {
            Log.Debug("DefaultSdk---WeChatpay");
        }

        public void WeChatShareImage(string path, string title, string desc, int shareType)
        {
            Log.Debug("DefaultSdk---WeChatShareImage");
        }

        public void WeChatShareUrl(string url, string title, string description, int shareType)
        {
            Log.Debug("DefaultSdk---WeChatShareUrl");
        }

        public float GetKeyboardHeight()
        {
            return 0;
        }

        public void ShowToast(string text)
        {
            Log.Debug("DefaultSdk---ShowToast");
        }

        public void UnityCallAndroidToast(string toast)
        {
            Log.Debug("DefaultSdk---UnityCallAndroidToast");
        }

        public void CreateToast(string toast)
        {
            Log.Debug("DefaultSdk---CreateToast");
        }

        public void  RestartApplication()
        {
            Log.Debug("DefaultSdk---RestartApplication");
        }

        public void CallThirdApp(string packName)
        {
            Log.Debug("DefaultSdk---CallThirdApp");
        }

        public void SendNotification(string[] contents)
        {
            Log.Debug("DefaultSdk---SendNotification");
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
