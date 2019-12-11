/******************************************************************************************
*         【模块】{ 斗地主模块 }                                                                                                                      
*         【功能】{ 滑动选牌组件 }                                                                                                                   
*         【修改日期】{ 2019年7月23日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/

using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETHotfix
{
    [ObjectSystem]
    public class DDZDragSelectionComponentAwakeSystem : AwakeSystem<DDZDragSelectionComponent, GameObject>
    {
        public override void Awake(DDZDragSelectionComponent self, GameObject panel)
        {
            self.Awake(panel);
        }
    }

    public class DDZDragSelectionComponent : Component
    {
        private GameObject panel;

        private int startNum = 0;

        private List<int> dragSelCards;
        

        public DDZDragSelectionComponent Awake(GameObject panel)
        {
            this.panel = panel;
            
            dragSelCards = new List<int>();

            MonoEventComponent.Instance.AddEventTrigger(this.panel, EventTriggerType.BeginDrag, this.OnBeginDrag);

            MonoEventComponent.Instance.AddEventTrigger(this.panel, EventTriggerType.EndDrag, this.OnEndDrag);

            MonoEventComponent.Instance.AddEventTrigger(this.panel, EventTriggerType.Drag, this.OnDrag);

            return this;
        }

        /// <summary>
        /// 开始拖动
        /// </summary>
        /// <param name="data"></param>
        public void OnBeginDrag(BaseEventData baseEventData)
        {
            PointerEventData data = baseEventData as PointerEventData;

            this.dragSelCards.Clear();

            this.startNum = 0;

            this.AddDragSelCards(data);
        }
        
        /// <summary>
        /// 拖动
        /// </summary>
        /// <param name="data"></param>
        public void OnDrag(BaseEventData baseEventData)
        {
            PointerEventData data = baseEventData as PointerEventData;

            this.AddDragSelCards(data);
        }

        /// <summary>
        /// 结束拖动
        /// </summary>
        /// <param name="data"></param>
        public void OnEndDrag(BaseEventData baseEventData)
        {
            this.startNum = 0;

            if (this.dragSelCards.Count > 0)
            {
                foreach (int index in dragSelCards)
                {
                    var uc = DDZConfig.GameScene.DDZHandCardPlugin.GetPokerComponentByIndex(index);

                    if (uc != null)
                    {
                        uc.OnClickCard();

                        uc.resumeGray();
                    }
                }
            }
        }

        /// <summary>
        /// 恢复所有选中牌色彩
        /// </summary>
        private void ResumeGrayDragSelCards()
        {
            if (this.dragSelCards.Count > 0)
            {
                foreach (int index in dragSelCards)
                {
                    var uc = DDZConfig.GameScene.DDZHandCardPlugin.GetPokerComponentByIndex(index);

                    if (uc != null)
                    {
                        uc.resumeGray();
                    }
                }
            }

            this.dragSelCards.Clear();
        }

        /// <summary>
        /// 增加选牌
        /// </summary>
        /// <param name="data"></param>
        private void AddDragSelCards(PointerEventData data)
        {
            if (data.pointerPressRaycast.gameObject == null || data.pointerCurrentRaycast.gameObject == null)
            {
                return;
            }

            if (data.pointerPressRaycast.gameObject.name != "Image") { return; }

            if (data.pointerCurrentRaycast.gameObject.name != "Image") { return; }

            if (data.pointerCurrentRaycast.gameObject.transform.childCount != 1) { return; }

            if (data.pointerCurrentRaycast.gameObject.transform.GetChild(0).name != "dizhu") { return; }
 
            this.startNum = int.Parse(data.pointerPressRaycast.gameObject.transform.parent.name);

            int nCurSelNum = this.startNum;

            try
            {
                nCurSelNum = int.Parse(data.pointerCurrentRaycast.gameObject.transform.parent.name);
            }
            catch (System.Exception e)
            {

                Log.Debug("转换失败!!");
            }
            
            int nCurStep = (nCurSelNum <= startNum) ? -1 : 1;

            this.ResumeGrayDragSelCards();

            for (int i = (nCurStep == 1 ? startNum : nCurSelNum); i <= (nCurStep == 1 ? nCurSelNum : startNum); i++)
            {
                if (!this.dragSelCards.Contains(i))
                {
                    this.dragSelCards.Add(i);

                    var uc = DDZConfig.GameScene.DDZHandCardPlugin.GetPokerComponentByIndex(i);

                    if (uc != null)
                    {
                        uc.setGray();
                    }
                }
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
