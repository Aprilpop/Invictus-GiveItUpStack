using UnityEngine;
using System.Collections;

/// <summary>
/// 单例类 ，不用挂载 
/// </summary>
public class UIAdapt
{

    private static UIAdapt _instance;
    private static object _lock = new object();
    public static UIAdapt Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new UIAdapt();
                }
                return _instance;
            }
        }
    }

    float originWidth = 640;
    float originHeight = 1334;

    float lastWidth;
    float lastHeight;

    Vector2 nowHW = new Vector2();


    public Vector2 GetNowHW()
    {
        if (Screen.width == lastWidth && Screen.height == lastHeight) return nowHW;

        float ratioHW = originHeight / originWidth;

        int height = (int)(Screen.width * ratioHW);
        int width = 0;
        if (height > Screen.height)
        {
            height = Screen.height;
            width = (int)(height / ratioHW);
        }
        else
        {
            width = Screen.width;
        }

        nowHW.x = width;
        nowHW.y = height;

        lastHeight = Screen.height;
        lastWidth = Screen.width;
        return nowHW;
    }

    public float GetScale()
    {

        Vector2 hw = GetNowHW();
        float yScale = hw.y / originHeight;
        float xScale = hw.x / originWidth;

        return yScale > xScale ? xScale : yScale;

    }
}