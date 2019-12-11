/******************************************************************************************
*         【模块】{ 压力测试组件 }                                                                                                                      
*         【功能】{ 对服务器端口压力测试 }
*         【修改日期】{ 2019年7月26日 }                                                                                                                        
*         【贡献者】{ 周瑜 ｝                                                                                                               
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class StressTestComponentAwakeSystem : AwakeSystem<StressTestComponent>
    {
        public override void Awake(StressTestComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class StressTestComponentUpdateSystem : UpdateSystem<StressTestComponent>
    {
        public override void Update(StressTestComponent self)
        {
            self.Update();
        }
    }

    public partial class StressTestComponent:Component
    {
        private class StressTestData
        {
            /// <summary>
            /// 开始时间
            /// </summary>
            public float StartTime = 0;

            /// <summary>
            /// 测试次数
            /// </summary>
            public int TestCount = 1;

            /// <summary>
            /// 结束时间
            /// </summary>
            public float EndTime = 0;

            /// <summary>
            /// 是否延迟
            /// </summary>
            public float Delay = 0;

            /// <summary>
            /// 是否在测试中
            /// </summary>
            public bool IsTesting = false;

            /// <summary>
            /// 测试用例名称
            /// </summary>
            public string TestName = "";

            /// <summary>
            /// 当前是第几次
            /// </summary>
            public int CurrentTestCount = 0;
        }

        /// <summary>
        /// 测试用例字典
        /// </summary>
        private Dictionary<string, StressTestData> StressTestDic;

        /// <summary>
        /// 测试结束回调方法
        /// </summary>
        private Dictionary<string, Action> StressTestEnd;
        
        
        public void Awake()
        {
            Log.Debug("初始化压力测试组件=======>>");

            StressTestDic = new Dictionary<string, StressTestData>();

            StressTestEnd = new Dictionary<string, Action>();

            //注册
            this.RegisterTest();
        }

        public void Update()
        {
            #region 测试快捷键

            /*
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Log.Debug("手动开启压力测试1====>>>>>>");

                this.RunTest(StressTestIdType.LoginTest);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Log.Debug("手动开启压力测试2====>>>>>>");

                this.RunTest(StressTestIdType.LoginOutTest);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Log.Debug("手动开启压力测试3====>>>>>>");

                this.RunTest(StressTestIdType.LoginInAndOutTest);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Log.Debug("手动开启压力测试4====>>>>>>");

                this.RunTest(StressTestIdType.LoginInAndOutTest1000);
            }
       
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Log.Debug("手动开启压力测试5====>>>>>>");

                this.RunTest(StressTestIdType.MyRecordReqTest100);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                Log.Debug("手动开启压力测试5====>>>>>>");

                this.RunTest(StressTestIdType.MyRecordReqTest1000);
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Game.PopupComponent.ShowStressTestUI();
            }
            */
            #endregion
            
        }

        /// <summary>
        /// 注册用例方法
        /// </summary>
        private void RegisterTest()
        {
            //测试用例
            this.Add(StressTestIdType.LoginTest, StressTestIdType.LoginTestName, 0, 1, null);

            this.Add(StressTestIdType.LoginOutTest, StressTestIdType.LoginOutTestName, 0, 1, null);

            this.Add(StressTestIdType.LoginInAndOutTest, StressTestIdType.LoginInAndOutTestName, 0, 100,null);

            this.Add(StressTestIdType.LoginInAndOutTest1000, StressTestIdType.LoginInAndOutTest1000Name, 0, 1000, null);

            this.Add(StressTestIdType.MyRecordReqTest100, StressTestIdType.MyRecordReqTest100Name, 0, 100, null);

            this.Add(StressTestIdType.MyRecordReqTest1000, StressTestIdType.MyRecordReqTest1000Name, 0, 1000, null);
        }

        public void Add(string key,string testName, float delay = 0f, int testCount = 1, Action endAction = null)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此StressTestComponent  组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (this.StressTestDic.TryGetValue(key, out StressTestData item))
            {
                Log.Error("已经添加过");

                return;
            }

            if (key == StressTestIdType.None || key == "" || key == null)
            {
                Log.Error("key需要值定义在StressTestIdType里面");

                return;
            }

            this.StressTestDic[key] = new StressTestData
            {
                Delay = delay,

                TestCount = testCount,

                TestName = testName
            };

            if (endAction != null)
            {
                this.StressTestEnd[key] = endAction;
            }
        }

        public void RunTest(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此StressTestComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.StressTestDic.TryGetValue(key, out StressTestData item))
            {
                Log.Error($"播放传入的key:{key} 没有对应的测试数据");

                return;
            }
            
            this.BeginRunTest(key);
        }

        private async void BeginRunTest(string key)
        {
            if (this == null || this.InstanceId == 0)
            {
                Log.Error("此StressTestComponent 组件已经被销毁，请不要再调用此方法");

                return;
            }

            if (!this.StressTestDic.TryGetValue(key, out StressTestData item))
            {
                Log.Error($"播放传入的key:{key} 没有对应的测试数据");

                return;
            }

            Log.Debug("开启压测===(" + "用例名称:" + item.TestName + ")");

            item.StartTime = Time.time;

            item.IsTesting = true;

            //根据类型调用对应的方法，待改进

            if (key == StressTestIdType.LoginTest) await StressTest_Login();

            if (key == StressTestIdType.LoginOutTest) await StressTest_LoginOut();

            if (key == StressTestIdType.LoginInAndOutTest) await StressTest_LoginInAndOut100(item);

            if (key == StressTestIdType.LoginInAndOutTest1000) await StressTest_LoginInAndOut1000(item);

            if (key == StressTestIdType.MyRecordReqTest100) await StressTest_MyRecordReqTest100(item);

            if (key == StressTestIdType.MyRecordReqTest1000) await StressTest_MyRecordReqTest1000(item);

            item.IsTesting = false;

            item.EndTime = Time.time;

            if (StressTestEnd.TryGetValue(key, out Action endAction))
            {
                endAction();
            }

            Log.Debug("结束压测===("+ "用例名称:" + item.TestName +  "  开始时间:"+ item.StartTime + " 结束时间:" + item.EndTime + ")" + " 总耗时:" + (item.EndTime - item.StartTime) + " 运行次数:"+item.TestCount + " 平均单次耗时:" + (item.EndTime - item.StartTime)/item.TestCount);
        }
        
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            StressTestDic.Clear();

            StressTestDic = null;

            StressTestEnd.Clear();

            StressTestEnd = null;
        }
    }
}
