using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    // Start is called before the first frame update

    public void ShowTipMsg(string msg)
    {
        // 
        var parent = Global.FindChild(MenuManager.Instance.transform, "Canvas");
        // 创建
        GameObject goShowTipMsg = Instantiate(Resources.Load<GameObject>("ShowTipMsg"), parent.transform);
        goShowTipMsg.GetComponent<ShowTipMsg>().m_strMsg = msg;
        Animator ani = goShowTipMsg.GetComponent<Animator>();        
        ani.Play("ShowTipMsg");
        Time.timeScale = 1;
        
    }

    /// <summary>
    /// 蒙皮提示
    /// </summary>
    public void ShowGreenHandCoveringTipMsg(string msg, Action close = null)
    {
        GreenHandCovering greenHandCovering = Global.FindChild<GreenHandCovering>(MenuManager.Instance.transform, "GreenHandCovering");

        greenHandCovering.m_OnClose = close;
        if (greenHandCovering)
        {
            greenHandCovering.m_strMessge = msg;
            greenHandCovering.gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// 显示新手提示框
    /// </summary>
    /// <param name="msg"></param>
    public void ShowGreenHandTipMsg(string msg, bool isGame=true, Action onOk=null)
    {

        GreenHandTipPopup greenHandTipPopup = Global.FindChild<GreenHandTipPopup>(MenuManager.Instance.transform, "GreenHandTipPopup");
        greenHandTipPopup.m_OnOk = onOk;
        if (greenHandTipPopup)
        {
            greenHandTipPopup.m_strMessge = msg;
            greenHandTipPopup.gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// 显示危险提示
    /// </summary>
    public void ShowStabilizationTip(bool isShow)
    {
        // 只显示一次
        if (GameLogic.Instance.IsShowStabilizationTip && isShow)
            return;

        GameObject stabilizationTip = Global.FindChild(MenuManager.Instance.transform, "StabilizationTip");
        if (stabilizationTip)
        {
            bool isShowStabilizationTip = GameLogic.Instance.IsShowStabilizationTip;
            stabilizationTip.SetActive(!isShowStabilizationTip && isShow);
        }

        GameLogic.Instance.IsShowStabilizationTip = isShow;
    }
}
