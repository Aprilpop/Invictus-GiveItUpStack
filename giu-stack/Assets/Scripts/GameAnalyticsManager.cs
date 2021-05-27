using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GameAnalyticsSDK;

public class GAProgressionStatus
{
    
}
public class GAErrorSeverity
{

}
public class GAAdAction
{

}
public class GAAdType
{

}
public class GAAdError
{
    public static GAAdError Unknown { get; internal set; }
}

public class GameAnalytics
{

}

public class GAResourceFlowType
{

}


public class GameAnalyticsManager : MonoBehaviour
{

    static bool initialized = false;

    public static void Initialize()
    {
        if (initialized)
            return;

        //GameAnalytics.Initialize();
        initialized = true;

        DebugManager.LogInfo("GameAnalytics Initialized");
    }

    //GameAnalyticsManager.LogProgressionEvent(GAProgressionStatus.Start, Application.version, GameManager.Instance.ActiveLineManager.id.ToString("00000"));
    //GameAnalyticsManager.LogProgressionEvent(GAProgressionStatus.Complete, Application.version, GameManager.Instance.ActiveLineManager.id.ToString("00000"));

    //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "World1");
    //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "World1", score);
    //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "World1", "Level1");
    //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "World1", "Level1", score);
    //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "World1", "Level1", "Phase1");
    //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "World1", "Level1", "Phase1", score);

    public static void LogProgressionEvent(GAProgressionStatus status, string progression)
    {
        //GameAnalytics.NewProgressionEvent(status, progression);
    }

    public static void LogProgressionEvent(GAProgressionStatus status, string progression, int score)
    {
       // GameAnalytics.NewProgressionEvent(status, progression, score);
    }

    public static void LogProgressionEvent(GAProgressionStatus status, string progression01, string progression02)
    {
       // GameAnalytics.NewProgressionEvent(status, progression01, progression02);
    }

    public static void LogProgressionEvent(GAProgressionStatus status, string progression01, string progression02, int score)
    {
       // GameAnalytics.NewProgressionEvent(status, progression01, progression02, score);
    }

    public static void LogProgressionEvent(GAProgressionStatus status, string progression01, string progression02, string progression03)
    {
       // GameAnalytics.NewProgressionEvent(status, progression01, progression02, progression03);
    }

    public static void LogProgressionEvent(GAProgressionStatus status, string progression01, string progression02, string progression03, int score)
    {
       // GameAnalytics.NewProgressionEvent(status, progression01, progression02, progression03, score);
    }

    public static void LogDesignEvent(string eventName)
    {
       // GameAnalytics.NewDesignEvent(eventName);
    }

    public static void LogErrorEvent(GAErrorSeverity severity, string message)
    {
       // GameAnalytics.NewErrorEvent(severity, message);
    }

    public static void SetCustomDimension(string customDimension)
    {
       // GameAnalytics.SetCustomDimension01(customDimension);
    }

    public static void LogBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string signature)
    {
#if (UNITY_ANDROID)
       // GameAnalytics.NewBusinessEventGooglePlay(currency, amount, itemType, itemId, cartType, receipt, signature);
#endif

#if (UNITY_IOS)
       // GameAnalytics.NewBusinessEventIOS(currency, amount, itemType, itemId, cartType, receipt);
#endif
    }

    public static void LogAdEvent(GAAdAction request, GAAdType adTypem, string adSDKName, string adPlacement)
    {
        //when requesting rewarded video ad
        //GameAnalytics.NewAdEvent(GAAdAction.Request, GAAdType.RewardedVideo, "admob", "");

        //when rewarded ad is loaded
        //GameAnalytics.NewAdEvent(GAAdAction.Loaded, GAAdType.RewardedVideo, "admob", "");

        //when failed to show 
        //GameAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.RewardedVideo, "admob", "");

        //when failed to show with error
        //GameAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.RewardedVideo, "admob", "", getLatestAdError(this.latestRewardedVideoError));

        //WHEN REQUESTING INTERSTITIAL AD
        //GameAnalytics.NewAdEvent(GAAdAction.Request, GAAdType.Interstitial, "admob", "");

        //WHEN INTERSTITIAL AD IS LOADED
        //GameAnalytics.NewAdEvent(GAAdAction.Loaded, GAAdType.Interstitial, "admob", "");

       // GameAnalytics.NewAdEvent(request, adTypem, adSDKName, adPlacement);
    }

    private GAAdError getLatestAdError(string error)
    {
        GAAdError result = GAAdError.Unknown;

        // ! Implement a switch statement to map ad network errors to known GAAdError types.
        // possible value:
        // GAAdError.Unknown
        // GAAdError.Offline
        // GAAdError.NoFill
        // GAAdError.InternalError
        // GAAdError.InvalidRequest
        // GAAdError.UnableToPrecached

        return result;
    }

    public static void StartTimer(string currentRewardedVideoPlacement)
    {
        //if (currentRewardedVideoPlacement != null)
           // GameAnalytics.StartTimer(currentRewardedVideoPlacement);
    }

    public static void PauseTimer(string currentRewardedVideoPlacement)
    {
        //if (currentRewardedVideoPlacement != null)
           // GameAnalytics.PauseTimer(currentRewardedVideoPlacement);
    }

    public static void ResumeTimer(string currentRewardedVideoPlacement)
    {
        //if (currentRewardedVideoPlacement != null)
           // GameAnalytics.ResumeTimer(currentRewardedVideoPlacement);
    }

    public static void LogRewardedAdShown(string currentRewardedVideoPlacement, GAAdAction rewardReceived, GAAdType rewardedVideo, string adSDKName, string adPlacement, bool trackElapsedTime)
    {
        if (currentRewardedVideoPlacement != null)
        {
            if (trackElapsedTime)
            {
                //long elapsedTime = GameAnalytics.StopTimer(currentRewardedVideoPlacement);

                // send ad event for tracking elapsedTime
               // GameAnalytics.NewAdEvent(rewardReceived, rewardedVideo, adSDKName, adPlacement, elapsedTime);
                currentRewardedVideoPlacement = null;
            }
            else // OR if you do not wish to track time
            {
                // send ad event without tracking elapsedTime
               // GameAnalytics.NewAdEvent(rewardReceived, rewardedVideo, adSDKName, adPlacement);
            }
        }
    }

    public static void LogResourceEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId)
    {
        //GameAnalytics.NewResourceEvent(flowType, currency, amount, itemType, itemId);

        //GameAnalytics.NewResourceEvent(GA_Resource.GAResourceFlowType.GAResourceFlowTypeSource, “Gems”, 400, “IAP”, “Coins400”);

        //GameAnalytics.NewResourceEvent(GA_Resource.GAResourceFlowType.GAResourceFlowTypeSink, “Gems”, 400, “Weapons”, “SwordOfFire”);

        //GameAnalytics.NewResourceEvent(GA_Resource.GAResourceFlowType.GAResourceFlowTypeSink, “Gems”, 100, “Boosters”, “BeamBooster5Pack”);

        //GameAnalytics.NewResourceEvent(GA_Resource.GAResourceFlowType.GAResourceFlowTypeSource, “BeamBooster”, 5, “Gems”, “BeamBooster5Pack”);

        //GameAnalytics.NewResourceEvent(GA_Resource.GAResourceFlowType.GAResourceFlowTypeSink, “BeamBooster”, 3, “Gameplay”, “BeamBooster5Pack”);
    }

}

