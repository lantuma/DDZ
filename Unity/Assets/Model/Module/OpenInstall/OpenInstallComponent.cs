/******************************************************************************************
*         【模块】{ OpenInstall模块 }                                                                                                                      
*         【功能】{ 控制OpenInstall}                                                                                                                   
*         【修改日期】{ 2019年5月6日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class OpenInstallComponentAwakeSystem : AwakeSystem<OpenInstallComponent>
    {
        public override void Awake(OpenInstallComponent self)
        {
            self.Awake();
        }
    }

    public class OpenInstallComponent : Component
    {
        private OpenInstall openinstall;
        public void Awake()
        {
            //Log.Debug("OpenInstall组件初始化>>>>>>");

            openinstall = GameObject.Find("OpenInstall").GetComponent<OpenInstall>();

            openinstall.RegisterWakeupHandler(getWakeupFinish);
        }

        /// <summary>
        /// 获取安装数据
        /// </summary>
        public void getInstall()
        {
            //Log.Debug("OpenInstallComponent getInstall");

            openinstall.GetInstall(8, getInstallFinish);
        }

        /// <summary>
        /// 注册上报(在用户注册成功后，调用接口上报注册量)
        /// </summary>
        public void reportRegister()
        {
            //Log.Debug("OpenInstallComponent reportRegister");

            openinstall.ReportRegister();
        }

        /// <summary>
        /// 效果点上报(统计终端用户对某些特殊业务的使用效果，如充值金额，分享次数等)
        /// </summary>
        /// <param name="pointID"></param>
        /// <param name="pointValue"></param>
        public void reportEffectPoint(string pointID = "effect_test", int pointValue = 1)
        {
            Log.Debug("OpenInstallComponent reportEffectPoint");

            openinstall.ReportEffectPoint(pointID, pointValue);
        }

        /// <summary>
        /// 获取安装数据回调
        /// </summary>
        /// <param name="installData"></param>
        public void getInstallFinish(OpenInstallData installData)
        {
            Log.Debug("OpenInstallComponent getInstallFinish:渠道编号=" + installData.channelCode + ",自定义参数=" + installData.bindData);

            Log.Debug("安装参数:" + JsonUtility.ToJson(installData));

            if (!string.IsNullOrEmpty(installData.bindData))
            {
                var bindDataObject = JsonUtility.FromJson<OpenInstallBindData>(installData.bindData);

                Log.Debug("获得的参数是：" + bindDataObject.PlayerID + "上传给服务器");
                Game.EventSystem.Run("PromotionBinding", bindDataObject.PlayerID);
            }
        }

        /// <summary>
        /// 获取拉起数据回调
        /// </summary>
        /// <param name="wakeupData"></param>
        public void getWakeupFinish(OpenInstallData wakeupData)
        {
            Log.Debug("OpenInstallComponent getWakeupFinish:渠道编号=" + wakeupData.channelCode + ",自定义参数=" + wakeupData.bindData);

            Log.Error("拉起参数:" + JsonUtility.ToJson(wakeupData));
        }
    }
    [System.Serializable]
    public class OpenInstallBindData
    {
        public string PlayerID;
    }
}
