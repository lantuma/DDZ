/******************************************************************************************
*         【模块】{ 移动Sdk模块 }                                                                                                                      
*         【功能】{ 封装Sdk方法 }                                                                                                                   
*         【修改日期】{ 2019年7月23日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class MobileSdkComponentAwakeSystem : AwakeSystem<MobileSdkComponent>
    {
        public override void Awake(MobileSdkComponent self)
        {
            self.Awake();
        }
    }

    public class MobileSdkComponent : Component
    {
        public static MobileSdkComponent instance;

        private IBaseSdk mSdk;

        //是否使用测试组件
        public bool UseTestComponent = false;

        public void Awake()
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            //Log.Debug("安卓平台");

            mSdk = new AndroidSdk();

#elif UNITY_IPHONE && !UNITY_EDITOR

            //Log.Debug("IOS平台");

            mSdk = new IOSSdk();
#else

            //Log.Debug("PC平台");

            mSdk = new DefaultSdk();
#endif
            if (UseTestComponent) Camera.main.gameObject.AddComponent<MobileSdkTestComponent>();

            Camera.main.gameObject.AddComponent<MobileSdkCallBack>();

            instance = this;
        }

        /// <summary>
        /// 安卓原生提示
        /// </summary>
        /// <param name="text"></param>
        /// <param name="activity"></param>
        public void ShowToast(string text)
        {
            mSdk.ShowToast(text);
        }

        /// <summary>
        /// 复制文本至剪切版(通用)
        /// </summary>
        /// <param name="text"></param>
        public void CopyText(string text)
        {
            GUIUtility.systemCopyBuffer = text;
        }

        /// <summary>
        /// 粘贴文本(通用)
        /// </summary>
        /// <returns></returns>
        public string PasteText()
        {
            return GUIUtility.systemCopyBuffer;
        }

        /// <summary>
        /// 震动(通用)
        /// </summary>
        public void Vibrate()
        {
            Handheld.Vibrate();
        }

        /// <summary>
        /// 获得安卓/IOS 键盘高度
        /// </summary>
        /// <returns></returns>
        public float GetKeyboardHeight()
        {
            return mSdk.GetKeyboardHeight();
        }

        /// <summary>
        /// 全屏截屏(通用)
        /// </summary>
        /// <param name="fileName">Application.persistentDataPath,生成PNG,不用加后缀</param>
        /// <param name="superSize">提高分辨率的因子</param>
        public void CaptureScreen(string fileName, int superSize = 1)
        {
            ScreenCapture.CaptureScreenshot(fileName, superSize);
        }

        /// <summary>
        /// 指定尺寸截屏(通用)
        /// </summary>
        /// <param name="rect">截图区域，左下角为0点</param>
        /// <param name="fileName">文件名</param>
        /// <returns>Texture2D 对象 </returns>
        public Texture2D CaptureScreenshot(Rect rect, string fileName = "Screenshot.png")
        {
            //创建空纹理，可自定义尺寸
            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

            //读取屏幕像素信息并存储为纹理数据
            screenShot.ReadPixels(rect, 0, 0);

            screenShot.Apply();

            //将纹理数据生成png图片文件
            byte[] bytes = screenShot.EncodeToPNG();

            string filename = Application.dataPath + "/" + fileName;

            System.IO.File.WriteAllBytes(filename, bytes);

            return screenShot;
        }

        /// <summary>
        /// 对相机截图(通用)
        /// </summary>
        /// <param name="camera">被截屏的相机</param>
        /// <param name="rect">截屏的区域</param>
        /// <param name="fileName">文件名</param>
        /// <returns>Texture2D 对象 </returns>
        public Texture2D CaptureScreenshot(Camera camera, Rect rect, string fileName = "Screenshot.png")
        {
            //创建一个RenderTexture对象
            RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);

            //临时设置相机的TargetTexture为rt,并手动渲染相机
            camera.targetTexture = rt;

            camera.Render();

            //激活rt，并从中读取像素
            RenderTexture.active = rt;

            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

            screenShot.ReadPixels(rect, 0, 0);

            screenShot.Apply();

            //重置相关参数，以使camera继续在屏幕上显示
            camera.targetTexture = null;

            RenderTexture.active = null;

            GameObject.Destroy(rt);

            //将纹理数据，生成png图片文件
            byte[] bytes = screenShot.EncodeToPNG();

            string filename = Application.dataPath + "/" + fileName;

            System.IO.File.WriteAllBytes(filename, bytes);

            return screenShot;
        }

        /// <summary>
        /// 获取网络类型(通用)
        /// </summary>
        /// <returns></returns>
        public int GetNetworkInfo()
        {
            int networkType = NetworkType.None;

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                networkType = NetworkType.None;
            }

            if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                networkType = NetworkType.Wifi;
            }

            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                networkType = NetworkType.Mobile;
            }

            return networkType;
        }

        /// <summary>
        /// 安卓原生提示(短)
        /// </summary>
        /// <param name="toast"></param>
        public void UnityCallAndroidToast(string toast)
        {
            mSdk.UnityCallAndroidToast(toast);
        }

        /// <summary>
        /// 安卓原生提示(长)
        /// </summary>
        /// <param name="toast"></param>
        public void CreateToast(string toast)
        {
            mSdk.CreateToast(toast);
        }


        /// <summary>
        /// 立即重启应用
        /// </summary>
        public void RestartApplication()
        {
            mSdk.RestartApplication();
        }

        /// <summary>
        /// 调用APP
        /// </summary>
        public void CallThirdApp(string packName = "com.tencent.mm")
        {
            mSdk.CallThirdApp(packName);
        }

        /// <summary>
        /// Unity本地推送
        /// </summary>
        /// <param name="contents"></param>
        public void SendNotification(string[] contents)
        {
            mSdk.SendNotification(contents);
        }

        /// <summary>
        /// 获取电池等级1:低 2:中 3：高
        /// </summary>
        /// <returns></returns>
        public int GetBatteryLevel()
        {
            return mSdk.GetBatteryLevel();
        }

        /// <summary>
        /// 获取WIFI强度等级 0:无网  1:低 2：中  3：高
        /// </summary>
        /// <returns></returns>
        public int GetWifiLevel()
        {
            return mSdk.GetWifiLevel();
        }

        /// <summary>
        /// 获取运营商名称
        /// </summary>
        /// <returns></returns>
        public string CheckSIM()
        {
            return mSdk.CheckSIM();
        }

        /// <summary>
        /// 动态更换APP ICON
        /// </summary>
        public void ChangeAppIcon()
        {
            mSdk.ChangeAppIcon();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// 微信登陆
        /// </summary>
        public void WeChatLogin()
        {
            mSdk.WeChatLogin();
        }

        /// <summary>
        /// 获取定位地址
        /// </summary>
        public void GetLocation()
        {
            mSdk.GetLocation();
        }

        /// <summary>
        /// 打开一个APK包 只有安卓端才有用
        /// </summary>
        /// <param name="fileFullPath"></param>
        public void installApk(string fileFullPath)
        {
            mSdk.InstallApk(fileFullPath);
        }

        /// <summary>
        /// 微信分享连接
        /// </summary>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="shareType"></param>
        public void WeChatShareUrl(string url, string title, string description, int shareType)
        {
            mSdk.WeChatShareUrl(url, title, description, shareType);
        }

        /// <summary>
        /// 微信分享图片
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="sharteType"></param>
        public void WeChatShareImage(string imagePath, string title, string desc, int sharteType)
        {
            mSdk.WeChatShareImage(imagePath, title, desc, sharteType);
        }

        /// <summary>
        /// 支付宝支付
        /// </summary>
        /// <param name="info"></param>
        public void Alipay(string info)
        {
            mSdk.Alipay(info);
        }

        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="prepayId"></param>
        /// <param name="nonceStr"></param>
        public void WeChatpay(string prepayId, string nonceStr)
        {
            mSdk.WeChatpay(prepayId, nonceStr);
        }
    }

    public class NetworkType
    {
        /// <summary>
        /// 没有网络
        /// </summary>
        public const int None = 0;

        /// <summary>
        /// Wifi
        /// </summary>
        public const int Wifi = 1;

        /// <summary>
        /// 移动网络
        /// </summary>
        public const int Mobile = 2;
    }

    public class WxShareSceneType
    {
        /// <summary>
        /// 好友
        /// </summary>
        public const int Friend = 1;

        /// <summary>
        /// 朋友圈
        /// </summary>
        public const int Circle = 2;
    }
}
