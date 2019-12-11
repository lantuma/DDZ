/******************************************************************************************
*         【模块】{ 大厅模块 }                                                                                                                      
*         【功能】{ 大厅轮播插件 }                                                                                                                   
*         【修改日期】{ 2019年7月18日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETModel;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameLobbyGameTypeSelectPluginAwakeSystem : AwakeSystem<GameLobbyGameTypeSelectPlugin, GameObject>
    {
        public override void Awake(GameLobbyGameTypeSelectPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class GameLobbyGameTypeSelectPlugin : Component
    {
        private GameObject panel;

        public ReferenceCollector _rf;

        private GameLobbyCpt lobby;

        public Text GameTypeGoldNumberText;
        public Text PlayerIDText;

        public Image GameTypeImage;

        public GameObject MaskBg;

        public GameObject _BackGameListsButton;

        public GameObject _GameLevelPanel;

        public GameObject _GameTypeBg;

        private GameObject LevelButton_low;

        private GameObject LevelButton_middle;

        private GameObject LevelButton_high;

        private Image GameTypeBgImage;


        public GameLobbyGameTypeSelectPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            this._GameTypeBg = this.panel;

            this._rf = this.panel.GetComponent<ReferenceCollector>();

            this.lobby = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIHallPanel).GetComponent<GameLobbyCpt>();

            GameTypeGoldNumberText = _rf.Get<GameObject>("GameTypeGoldNumberText").Get<Text>();

            GameTypeImage = _rf.Get<GameObject>("GameTypeImage").Get<Image>();
            GameTypeBgImage= _rf.Get<GameObject>("GameTypeBgImage").Get<Image>();

            MaskBg = _rf.Get<GameObject>("MaskBg");

            _BackGameListsButton = _rf.Get<GameObject>("BackGameListsButton");

            _GameLevelPanel = _rf.Get<GameObject>("GameLevelPanel");

            LevelButton_low = _rf.Get<GameObject>("LevelButton_low");

            LevelButton_middle = _rf.Get<GameObject>("LevelButton_middle");

            LevelButton_high = _rf.Get<GameObject>("LevelButton_high");

            ButtonHelper.RegisterButtonEvent(_rf, "BackGameListsButton", () => {

                this.OnBackGameListsButton();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "QuickStartGameButton", () => {

                //this.OnQuickStartGameButton();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "LevelButton_low", () => {

                lobby.RequestJoinRoom(0);
            });

            ButtonHelper.RegisterButtonEvent(_rf, "LevelButton_middle", () => {

                lobby.RequestJoinRoom(1);
            });

            ButtonHelper.RegisterButtonEvent(_rf, "LevelButton_high", () => {

                lobby.RequestJoinRoom(2);
            });

            return this;
        }

        /// <summary>
        /// 设置等级UI显示数据
        /// </summary>
        /// <param name="go"></param>
        /// <param name="level"></param>
        private void SetLevelUIData(GameObject go, AreaInfo data)
        {
            _rf.Get<GameObject>("playerHeadImg").GetComponent<Image>().sprite = lobby.GameLobbyTopPlugin._HeadImage.sprite;
            _rf.Get<GameObject>("PlayerIDText").GetComponent<Text>().text = $"ID：{ lobby.GameLobbyTopPlugin._PlayerIdText.text}";
            if (data.GameId == 2)
            {
                //百人牛牛固定底分1 --只处理百人牛牛的
                go.TryGetInChilds<Text>("BottomScoreText").text = "1";
            }
            else
            {
                go.TryGetInChilds<Text>("BottomScoreText").text = data.Score.ToString();
            }
            go.TryGetInChilds<Text>("ZhunRuText").text = $"入场：{data.Score}";
        }

        /// <summary>
        /// 通用场等级选择方法
        /// </summary>
        /// <param name="gameType"></param>
        public async Task<bool> OnSelectedArea(GameType gameType)
        {
            GameHelper.CurrentGameInfo = GameHelper.GetGameInfo(gameType);

            if (GameHelper.CurrentGameInfo == null)
            {
                Game.PopupComponent.ShowMessageBox(DataCenterComponent.Instance.tipInfo.GameNotOpenTip);

                return false;
            }

            GameTypeImage.sprite = GameHelper.GetSprite("chooseareaatlas", $"choosearea_game{((int)gameType) - 1}");

            GameTypeImage.SetNativeSize();
            GameTypeBgImage.sprite = GameHelper.GetSprite("chooseareaatlas", $"bgchoosearea_game{((int)gameType) - 1}");
            // 获取场
            var ares = await GameHelper.GetGameAreaList(GameHelper.CurrentGameInfo.GameId);

            if (ares != null)
            {
                lobby.GameLobbyGameListPlugin._GameType.SetActive(false);

                _BackGameListsButton.SetActive(true);
            }
            else
            {
                return false;
            }

            if (ares.Count == 2)
            {
                LevelButton_low.SetActive(true);

                LevelButton_middle.SetActive(false);

                LevelButton_high.SetActive(true);

                _GameLevelPanel.GetComponent<GridLayoutGroup>().spacing = new Vector2(250, 0);

                SetLevelUIData(LevelButton_low, ares[0]);

                SetLevelUIData(LevelButton_high, ares[1]);
            }
            else if (ares.Count == 3)
            {
                LevelButton_low.SetActive(true);

                LevelButton_middle.SetActive(true);

                LevelButton_high.SetActive(true);

                _GameLevelPanel.GetComponent<GridLayoutGroup>().spacing = new Vector2(139, 0);

                SetLevelUIData(LevelButton_low, ares[0]);

                SetLevelUIData(LevelButton_middle, ares[1]);

                SetLevelUIData(LevelButton_high, ares[2]);
            }
            else if (ares.Count == 1)
            {
                lobby.GameLobbyGameListPlugin._GameType.SetActive(true);

                _BackGameListsButton.SetActive(false);

                return true;
            }
            else
            {
                return false;
            }

            // 显示场等级选择
            _GameLevelPanel.SetActive(true);

            _GameTypeBg.SetActive(true);

            MaskBg.SetActive(true);

            _BackGameListsButton.SetActive(true);

            return true;
        }

        /// <summary>
        /// 快速开始游戏
        /// </summary>
        public void OnQuickStartGameButton()
        {
            lobby.RequestJoinRoom(0);
        }

        /// <summary>
        /// 返回游戏列表
        /// </summary>
        public void OnBackGameListsButton()
        {
            ETModel.Game.Scene.GetComponent<SoundComponent>().PlayClip(DataCenterComponent.Instance.soundInfo.click);

            SoundHelper.FadeOutPlaySound();

            Game.EventSystem.Run(EventIdType.ClickBackGameList);

            GameHelper.CurrentGameInfo = null;

            lobby.GameLobbyGameListPlugin._GameType.SetActive(true);

            this._GameTypeBg.SetActive(false);

            this.MaskBg.SetActive(false);

            this._BackGameListsButton.SetActive(false);
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
        }
    }
}
