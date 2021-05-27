using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ePopupType
{
    general = 0,    // 通用框
    confirm,        // 确定框

    buy,            // 购买角色等
    currencyDef,    // 购买货币不足
    buySucceed,     // 购买成功

}

/// <summary>
/// 通用提示框
/// </summary>
public class TipPopup : MonoBehaviour
{
    public Button m_btnYes;
    public Button m_btnCancel;
    public Button m_btnOk;
    public Button m_btnAd;
    public Text m_textHander;
    public Text m_textMessage;

    public Text m_textHeader;
    public Text m_textShop;
    public Image m_imgCharacter;
    private string m_strHeader = "提示";
    private string m_strMessage;
    private ePopupType m_ePopupType;
    public int m_iCout;
    public Sprite m_spriteCharacter;
    [HideInInspector]
    public Action OnYesPress = delegate { };
    [HideInInspector]
    public Action OnNoPress = delegate { };
    [HideInInspector]
    public Action OnOkPress = delegate { };


    private static TipPopup instance;

    public static TipPopup Instance
    {
        get
        {
            if (instance == null)
            {
                instance = MenuManager.Instance.m_tipPopup;
                return instance;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {

        m_btnAd = Global.FindChild(transform, "btn_ad").GetComponent<Button>();
    }

    private void OnEnable()
    {
        // 注册广告加载成功的回调
        EventDispatcher.Instance.AddEventListener(EventKey.AdShowSuccessCallBack, SelectMenu.Instance.onAdShowSuccessCallBack);
        Debug.Log("初始化成功：");
        this.Init();

        m_textHeader.text = m_strHeader;
        switch (m_ePopupType)
        {
            case ePopupType.general:
                this.General();
                break;
            case ePopupType.confirm:
                this.Confirm();
                break;
            case ePopupType.buy:
                this.ShopTip();
                break;
            case ePopupType.currencyDef:
                this.CurrencyDef();
                break;
            case ePopupType.buySucceed:
                this.BuySucceed();
                break;
            default:
                break;
        }

    }

    private void OnDisable()
    {
        m_btnYes.onClick.RemoveAllListeners();
        m_btnCancel.onClick.RemoveAllListeners();
        m_btnOk.onClick.RemoveAllListeners();
        m_btnAd.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// 初始化全部隐藏
    /// </summary>
    public void Init()
    {
        m_btnYes.gameObject.SetActive(false);
        m_btnCancel.gameObject.SetActive(false);
        m_btnOk.gameObject.SetActive(false);
        m_textShop.gameObject.SetActive(false);
        m_btnAd.gameObject.SetActive(false);
        m_imgCharacter.gameObject.SetActive(false);
    }
    /// <summary>
    /// 通用
    /// </summary>
    public void General()
    {
        m_btnYes.gameObject.SetActive(true);
        m_btnCancel.gameObject.SetActive(true);
        m_textMessage.gameObject.SetActive(true);

        m_btnYes.onClick.AddListener(() => { OnYesPress(); });
        m_btnCancel.onClick.AddListener(() => { OnNoPress(); });

        m_textMessage.text = m_strMessage;
    }
    /// <summary>
    /// 确定
    /// </summary>
    public void Confirm()
    {
        m_btnOk.gameObject.SetActive(true);
        m_btnCancel.onClick.AddListener(() => { OnOkPress(); });
    }

    void ShopTip()
    {
        this.General();
        m_textShop.gameObject.SetActive(true);
        m_textShop.text = m_iCout.ToString();
    }

    /// <summary>
    /// 货币不足
    /// </summary>
    void CurrencyDef()
    {
        this.General();
        m_btnOk.gameObject.SetActive(false);
        m_btnYes.gameObject.SetActive(false);

        m_btnAd.gameObject.SetActive(true);
        m_btnAd.onClick.AddListener(onCallBackAd);
    }

    /// <summary>
    /// 购买成功
    /// </summary>
    void BuySucceed()
    {
        this.General();
        m_textMessage.gameObject.SetActive(false);
        m_imgCharacter.gameObject.SetActive(true);
        m_imgCharacter.sprite = m_spriteCharacter;
        m_imgCharacter.GetComponentInChildren<Text>().text = m_strMessage;
    }


    /// <summary>
    /// 打开通用弹窗
    /// </summary>
    /// <param name=""></param>
    public void Open(ePopupType eType, string msg, Action onYes = null, Action onNo = null, string strHeader = "提示")
    {
        m_ePopupType = eType;
        m_strHeader = strHeader;
        m_strMessage = msg;
        this.gameObject.SetActive(true);

        OnYesPress = () =>
        {
            this.gameObject.SetActive(false);
            if (onYes != null)
                onYes();
        };
        OnNoPress = () =>
        {
            this.gameObject.SetActive(false);
            if (onNo != null)
                onNo();
        };
        OnOkPress = () =>
        {
            this.gameObject.SetActive(false);
            if (onYes != null)
                onYes();
        };
    }

    void onCallBackAd()
    {
        PluginMercury.Instance.ActiveRewardVideo();
    }

}
