using ETModel;
using System;
using UnityEngine;

public class LText : MonoBehaviour
{
    public void Start()
    {
        Debug.Log(GetTimeStamp());
     
    }
    public  string GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds).ToString();
    }
}









