using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class UserCenter_SettingCptAwakeSystem : AwakeSystem<UserCenter_SettingCpt, GameObject, GameObject, ButtonGroup>
    {
        public override void Awake(UserCenter_SettingCpt self, GameObject button, GameObject panel, ButtonGroup group)
        {
            self.Awake(button, panel, group);
        }
    }

    public class UserCenter_SettingCpt : SimpleToggle
    {
        private ButtonGroup subBtnGroup;

        #region 组件

        private Setting_PersonalCpt _settingPersonalCpt;
        private Setting_ReportCpt _settingReportCpt;

        #endregion

        private GameObject onGo;
        private GameObject offGo;

        public override void Init()
        {
            base.Init();

            subBtnGroup = AddComponent<ButtonGroup>();

            _settingPersonalCpt = ComponentFactory.Create<Setting_PersonalCpt, GameObject, GameObject, ButtonGroup>(
                ReferenceCollector.Get<GameObject>("SoundTableToggle"),
                ReferenceCollector.Get<GameObject>("SoundView"),
                subBtnGroup);

            _settingReportCpt = ComponentFactory.Create<Setting_ReportCpt, GameObject, GameObject, ButtonGroup>(
                ReferenceCollector.Get<GameObject>("ReportTableToggle"),
                ReferenceCollector.Get<GameObject>("ReportView"),
                subBtnGroup);

            subBtnGroup.OnButtonClick(_settingPersonalCpt);
        }

        public override void ChangeToggle(bool state)
        {
            base.ChangeToggle(state);
        }
    }
}