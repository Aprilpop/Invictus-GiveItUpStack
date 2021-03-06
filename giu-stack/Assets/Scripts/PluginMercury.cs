using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Runtime.InteropServices;
public class PluginMercury : MonoBehaviour
{

#if UNITY_ANDROID
    public static AndroidJavaObject _plugin;
#elif UNITY_IPHONE
    [DllImport ("__Internal")]
    private static extern void ActiveRewardVideo_IOS();
    [DllImport ("__Internal")]
    private static extern void ActiveInterstitial_IOS();
    [DllImport ("__Internal")]
    private static extern void ActiveBanner_IOS();
    [DllImport ("__Internal")]
    private static extern void ActiveNative_IOS();
    [DllImport ("__Internal")]
    private static extern void GameInit(string name);
    [DllImport("__Internal")]
    private static extern void BuyProduct(string s);//购买商品(AppStore)
#endif

    public static PluginMercury pInstance;
    public static PluginMercury Instance
    {
        get
        {
            Debug.Log("syj:::::::::");
            return pInstance;
        }
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }
    void Awake()
    {
    //    PlayerPrefs.DeleteAll();
        if (pInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        pInstance = this;
        GetAndroidInstance();//得到安卓实例
    }

    public void GetAndroidInstance()
    {
#if UNITY_EDITOR
    print("[UNITY_EDITOR]->GetAndroidInstance");
#elif UNITY_ANDROID
        //安卓获取实例
        using (var pluginClass = new AndroidJavaClass("com.singmaan.game.GameActivity"))
        {
            _plugin = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
        }
#elif UNITY_IPHONE
        print("[UNITY_IPHONE]->GameInit()");
        GameInit("GameInit");
#endif
    }

    public void Purchase(string strProductId)
    {
#if UNITY_EDITOR
        print("[UNITY_EDITOR]->Purchase()->strProductId=" + strProductId);
#elif UNITY_ANDROID
        print("[UNITY_ANDROID]->Purchase()->strProductId="+strProductId);
        _plugin.Call("Purchase", strProductId );
#elif UNITY_IPHONE
        BuyProduct(strProductId);
#endif
    }
    public void Redeem()
    {
#if UNITY_EDITOR
        print("[UNITY_EDITOR]->Exchange()");
#elif UNITY_ANDROID
        print("[Android]->Exchange()");_plugin.Call("Redeem");
#endif
    }

    public void GetProductionInfo()
    {
#if UNITY_EDITOR
        print("[UNITY_EDITOR]->Exchange()");
#elif UNITY_ANDROID
        print("[Android]->Exchange()");_plugin.Call("Redeem");
#endif
    }

    public void RestoreProduct()
    {
#if UNITY_EDITOR
        print("[UNITY_EDITOR]->Exchange()");
#elif UNITY_ANDROID
        print("[Android]->Exchange()");_plugin.Call("RestoreProduct");
#endif
    }


    public void ExitGame()
    {
#if UNITY_EDITOR
        print("[UNITY_EDITOR]->ExitGame()");
#elif UNITY_ANDROID
        print("[Android]->ExitGame()");_plugin.Call("ExitGame");
#endif
    }
    public void ActiveRewardVideo()
    {
        ByteDanceSDKManager.Instance.ShowRewardVideo();
        return;
#if UNITY_EDITOR
        print("[UNITY_EDITOR]->ActiveRewardVideo()");
        AdShowSuccessCallBack("playOver");
#elif UNITY_ANDROID
        print("[Android]->ActiveRewardVideo()");_plugin.Call("ActiveRewardVideo");
#elif UNITY_IPHONE
        print("[UNITY_IPHONE]->ActiveRewardVideo()");
        ActiveRewardVideo_IOS();
#endif
    }

    public void ActiveInterstitial()
    {
#if UNITY_EDITOR
        print("[UNITY_EDITOR]->ActiveInterstitial()");
#elif UNITY_ANDROID
        print("[Android]->ActiveInterstitial()");_plugin.Call("ActiveInterstitial");
#elif UNITY_IPHONE
        print("[UNITY_IPHONE]->ActiveInterstitial()");
        ActiveInterstitial_IOS();
#endif
    }
    public void ActiveBanner()
    {
#if UNITY_EDITOR
        print("[UNITY_EDITOR]->ActiveBanner()");
#elif UNITY_ANDROID
        print("[Android]->ActiveBanner()");_plugin.Call("ActiveBanner");
#elif UNITY_IPHONE
        print("[UNITY_IPHONE]->ActiveBanner()");
        ActiveBanner_IOS();
#endif
    }
    public void ActiveNative()
    {
#if UNITY_EDITOR
        print("[UNITY_EDITOR]->ActiveNative()");
#elif UNITY_ANDROID
        print("[Android]->ActiveNative()");_plugin.Call("ActiveNative");
#elif UNITY_IPHONE
        print("[UNITY_IPHONE]->ActiveNative()");
        ActiveNative_IOS();
#endif
    }

    public void PurchaseSuccessCallBack(string msg)
    {
        print("[Unity]->PurchaseSuccessCallBack");
        EventDispatcher.Instance.Dispatch(EventKey.PurchaseSuccessCallBack, msg);
    }
    public void PurchaseFailedCallBack(string msg)
    {
        print("[Unity]->PurchaseFailedCallBack");
        EventDispatcher.Instance.Dispatch(EventKey.PurchaseFailedCallBack, msg);
    }
    public void LoginSuccessCallBack(string msg)
    {
        print("[Unity]->LoginSuccessCallBack");
        EventDispatcher.Instance.Dispatch(EventKey.LoginSuccessCallBack, msg);
    }
    public void LoginCancelCallBack(string msg)
    {
        print("[Unity]->LoginCancelCallBack");
        EventDispatcher.Instance.Dispatch(EventKey.LoginCancelCallBack, msg);
    }
    public void AdLoadSuccessCallBack(string msg)
    {
        print("[Unity]->AdLoadSuccessCallBack");
        EventDispatcher.Instance.Dispatch(EventKey.AdLoadSuccessCallBack, msg);
    }
    public void AdLoadFailedCallBack(string msg)
    {
        print("[Unity]->AdLoadFailedCallBack");
        EventDispatcher.Instance.Dispatch(EventKey.AdLoadFailedCallBack, msg);
    }
	public void AdShowSuccessCallBack(string msg)
    {
        print("[Unity]->AdShowSuccessCallBack");
        EventDispatcher.Instance.Dispatch(EventKey.AdShowSuccessCallBack, msg);
    }
    public void AdShowFailedCallBack(string msg)
    {
        print("[Unity]->AdShowFailedCallBack");
        EventDispatcher.Instance.Dispatch(EventKey.AdShowFailedCallBack, msg);
    }
    public void onFunctionCallBack(string msg)
    {
        print("[Unity]->onFunctionCallBack");
        EventDispatcher.Instance.Dispatch(EventKey.onFunctionCallBack, msg);
    }

}

