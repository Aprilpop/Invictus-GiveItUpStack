using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[System.Serializable]
public class allShopConfig
{

    public ShopInfo[] m_arrShopinfo;
}

public class ShopControllerPanel : MonoBehaviour
{

    public Button[] selectButton;

    public eCurrencyType curSelect;

    public ScrollRect m_scrollRect;

    public ShopItemController shopItem;

    [Header("商品配置信息")]
    public allShopConfig[] m_shopConfig;

    protected void OnEnable()
    {
        // 点击广告
        Global.FindChild(transform, "btn_ad").GetComponent<Button>().onClick.AddListener(this.onCallOpenAd);

        // 注册广告加载成功的回调
        EventDispatcher.Instance.AddEventListener(EventKey.AdShowSuccessCallBack, onAdShowSuccessCallBack);
        Debug.Log("初始化成功：");
        EventDispatcher.Instance.AddEventListener(EventKey.PurchaseSuccessCallBack, onPurchaseSuccessCallBack);

    }


    private void Start()
    {

        this.InitShop();
    }

    void OnDisable()
    {
        // 注销广告加载成功事件
        EventDispatcher.Instance.RemoveEventListener(EventKey.AdShowSuccessCallBack, onAdShowSuccessCallBack);
        EventDispatcher.Instance.RemoveEventListener(EventKey.PurchaseSuccessCallBack, onPurchaseSuccessCallBack);
    }

    private void ButtonSelect()
    {
        GameObject curButton = EventSystem.current.currentSelectedGameObject;
        string strIndex = curButton.name.Split(new char[] { '_' })[1]; // 获取下标
        this.curSelect = (eCurrencyType)Convert.ToInt32(strIndex);

        if (m_scrollRect == null)
        {
            this.m_scrollRect = this.transform.GetComponentInChildren<ScrollRect>();
        }

        this.Select();
    }


    private void Select()
    {

        if (null == selectButton)
        {
            return;
        }

        for (int index = 0; index < this.selectButton.Length; index++)
        {
            Transform button_shop_select = this.selectButton[index].transform.Find("button_shop_select");
            Transform button_shop = this.selectButton[index].transform.Find("button_shop");
            button_shop_select.gameObject.SetActive((eCurrencyType)index == this.curSelect);
            button_shop.gameObject.SetActive((eCurrencyType)index != this.curSelect);
        }

        // 先清理掉所有
        for (int i = 0; i < m_scrollRect.content.childCount; i++)
        {
            Destroy(m_scrollRect.content.GetChild(i).gameObject);
        }

        Invoke("crate", Time.deltaTime);
        //再刷新


        //this.m_scrollRect.UpdateLayout();
    }


    private void crate()
    {
        if (m_shopConfig.Length <= 0)
        {
            return;
        }

        // 得到当前选中商品类型的商品数量
        int curCount = m_shopConfig[(int)this.curSelect].m_arrShopinfo.Length;

        for (int i = 0; i < curCount; i++)
        {
            ShopItemController go = Instantiate<ShopItemController>(shopItem);
            go.m_parent = this;

            go.m_shopInfo = m_shopConfig[(int)this.curSelect].m_arrShopinfo[i];
            go.curSelect = this.curSelect;
            go.transform.SetParent(m_scrollRect.content);

            //this.m_scrollRect.AddChild(go.gameObject);
        }
    }

    private void InitShop()
    {
        foreach (var item in selectButton)
        {
            item.onClick.AddListener(this.ButtonSelect);
        }

        this.Select();
    }


    // 根据差额 获取充值钻石登记 
    public ShopInfo getRechargeDiaGrade(int balanceCount, eCurrencyType type = eCurrencyType.diamond)
    {
        foreach (var shopInfo in m_shopConfig[(int)type].m_arrShopinfo)
        {
            if (balanceCount <= shopInfo.m_iCout)
            {
                return shopInfo;
            }

        }

        // 如果找不到那么返回最大充值等级
        return m_shopConfig[(int)type].m_arrShopinfo[m_shopConfig[(int)type].m_arrShopinfo.Length - 1];
    }

    // 返回
    public void onCallBack()
    {
        // 隐藏当前
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 打开广告
    /// </summary>
    public void onCallOpenAd()
    {
        PluginMercury.Instance.ActiveRewardVideo();
    }

    ShopInfo onFindShopInfo(string m_strProductId)
    {
        // 商品总类
        foreach (var item in m_shopConfig)
        {
            // 每类个数
            foreach (var shopInfo in item.m_arrShopinfo)
            {
                if (shopInfo.m_strProductId == m_strProductId)
                {
                    return shopInfo;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 支付成功回调
    /// </summary>
    void onPurchaseSuccessCallBack(string msg)
    {
        DebugManager.LogInfo("支付回调成功->正在加游戏币,当前订单ID" + msg);
        ShopInfo shopInfo = onFindShopInfo(msg);
        if (shopInfo == null)
        {
            DebugManager.LogInfo("找不到该订单");
            return;
        }

        switch (shopInfo.m_eType)
        {
            case eCurrencyType.gold:
                ProfileManager.Instance.Gold += shopInfo.m_iCout;
                break;
            case eCurrencyType.diamond:
                ProfileManager.Instance.Diamond += shopInfo.m_iCout;
                break;
            default:
                break;
        }
        
        UIManager.Instance.ShowTipMsg("充值成功！");
    }
    
    /// <summary>
    /// 当广告加载成功之后
    /// </summary>
    void onAdShowSuccessCallBack(string msg)
    {
        UIManager.Instance.ShowTipMsg("金币领取成功！");
        // 加金币
        ProfileManager.Instance.Gold += ProfileManager.Instance.AdGold;
    }
}
