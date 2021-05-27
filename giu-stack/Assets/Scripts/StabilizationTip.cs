using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilizationTip : MonoBehaviour
{
    // Start is called before the first frame update
    /// <summary>
    /// 动画播完隐藏
    /// </summary>
    void OnAnimationEventHide()
    {
        gameObject.SetActive(false);
    }
}
