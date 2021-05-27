using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectChallange : SelectItem
{

    public eChallengeType challengeType;

    public override void Start()
    {
        base.Start();
        button.onClick.AddListener(ChallengeSelection);
    }

    private void ChallengeSelection()
    {
        //ProfileManager.Instance.GetChallengeLevelNumber(challengeType, index-1);
        ProfileManager.Instance.SavePlatformSpeedChallange(challengeType, index-1);
        ProfileManager.Instance.SetSpeed(1);
        GameLogic.Instance.SetDifficulty();
        GameLogic.Instance.challengeLevelNumber = index-1;
        MenuManager.Instance.challenges.SetActive(false);
        ProfileManager.Instance.SetMaxPlatform(challengeType, index-1);
        MenuManager.Instance.Play(challengeType);
        Debug.Log("点击关卡开始游戏！");
        RecordTheScreenToStart();
    }


    public void RecordTheScreenToStart()//游戏录屏开始
    {

    }
}
