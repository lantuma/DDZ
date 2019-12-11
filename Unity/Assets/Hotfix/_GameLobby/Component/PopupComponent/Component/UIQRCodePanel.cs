/******************************************************************************************
*         【模块】{ 通用模块 }                                                                                                                      
*         【功能】{ 二维码截屏面板}                                                                                                                   
*         【修改日期】{ 2019年5月7日 }                                                                                                                        
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
    public class UIQRCodePanelAwakeSystem : AwakeSystem<UIQRCodePanel>
    {
        public override void Awake(UIQRCodePanel self)
        {
            self.Awake();
        }
    }

    public class UIQRCodePanel : Component
    {
        private ReferenceCollector _rf;

        public GameObject Root;
        private Text CodeText;

        private Image QRCode;

        public void Awake()
        {
            _rf = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            Root = _rf.Get<GameObject>("Root");

            QRCode = _rf.Get<GameObject>("QRCode").GetComponent<Image>();
            CodeText = _rf.Get<GameObject>("CodeText").Get<Text>();

            QRCode.sprite = ETModel.Game.Scene.GetComponent<QRCodeComponent>().GenerateQRImage(GameHelper.GetShareQRCodeURL() + GamePrefs.GetUserId(),512,512);
            CodeText.text = UserDataHelper.UserInfo.PlayerId.ToString();
            ButtonHelper.RegisterButtonEvent(_rf, "mask", OnClose);
        }

        public async void OnClose()
        {
            SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

            Game.Scene.GetComponent<UIComponent>().Get(UIType.UIQRCodePanel).GetComponent<TweenEffectComponent>().PlayBackwards(TweenAnimationIdType.QRCodePanel);

            await Task.Delay(200);

            Game.PopupComponent.CloseQRCodePanel();
        }

        public void Reset()
        {

        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.Reset();
        }
    }
}
