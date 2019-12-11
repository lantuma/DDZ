/******************************************************************************************
*         【模块】{ 大厅模块 }                                                                                                                      
*         【功能】{ 游戏大版本检测 }                                                                                                                   
*         【修改日期】{ 2019年7月19日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Linq;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [Event(EventIdType.CheckLargeVersion)]
    public class CheckLargeVersionSystem:AEvent<Action>
    {
        
        public async override void Run(Action action)
        {
            Log.Debug("开始游戏大版本检测============>>>");

            bool NeedLoad = await CheckLargeVersion();

            if (!NeedLoad)
            {
                action();

                return;
            }

            string mes = DataCenterComponent.Instance.tipInfo.NeedLoadNewAPKTip;

            MessageData messData = new MessageData();

            messData.ok = OkAction;

            messData.cancel = OkAction;

            messData.mes = mes;

            messData.okStr = DataCenterComponent.Instance.tipInfo.OkStrTip;

            messData.onlyOk = true;

            messData.canClose = false;

            Game.PopupComponent.ShowMessageBox(mes, messData);
        }

        /// <summary>
        /// 确认回调
        /// </summary>
        public void OkAction()
        {
            Application.OpenURL(GameHelper.GetShareQRCodeURL());
        }

        /// <summary>
        /// 大版本比对
        /// </summary>
        public async Task<bool> CheckLargeVersion()
        {
            //从资源服Version.txt 获得大版本号

            VersionConfig remoteConfig = await BundleHelper.GetRemoteVersion();

            int remoteLargeVersion = remoteConfig.Version;

            Log.Debug("远程服务器版本号:" + remoteLargeVersion);

            //从本地资源Version.tex 获得大版本号

            VersionConfig localConfig = await BundleHelper.GetLocalVersion();

            int localLargeVersion = remoteLargeVersion;

            if (localConfig != null) localLargeVersion = localConfig.Version;

            Log.Debug("本地大版本号:" + localLargeVersion);

            return remoteLargeVersion > localLargeVersion;
        }
    }
}
