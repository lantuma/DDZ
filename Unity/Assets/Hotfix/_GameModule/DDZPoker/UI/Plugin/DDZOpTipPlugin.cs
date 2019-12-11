/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 操作提示插件}                                                                                                                   
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
    public class DDZOpTipPluginAwakeSystem : AwakeSystem<DDZOpTipPlugin, GameObject>
    {
        public override void Awake(DDZOpTipPlugin self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class DDZOpTipPlugin : Component
    {
        private GameObject panel;

        private ReferenceCollector _rf;


        public DDZOpTipPlugin Awake(GameObject panel)
        {
            this.panel = panel;

            _rf = this.panel.GetComponent<ReferenceCollector>();
            
            return this;
        }

        public void Reset()
        {
            for (int i = 0; i < 3; i++)
            {
                _rf.Get<GameObject>("Op" + i).SetActive(false);

                _rf.Get<GameObject>("Warn" + i).SetActive(false);
            }
        }

        public void HideByIndex(int _index)
        {
            _rf.Get<GameObject>("Op" + _index).SetActive(false);
        }

        public void CheckClearALL(int realSeatID)
        {
            GameObject op1 = null;

            GameObject op2 = null;

            if (realSeatID == 0)
            {

                op1 = _rf.Get<GameObject>("Op" + 1);

                op2 = _rf.Get<GameObject>("Op" + 2);
            }
            else if (realSeatID == 1)
            {
                op1 = _rf.Get<GameObject>("Op" + 0);

                op2 = _rf.Get<GameObject>("Op" + 2);
            }
            else if (realSeatID == 2)
            {
                op1 = _rf.Get<GameObject>("Op" + 0);

                op2 = _rf.Get<GameObject>("Op" + 1);
            }

            if (op1.name == op2.name && (op1.name == "buchu" || op1.name == "fen") && op1.activeSelf && op2.activeSelf)
            {
                //先在这里清除上一局的出牌

                DDZGameConfigComponent.Instance.preOutCard = null;

                DDZConfig.GameScene.DDZMaskPlugin.Hide();

                this.Reset();
            }
        }

        /// <summary>
        /// 检测是否自己主动出牌
        /// </summary>
        /// <returns></returns>
        public bool SelfIsFirstOut()
        {
            GameObject op1 = _rf.Get<GameObject>("Op" + 1);

            GameObject op2 = _rf.Get<GameObject>("Op" + 2);

            if (op1.name == op2.name && (op1.name == "buchu" || op1.name == "fen") && op1.activeSelf && op2.activeSelf)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 显示操作提示
        /// </summary>
        /// <param name="_index">位置ID</param>
        /// <param name="opType">操作类型0:要不起 </param>
        public async void ShowOpTipById(int _index, int opType,bool destroy = false)
        {
            var tip = _rf.Get<GameObject>("Op" + _index).GetComponent<Image>();

            tip.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ2_buchupai");

            tip.gameObject.SetActive(true);

            tip.gameObject.name = "buchu";

            tip.SetNativeSize();

            if (destroy)
            {

                await Task.Delay(1000);

                tip.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 显示叫分
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="score"></param>
        public void CallScoreById(int _index, int score)
        {
            var tip = _rf.Get<GameObject>("Op" + _index).GetComponent<Image>();

            if (score == 0)
            {
                tip.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ2_bujiaodizhu");
            }
            else
            {
                tip.sprite = SpriteHelper.GetSprite("ddzgame", "DDZ2_" + score + "fen");
            }

            tip.gameObject.SetActive(true);

            tip.SetNativeSize();

            tip.gameObject.name = "fen";
        }

        /// <summary>
        /// 显示警报器
        /// </summary>
        /// <param name="_index"></param>
        public void ShowWarnById(int _index)
        {
            _rf.Get<GameObject>("Warn" + _index).SetActive(true);
        }

        /// <summary>
        /// 显示警报器
        /// </summary>
        public void ClearAllWarn()
        {
            for (int i = 0; i < 3; i++)
            {
                _rf.Get<GameObject>("Warn" + i).SetActive(false);
            }
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
