using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    private void Awake()
    {
        if (SystemInfo.graphicsShaderLevel >= 35)
            QualitySettings.shadows = ShadowQuality.HardOnly;
        else
            QualitySettings.shadows = ShadowQuality.Disable;
    }

}
