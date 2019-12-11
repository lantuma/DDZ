/******************************************************************************************
*         【模块】{ 安卓行为模块 }                                                                                                                      
*         【功能】{ 测试类 }                                                                                                                   
*         【修改日期】{ 2019年7月23日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class MobileSdkTestComponent : MonoBehaviour
    {
        private string batteryData;

        private string wifiData;

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 140, 40), "发送通知"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                jo.Call("UnityCallAndroidToast", "这是Unity调用Android的Toast！");
            }

            if (GUI.Button(new Rect(10, 70, 140, 40), "求和"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                int sum = jo.Call<int>("Sum", new object[] { 10, 20 });

                Debug.Log("sum:" + sum);

                jo.Call("ClickShake");//调用安卓震动
            }

            if (GUI.Button(new Rect(10, 130, 140, 40), "Toast"))//创建安卓 Toast
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                jo.Call("CreateToast", "初始化中...");
            }

            if (GUI.Button(new Rect(10, 190, 140, 40), "立即重启应用"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                jo.Call("RestartApplication");
            }

            if (GUI.Button(new Rect(10, 250, 140, 40), "UI线程重启应用"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                jo.Call("RestartApplicationOnUIThread");
            }

            if (GUI.Button(new Rect(10, 310, 140, 40), "重启应用"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                jo.Call("RestartApplication1");
            }

            if (GUI.Button(new Rect(10, 370, 140, 40), "5s重启应用"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                jo.Call("RestartApplication2");
            }

            if (GUI.Button(new Rect(10, 430, 140, 40), "获取安装apk"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                jo.Call("GetAllPackageName");
            }

            if (GUI.Button(new Rect(10, 490, 140, 40), "调用APP"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                jo.Call("CallThirdApp", "com.tencent.mm");
            }

            if (GUI.Button(new Rect(10, 550, 140, 40), "Unity本地推送"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                jo.Call("SendNotification", new string[] { "奇迹:最强者", "勇士们 魔龙讨伐即将开始" });
            }

            if (GUI.Button(new Rect(10, 610, 140, 40), "获取所有App"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                jo.Call("GetAllWidget");
            }

            if (GUI.Button(new Rect(10, 670, 140, 40), "获取已安装的App"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                jo.Call("GetInstalledPackageName");
            }

            if (GUI.Button(new Rect(10, 730, 140, 40), "获取电池信息"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                batteryData = jo.Call<string>("MonitorBatteryState");

                OnBatteryDataBack(batteryData);
            }

            if (GUI.Button(new Rect(10, 790, 140, 40), "获取wifi强度"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                wifiData = jo.Call<string>("ObtainWifiInfo");

                OnWifiDataBack(wifiData);
            }
            if (GUI.Button(new Rect(10, 850, 140, 40), "获取运营商名称"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                string simType = jo.Call<string>("CheckSIM");

                Debug.Log("运营商名称:" + simType);
            }

            if (GUI.Button(new Rect(210, 10, 140, 40), "更换APP ICON"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

                jo.Call("ChangeAppIcon");

                Debug.Log("动态更换APP图标");
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
            {
                Application.Quit();
            }

        }

        void GetBatteryAnWifiData()
        {
            batteryData = "";

            wifiData = "";

            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

            batteryData = jo.Call<string>("MonitorBatteryState");

            Debug.Log("batteryData:" + batteryData);

            AndroidJavaClass jc1 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            AndroidJavaObject jo1 = jc.GetStatic<AndroidJavaObject>("currentActivity");

            wifiData = jo1.Call<string>("ObtainWifiInfo");

            Debug.Log("wifiData:" + wifiData);

            OnBatteryDataBack(batteryData);

            OnWifiDataBack(wifiData);
        }
        void OnBatteryDataBack(string batteryData)//level+"|"+scale+"|"+status;
        {
            string[] args = batteryData.Split('|');

            if (args[2] == "2")
            {
                Debug.Log("电池充电中");
            }
            else
            {
                Debug.Log("电池放电中");
            }
            float percent = int.Parse(args[0]) / float.Parse(args[1]);

            Debug.Log("电池百分比:" + (Mathf.CeilToInt(percent) + "%").ToString());
        }
        void OnWifiDataBack(string wifiData)//strength+"|"+intToIp(ip)+"|"+mac+"|"+ssid;
        {
            //分析wifi信号强度

            //获取RSSI，RSSI就是接受信号强度指示。

            //得到的值是一个0到-100的区间值，是一个int型数据，其中0到-50表示信号最好，

            //-50到-70表示信号偏差，小于-70表示最差，

            //有可能连接不上或者掉线，一般Wifi已断则值为-200。
            Debug.Log("wifiData:" + wifiData);

            string[] args = wifiData.Split('|');

            if (int.Parse(args[0]) > -50 && int.Parse(args[0]) < 0)
            {
                Debug.Log("Wifi信号强度很棒");
            }
            else if (int.Parse(args[0]) > -70 && int.Parse(args[0]) < -50)
            {
                Debug.Log("Wifi信号强度一般");
            }
            else if (int.Parse(args[0]) > -150 && int.Parse(args[0]) < -70)
            {
                Debug.Log("Wifi信号强度很弱");
            }
            else if (int.Parse(args[0]) < -200)
            {
                Debug.Log("Wifi信号JJ了");
            }
            string ip = "IP：" + args[1];

            string mac = "MAC:" + args[2];

            string ssid = "Wifi Name:" + args[3];

            Debug.Log(ip);

            Debug.Log(mac);

            Debug.Log(ssid);
        }

        /// <summary>
        /// 安卓日志
        /// </summary>
        /// <param name="str"></param>
        void OnCoderReturn(string str)
        {
            Debug.Log(str);
        }

        void OnBatteryDataReturn(string batteryData)
        {
            string[] args = batteryData.Split('|');

            if (args[2] == "2")
            {
                Debug.Log("电池充电中");
            }
            else
            {
                Debug.Log("电池放电中");
            }

            Debug.Log("电量百分比:" + (args[0] + "%").ToString());
        }

        void OnWifiDataReturn(string wifiData)
        {
            Debug.Log("wifiData:" + wifiData);

            string[] args = wifiData.Split('|');

            if (int.Parse(args[0]) > -50 && int.Parse(args[0]) < 100)
            {
                Debug.Log("Wifi信号强度很棒");
            }
            else if (int.Parse(args[0]) > -70 && int.Parse(args[0]) < -50)
            {
                Debug.Log("Wifi信号强度一般");
            }
            else if (int.Parse(args[0]) > -150 && int.Parse(args[0]) < -70)
            {
                Debug.Log("Wifi信号强度很弱");
            }
            else if (int.Parse(args[0]) < -200)
            {
                Debug.Log("Wifi信号JJ了");
            }
            string ip = "IP：" + args[1];

            string mac = "MAC:" + args[2];

            string ssid = "Wifi Name:" + args[3];

            Debug.Log(ip);

            Debug.Log(mac);

            Debug.Log(ssid);
        }
    }
}
