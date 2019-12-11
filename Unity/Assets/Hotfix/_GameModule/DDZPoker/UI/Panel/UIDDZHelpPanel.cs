/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 帮助面板}                                                                                                                   
*         【修改日期】{ 2019年4月2日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIDDZHelpPanelAwakeSystem : AwakeSystem<UIDDZHelpPanel>
    {
        public override void Awake(UIDDZHelpPanel self)
        {
            self.Awake();
        }
    }

    public class UIDDZHelpPanel : Component
    {
        private ReferenceCollector _rf;

        private Button mask;

        private GameObject Root;

        private Button CloseButton;
        
        private GameObject pxjsView;

        private GameObject wfjsView;

        private Text pxjsBtn;//改为程序字

        private Text wfjsBtn;

        private Image pxBtn;

        private Image wfBtn;

        public void Awake()
        {
            _rf = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            mask = _rf.Get<GameObject>("mask").GetComponent<Button>();

            Root = _rf.Get<GameObject>("Root");

            CloseButton = _rf.Get<GameObject>("CloseButton").GetComponent<Button>();

            pxjsView = _rf.Get<GameObject>("pxjsView");

            wfjsView = _rf.Get<GameObject>("wfjsView");

            pxjsBtn = _rf.Get<GameObject>("pxjsBtn").transform.Find("Text").GetComponent<Text>();

            wfjsBtn = _rf.Get<GameObject>("wfjsBtn").transform.Find("Text").GetComponent<Text>();

            pxBtn = _rf.Get<GameObject>("pxjsBtn").GetComponent<Image>();

            wfBtn = _rf.Get<GameObject>("wfjsBtn").GetComponent<Image>();

            mask.onClick.AddListener(() => { OnClose(); });

            CloseButton.onClick.AddListener(() => { OnClose(); });

            ButtonHelper.RegisterButtonEvent(_rf, "pxjsBtn", () =>
            {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                pxjsBtn.color = Color.yellow;

                wfjsBtn.color = Color.black;

                pxBtn.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ2_youxuanzhong@2x");

                wfBtn.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ2_zuoweixuan@2x");

                pxjsView.SetActive(true);

                wfjsView.SetActive(false);
            });

            ButtonHelper.RegisterButtonEvent(_rf, "wfjsBtn", () =>
            {
                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                pxjsBtn.color = Color.black;

                wfjsBtn.color = Color.yellow;

                pxBtn.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ2_youweixuan@2x");

                wfBtn.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ2_zuoxuanzhong@2x");

                pxjsView.SetActive(false);

                wfjsView.SetActive(true);
            });

            Root.transform.localScale = Vector3.zero;

            Root.transform.DOScale(1f, 0.2f);
        }

        private void OnClose()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

            DDZUIFactory.helpPanel.Remove();
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
