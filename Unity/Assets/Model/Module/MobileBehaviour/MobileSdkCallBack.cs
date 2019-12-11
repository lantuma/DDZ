/******************************************************************************************
*         【模块】{ Sdk回调模块 }                                                                                                                      
*         【功能】{ 处理SDK回调 }                                                                                                                   
*         【修改日期】{ 2019年8月1日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using UnityEngine;

namespace ETModel
{
    public class MobileSdkCallBack: MonoBehaviour
    {
        public static MobileSdkCallBack instance { get; private set; }

        public Action<string> WeChatLoginAction;

        public Action<string> LocationAction;

        public Action<string> UrlOpenAppAction;
        
        private void Awake()
        {
            instance = this;
        }

        public void LocationCall(string message)
        {
            LocationAction?.Invoke(message);

            Log.Debug("收到定位回调:" + message);
        }

        public void WxLoginCall(string message)
        {
            WeChatLoginAction?.Invoke(message);

            Log.Debug("收到微信登陆回调:" + message);
        }

        public void WxPayCall(string message)
        {
            Log.Debug("收到微信支付回调:" + message);
        }

        //打开APP的Url
        public static string OpenAppUrl = string.Empty;

        public void UrlOpenAppCall(string message)
        {
            if (UrlOpenAppAction != null)
            {
                UrlOpenAppAction?.Invoke(message);
            }
            else
            {
                OpenAppUrl = message;
            }

            Log.Debug("收到链接打开APP回调" + message);
        }
    }
}
