using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class Setting_ReportCptAwakeSystem : AwakeSystem<Setting_ReportCpt, GameObject, GameObject, ButtonGroup>
    {
        public override void Awake(Setting_ReportCpt self, GameObject a, GameObject b, ButtonGroup c)
        {
            self.Awake(a, b, c);
        }
    }

    public class Setting_ReportCpt : SimpleToggle
    {
        private InputField _reportField;

        private Text VerInfoLab;

        public override void Init()
        {
            base.Init();

            _reportField = ReferenceCollector.Get<GameObject>("InputField").GetComponent<InputField>();

            VerInfoLab = ReferenceCollector.Get<GameObject>("VerInfoLab").GetComponent<Text>();

            var webConfig = ClientComponent.Instance.webConfig;

            if (webConfig != null)
            {
                VerInfoLab.text = $"当前版本号：{webConfig.dbbh}";
            }

            ReferenceCollector.Get<GameObject>("submitBtn").GetComponent<Button>().onClick.AddListener(SubmitReport);
        }

        /// <summary>
        /// 发送反馈
        /// </summary>
        private void SubmitReport()
        {
            Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.ModuleNotOpenTip);
            _reportField.text = "";
            return;

            if (string.IsNullOrEmpty(_reportField.text))
            {
                //                GameHelper.ShowMessageBox("请输入要反馈的内容!");
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.InputReportContentTip);
                return;
            }

            try
            {

            }
            catch (Exception e)
            {
                //                GameHelper.ShowMessageBox(e.Message);
                Game.PopupComponent.ShowMessageBox(e.Message);
                throw;
            }
        }

        public override void ChangeToggle(bool state)
        {
            base.ChangeToggle(state);
        }
    }
}