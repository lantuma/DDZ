/******************************************************************************************
*         【模块】{ 通用模块 }                                                                                                                      
*         【功能】{ 压力测试面板}                                                                                                                   
*         【修改日期】{ 2019年7月26日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIStressTestPanelAwakeSystem : AwakeSystem<UIStressTestPanel>
    {
        public override void Awake(UIStressTestPanel self)
        {
            self.Awake();
        }
    }

    public class UIStressTestPanel:Component
    {
        private ReferenceCollector _rf;
        
        private StressTestComponent StressTestComponent;
        

        public void Awake()
        {
            _rf = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            StressTestComponent = Game.Scene.GetComponent<StressTestComponent>();

            ButtonHelper.RegisterButtonEvent(_rf, "closeBtn", OnClose);

            ButtonHelper.RegisterButtonEvent(_rf, "mask", OnClose);

            //单次登录
            ButtonHelper.RegisterButtonEvent(_rf, "btn0", ()=> { StressTestComponent.RunTest(StressTestIdType.LoginTest); });

            //单次登出
            ButtonHelper.RegisterButtonEvent(_rf, "btn1", ()=> { StressTestComponent.RunTest(StressTestIdType.LoginOutTest); });

            //登入\登出100次
            ButtonHelper.RegisterButtonEvent(_rf, "btn2", ()=> { StressTestComponent.RunTest(StressTestIdType.LoginInAndOutTest); });

            //登入\登出1000次
            ButtonHelper.RegisterButtonEvent(_rf, "btn3", ()=> { StressTestComponent.RunTest(StressTestIdType.LoginInAndOutTest1000); });

            //请求战绩100次
            ButtonHelper.RegisterButtonEvent(_rf, "btn4", () => { StressTestComponent.RunTest(StressTestIdType.MyRecordReqTest100); });

            //请求战绩1000次
            ButtonHelper.RegisterButtonEvent(_rf, "btn5", () => { StressTestComponent.RunTest(StressTestIdType.MyRecordReqTest1000); });
        }

        public  void OnClose()
        {
            Game.PopupComponent.CloseStressTestUI();
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
        }
    }
}
