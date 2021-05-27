using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondChance : MonoBehaviour
{

    Button m_btnAdResurgence;
    Button m_btnGoldResurgence;
    public static bool isNotBlock { get; set; }
    
    Text m_textShow;
    void OnEnable()
    {
        EventDispatcher.Instance.AddEventListener(EventKey.AdShowSuccessCallBack, OnAdShowSuccessCallBack);
        isNotBlock = true;
    }

    void Start()
    {
        m_btnAdResurgence = Global.FindChild(this.transform, "AdResurgence").GetComponent<Button>();
        m_btnGoldResurgence = Global.FindChild(this.transform, "GoldResurgence").GetComponent<Button>();
        m_textShow = Global.FindChild(this.transform, "txt_show").GetComponent<Text>();

        m_textShow.text = "消耗"+ProfileManager.Instance.ResurgenceGold+"金币复活";
  
        if (m_btnAdResurgence)
        {
            m_btnAdResurgence.onClick.AddListener(this.onCallAdResurgence);
        }

        if (m_btnGoldResurgence)
        {
            m_btnGoldResurgence.onClick.AddListener(this.onCallGoldResurgence);
        }
    }

    void OnDisable()
    {
        EventDispatcher.Instance.RemoveEventListener(EventKey.AdShowSuccessCallBack, OnAdShowSuccessCallBack);   
    }

    // 广告
    void onCallAdResurgence()
    {
        isNotBlock = false;
        PluginMercury.Instance.ActiveRewardVideo();
    }

    // 购买
    void onCallGoldResurgence()
    {
        DebugManager.LogInfo("点击购买复活");


        if (ProfileManager.Instance.Gold < ProfileManager.Instance.ResurgenceGold)
        {
            UIManager.Instance.ShowTipMsg("金币不足");
            // 金币不足
            return;
        }

        ProfileManager.Instance.Gold -= ProfileManager.Instance.ResurgenceGold;
        GameResurgence();

    }

    /// <summary>
    /// 游戏复活
    /// </summary>
    void GameResurgence()
    {
        // 先恢复所有平台
        Spawner.Instance.GameResurgence();
        // 再回复角色位置
        ProfileManager.Instance.CharacterController.GameResurgence();

        GameLogic.Instance.InGame = true;

        this.gameObject.SetActive(false);

        MenuManager.Instance.PauseGame(false);  //显示游戏暂停按钮

        ParticleManager.Instance.death.gameObject.SetActive(false);

        // 开始平台
        Spawner.Instance.CreatePlatform();
    }

    // 广告看完返回。之后复活
    void OnAdShowSuccessCallBack(string msg="")
    {
        DebugManager.LogInfo("==========看广告完成-->复活");
        GameResurgence();
    }
}
