namespace ETModel
{
    public interface IBaseSdk
   {
        /// <summary>
        /// 微信登陆
        /// </summary>
        void WeChatLogin();

        /// <summary>
        /// 微信分享图片
        /// </summary>
        /// <param name="path"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="shareType"></param>
        void WeChatShareImage(string path, string title, string desc, int shareType);

        /// <summary>
        /// 微信分享url链接
        /// </summary>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="shareType"></param>
        void WeChatShareUrl(string url, string title, string description, int shareType);

        /// <summary>
        /// 获取定位地址
        /// </summary>
        void GetLocation();

        /// <summary>
        /// 支付宝支付
        /// </summary>
        /// <param name="info"></param>
        void Alipay(string info);

        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="prepayId"></param>
        /// <param name="nonceStr"></param>
        void WeChatpay(string prepayId, string nonceStr);

        /// <summary>
        /// 打开一个APP 没有安装就下载 苹果跳APP Store
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="appName"></param>
        /// <param name="versionUrl"></param>
        void OpenApp(string packageName, string appName, string versionUrl);

        /// <summary>
        /// 打开一个APK安卓包 此方法只有安卓端才有用
        /// </summary>
        /// <param name="fileFullPath"></param>
        void InstallApk(string fileFullPath);


        /// <summary>
        /// 获得安卓/IOS 键盘高度
        /// </summary>
        /// <returns></returns>
        float GetKeyboardHeight();

        /// <summary>
        /// 显示原生提示
        /// </summary>
        /// <param name="text"></param>
        void ShowToast(string text);

        /// <summary>
        /// 原生提示(短)
        /// </summary>
        /// <param name="toast"></param>
        void UnityCallAndroidToast(string toast);

        /// <summary>
        /// 原生提示(长)
        /// </summary>
        /// <param name="toast"></param>
        void CreateToast(string toast);

        /// <summary>
        /// 立即重启应用
        /// </summary>
        void RestartApplication();

        /// <summary>
        /// 调用APP
        /// </summary>
        void CallThirdApp(string packName);

        /// <summary>
        /// 本地推送
        /// </summary>
        /// <param name="contents"></param>
        void SendNotification(string[] contents);

        /// <summary>
        /// 分析电池强度 1:低  2：中  3：高
        /// </summary>
        /// <returns></returns>
        int GetBatteryLevel();

        /// <summary>
        /// 分析WIFI信号强度 0:连接不上或者掉线 1:信号偏号 2:信号良好  1：信号最好
        /// </summary>
        /// <returns></returns>
        int GetWifiLevel();

        /// <summary>
        /// 获取运营商名称
        /// </summary>
        /// <returns></returns>
        string CheckSIM();

        /// <summary>
        /// 动态更换APP Icon
        /// </summary>
        void ChangeAppIcon();
   }
}
