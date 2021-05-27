using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 用户信息
/// </summary>
public class UserInfoPanel : MonoBehaviour
{
    private Text m_textGold;

    private Image m_imgHead;

    Button Ad_Button;

    void Awake()
    {
        m_textGold = Global.FindChild(transform, "text_gold").GetComponent<Text>();
        m_imgHead = Global.FindChild(transform, "head").GetComponent<Image>();

        EventDispatcher.Instance.AddEventListener(EventKey.OnGoldChange, onCallBackGoldChange);
        EventDispatcher.Instance.AddEventListener(EventKey.OnCharacterChange, onCallBackCharacterChange);
      
        // m_imgHead
        // 初始化显示
        onCallBackGoldChange();
        onCallBackCharacterChange();
    }

    //private void Start()
    //{
    //    Ad_Button = Global.FindChild(transform, "advertising_Button").GetComponent<Button>();
    //    if (Ad_Button)
    //    {
           
    //    }
    //}
    void OnDestroy()
    {
        EventDispatcher.Instance.RemoveEventListener(EventKey.OnGoldChange, onCallBackGoldChange);
        EventDispatcher.Instance.RemoveEventListener(EventKey.OnCharacterChange, onCallBackCharacterChange);
    }

    void onCallBackGoldChange(string msg = "")
    {
        m_textGold.text = ProfileManager.Instance.Gold.ToString();
    }

    void onCallBackCharacterChange( string msg = "" )
    {
        m_imgHead.sprite = ProfileManager.Instance.blobs[ProfileManager.Instance.BlobIndex].Image;
    }
    void OnDisable()
    {
       
    }

    public void ClickAdButton()
    {
        EventDispatcher.Instance.AddEventListener(EventKey.AdShowSuccessCallBack, OnAdShowSuccessCallBack);
        PluginMercury.Instance.ActiveRewardVideo();
    }
    public void OnAdShowSuccessCallBack(string msg = "")
    {
        ProfileManager.Instance.Gold += 200;
        Debug.LogError("看广告获得奖励！");
        EventDispatcher.Instance.RemoveEventListener(EventKey.AdShowSuccessCallBack, OnAdShowSuccessCallBack);
    }
}
