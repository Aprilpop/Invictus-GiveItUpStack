using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public Image image;

    [HideInInspector]
    public eUnlockType unlockType;

    public Text header;

    public Text description;

    public Text videoText;

    [HideInInspector]
    public int count;

    [HideInInspector]
    public new string name = "Unlock";

    [HideInInspector]
    public int videoCount;

    [HideInInspector]
    public bool videoUnlockable;

    public Text counter;

    public Button videoButton;
    public Button AdButton;

    public SelectItem selectItem;

    private void OnEnable()
    {
      
        header.text = name;

        switch (unlockType)
        {
            case eUnlockType.Play:
                description.text = "Play " + count + " games";
                counter.gameObject.SetActive(true);
                counter.text = ProfileManager.Instance.PlayedGames.ToString() + "/" + count;
                SetVideoText();
                break;
            case eUnlockType.Point:
                description.text = "Reach " + count + " points";
                counter.gameObject.SetActive(false);
                SetVideoText();
                break;
            case eUnlockType.Perfect:
                description.text = "Do " + count + " perfect jump";
                counter.gameObject.SetActive(true);
                counter.text = ProfileManager.Instance.FullPerfectJump.ToString() + "/" + count;
                SetVideoText();
                break;
            case eUnlockType.SecondChance:
                description.text = "Use " + count + " Second chance";
                counter.gameObject.SetActive(true);
                counter.text = ProfileManager.Instance.SecondChanceCount.ToString() + "/" + count;
                SetVideoText();
                break;
            case eUnlockType.Day:
                description.text = "Play " + count + " days in row";
                counter.gameObject.SetActive(true);
                counter.text = ProfileManager.Instance.DayInRow.ToString() + "/" + count;
                SetVideoText();
                break;
            case eUnlockType.HighSpeedChallenge:
                description.text = "Complete " + count + "\n High Speed challenges";
                counter.gameObject.SetActive(true);
                counter.text = ProfileManager.Instance.HighJumpChallenges.ToString() + "/" + count;
                SetVideoText();
                break;
            case eUnlockType.JumpOverChallenge:
                description.text = "Complete " + count + "\n Jump Over challenges";
                counter.gameObject.SetActive(true);
                counter.text = ProfileManager.Instance.JumpOverChallenges.ToString() + "/" + count;
                SetVideoText();
                break;
            case eUnlockType.PerfectJumpChallenge:
                description.text = "Complete " + count + "\n Perfect Jump challenges";
                counter.gameObject.SetActive(true);
                counter.text = ProfileManager.Instance.PerfectJumpChallenges.ToString() + "/" + count;
                SetVideoText();
                break;
            case eUnlockType.ScalePlatformChallenge:
                description.text = "Complete " + count + "\n Scale Platform challenges";
                counter.gameObject.SetActive(true);
                counter.text = ProfileManager.Instance.ScalePlatformChallenges.ToString() + "/" + count;
                SetVideoText();
                break;
            case eUnlockType.GoldBuy:
                // 金币购买
                    description.text = "价格" + "\n" + count + "金币";
                    counter.gameObject.SetActive(true);
                    counter.text = ProfileManager.Instance.Gold.ToString() + "/" + count;
                    SetVideoText();                   
                Debug.Log("测试这个位置的金币获取数是多少：" + count);
                break;
            case eUnlockType.DiamondBuy:
                // 钻石购买
                description.text = "价格" + "\n" + count + "钻石";
                counter.gameObject.SetActive(true);
                counter.text = ProfileManager.Instance.Diamond.ToString() + "/" + count;
                SetVideoText();
                break;
            case eUnlockType.Advertising:
                // 广告解锁
                description.text = "通过视频广告解锁音乐";
                counter.gameObject.SetActive(false);
                SetVideoText();
                break;
            default:
                break;
        }
 
    }
    private void OnDisable()
    {
        videoButton.onClick.RemoveAllListeners();
    //    AdButton.onClick.RemoveAllListeners();

    }

    private void SetVideoText()
    {

        if (videoUnlockable)
        {
            videoButton.gameObject.SetActive(false);
            AdButton.gameObject.SetActive(false);
            if (selectItem.GetType() == typeof(SelectBlob))
                videoText.text = ProfileManager.Instance.blobs[selectItem.index].unlockInfo.CurrentVideoUnlockCount + "/" + ProfileManager.Instance.blobs[selectItem.index].unlockInfo.VideoCount;
            else if (selectItem.GetType() == typeof(SelectEnviroment))
                videoText.text = ProfileManager.Instance.enviroments[selectItem.index].unlockInfo.CurrentVideoUnlockCount + "/" + ProfileManager.Instance.enviroments[selectItem.index].unlockInfo.VideoCount;
            else if (selectItem.GetType() == typeof(SelectMusic))
                videoText.text = ProfileManager.Instance.Musics[selectItem.index].unlockInfo.CurrentVideoUnlockCount + "/" + ProfileManager.Instance.Musics[selectItem.index].unlockInfo.VideoCount;

           
            if(unlockType != eUnlockType.Advertising)
            {
                videoButton.gameObject.SetActive(true);
                videoButton.onClick.AddListener(this.Buy);
            }
            else
            {
                AdButton.gameObject.SetActive(true);
                EventDispatcher.Instance.AddEventListener(EventKey.AdShowSuccessCallBack, OnAdShowSuccessCallBack);
                AdButton.onClick.AddListener(onCallAdResurgence);
            }
           

            //videoButton.onClick.AddListener(()=> 
            //{
            //    AdManagerIronsrc.Instance.ShowVideoAd((succes) =>
            //    {
            //        if (succes)
            //        {
            //            if (selectItem.GetType() == typeof(SelectBlob))
            //            {
            //                ProfileManager.Instance.blobs[selectItem.index].unlockInfo.CurrentVideoUnlockCount++;
            //                if (ProfileManager.Instance.blobs[selectItem.index].unlockInfo.CurrentVideoUnlockCount == ProfileManager.Instance.blobs[selectItem.index].unlockInfo.VideoCount)
            //                {
            //                    ProfileManager.Instance.blobs[selectItem.index].Unlocked = true;
            //                    selectItem.unlocked = true;
            //                    MenuManager.Instance.ClosePopUp();
            //                    SelectMenu.Instance.ChangeImage();
            //                    GameAnalyticsManager.LogDesignEvent("Unlocked" + ":" + selectItem.name + ":VideoAd");
            //                }
            //                videoText.text = ProfileManager.Instance.blobs[selectItem.index].unlockInfo.CurrentVideoUnlockCount + "/" + ProfileManager.Instance.blobs[selectItem.index].unlockInfo.VideoCount;
            //            }
            //            else if (selectItem.GetType() == typeof(SelectEnviroment))
            //            {
            //                ProfileManager.Instance.enviroments[selectItem.index].unlockInfo.CurrentVideoUnlockCount++;
            //                if (ProfileManager.Instance.enviroments[selectItem.index].unlockInfo.CurrentVideoUnlockCount == ProfileManager.Instance.enviroments[selectItem.index].unlockInfo.VideoCount)
            //                {
            //                    ProfileManager.Instance.enviroments[selectItem.index].Unlocked = true;
            //                    selectItem.unlocked = true;
            //                    MenuManager.Instance.ClosePopUp();
            //                    SelectMenu.Instance.ChangeThemeImage();
            //                    GameAnalyticsManager.LogDesignEvent("Unlocked" + ":" + selectItem.name + ":VideoAd");
            //                }
            //                videoText.text = ProfileManager.Instance.enviroments[selectItem.index].unlockInfo.CurrentVideoUnlockCount + "/" + ProfileManager.Instance.enviroments[selectItem.index].unlockInfo.VideoCount;
            //            }
            //            else if (selectItem.GetType() == typeof(SelectMusic))
            //            {
            //                ProfileManager.Instance.Musics[selectItem.index].unlockInfo.CurrentVideoUnlockCount++;
            //                if (ProfileManager.Instance.Musics[selectItem.index].unlockInfo.CurrentVideoUnlockCount == ProfileManager.Instance.Musics[selectItem.index].unlockInfo.VideoCount)
            //                {
            //                    ProfileManager.Instance.Musics[selectItem.index].Unlocked = true;
            //                    selectItem.unlocked = true;
            //                    MenuManager.Instance.ClosePopUp();
            //                    SelectMenu.Instance.ChangeMusicImage();
            //                    GameAnalyticsManager.LogDesignEvent("Unlocked" + ":" + selectItem.name + ":VideoAd");
            //                }
            //                videoText.text = ProfileManager.Instance.Musics[selectItem.index].unlockInfo.CurrentVideoUnlockCount + "/" + ProfileManager.Instance.Musics[selectItem.index].unlockInfo.VideoCount;
            //            }
            //        }
            //    });
            //});

        }
        else
        {
            videoButton.gameObject.SetActive(false);
            AdButton.gameObject.SetActive(false);
            
        }
           
    }

    // 购买
    private void Buy()
    {
        // 如果货币充足 直接购买
        switch (unlockType)
        {
            case eUnlockType.GoldBuy:
                GoldBuy();
                break;
            case eUnlockType.DiamondBuy:
                DiamondBuy();
                break;
            case eUnlockType.Advertising:
                AdBuy();
                break;
            default:
                break;
        }

    }

    // 金币购买
    private void GoldBuy()
    {

        if (ProfileManager.Instance.Gold >= count)
        {

            string strTip = "你正在花费金币购买选中的物品\r\n(" + name + ")";
            // TipPopup.Instance.m_iCout = count;
            // TipPopup.Instance.Open(ePopupType.buy, strTip, ()=>{ 
            //     ProfileManager.Instance.Gold -= count;
            //     onCallBackBuySucceed();
            //  }, Close);

            ProfileManager.Instance.Gold -= count;
           onCallBackBuySucceed();
            return;
        }

        // 金币不够
        // this.gameObject.SetActive(false);        
        TipPopup.Instance.Open(ePopupType.currencyDef, "您当前金币余额不足，是否需要通过观看广告获取金币？", OpenShop, Close, "金币告急");
    }

    // 钻石购买
    private void DiamondBuy()
    {
        if (ProfileManager.Instance.Diamond >= count)
        {
            string strTip = "你正在花费钻石购买选中的物品(" + name + ")";

            TipPopup.Instance.m_iCout = count;
            TipPopup.Instance.Open(ePopupType.buy, strTip, () =>
            {
                // 购买成功            
                ProfileManager.Instance.Diamond -= count;
                onCallBackBuySucceed();
            }, Close);

            return;
        }

        // 钻石不够
        // this.gameObject.SetActive(false);
        TipPopup.Instance.Open(ePopupType.general, "当前钻石不足，请前往商城", OpenShop, Close);
    }

    public void AdBuy()
    {
        onCallBackBuySucceed();
    }

    private void OpenShop()
    {
        MenuManager.Instance.OpenShop();
    }

    private void Close()
    {
        this.gameObject.SetActive(false);
    }

    // 购买成功
    public void onCallBackBuySucceed()
    {
        // 购买成功        
        this.transform.gameObject.SetActive(false);
        this.selectItem.BuySucceed();

        string strTip = "恭喜您获得" + name;
        TipPopup.Instance.m_spriteCharacter = image.sprite;
        // 提示购买成功
        TipPopup.Instance.Open(ePopupType.buySucceed, strTip, BuySucceedOk, null, "购买成功");
    }

    private void BuySucceedOk()
    {
        this.gameObject.SetActive(false);
    }

    void onCallAdResurgence()
    {      
        PluginMercury.Instance.ActiveRewardVideo();
        Debug.Log("看广告解锁音乐！");
    }
    public void OnAdShowSuccessCallBack(string msg = "")
    {
        Buy();
        EventDispatcher.Instance.RemoveEventListener(EventKey.AdShowSuccessCallBack, OnAdShowSuccessCallBack);
    }
}
