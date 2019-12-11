/******************************************************************************************
*         【模块】{ 弹窗管理组件 }                                                                                                                      
*         【功能】{ 通用弹窗，通用TIP}
*         【修改日期】{ 2019年3月29日 }                                                                                                                        
*         【贡献者】{ 周瑜                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    /// <summary>
    /// 弹窗参数
    /// </summary>
    public class MessageData
    {
        /// <summary>
        /// 确认回调
        /// </summary>
        public System.Action ok = null;

        /// <summary>
        /// 取消回调
        /// </summary>
        public System.Action cancel = null;

        /// <summary>
        /// 消息文本
        /// </summary>
        public string mes = "";

        /// <summary>
        /// 点击遮罩是否关闭
        /// </summary>
        public bool click = false;

        /// <summary>
        /// 只有确认按钮
        /// </summary>
        public bool onlyOk = true;

        /// <summary>
        /// 确定按钮文字
        /// </summary>
        public string okStr = "确 定";

        /// <summary>
        /// 取消按钮文字
        /// </summary>
        public string noStr = "取 消";

        /// <summary>
        /// 标题文字
        /// </summary>
        public string titleStr = "信息提示";

        /// <summary>
        /// 快速关闭
        /// </summary>
        public bool fastClose = false;

        /// <summary>
        /// 是否可关闭
        /// </summary>
        public bool canClose = true;
    }

    /// <summary>
    /// 加载锁定动画参数
    /// </summary>
    public class LoadingLockData
    {
        /// <summary>
        /// 文本
        /// </summary>
        public string mes = "Loading...";

        /// <summary>
        /// 超时回调
        /// </summary>
        public System.Action timeOutAction = null;

        /// <summary>
        /// 多少秒后算超时
        /// </summary>
        public int timeOut = 5;
    }

    /// <summary>
    /// 公告参数
    /// </summary>
    public class NoticeData
    {
        /// <summary>
        /// 文本
        /// </summary>
        public string mes = "";

        /// <summary>
        /// 是否循环
        /// </summary>
        public bool isLoop = true;

        /// <summary>
        /// 公告位置
        /// </summary>
        public Vector3 Pos=Vector3.zero;
    }

    [ObjectSystem]
    public class PopupComponentAwakeSystem : AwakeSystem<PopupComponent>
    {
        public override void Awake(PopupComponent self)
        {
            self.Awake();
        }
    }

    public class PopupComponent : Component
    {
        private static Tweener messageTweener;

        private static Tweener tipTweener;
        
        private static bool isAct = false;

        public bool isShowBox = false;

        private ObjPool _tipPool;
        
        private Queue<GameObject> repeatTipQueue;

        private UIClickLock UIClickLock = null;


        public void Awake()
        {
            _tipPool = ETModel.Game.Scene.GetComponent<SimpleObjectPoolComponent>().GreateObjectPool(UIType.UITip, UIType.CommonUI, UIType.UITip, 10);

            repeatTipQueue = new Queue<GameObject>();
            
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();

            resourcesComponent.LoadBundle(UIType.CommonUI.StringToAB());
        }
        
        /// <summary>
        /// 弹窗
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public void ShowMessageBox(string message, MessageData data = null)
        {
            if (isShowBox) { return; }

            GameObject gameObject = LoadUIAsset(UIType.UIMessageBoxPanel);

            UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.UIMessageBoxPanel, gameObject, false);

            var messageBox = ui.AddComponent<UIMessageBoxCpt>();

            var tweenEffectComponent = ui.AddComponent<TweenEffectComponent>();
            
            Game.Scene.GetComponent<UIComponent>().Add(ui);

            isShowBox = true;

            if (data == null) data = new MessageData();

            data.mes = message;

            messageBox.Awake(data);

            var _tra = messageBox.Root.transform;

            tweenEffectComponent.Add(TweenAnimationIdType.MessageBox, _tra);
            
            tweenEffectComponent.Play(TweenAnimationIdType.MessageBox);

        }

        /// <summary>
        /// 关掉弹窗
        /// </summary>
        public void CloseMessageBox()
        {
            Game.PopupComponent.isShowBox = false;

            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UIMessageBoxPanel);
        }
        
        /// <summary>
        /// 文字提示
        /// </summary>
        /// <param name="message"></param>
        /// <param name="showTime"></param>
        public async void ShowTips(string message, int showTime = 1)
        {
            if (isAct) return;
            
            GameObject gameObject = LoadUIAsset(UIType.UITip);

            UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.UITip, gameObject, false);

            var tip = ui.AddComponent<UITipCpt>();

            Game.Scene.GetComponent<UIComponent>().Add(ui);

            isAct = true;

            var _tra = tip.GetParent<UI>().GameObject.transform;

            _tra.Find("Text").GetComponent<Text>().text = message;

            _tra.localPosition = new Vector3(0, 300, 0);

            tipTweener = _tra.DOLocalMoveY(0, 0.3f);

            tipTweener.Restart();

            await Task.Delay(showTime * 1000);

            _tra.DOLocalMoveY(300, 0.3f);

            await Task.Delay(300);

            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UITip);

            isAct = false;

        }

        /// <summary>
        /// 重复文字提示
        /// </summary>
        /// <param name="message"></param>
        /// <param name="showTime"></param>
        public  async void ShowRepeatTip(string message, int showTime = 1)
        {
            GameObject tipObj = _tipPool.Get();
            
            tipObj.SetActive(true);

            tipObj.transform.localPosition = new Vector3(0, 0, 0);

            tipObj.transform.localScale = Vector3.one;

            tipObj.transform.Find("Text").GetComponent<Text>().text = message;
            
            GameObject Root = Component.Global.transform.Find("UI").gameObject;

            string cavasName = tipObj.GetComponent<CanvasConfig>().CanvasName.ToString();

            tipObj.transform.SetParent(Root.Get<GameObject>(cavasName).transform, false);

            repeatTipQueue.Enqueue(tipObj);

            await Task.Delay(showTime * 1000);

            GameObject obj = repeatTipQueue.Dequeue();

            obj.transform.DOLocalMoveY(300, 0.3f);

            await Task.Delay(300);

            _tipPool.Recycle(obj);

        }

        /// <summary>
        /// 打开设置面板
        /// </summary>
        public void ShowSettingPanel()
        {
            GameObject gameObject = LoadUIAsset(UIType.UIGameSettingPanel);

            UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.UIGameSettingPanel, gameObject, false);

            var panel = ui.AddComponent<UIGameSettingPanel>();

            var tec = ui.AddComponent<TweenEffectComponent>();

            Game.Scene.GetComponent<UIComponent>().Add(ui);

            tec.Add(TweenAnimationIdType.GameSettingPanel, panel.Root.transform);

            tec.Play(TweenAnimationIdType.GameSettingPanel);
        }

        /// <summary>
        /// 关闭设置面板
        /// </summary>
        public void CloseSettingPanel()
        {
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UIGameSettingPanel);
        }

        /// <summary>
        /// 打开玩家列表面板
        /// </summary>
        public void ShowPlayerListPanel()
        {
            GameObject gameObject = LoadUIAsset(UIType.UIGamePlayerListsPanel);

            UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.UIGamePlayerListsPanel, gameObject, false);
            
            var panel = ui.AddComponent<UIGamePlayerListsPanel>();

            var tec = ui.AddComponent<TweenEffectComponent>();

            tec.Add(TweenAnimationIdType.GamePlayerListPanel, panel.Root.transform);

            Game.Scene.GetComponent<UIComponent>().Add(ui);

            panel.OnShow();
        }

        /// <summary>
        /// 关闭玩家列表面板
        /// </summary>
        public void ClosePlayerListPanel()
        {
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UIGamePlayerListsPanel);
        }

        /// <summary>
        /// 打开二维码截屏面板
        /// </summary>
        public void ShowQRCodePanel()
        {
            GameObject gameObject = LoadUIAsset(UIType.UIQRCodePanel);

            UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.UIQRCodePanel, gameObject, false);

            var panel = ui.AddComponent<UIQRCodePanel>();

            var tec = ui.AddComponent<TweenEffectComponent>();

            Game.Scene.GetComponent<UIComponent>().Add(ui);

            tec.Add(TweenAnimationIdType.QRCodePanel, panel.Root.transform);

            tec.Play(TweenAnimationIdType.QRCodePanel);
        }

        public void CloseQRCodePanel()
        {
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UIQRCodePanel);
        }

        /// <summary>
        /// 打开加载等待动画
        /// </summary>
        public void ShowLoadingLockUI(string message= "Loading...", LoadingLockData data = null)
        {
            var uic = Game.Scene.GetComponent<UIComponent>();

            if (uic.Get(UIType.UILoadingLock) != null) { return; }
            
            GameObject gameObject = LoadUIAsset(UIType.UILoadingLock);

            UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.UILoadingLock, gameObject, false);

            var loadingLock = ui.AddComponent<UILoadingLock>();

            Game.Scene.GetComponent<UIComponent>().Add(ui);

            if (data == null) data = new LoadingLockData();

            data.mes = message;

            loadingLock.OnOpen(data);
        }
        
        /// <summary>
        /// 关闭加载等待动画
        /// </summary>
        public void CloseLoadingLockUI()
        {
            var uic = Game.Scene.GetComponent<UIComponent>();

            if (uic.Get(UIType.UILoadingLock) != null)
            {
                Game.Scene.GetComponent<UIComponent>().Remove(UIType.UILoadingLock);
            }
        }

        /// <summary>
        /// 打开点击锁定界面
        /// </summary>
        /// <param name="lockTime"></param>
        public void SetClickLock(float lockTime = 0.5f)
        {
            if (this.UIClickLock == null)
            {
                GameObject gameObject = LoadUIAsset(UIType.UIClickLock);

                UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.UIClickLock, gameObject, false);

                this.UIClickLock = ui.AddComponent<UIClickLock>();

                Game.Scene.GetComponent<UIComponent>().Add(ui);
            }

            this.UIClickLock.OnOpen(lockTime);
        }

        /// <summary>
        /// 隐藏点击锁定界面
        /// </summary>
        public void HideClickLock()
        {
            if (this.UIClickLock != null)
            {
                this.UIClickLock.OnHide();
            }
        }

        /// <summary>
        /// 打开跑马灯
        /// </summary>
        /// <param name="message"></param>
        public void ShowNoticeUI(string message = "", NoticeData data = null)
        {
            GameObject gameObject = LoadUIAsset(UIType.UIScrollNoticePanel);

            UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.UIScrollNoticePanel, gameObject, false);

            var pmd = ui.AddComponent<ScrollNoticeComponent>();

            Game.Scene.GetComponent<UIComponent>().Add(ui);

            if (data == null) data = new NoticeData();

            data.mes = message;

            pmd.OnOpen(data);
        }

        /// <summary>
        /// 关闭跑马灯
        /// </summary>
        public void CloseNoticeUI()
        {
            var uic = Game.Scene.GetComponent<UIComponent>();

            if (uic.Get(UIType.UIScrollNoticePanel) != null)
            {
                Game.Scene.GetComponent<UIComponent>().Remove(UIType.UIScrollNoticePanel);
            }
        }

        /// <summary>
        /// 打开压力测试UI
        /// </summary>
        public void ShowStressTestUI()
        {
            GameObject gameObject = LoadUIAsset(UIType.UIStressTestPanel);

            UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.UIStressTestPanel, gameObject, false);

            var pmd = ui.AddComponent<UIStressTestPanel>();

            Game.Scene.GetComponent<UIComponent>().Add(ui);
        }

        /// <summary>
        /// 关闭压力测试UI
        /// </summary>
        public void CloseStressTestUI()
        {
            var uic = Game.Scene.GetComponent<UIComponent>();

            if (uic.Get(UIType.UIStressTestPanel) != null)
            {
                Game.Scene.GetComponent<UIComponent>().Remove(UIType.UIStressTestPanel);
            }
        }

        /// <summary>
        /// 从通用资源加载预置
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        public GameObject LoadUIAsset(string uiType)
        {
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();

            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(UIType.CommonUI.StringToAB(), uiType);

            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);

            return gameObject;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            isAct = false;
        }
    }
}
