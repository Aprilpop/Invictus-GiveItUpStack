using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class FailPanel : MonoBehaviour
{

    public Text m_textRestart;
    void Awake()
    {
        m_textRestart.text = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Restart"));
    }

    // banner
    private void OnEnable()
    {
        //PluginMercury.Instance.ActiveBanner();
    }
}
