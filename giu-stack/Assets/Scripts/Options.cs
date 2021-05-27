using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{

    const string open = "Options_open";
    const string close = "Options_close";

    [SerializeField]
    Animator anim;

    [SerializeField]
    Toggle music;
    [SerializeField]
    Toggle sound;

    bool isOpen;

    private void Awake()
    {
        music.onValueChanged.AddListener((isOn) => 
        {
            ProfileManager.Instance.Music = !isOn;
            GameAnalyticsManager.LogDesignEvent("ChangedMusic" + ":" + ProfileManager.Instance.Music);
        });

        sound.onValueChanged.AddListener((isOn) => 
        {
            ProfileManager.Instance.Sound = !isOn;
            GameAnalyticsManager.LogDesignEvent("ChangedSound" + ":" + ProfileManager.Instance.Sound);
        });

        music.isOn = !ProfileManager.Instance.Music;
        sound.isOn = !ProfileManager.Instance.Sound;
    }

    private void OnEnable()
    {
        anim.Play("Options_close");
        isOpen = false;
    }

    public void OpenClose()
    {
        if (isOpen)
        {
            anim.Play(close);
            isOpen = false;
        }
        else
        {
            anim.Play(open);
            isOpen = true;
        }
    }

}
