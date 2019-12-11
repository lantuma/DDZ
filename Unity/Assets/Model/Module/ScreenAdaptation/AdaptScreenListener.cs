using UnityEngine;
using System.Collections;


public class AdaptScreenListener : MonoBehaviour
{
    //设计分辨率宽
    public float DESIGN_WIDTH = 1136.0f;
    //设计分辨率高
    public float DESIGN_HEIGHT = 640.0f;


    //X宽(偏差一个像素做调整)
    private const float SPECIAL_WIDHT  = 2436.0f - 1.0f;
    //X高
    private const float SPRECAL_HEIGHT = 1125.0f;
    //X刘海长度
    //X刘海长度
    private const float DELTA_PIXEL = 90.0f;

    //添加渐变的临界比率
    private float _limitScsle = 0;

    //记录Screen属性，方便动态修改
    private float _screenWidth   = 0.0f;
    private float _screenHeight  = 0.0f;

    // Use this for initialization
    void Start()
    {
        //求出临界值
        _limitScsle = SPECIAL_WIDHT/SPRECAL_HEIGHT;

        //记录上一次Scene属性
        _screenHeight = Screen.height;
        _screenWidth = Screen.width;

        ResetSize();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_screenWidth != Screen.width || _screenHeight != Screen.height)
        {
            _screenHeight = Screen.height;
            _screenWidth = Screen.width;

            ResetSize();
        }
	}

    private void ResetSize()
    {
        float limitWidth = _limitScsle*DESIGN_HEIGHT;
        float trueWidth = (_screenWidth/_screenHeight)*DESIGN_HEIGHT;
        if (trueWidth - limitWidth > 0)
        {
            Vector2 v2 = ((RectTransform)transform).sizeDelta;
            float delta =2 * (DELTA_PIXEL*DESIGN_HEIGHT/SPRECAL_HEIGHT );
            v2.x = -1 * delta;
            ((RectTransform)transform).sizeDelta = v2;
            
        }
        else
        {
            Vector2 v2 = ((RectTransform)transform).sizeDelta;
            float delta = 0;
            v2.x = delta;
            ((RectTransform)transform).sizeDelta = v2;
            
        }
    }
}
