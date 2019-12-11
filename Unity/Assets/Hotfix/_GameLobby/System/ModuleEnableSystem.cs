
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [Event(EventIdType.ModuleEnable)]
    public class ModuleEnableSystem : AEvent
    {
        public override void Run()
        {

            var uic = Game.Scene.GetComponent<UIComponent>();

            var WebConfigInfo = DataCenterComponent.Instance.WebConfigInfo;

            var WebCofnig = ClientComponent.Instance.webConfig;

            if (uic.Get(UIType.UIHallPanel) != null && WebCofnig != null)
            {
                var lobbyCpt = uic.Get(UIType.UIHallPanel).GetComponent<GameLobbyCpt>();

                if (lobbyCpt != null)
                {
                    //斗地主
                    lobbyCpt.GameLobbyGameListPlugin._rf.Get<GameObject>("SubGame0").SetActive(WebCofnig.ddz == 1);


                    //-------------------------------------------------------------------------------------

                    //广告页
                    lobbyCpt.GameLobbyCarousePlugin.panel.SetActive(WebCofnig.ggy == 1);

                    //提现
                    lobbyCpt.GameLobbyBottomPlugin.CashButton.SetActive(WebCofnig.tx == 1);

                    //战绩
                    lobbyCpt.GameLobbyBottomPlugin.MyRecordButton.SetActive(WebCofnig.zj == 1);

                    //公告
                    lobbyCpt.GameLobbyBottomPlugin.NoticeButton.SetActive(WebCofnig.gg == 1);

                    //设置
                    lobbyCpt.GameLobbyBottomPlugin.PersonSettingButton.SetActive(WebCofnig.sz == 1);
                }
            }
        }
    }
}
