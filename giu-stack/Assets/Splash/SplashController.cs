using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour
{

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void OnAnimationFinished()
    {
        SceneManager.LoadSceneAsync("Game");
    }

}
