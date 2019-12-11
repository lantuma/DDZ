using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class TestComponent_AwakeSystem : AwakeSystem<TestComponent>
    {
        public override void Awake(TestComponent self)
        {
            self.Awake();
        }
    }

    public class TestComponent:UIBaseComponent
    {
        private ReferenceCollector rc;

        public void Awake()
        {
            rc = this.GetParent<UIEx>().GameObject.GetComponent<ReferenceCollector>();

            ButtonHelper.RegisterButtonEvent(rc, "VIPLoginBtn", OnClickReturnBtn);

            
        }

        public override void Show()
        {
            base.Show();

            //逻辑
        }

        public override void Close()
        {
            base.Close();

            Log.Error("处理关闭的逻辑....");

            //关闭逻辑

            ETimerComponent.Instance.OnCompleted(ETimer.AddTimer(3, "close"), () => {

               // Game.Scene.GetComponent<UIComponent_Z>().Create(UIType.Login);
            });
        }

        private void OnClickReturnBtn()
        {
            //this.Close();

            //rc.Get<GameObject>("LoginSelectView").SetActive(false);

            //rc.Get<GameObject>("VIPLoginView").SetActive(true);

            // Scene.Global.GetComponent<UIComponent_Z>().Close(UIType.Login);

            Game.Scene.GetComponent<UIManagerComponent>().Close(UIType.Login);

            //Game.Scene.GetComponent<UIComponent_Z>().Create(UIType.Login);//打开
        }
    }
}
