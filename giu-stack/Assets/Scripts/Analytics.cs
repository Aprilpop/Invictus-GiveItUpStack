using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analytics : MonoBehaviour
{

    private Analytics instance;
    public Analytics Instance
    {
        get
        {
            if (instance == null)
                Instantiate(Resources.Load("Analytics"));
            return instance;

        }
    }

    private void Awake()
    {

        if (instance == null)
            instance = this;

        DontDestroyOnLoad(gameObject);

        if (Application.platform == RuntimePlatform.IPhonePlayer)
            Application.targetFrameRate = 60;

        
    }

    private void FBInitCallBack()
    {

    }

    public void OnApplicationPause(bool paused)
    {

    }

}

