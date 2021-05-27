using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GreenHandCovering : MonoBehaviour
{
    [HideInInspector]
    public string m_strMessge;

    public Action m_OnClose = delegate { };

    private Button m_btnContinue;
    private Text m_textMessge;
    void Awake()
    {
        m_textMessge = Global.FindChild<Text>(this.transform, "messge");
        m_btnContinue = Global.FindChild<Button>(this.transform, "m_btnContinue");        
    }

    void OnEnable()
    {
        m_btnContinue.onClick.AddListener(onCallBackContinue);
        Time.timeScale = 0;
        m_textMessge.text = m_strMessge;
    }

    void OnDisable()
    {
        m_btnContinue.onClick.RemoveAllListeners();
        Time.timeScale = 1;
    }

    // 点击继续
    void onCallBackContinue()
    {
        // 关闭
        gameObject.SetActive(false);

        // 游戏继续
        if (m_OnClose != null)
        {
            m_OnClose();
        }
    }
}
