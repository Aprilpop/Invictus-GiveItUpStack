using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarkSDKSpace;
using System;

public class ByteDanceSDKManager
{
    private static ByteDanceSDKManager instance;

    private float startTime;

    private float lastPlayTime;

    private bool initialized = false;

    public Action onShareResult;

    float recordStart;

    float recordEnd;

    public static ByteDanceSDKManager Instance
    {
        get
        {
            if (instance == null)
                instance = new ByteDanceSDKManager();
            return instance;
        }
    }
    public void Initialize()
    {
        if (initialized) return;
        if (Debug.isDebugBuild)
        {
            MockSetting.OpenAllMockModule();
        }
        startTime = Time.time;

        lastPlayTime = 0;

        initialized = true;
    }
    #region record&share
    public void StartRecord()
    {
        StarkSDK.API.GetStarkGameRecorder().StartRecord();
        recordStart = Time.time;
    }

    public void StopRecord()
    {
        StarkSDK.API.GetStarkGameRecorder().StopRecord();
        recordEnd = Time.time;
    }

    void ShareReward()
    {
        if (onShareResult != null)
        {
            onShareResult();
            onShareResult = null;
        }
    }

    public void Share()
    {
        //int duration = StarkSDK.API.GetStarkGameRecorder().GetRecordDuration();
        float duration = recordEnd - recordStart;
        if (duration > 3 && duration < 600)
        {
            //share topic
            List<string> topics = new List<string>();
            topics.Add("永不言弃登峰");
            StarkSDK.API.GetStarkGameRecorder().ShareVideoWithTitleTopics(ShareReward, null, null, "", topics);
        }
    }
    #endregion

    #region ads
    public void ShowRewardVideo()
    {
        StarkSDKSpace.StarkSDK.API.GetStarkAdManager().ShowVideoAdWithId("l2jr0s3ll615lht4r7", VideoReward);
        return;
    }

    public void VideoReward(bool complete)
    {
        if (complete)
        {
            Debug.Log("完成");
            PluginMercury.Instance.AdShowSuccessCallBack("playOver");
        }
    }

    public void ShowInterstitial()
    {
        float curTime = Time.time;
        if (curTime - startTime < 15 || (lastPlayTime != 0 && curTime - lastPlayTime < 30))
        {
            return;
        }
        StarkSDKSpace.StarkSDK.API.GetStarkAdManager().CreateInterstitialAd("5mch4c0idmjif6kb88");
        lastPlayTime = curTime;
    }

    public void ShowBanner()
    {
        StarkAdManager.BannerStyle banner = new StarkAdManager.BannerStyle();
        banner.left = 0;
        banner.top = 200;
        banner.width = 300;
        StarkSDK.API.GetStarkAdManager().CreateBannerAd("d8d2mbco0il2af5jie", banner);
    }
    #endregion

    #region other functions
    public void SaveToDesktop()
    {
        StarkSDK.API.CreateShortcut(null);
    }

    public void FollowDouyin()
    {
        StarkSDK.API.FollowDouYinUserProfile(null,null);
    }
    #endregion
}
