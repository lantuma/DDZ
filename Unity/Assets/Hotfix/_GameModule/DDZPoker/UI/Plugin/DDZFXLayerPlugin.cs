/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 特效层插件}                                                                                                                   
*         【修改日期】{ 2019年5月28日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class DDZFXLayerPluginAwakeSystem : AwakeSystem<DDZFXLayerPlugin, GameObject>
    {
        public override void Awake(DDZFXLayerPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    [ObjectSystem]
    public class DDZFXLayerPluginUYpdateSystem : UpdateSystem<DDZFXLayerPlugin>
    {
        public override void Update(DDZFXLayerPlugin self)
        {
            self.Update();
        }
    }

    public class DDZFXLayerPlugin : Entity
    {
        private GameObject panel;

        private ReferenceCollector _rf;

        private GameObject DDZFX_Bomb;//炸弹

        private GameObject DDZFX_LianDui;//连队

        private GameObject DDZFX_Plane;//飞机

        private GameObject DDZFX_ShunZi;//顺子

        private GameObject DDZFX_Spring;//春天

        private GameObject DDZFX_WangFrie;//王炸

        private GameObject DDZFX_LordWin;//地主赢

        private GameObject DDZFX_LordLost;//地主输

        private GameObject DDZFX_NMWin;//农民赢

        private GameObject DDZFX_NMLost;//农民输

        private SoundInfo soundInfo = DataCenterComponent.Instance.soundInfo;

        private FXAnimationComponent FXC;

        public DDZFXLayerPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            _rf = this.panel.GetComponent<ReferenceCollector>();

            this.FXC = this.AddComponent<FXAnimationComponent>();

            this.DDZFX_Bomb = _rf.Get<GameObject>("DDZFX_Bomb");

            this.DDZFX_LianDui = _rf.Get<GameObject>("DDZFX_LianDui");

            this.DDZFX_Plane = _rf.Get<GameObject>("DDZFX_Plane");

            this.DDZFX_ShunZi = _rf.Get<GameObject>("DDZFX_ShunZi");

            this.DDZFX_Spring = _rf.Get<GameObject>("DDZFX_Spring");

            this.DDZFX_WangFrie = _rf.Get<GameObject>("DDZFX_WangFrie");

            this.DDZFX_LordWin = _rf.Get<GameObject>("DDZFX_LordWin");

            this.DDZFX_LordLost = _rf.Get<GameObject>("DDZFX_LordLost");

            this.DDZFX_NMWin = _rf.Get<GameObject>("DDZFX_NMWin");

            this.DDZFX_NMLost = _rf.Get<GameObject>("DDZFX_NMLost");

            for (int i = 0; i < 3; i++)
            {
                this.FXC.Add(FXAnimationIdType.DDZFX_Bomb + i, "ddzgame", 2, this.DDZFX_Bomb.transform,null,0,0,false);

                this.FXC.Add(FXAnimationIdType.DDZFX_LianDui + i, "ddzgame", 2f, this.DDZFX_LianDui.transform,null,0,0,false);

                this.FXC.Add(FXAnimationIdType.DDZFX_Plane + i, "ddzgame", 2, this.DDZFX_Plane.transform,null,0,0,false);

                this.FXC.Add(FXAnimationIdType.DDZFX_ShunZi + i, "ddzgame", 2, this.DDZFX_ShunZi.transform,null,0,0,false);
            }

            this.FXC.Add(FXAnimationIdType.DDZFX_Spring, "ddzgame", 2, this.DDZFX_Spring.transform);

            this.FXC.Add(FXAnimationIdType.DDZFX_WangFrie, "ddzgame", 2, this.DDZFX_WangFrie.transform,null,0,0,false);


            this.FXC.Add(FXAnimationIdType.DDZFX_LordWin, "ddzgame", 2f, this.DDZFX_LordWin.transform,null,0,0,false);

            this.FXC.Add(FXAnimationIdType.DDZFX_LordLost, "ddzgame", 2f, this.DDZFX_LordLost.transform,null,0,0,false);

            this.FXC.Add(FXAnimationIdType.DDZFX_NMWin, "ddzgame", 2f, this.DDZFX_NMWin.transform,null,0,0,false);

            this.FXC.Add(FXAnimationIdType.DDZFX_NMLost, "ddzgame", 2f, this.DDZFX_NMLost.transform,null,0,0,false);

            return this;
        }

        public void Update()
        {
            /*

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayFX(DDZ_FX_TYPE.ShunZi, 1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayFX(DDZ_FX_TYPE.LianDui, 1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PlayFX(DDZ_FX_TYPE.Bomb, 1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                PlayFX(DDZ_FX_TYPE.Plane, 1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                PlayFX(DDZ_FX_TYPE.WangFire, 1);
            }
            */
        }

        public void Reset()
        {
            
        }

        /// <summary>
        /// 播放特效
        /// </summary>
        /// <param name="fx"></param>
        public  void PlayFX(DDZ_FX_TYPE fx,int pos)
        {
            if (fx == DDZ_FX_TYPE.Bomb) this.Handle_FXBomb(pos);
            
            if (fx == DDZ_FX_TYPE.LianDui) this.Handle_LianDui(pos);
            
            if (fx == DDZ_FX_TYPE.Plane) this.Handle_Plane(pos);

            if (fx == DDZ_FX_TYPE.ShunZi) this.Handle_ShunZi(pos);

            if (fx == DDZ_FX_TYPE.Spring) this.Handle_FXSpring();

            if (fx == DDZ_FX_TYPE.WangFire) this.Handle_FXWangFrie();

            if (fx == DDZ_FX_TYPE.LordWin) this.Handle_FXLordWin();

            if (fx == DDZ_FX_TYPE.LordLost) this.Handle_FXLordLose();

            if (fx == DDZ_FX_TYPE.NMWin) this.Handle_FXNMWin();

            if (fx == DDZ_FX_TYPE.NMLost) this.Handle_FXNMLose();
        }

        /// <summary>
        /// 炸弹
        /// </summary>
        private void Handle_FXBomb(int pos)
        {
            SoundComponent.Instance.PlayClip(soundInfo.DDZ_fx_boom);

            this.FXC.Play(FXAnimationIdType.DDZFX_Bomb + pos);
        }

        /// <summary>
        /// 连队
        /// </summary>
        /// <param name="pos"></param>
        private void Handle_LianDui(int pos)
        {
            SoundComponent.Instance.PlayClip(soundInfo.DDZ_fx_shuanzi);

            this.FXC.Play(FXAnimationIdType.DDZFX_LianDui + pos);
        }

        /// <summary>
        /// 飞机
        /// </summary>
        private void Handle_Plane(int pos)
        {
            SoundComponent.Instance.PlayClip(soundInfo.DDZ_fx_plane);

            this.FXC.Play(FXAnimationIdType.DDZFX_Plane + pos);
        }

        /// <summary>
        /// 顺子
        /// </summary>
        /// <param name="pos"></param>
        private  void Handle_ShunZi(int pos)
        {
            SoundComponent.Instance.PlayClip(soundInfo.DDZ_fx_shuanzi);

            this.FXC.Play(FXAnimationIdType.DDZFX_ShunZi + pos);
        }

        /// <summary>
        /// 春天
        /// </summary>
        private void Handle_FXSpring()
        {
            SoundComponent.Instance.PlayClip(soundInfo.DDZ_fx_spring);

            this.FXC.Play(FXAnimationIdType.DDZFX_Spring);
        }

        /// <summary>
        /// 王炸
        /// </summary>
        private void Handle_FXWangFrie()
        {
            SoundComponent.Instance.PlayClip(soundInfo.DDZ_fx_huojian);

            this.FXC.Play(FXAnimationIdType.DDZFX_WangFrie);
        }

        /// <summary>
        /// 地主赢
        /// </summary>
        private void Handle_FXLordWin()
        {
            SoundComponent.Instance.PlayClip(soundInfo.DDZ_sound_win);

            this.FXC.Play(FXAnimationIdType.DDZFX_LordWin);
        }

        /// <summary>
        /// 地主输
        /// </summary>
        private void Handle_FXLordLose()
        {
            SoundComponent.Instance.PlayClip(soundInfo.DDZ_sound_lose);

            this.FXC.Play(FXAnimationIdType.DDZFX_LordLost);
        }

        /// <summary>
        /// 农民赢
        /// </summary>
        private void Handle_FXNMWin()
        {
            SoundComponent.Instance.PlayClip(soundInfo.DDZ_sound_win);

            this.FXC.Play(FXAnimationIdType.DDZFX_NMWin);
        }

        /// <summary>
        /// 农民输
        /// </summary>
        private void Handle_FXNMLose()
        {
            SoundComponent.Instance.PlayClip(soundInfo.DDZ_sound_lose);

            this.FXC.Play(FXAnimationIdType.DDZFX_NMLost);
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
