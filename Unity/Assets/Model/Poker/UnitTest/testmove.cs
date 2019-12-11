using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testmove : MonoBehaviour
{
    public Text xx;
    public List<GameObject> rewardList;
    // 所有奖励列表 
    public GameObject signNode;
    // 停止标记 
    public int targetIndex;
    // 停止的目标位置(从1开始) 
    public float startSpeed = 0.08f;
    //初始间隔 
    public float durationVal = 0.005f;//减速时间递增数 
    public int loopTimes = 3;//减速循环圈数 
    public int startLoopTime = 8;//初始循环次数(不减速) 
    public float startDur = 0.01f;//初始循 环间隔 
    class StepData
    {
        public int index;//停留位置 
        public float waitTime;//该步骤等待时间 
    } 
    // 每一步数据 
    private List<StepData> m_pieceList = new List<StepData>();
    private float m_time = 0;
    //记录每一步已经消耗的时间 
    private bool m_isStart = false;
    private int m_playIdx = 0;//当前播放的步骤下标 

    private void Start()
    {
        string bb = "00000000000";
        bb = bb.Insert(2, " ");
        bb=bb.Insert(6, " ");
        bb = bb.Insert(10, " ");
        xx.text = bb;
        float slowDown_time = 0;
        var useTime = startSpeed;
        var duration = durationVal; // 随机初始位置 
        var startIndex = Random.Range(0, rewardList.Count); setPostion(startIndex);
        var curIdx = startIndex; // 当前停止位置(用于计算循环次数) 
        var curCalIdx = startIndex; // 当前减速状态循环圈数 
        int m_curLoopTime = 0; // 当前初始不减速状态循环总圈数 
        var m_curStartLoopTime = 0; // 总循环圈数 
        var subLoopTime = 0; // 是否可以开始减速 
        bool canAddDur = false;
        while (true)
        {
            curIdx = getRightIndex(curIdx);
            var step_data = new StepData();
            step_data.index = curIdx;
            curIdx++; // 回到初始位置 则圈数+1 
            if (curCalIdx == 0)
            {
                if (canAddDur) m_curLoopTime += 1;
                m_curStartLoopTime += 1;
                subLoopTime += 1;
            }
            // 是否可以开始减速 
            canAddDur = m_curStartLoopTime >= startLoopTime;
            if (canAddDur)
            { //累计减速时间 逐渐变慢 
                slowDown_time += duration;
                step_data.waitTime = slowDown_time;
            }
            else//匀速播放 
                step_data.waitTime = startDur; // 已到达最终奖励位置 
            if (curCalIdx == targetIndex && m_curLoopTime == loopTimes && canAddDur)
                break;
            m_pieceList.Add(step_data);
            curCalIdx += 1;
            curCalIdx = getRightIndex(curCalIdx);
        }
        Debug.Log("loop finish,times = " + subLoopTime);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_isStart = !m_isStart;
            if (m_isStart)
            {
                targetIndex = UnityEngine.Random.Range(0, 9);
                Start();
            }
        }
        if (m_isStart)
        {
            m_time += Time.deltaTime;
            if (m_pieceList.Count > 0)
            {
                if (m_playIdx >= m_pieceList.Count)
                {
                    Debug.Log("Play done!");
                    m_isStart = false;
                    return;
                }
                var data = m_pieceList[m_playIdx];
                if (m_time >= data.waitTime)
                {
                    m_time = 0; m_playIdx++;
                    setPostion(data.index);
                }
            }
        }
    }

    /// <summary>
    /// 下标修正
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    int getRightIndex(int idx)
    {
        if (idx >= rewardList.Count)
        {
            idx = 0;
        }
        return idx;
    }

    /// <summary>
    /// 设置标记位置
    /// </summary>
    /// <param name="index"></param>
    void setPostion(int index)
    {
        signNode.transform.position = rewardList[index].gameObject.transform.position;
    }
}