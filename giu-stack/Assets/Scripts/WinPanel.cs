using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WinPanel : MonoBehaviour
{

    public Text m_textRestart;
    public Text m_textNext;

    public int m_index;
    Text m_textAward;
    Button m_btn4BetAward;
    Image m_imgIcon;

    void Awake()
    {
        m_textRestart.text = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Restart"));
        m_textNext.text = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Next"));


        m_textAward = Global.FindChild(transform, "text_award").GetComponent<Text>();
        m_btn4BetAward = Global.FindChild(transform, "btn_4BetAward").GetComponent<Button>();
        m_imgIcon = Global.FindChild(transform, "img_icon").GetComponent<Image>();

        m_btn4BetAward.onClick.AddListener(OnClickBack4BetAward);

    
    }

    void OnEnable()
    {
       
        EventDispatcher.Instance.AddEventListener(EventKey.AdShowSuccessCallBack, OnAdShowSuccessCallBack);
        WinAward winAward = ProfileManager.Instance.GetWinAward(GameLogic.Instance.challengeType, m_index);
        m_textAward.text = "X" + winAward.m_iAwardCount;

        m_textAward.gameObject.SetActive(winAward.m_iAwardCount>0);
        m_btn4BetAward.gameObject.SetActive(winAward.m_iAwardCount>0);
        m_imgIcon.gameObject.SetActive(winAward.m_iAwardCount>0);

        // banner
        //PluginMercury.Instance.ActiveBanner();
    }

    void OnDisable()
    {
        EventDispatcher.Instance.RemoveEventListener(EventKey.AdShowSuccessCallBack, OnAdShowSuccessCallBack);
    }

    void OnClickBack4BetAward()
    {
        // 进入广告
        PluginMercury.Instance.ActiveRewardVideo();
    }

    void OnAdShowSuccessCallBack(string msg = "")
    {
        m_btn4BetAward.gameObject.SetActive(false);

        WinAward winAward = ProfileManager.Instance.GetWinAward(eChallengeType.PerfectJump, m_index);
        if (winAward == null || winAward.m_iAwardCount<=0 )
        {
            return;
        }

        UIManager.Instance.ShowTipMsg("金币领取成功！");
        m_textAward.text = "X" + ProfileManager.Instance.MultipleAward * winAward.m_iAwardCount;
        // 观看结算之后加金币 = (倍数-1) * 设定关卡基础奖励
        // 为什么-1    是因为结算框时以及把基础奖励加入了。
        ProfileManager.Instance.Gold += (ProfileManager.Instance.MultipleAward - 1) * winAward.m_iAwardCount;
    }
}



