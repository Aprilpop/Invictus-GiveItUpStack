using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
/// <summary>
/// 选择挑战关卡模式
/// </summary>
public class ChallengSelect : MonoBehaviour
{

    [Header("替换文本")]
    public Text m_textHeader;
    public Text m_textHeaderHighSpeed;
    public Text m_textHeaderPerfectJump;
    public Text m_textHeaderJumpOver;
    public Text m_textHeaderScalePlatform;
    public Text m_textPrev;
    public Text m_textNext;

    [SerializeField]
    Transform highSpeed;
    [SerializeField]
    Text highSpeedCounter;


    [SerializeField]
    Transform perfectJump;

    [SerializeField]
    Text perfectJumpCounter;

    [SerializeField]
    Transform jumpOver;

    [SerializeField]
    Text jumpOvercounter;

    [SerializeField]
    Transform scalingPlatform;

    [SerializeField]
    Text scallingPlatformcounter;

    private void Awake()
    {

        m_textHeader.text = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Challenges"));

        m_textHeaderHighSpeed.text = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Challenges.HighSpeed"));
        m_textHeaderPerfectJump.text = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Challenges.PerfectJump"));
        m_textHeaderJumpOver.text = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Challenges.JumpOver"));
        m_textHeaderScalePlatform.text = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Challenges.ScalePlatform"));

        m_textPrev.text = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Challenges.Prev"));
        m_textNext.text = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Challenges.Next"));

        foreach (var item in ProfileManager.Instance.Challenges)
        {
            foreach (var item2 in ProfileManager.Instance.Challenges[0].challengesLevels)
            {
                switch (item.challengeType)
                {
                    case eChallengeType.None:
                        break;
                    case eChallengeType.HighSpeed:
                        SelectChallange challengeButton = Instantiate(Resources.Load<SelectChallange>("Ui/SelectChallenge"), highSpeed);
                        challengeButton.index = item2.level;
                        challengeButton.name = item2.level.ToString();
                        challengeButton.challengeType = eChallengeType.HighSpeed;
                        break;
                    case eChallengeType.PerfectJump:
                        SelectChallange challengeButton1 = Instantiate(Resources.Load<SelectChallange>("Ui/SelectChallenge"), perfectJump);
                        challengeButton1.index = item2.level;
                        challengeButton1.name = item2.level.ToString();
                        challengeButton1.challengeType = eChallengeType.PerfectJump;
                        break;
                    case eChallengeType.JumpOver:
                        SelectChallange challengeButton2 = Instantiate(Resources.Load<SelectChallange>("Ui/SelectChallenge"), jumpOver);
                        challengeButton2.index = item2.level;
                        challengeButton2.name = item2.level.ToString();
                        challengeButton2.challengeType = eChallengeType.JumpOver;
                        break;
                    case eChallengeType.ScalePlatform:
                        SelectChallange challengeButton3 = Instantiate(Resources.Load<SelectChallange>("Ui/SelectChallenge"), scalingPlatform);
                        challengeButton3.index = item2.level;
                        challengeButton3.name = item2.level.ToString();
                        challengeButton3.challengeType = eChallengeType.ScalePlatform;
                        break;
                    default:
                        break;
                }
            }

        }

    }

    private void OnEnable()
    {
        ChangeStatus();
    }

    public void ChangeStatus()
    {
        int c = 0;
        foreach (Transform item in highSpeed)
        {
            if (item.GetComponent<SelectChallange>())
            {
                SelectChallange selectChallange = item.GetComponent<SelectChallange>();
                foreach (var item2 in ProfileManager.Instance.Challenges[0].challengesLevels)
                {
                    if (item2.level == selectChallange.index)
                    {
                        item.GetComponent<Button>().interactable = item2.unlocked;

                        if (item2.unlocked)
                            c++;
                    }
                }
            }
        }

        highSpeedCounter.text = c + "/12";

        c = 0;
        foreach (Transform item in perfectJump)
        {
            if (item.GetComponent<SelectChallange>())
            {
                SelectChallange selectChallange = item.GetComponent<SelectChallange>();
                foreach (var item2 in ProfileManager.Instance.Challenges[1].challengesLevels)
                {
                    if (item2.level == selectChallange.index)
                    {
                        item.GetComponent<Button>().interactable = item2.unlocked;
                        if (item2.unlocked)
                            c++;
                    }
                }
            }
        }
        perfectJumpCounter.text = c + "/12";

        c = 0;

        foreach (Transform item in jumpOver)
        {
            if (item.GetComponent<SelectChallange>())
            {
                SelectChallange selectChallange = item.GetComponent<SelectChallange>();
                foreach (var item2 in ProfileManager.Instance.Challenges[2].challengesLevels)
                {
                    if (item2.level == selectChallange.index)
                    {
                        item.GetComponent<Button>().interactable = item2.unlocked;
                        if (item2.unlocked)
                            c++;
                    }
                }
            }
        }

        jumpOvercounter.text = c + "/12";

        c = 0;
        foreach (Transform item in scalingPlatform)
        {
            if (item.GetComponent<SelectChallange>())
            {
                SelectChallange selectChallange = item.GetComponent<SelectChallange>();
                foreach (var item2 in ProfileManager.Instance.Challenges[3].challengesLevels)
                {
                    if (item2.level == selectChallange.index)
                    {
                        item.GetComponent<Button>().interactable = item2.unlocked;
                        if (item2.unlocked)
                            c++;
                    }
                }
            }
        }
        scallingPlatformcounter.text = c + "/12";
    }


}
