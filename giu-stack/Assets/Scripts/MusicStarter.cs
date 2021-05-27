using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStarter : MonoBehaviour
{

    private void Start()
    {
        MusicManager.Instance.PlayMusic(ProfileManager.Instance.EMusic);
        DontDestroyOnLoad(gameObject);
    }

}
