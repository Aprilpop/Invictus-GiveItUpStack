using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GreenHandTipPopup : MonoBehaviour
{

    public Action m_OnOk = delegate { };

    [HideInInspector] public string m_strMessge;
    private Text m_textMessge;
    private Button m_btnContinue;
    void Awake()
    {
         m_textMessge = Global.FindChild<Text>(this.transform, "messge");
         m_btnContinue = Global.FindChild<Button>(this.transform, "btn_continue");
         
         Global.FindChild<Button>(this.transform, "Panel").onClick.AddListener( onCallBackContinue );         
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

    void Update()
    {
        
    }

    // 点击继续
    void onCallBackContinue()
    {
        // 关闭
        gameObject.SetActive(false);

        // 游戏继续
        if (m_OnOk != null)
        {
            m_OnOk();
        }
    }
}
