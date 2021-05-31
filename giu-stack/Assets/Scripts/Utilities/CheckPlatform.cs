using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if ((int)StarkSDKSpace.StarkSDK.s_ContainerEnv.m_HostEnum == 2 || (int)StarkSDKSpace.StarkSDK.s_ContainerEnv.m_HostEnum == 4)
        {
            gameObject.SetActive(true);
        }
        else gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
