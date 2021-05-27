using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager
{
    /// <summary>
    /// 是否打印错误
    /// </summary>
    public static bool isPrintErr { set; get; }
    /// <summary>
    /// 是否打印信息
    /// </summary>
    public static bool isPrintInfo { set; get; }

    /// <summary>
    /// 打印信息
    /// </summary>
    /// <param name="value"></param>
    public static void LogInfo(string value)
    {
        if(isPrintInfo) Debug.Log(value);
    }

    /// <summary>
    /// 打印错误
    /// </summary>
    /// <param name="value"></param>
    public static void LogError(string value)
    {
        if (isPrintErr) Debug.Log(value);
    }
}
