/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 结算面板}                                                                                                                   
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
    public class UIDDZOverPanelAwakeSystem : AwakeSystem<UIDDZOverPanel>
    {
        public override void Awake(UIDDZOverPanel self)
        {
            self.Awake();
        }
    }

    public class UIDDZOverPanel : Entity
    {
        private ReferenceCollector _rf;
        
        private GameObject Root;

        private Image Title;
        
        private Text baseScoreLab;

        private Text bombScoreLab;

        private Text FlowerScoreLab;

        private GameObject LoseLab;

        private GameObject winLab;

        private Image Bg;

        private TweenEffectComponent tweenEffectComponent;

        public void Awake()
        {
            _rf = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            
            Root = _rf.Get<GameObject>("Root");

            tweenEffectComponent = AddComponent<TweenEffectComponent>();

            tweenEffectComponent.Add(TweenAnimationIdType.DDZOverPanel, Root.transform);

            tweenEffectComponent.Play(TweenAnimationIdType.DDZOverPanel);

            Title = _rf.Get<GameObject>("Title").GetComponent<Image>();

            baseScoreLab = _rf.Get<GameObject>("baseScoreLab").GetComponent<Text>();

            bombScoreLab = _rf.Get<GameObject>("bombScoreLab").GetComponent<Text>();

            FlowerScoreLab = _rf.Get<GameObject>("FlowerScoreLab").GetComponent<Text>();

            LoseLab = _rf.Get<GameObject>("LoseLab");

            winLab = _rf.Get<GameObject>("winLab");

            Bg = _rf.Get<GameObject>("Bg").GetComponent<Image>();

            ButtonHelper.RegisterButtonEvent(_rf, "mask", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZUIFactory.overPanel.Remove();

                DDZConfig.GameScene.DDZReadyPlugin.Show();

                DDZGameConfigComponent.Instance.Clear();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "ChangeTableBtn", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZConfig.GameScene.RequestChangeRoom();
            });

            ButtonHelper.RegisterButtonEvent(_rf, "ContinueBtn", () =>
            {
                Game.PopupComponent.SetClickLock();

                SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.click);

                DDZUIFactory.overPanel.Remove();

                DDZConfig.GameScene.Reset();

                DDZConfig.GameScene.RequestPrepare();

                DDZGameConfigComponent.Instance.Clear();
            });
            
            this.InitData();

        }

        public void InitData()
        {
            if (DDZGameHelper.settle != null)
            {
                foreach (var item in DDZGameHelper.settle.PlayerData)
                {
                    var playerData = item;

                    int realSeatID = DDZGameHelper.ChangeSeat(playerData.ChairId);

                    //if (realSeatID == i)
                   // {
                        var item1 = _rf.Get<GameObject>("Item" + realSeatID);

                        var _ref = item1.GetComponent<ReferenceCollector>();

                        var WinScore = _ref.Get<GameObject>("WinScore").GetComponent<Text>();

                        var LoseScore = _ref.Get<GameObject>("LoseScore").GetComponent<Text>();


                        if (playerData.Score > 0)
                        {
                            WinScore.gameObject.SetActive(true);

                            LoseScore.gameObject.SetActive(false);

                            WinScore.text = "+" + playerData.Score;
                        }
                        else
                        {
                            WinScore.gameObject.SetActive(false);

                            LoseScore.gameObject.SetActive(true);

                            LoseScore.text = playerData.Score.ToString();
                        }
                    //}
                }
            }
        }


        public void _InitData()
        {
            var myVo = DataCenterComponent.Instance.userInfo.getMyUserVo();

            for (int i = 0; i < 3; i++)
            {
                var item = _rf.Get<GameObject>("Item" + i);

                var _ref = item.GetComponent<ReferenceCollector>();

                var Status = _ref.Get<GameObject>("Status").GetComponent<Image>();

                var HeadBox = _ref.Get<GameObject>("HeadBox").GetComponent<Image>();

                var PlayerHead = _ref.Get<GameObject>("PlayerHead").GetComponent<Image>();

                var PlayerName = _ref.Get<GameObject>("PlayerName").GetComponent<Text>();

                var WinScore = _ref.Get<GameObject>("WinScore").GetComponent<Text>();

                var LoseScore = _ref.Get<GameObject>("LoseScore").GetComponent<Text>();

                var bg = _ref.Get<GameObject>("bg").GetComponent<Image>();

                var baseScore = _ref.Get<GameObject>("baseScore").GetComponent<Text>();

                var besu = _ref.Get<GameObject>("besu").GetComponent<Text>();

                if (DDZGameHelper.settle != null)
                {
                    var playerData = DDZGameHelper.settle.PlayerData[i];

                    int realSeatID = DDZGameHelper.ChangeSeat(playerData.ChairId);

                    PlayerHead.sprite = SpriteHelper.GetPlayerHeadSpriteName(playerData.HeadId);

                    PlayerName.text = playerData.NickeName;

                    Status.sprite = playerData.IsLord ? SpriteHelper.GetSprite("ddzgame", "DDZ_dizhu2") : SpriteHelper.GetSprite("ddzgame", "DDZ_nm");

                    Status.gameObject.SetActive(true);

                    besu.text = DDZGameHelper.settle.Beishu[realSeatID].ToString();

                    

                    if (playerData.Score > 0)
                    {
                        WinScore.gameObject.SetActive(true);

                        LoseScore.gameObject.SetActive(false);

                        WinScore.text = "+" + playerData.Score;
                    }
                    else
                    {
                        WinScore.gameObject.SetActive(false);

                        LoseScore.gameObject.SetActive(true);

                        LoseScore.text = playerData.Score.ToString();
                    }

                    //判断自己的分数为正还是为负
                    if (myVo != null)
                    {
                        if (myVo.userID == playerData.UserId)
                        {
                            PlayerName.color = Color.yellow;

                            baseScore.color = Color.yellow;

                            besu.color = Color.yellow;

                            if (playerData.Score >= 0)
                            {
                                Title.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ_Result_shengli");

                                //SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.DDZ_sound_win);

                                Bg.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ_Result_shenglidi");

                                winLab.SetActive(true);

                                LoseLab.SetActive(false);

                                bg.gameObject.SetActive(true);

                                bg.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ_Result_shenglitiao");
                            }
                            else
                            {
                                Title.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ_Result_shibai");

                                //SoundComponent.Instance.PlayClip(DataCenterComponent.Instance.soundInfo.DDZ_sound_lose);

                                Bg.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ_Result_shibaidi");

                                winLab.SetActive(false);

                                LoseLab.SetActive(true);

                                bg.gameObject.SetActive(true);

                                bg.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ_Result_sibaitiao");
                            }

                            Title.SetNativeSize();
                        }
                    }
                }
            }

            if (DDZGameHelper.settle != null)
            {
                baseScoreLab.text = "X" + DDZGameHelper.settle.OtherData[0].ToString();

                bombScoreLab.text = "X"+ DDZGameHelper.settle.OtherData[1].ToString();

                FlowerScoreLab.text = "X" + DDZGameHelper.settle.OtherData[2].ToString();
            }

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
