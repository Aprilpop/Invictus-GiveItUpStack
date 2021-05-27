using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
// using InvictusMoreGames;
using SmartLocalization;
using System.Diagnostics.Contracts;

/// <summary>
/// 已有的关卡模式
/// </summary>
public enum eChallengeType
{
    None,
    HighSpeed,      //高速关卡
    PerfectJump,    //完美关卡
    JumpOver,       //跳过
    ScalePlatform   //规模平台
}

/// <summary>
/// 解锁类型
/// </summary>
public enum eUnlockType
{
    Play,                   //玩解锁
    Point,                  //得分解锁
    Perfect,                //完美解锁
    SecondChance,           //第二次机会
    Day,                    //一天
    HighSpeedChallenge,     //高速关卡模式
    PerfectJumpChallenge,   //完美关卡模式
    JumpOverChallenge,      //跳过的挑战
    ScalePlatformChallenge, //规模平台的挑战

    GoldBuy,                // 通过购买解锁(新加)
    DiamondBuy,             // 钻石购买
    Advertising,                //看广告解锁(自己添加)
    None,
}

/// <summary>
/// 货币类型
/// </summary>
public enum eCurrencyType
{
    gold = 0,
    diamond,
    treasureBox
}

/// <summary>
/// 结算奖励
/// </summary>
[System.Serializable]

public class WinAward
{
    public eCurrencyType m_eAward;     // 奖励的货币类型
    public int m_iAwardCount;          // 奖励数量
}

[System.Serializable]
public class ChallengesLevel
{

    public string name;

    public int level;

    public PlatformDifficulty[] platformDifficulties;

    public int goalPlatform;

    public bool unlocked;

    public bool complete;

    public WinAward m_winAward;         // 结算奖励
}

[System.Serializable]
public class Challenges
{

    [SerializeField]
    string name;

    [SerializeField]
    string challengeEnviroment;

    public eChallengeType challengeType;

    public ChallengesLevel[] challengesLevels;

}

[System.Serializable]
public class UnlockInfo
{
    [SerializeField]
    bool videoUnlockable;
    public bool VideoUnlockable { get { return videoUnlockable; } set { videoUnlockable = value; } }

    [SerializeField]
    int videoCount;
    public int VideoCount { get { return videoCount; } }

    [SerializeField]
    eUnlockType unlockType;
    public eUnlockType UnlockType { get { return unlockType; } }

    [SerializeField]
    int unlockCount;
    public int UnlockCount { get { return unlockCount; } }

    int currentVideoUnlockCount;
    public int CurrentVideoUnlockCount { get { return currentVideoUnlockCount; } set { currentVideoUnlockCount = value; } }
}

/// <summary>
/// 角色
/// </summary>
[System.Serializable]
public class Blob : IUnlockItem
{
    [SerializeField]
    string name;

    public string Name { get { return name; } }

    [SerializeField]
    bool unlocked;

    public bool Unlocked { get { return unlocked; } set { unlocked = value; } }

    [SerializeField]
    string prefab;

    [SerializeField]
    Sprite image;

    public Sprite Image { get { return image; } }

    [SerializeField]
    Sprite unlockImage;

    public Sprite UnlockImage { get { return unlockImage; } }

    [SerializeField]
    int index;

    public int Index { get { return index; } }

    public UnlockInfo unlockInfo;

    public GameObject Create(Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject blob = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("characters/" + prefab), parent);
        GameObject tmpObj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("characters/Ring"), parent);
        ProfileManager.Instance.CurrentRing  = tmpObj;
        CharacterController.ring = tmpObj.GetComponentInChildren<Animator>();
        if (blob)
        {
            blob.transform.position = position;
            blob.transform.rotation = rotation;
            return blob;
        }
        return null;
    }

    public UnlockInfo GetUnlockInfo()
    {
        return unlockInfo;
    }

}

[System.Serializable]
public class PlatformDifficulty
{
    [SerializeField]
    int fromPlatform;
    [SerializeField]
    float minSpeed;
    [SerializeField]
    float maxSpeed;

    public int FromPlatform { get { return fromPlatform; } }
    public float MinSpeed { get { return minSpeed; } }
    public float MaxSpeed { get { return maxSpeed; } }
}

public enum eStartType
{
    None,
    IsEasy,     //简单
    IsMedium,   //一般
    IsHard      //困难
}

[Serializable]
public class PlatformStarter
{

    [SerializeField]
    int easy;
    [SerializeField]
    int medium;
    [SerializeField]
    int hard;

    public int Easy { get { return easy; } }
    public int Medium { get { return medium; } }
    public int Hard { get { return hard; } }

    public eStartType StartType(int counter)
    {
        if (easy < counter && medium > counter && hard > counter)
            return eStartType.IsEasy;
        else if (easy < counter && medium < counter && hard > counter)
            return eStartType.IsMedium;
        else if (easy < counter && medium < counter && hard < counter)
            return eStartType.IsHard;
        else
            return eStartType.None;
    }
}

/// <summary>
/// 环境
/// </summary>
[System.Serializable]
public class Enviroment : IUnlockItem
{
    [SerializeField]
    string name;
    public string Name { get { return name; } }

    [SerializeField]
    bool unlocked;

    public bool Unlocked { get { return unlocked; } set { unlocked = value; } }

    [SerializeField]
    string prefab;
    public string Prefab { get { return prefab; } }

    [SerializeField]
    int index;
    public int Index { get { return index; } }

    int record;
    public int Rercord { get { return record; } }

    public PlatformDifficulty[] platformDifficulties;

    [SerializeField]
    Sprite image;

    public Sprite Image { get { return image; } }

    [SerializeField]
    Sprite unlockImage;

    public Sprite UnlockImage { get { return unlockImage; } }

    public UnlockInfo unlockInfo;

    [SerializeField]
    int startRandomSpawn;
    public int StartRandomSpawn { get { return startRandomSpawn; } }

    //New endless mode
    /*[SerializeField]
    int startScaleSizing;
    public int StartScaleSizing { get { return startScaleSizing; } }

    [SerializeField]
    int startSaw;
    public int StartSaw { get { return startSaw; } }
    */

    public PlatformStarter sawStart;
    public PlatformStarter scaleStart;

    //End new endless mode

    public string platform;

    [SerializeField]
    Material succes;
    [SerializeField]
    Material gGrid;
    [SerializeField]
    Material gSplash;
    [SerializeField]
    Material white;


    public Material Succes { get { return succes; } }

    public Material glodGrid { get { return gGrid; } }

    public Material goldSplash { get { return gSplash; } }

    public Material White { get { return white; } }



    [SerializeField]
    Material nGrid;//normal Grid
    [SerializeField]
    Material nSplash;//normal Splash

    public Material NormalGrid { get { return nGrid; } }
    public Material NormalSplash { get { return nSplash; } }



    public GameObject Create(Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject enviroment = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Enviroment/" + prefab), parent);

        if (enviroment)
        {
            enviroment.transform.position = position;
            enviroment.transform.rotation = rotation;
            return enviroment;
        }
        return null;
    }

    public UnlockInfo GetUnlockInfo()
    {
        return unlockInfo;
    }
}

/// <summary>
/// 音乐
/// </summary>
[Serializable]
public class Music : IUnlockItem
{
    [SerializeField]
    string name;
    public string Name { get { return name; } }

    [SerializeField]
    bool unlocked;
    public bool Unlocked { get { return unlocked; } set { unlocked = value; } }

    [SerializeField]
    MusicArrayEnum emusic;
    public MusicArrayEnum eMusic { get { return emusic; } }

    [SerializeField]
    Sprite unlockImage;
    public Sprite UnlockImage { get { return unlockImage; } }

    [SerializeField]
    int index;
    public int Index { get { return index; } }

    public UnlockInfo unlockInfo;


}

/// <summary>
/// 用户信息
/// </summary>
public class UserInfo
{
    private string m_strName;
    private int m_iGold;      //金币
    private int m_iDiamond;  //钻石

    public string Name { set { m_strName = value; } get { return m_strName; } }
    public int Gold { set { m_iGold = value; } get { return m_iGold; } }
    public int Diamond { set { m_iDiamond = value; } get { return m_iDiamond; } }

}

/// <summary>
/// 配置文件管理
/// </summary>
public class ProfileManager : MonoBehaviour
{
    private static ProfileManager instance;

    public static ProfileManager Instance
    {
        get
        {
            if (instance == null)
                Instantiate(Resources.Load("ProfileManager"));
            return instance;
        }
    }

    int enviromentIndex;
    public int EnviromentIndex { get { return enviromentIndex; } }

    int blobIndex;
    public int BlobIndex { get { return blobIndex; } }

    MusicArrayEnum eMusic;
    public MusicArrayEnum EMusic { get { return eMusic; } set { eMusic = value; } }

    [Header("复活金币")]
    [SerializeField] private int m_iResurgenceGold = 10;
    public int ResurgenceGold { get { return m_iResurgenceGold; } }


    [Header("看广告得金币")]
    [SerializeField] private int m_iAdGold = 10;
    public int AdGold { get { return m_iAdGold; } }

    [Header("结算翻倍倍数")]
    [SerializeField] private int m_iMultipleAward = 4;
    public int MultipleAward { get { return m_iMultipleAward; } }

    [Header("Blobs")]
    public Blob[] blobs;

    public Sprite characterOff;
    public Sprite characterOn;
    public Sprite characterSelected;

    GameObject currentBlobs;
    public GameObject CurrentBlobs { get { return currentBlobs; } set { currentBlobs = value; } }

    public GameObject CurrentRing { get; set; }

    [Header("Enviroment")]
    public Enviroment[] enviroments;

    GameObject currentEnviroment;
    public GameObject CurrentEnviroment { get { return currentEnviroment; } }

    [SerializeField]
    float secondChanceTime;

    public float SecondChanceTime { get { return secondChanceTime; } }

    bool musicEnabled = true;
    bool soundEnabled = true;

    public bool musicHelper = true;
    public bool soundHelper = true;

    public bool Sound
    {
        get { return soundEnabled; }
        set { soundEnabled = value; if (MusicManager.Instance) MusicManager.Instance.SetSoundVol(value); }
    }
    public bool Music
    {
        get { return musicEnabled; }
        set { musicEnabled = value; if (MusicManager.Instance) MusicManager.Instance.SetMusicVol(value); }
    }

    int interstitialCounter;
    public int IntersitialCounter { get { return interstitialCounter; } set { interstitialCounter = value; } }

    bool isRestart;
    public bool IsRestart { get { return isRestart; } set { isRestart = value; } }

    bool firstPlay;
    public bool FirstPlay { get { return firstPlay; } set { firstPlay = value; } }

    GameObject platform;
    public GameObject Platform { get { return platform; } }

    int record;
    public int Record { get { return record; } set { record = value; } }

    [Header("Challenges")]

    [SerializeField]
    Challenges[] challlenges;

    public Challenges[] Challenges { get { return challlenges; } }

    [Header("Music")]
    [SerializeField]
    Music[] musics;
    public Music[] Musics { get { return musics; } }

    int playedGames;
    public int PlayedGames { get { return playedGames; } set { playedGames = value; } }

    int fullPerfectJump;
    public int FullPerfectJump { get { return fullPerfectJump; } set { fullPerfectJump = value; } }

    int secondChanceCount;
    public int SecondChanceCount { get { return secondChanceCount; } set { secondChanceCount = value; } }

    int dayInRow;
    public int DayInRow { get { return dayInRow; } set { dayInRow = value; } }

    long checkInDay;
    public long CheckInDay { get { return checkInDay; } set { checkInDay = value; } }

    int highJumpChallenges;
    public int HighJumpChallenges { get { return highJumpChallenges; } set { highJumpChallenges = value; } }

    int perfetJumpChallenges;
    public int PerfectJumpChallenges { get { return perfetJumpChallenges; } set { perfetJumpChallenges = value; } }

    int jumpOverChallenges;
    public int JumpOverChallenges { get { return jumpOverChallenges; } set { jumpOverChallenges = value; } }

    int scalePlatformChallenges;
    public int ScalePlatformChallenges { get { return scalePlatformChallenges; } set { scalePlatformChallenges = value; } }

    eChallengeType challengeType;
    public eChallengeType ChallengeType { get { return challengeType; } set { challengeType = value; } }

    [HideInInspector]
    public int challengeNumber;

    int maxPlatform;
    public int MaxPlatform { get { return maxPlatform; } set { maxPlatform = value; } }

    GameObject recordLine;

    bool noAds;
    public bool NoAds { get { return noAds; } set { noAds = value; } }

    CharacterController characterController;
    public CharacterController CharacterController { get { return characterController; } }

    public Enviroment Enviroment { get { return enviroments[EnviromentIndex]; } }

    System.DateTime dateTime;

    /// <summary>
    ///  是否第一次进入主菜单页面
    /// </summary>
    public bool IsFirstEnterMenu;

    public float Ratio
    {
        get
        {
            int width = Screen.width;
            int height = Screen.height;

            float ratio = (height > width) ? (float)height / width : (float)width / height;
            return ratio;
        }
    }


    private bool m_isFirstOpenChallenges = true;
    public bool FirstOpenChallenges {get {return m_isFirstOpenChallenges;} set {m_isFirstOpenChallenges=value;} }

    // 用户信息
    private UserInfo m_userInfo;

    private void Awake()
    {
        IsFirstEnterMenu = true;
        // 初始化本地化语言插件
        DontDestroyOnLoad(SmartLocalization.LanguageManager.Instance);

        // 默认cn语言
        SmartCultureInfo info = LanguageManager.Instance.GetCultureInfo("zh-CHS");
        LanguageManager.Instance.ChangeLanguage(info);


        GameAnalyticsManager.Initialize();

        if (instance == null)
        {
            instance = this;

            m_userInfo = new UserInfo();

            DontDestroyOnLoad(instance);

            SaveChallenges();

            Load();

            SavePlatformSpeed();

            dateTime = new System.DateTime(checkInDay);

            if (dateTime.AddDays(1).Ticks <= System.DateTime.Now.Ticks)
            {
                checkInDay = System.DateTime.Now.Ticks;
                UnlockItems(eUnlockType.Day);
                dateTime = System.DateTime.Now;
            }

            if (checkInDay == 0)
                UnlockItems(eUnlockType.Day);

        }
        else
            Destroy(gameObject);

        Time.timeScale = 0;

    }


    public void SetMaxPlatform(eChallengeType challengeType, int index)
    {
        if (challengeType != eChallengeType.None)
        {
            int count = challengeLevelNumber[challengeType][index].goalPlatform;
            recordLine = Instantiate(Resources.Load<GameObject>("Record"));
            recordLine.transform.position = new Vector3(0f, 0.34f * count, 1f);
        }
    }


    Dictionary<int, float[]> minMaxSpeeds;

    float minSpeed;
    public float MinSpeed { get { return minSpeed; } }

    float maxSpeed;
    public float MaxSpeed { get { return maxSpeed; } }

    public void SavePlatformSpeed()
    {
        // minMaxSpeeds.Clear();
        minMaxSpeeds = new Dictionary<int, float[]>();

        foreach (var item in enviroments[enviromentIndex].platformDifficulties)
        {
            float[] minMax = { item.MinSpeed, item.MaxSpeed };
            minMaxSpeeds.Add(item.FromPlatform, minMax);
        }
    }

    public void SetSpeed(int platformCount)
    {
        if (minMaxSpeeds.ContainsKey(platformCount))
        {
            minSpeed = minMaxSpeeds[platformCount][0];
            maxSpeed = minMaxSpeeds[platformCount][1];
        }
    }

    Dictionary<eChallengeType, ChallengesLevel[]> challengeLevelNumber;

    private void SaveChallenges()
    {
        challengeLevelNumber = new Dictionary<eChallengeType, ChallengesLevel[]>();

        foreach (var item in challlenges)
        {
            challengeLevelNumber[item.challengeType] = item.challengesLevels;
        }
    }

    public void SavePlatformSpeedChallange(eChallengeType challengeType, int index)
    {
        minMaxSpeeds.Clear();
        minMaxSpeeds = new Dictionary<int, float[]>();

        foreach (var item in challengeLevelNumber[challengeType][index].platformDifficulties)
        {
            float[] minMax = { item.MinSpeed, item.MaxSpeed };
            minMaxSpeeds.Add(item.FromPlatform, minMax);

        }

    }

    public int GetGoalPlatform(eChallengeType challengeType, int index)
    {
        if (challengeType != eChallengeType.None)
            return challengeLevelNumber[challengeType][index].goalPlatform;
        else
            return 0;
    }

    /// <summary>
    /// 获取结算奖励
    /// </summary>
    /// <returns></returns>
    public WinAward GetWinAward(eChallengeType challengeType, int index)
    {
        if (challengeType != eChallengeType.None)
            return challengeLevelNumber[challengeType][index].m_winAward;
        else
            return null;
    }

    public GameObject EnviromentPlatform()
    {
        return platform;
    }

    public Material SuccesMaterial()
    {
        return enviroments[enviromentIndex].Succes;
    }

    public Material NormalMaterial()
    {
        return enviroments[enviromentIndex].NormalGrid;
    }
    public Material NormalSplashMaterial()
    {
        return enviroments[enviromentIndex].NormalSplash;
    }

    public Material GoldGridMaterial()
    {
        return enviroments[enviromentIndex].glodGrid;
    }

    public Material GoldSplashMaterial()
    {
        return enviroments[enviromentIndex].goldSplash;
    }

    public Material WhiteMaterial()
    {
        return enviroments[enviromentIndex].White;
    }
    public void SetEnviroment(int index, Transform enviromentPoint)
    {
         Destroy(currentEnviroment);
        GameObject enviroment = enviroments[index].Create(enviromentPoint.position, Quaternion.identity, enviromentPoint);
        currentEnviroment = enviroment;
        enviromentIndex = index;
        platform = Resources.Load<GameObject>("Platforms/" + enviroments[index].platform);
        Save();
    }

    public void SetBlob(int index, Transform characterStartPoint)
    {
        Destroy(currentBlobs);
        GameObject blob = blobs[index].Create(characterStartPoint.position, Quaternion.identity, characterStartPoint);
        currentBlobs = blob;
        blobIndex = index;
        characterController = blob.GetComponent<CharacterController>();
        Save();
        // 广播角色发生改变
        EventDispatcher.Instance.Dispatch(EventKey.OnCharacterChange);        
    }

    public List<IUnlockItem> unlockedItems = new List<IUnlockItem>();

    public void UnlockItems(eUnlockType unlockType, int score = 0)
    {

        switch (unlockType)
        {
            case eUnlockType.Play:
                playedGames++;
                foreach (var item in blobs)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= playedGames)
                    {
                        item.Unlocked = true;
                        unlockedItems.Add(item);
                    }
                foreach (var item in enviroments)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= playedGames)
                    {
                        item.Unlocked = true;
                        unlockedItems.Add(item);
                    }
                foreach (var item in musics)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= playedGames)
                    {
                        item.Unlocked = true;
                        unlockedItems.Add(item);
                    }

                break;
            case eUnlockType.Point:
                foreach (var item in blobs)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= score)
                    {
                        item.Unlocked = true;
                        unlockedItems.Add(item);
                    }
                foreach (var item in enviroments)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= score)
                    {
                        item.Unlocked = true;
                        unlockedItems.Add(item);
                    }
                foreach (var item in musics)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= score)
                    {
                        item.Unlocked = true;
                        unlockedItems.Add(item);
                    }
                break;
            case eUnlockType.Perfect:
                fullPerfectJump++;
                foreach (var item in blobs)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= fullPerfectJump)
                    {
                        item.Unlocked = true;
                        unlockedItems.Add(item);
                    }
                foreach (var item in enviroments)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= fullPerfectJump)
                    {
                        item.Unlocked = true;
                        unlockedItems.Add(item);
                    }
                foreach (var item in musics)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= fullPerfectJump)
                    {
                        item.Unlocked = true;
                        unlockedItems.Add(item);
                    }
                break;
            case eUnlockType.SecondChance:
                secondChanceCount++;
                foreach (var item in blobs)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= secondChanceCount)
                    {
                        item.Unlocked = true;
                        unlockedItems.Add(item);
                    }
                foreach (var item in enviroments)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= secondChanceCount)
                    {
                        item.Unlocked = true;
                        unlockedItems.Add(item);
                    }
                foreach (var item in musics)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= secondChanceCount)
                    {
                        item.Unlocked = true;
                        unlockedItems.Add(item);
                    }
                break;
            case eUnlockType.Day:
                dayInRow++;
                foreach (var item in blobs)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= dayInRow)
                        item.Unlocked = true;
                foreach (var item in enviroments)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= dayInRow)
                        item.Unlocked = true;
                foreach (var item in musics)
                    if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= dayInRow)
                        item.Unlocked = true;
                break;
            default:
                break;
        }

    }

    public void UnlockItemsByChallenge(eChallengeType challengeType, int index)
    {

        eUnlockType unlockType;
        bool unlockedLevel = false;

        switch (challengeType)
        {
            case eChallengeType.HighSpeed:
                unlockType = eUnlockType.HighSpeedChallenge;
                unlockedLevel = challengeLevelNumber[challengeType][index].complete;
                break;
            case eChallengeType.PerfectJump:
                unlockType = eUnlockType.PerfectJumpChallenge;
                unlockedLevel = challengeLevelNumber[challengeType][index].complete;
                break;
            case eChallengeType.JumpOver:
                unlockType = eUnlockType.JumpOverChallenge;
                unlockedLevel = challengeLevelNumber[challengeType][index].complete;
                break;
            case eChallengeType.ScalePlatform:
                unlockType = eUnlockType.ScalePlatformChallenge;
                unlockedLevel = challengeLevelNumber[challengeType][index].complete;
                break;
            default:
                unlockType = eUnlockType.None;
                break;
        }

        if (!unlockedLevel)
        {
            switch (unlockType)
            {
                case eUnlockType.HighSpeedChallenge:
                    highJumpChallenges++;
                    foreach (var item in blobs)
                        if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= highJumpChallenges)
                        {
                            item.Unlocked = true;
                            unlockedItems.Add(item);
                        }
                    foreach (var item in enviroments)
                        if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= highJumpChallenges)
                        {
                            item.Unlocked = true;
                            unlockedItems.Add(item);
                        }
                    break;
                case eUnlockType.PerfectJumpChallenge:
                    perfetJumpChallenges++;
                    foreach (var item in blobs)
                        if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= perfetJumpChallenges)
                        {
                            item.Unlocked = true;
                            unlockedItems.Add(item);
                        }
                    foreach (var item in enviroments)
                        if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= perfetJumpChallenges)
                        {
                            item.Unlocked = true;
                            unlockedItems.Add(item);
                        }
                    break;
                case eUnlockType.JumpOverChallenge:
                    jumpOverChallenges++;
                    foreach (var item in blobs)
                        if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= jumpOverChallenges)
                        {
                            item.Unlocked = true;
                            unlockedItems.Add(item);
                        }
                    foreach (var item in enviroments)
                        if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= jumpOverChallenges)
                        {
                            item.Unlocked = true;
                            unlockedItems.Add(item);
                        }
                    break;
                case eUnlockType.ScalePlatformChallenge:
                    scalePlatformChallenges++;
                    foreach (var item in blobs)
                        if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= scalePlatformChallenges)
                        {
                            item.Unlocked = true;
                            unlockedItems.Add(item);
                        }
                    foreach (var item in enviroments)
                        if (!item.Unlocked && item.unlockInfo.UnlockType == unlockType && item.unlockInfo.UnlockCount <= scalePlatformChallenges)
                        {
                            item.Unlocked = true;
                            unlockedItems.Add(item);
                        }
                    break;
                default:
                    break;

            }
        }
    }

    public void UnlockChallenge(eChallengeType challengeType, int index)
    {
        challengeLevelNumber[challengeType][index].unlocked = true;
    }

    public void CompleteChallenge(eChallengeType challengeType, int index)
    {
        if (!challengeLevelNumber[challengeType][index].complete)
            challengeLevelNumber[challengeType][index].complete = true;
    }

    public void SaveChallenge()
    {
        foreach (var item in challengeLevelNumber[eChallengeType.HighSpeed])
        {
            if (item.level != 1)
            {
                PlayerPrefs.SetInt(eChallengeType.HighSpeed + item.name, item.unlocked ? 1 : 0);
            }
            PlayerPrefs.SetInt(eChallengeType.HighSpeed + item.name + "Compelte", item.complete ? 1 : 0);
        }
        foreach (var item in challengeLevelNumber[eChallengeType.PerfectJump])
        {
            if (item.level != 1)
            {
                PlayerPrefs.SetInt(eChallengeType.PerfectJump + item.name, item.unlocked ? 1 : 0);
            }
            PlayerPrefs.SetInt(eChallengeType.PerfectJump + item.name + "Compelte", item.complete ? 1 : 0);
        }
        foreach (var item in challengeLevelNumber[eChallengeType.JumpOver])
        {
            if (item.level != 1)
            {
                PlayerPrefs.SetInt(eChallengeType.JumpOver + item.name, item.unlocked ? 1 : 0);
            }
            PlayerPrefs.SetInt(eChallengeType.JumpOver + item.name + "Compelte", item.complete ? 1 : 0);
        }
        foreach (var item in challengeLevelNumber[eChallengeType.ScalePlatform])
        {
            if (item.level != 1)
            {
                PlayerPrefs.SetInt(eChallengeType.ScalePlatform + item.name, item.unlocked ? 1 : 0);
            }
            PlayerPrefs.SetInt(eChallengeType.ScalePlatform + item.name + "Compelte", item.complete ? 1 : 0);
        }
    }

    public void LoadChallenge()
    {
        foreach (var item in challengeLevelNumber[eChallengeType.HighSpeed])
        {
            if (item.level != 1)
            {
                item.unlocked = PlayerPrefs.GetInt(eChallengeType.HighSpeed + item.name, 0) == 1 ? true : false;

            }
            item.complete = PlayerPrefs.GetInt(eChallengeType.HighSpeed + item.name + "Compelte", 0) == 1 ? true : false;
        }
        foreach (var item in challengeLevelNumber[eChallengeType.PerfectJump])
        {
            if (item.level != 1)
            {
                item.unlocked = PlayerPrefs.GetInt(eChallengeType.PerfectJump + item.name, 0) == 1 ? true : false;
            }
            item.complete = PlayerPrefs.GetInt(eChallengeType.PerfectJump + item.name + "Compelte", 0) == 1 ? true : false;
        }
        foreach (var item in challengeLevelNumber[eChallengeType.JumpOver])
        {
            if (item.level != 1)
            {
                item.unlocked = PlayerPrefs.GetInt(eChallengeType.JumpOver + item.name, 0) == 1 ? true : false;
            }
            item.complete = PlayerPrefs.GetInt(eChallengeType.JumpOver + item.name + "Compelte", 0) == 1 ? true : false;
        }
        foreach (var item in challengeLevelNumber[eChallengeType.ScalePlatform])
        {
            if (item.level != 1)
            {
                item.unlocked = PlayerPrefs.GetInt(eChallengeType.ScalePlatform + item.name, 0) == 1 ? true : false;
            }
            item.complete = PlayerPrefs.GetInt(eChallengeType.ScalePlatform + item.name + "Compelte", 0) == 1 ? true : false;
        }
    }

    public void ResetChallenge()
    {
        foreach (var item in challengeLevelNumber[eChallengeType.HighSpeed])
        {
            if (item.level != 1)
                item.unlocked = false;
            item.complete = false;
        }
        foreach (var item in challengeLevelNumber[eChallengeType.PerfectJump])
        {
            if (item.level != 1)
                item.unlocked = false;
            item.complete = false;
        }
        foreach (var item in challengeLevelNumber[eChallengeType.JumpOver])
        {
            if (item.level != 1)
                item.unlocked = false;
            item.complete = false;
        }
        foreach (var item in challengeLevelNumber[eChallengeType.ScalePlatform])
        {
            if (item.level != 1)
                item.unlocked = false;
            item.complete = false;
        }
    }

    public void Save()
    {

        foreach (var item in blobs)
            if (item.Index != 0)
            {
                PlayerPrefs.SetInt("UnlockedBlob" + item.Name, item.Unlocked ? 1 : 0);
                PlayerPrefs.SetInt("CurrentVideoUnlockCountBlob" + item.Name, item.unlockInfo.CurrentVideoUnlockCount);
            }
        foreach (var item in enviroments)
            if (item.Index != 0)
            {
                PlayerPrefs.SetInt("UnlockedEnviroments" + item.Name, item.Unlocked ? 1 : 0);
                PlayerPrefs.SetInt("CurrentVideoUnlockCountEnviroments" + item.Name, item.unlockInfo.CurrentVideoUnlockCount);
            }
        foreach (var item in musics)
            if (item.Index != 0)
            {
                PlayerPrefs.SetInt("UnlockedMusic" + item.Index, item.Unlocked ? 1 : 0);
                PlayerPrefs.SetInt("CurrentVideoUnlockCountMusic" + item.Name, item.unlockInfo.CurrentVideoUnlockCount);
            }


        SaveChallenge();

        PlayerPrefs.SetInt("EnviromentIndex", enviromentIndex);
        PlayerPrefs.SetInt("BlobIndex", blobIndex);
        PlayerPrefs.SetInt("MusicIndex", (int)eMusic);
        PlayerPrefs.SetInt("Music", musicEnabled ? 1 : 0);
        PlayerPrefs.SetInt("Sound", soundEnabled ? 1 : 0);
        PlayerPrefs.SetInt("FirstPlay", firstPlay ? 1 : 0);
        PlayerPrefs.SetInt("HighScore", record);
        PlayerPrefs.SetInt("PlayedGames", playedGames);
        PlayerPrefs.SetInt("FullPerfectJump", fullPerfectJump);
        PlayerPrefs.SetInt("SecondChanceCount", secondChanceCount);
        PlayerPrefs.SetInt("DayInRow", dayInRow);
        PlayerPrefs.SetString("CheckInDay", Convert.ToString(checkInDay));
        PlayerPrefs.SetInt("MaxPlatform", maxPlatform);
        PlayerPrefs.SetInt("HighSpeedChallenges", highJumpChallenges);
        PlayerPrefs.SetInt("PerfectJumpChallenges", perfetJumpChallenges);
        PlayerPrefs.SetInt("JumpOverChallenges", jumpOverChallenges);
        PlayerPrefs.SetInt("ScalePlatformChallenges", scalePlatformChallenges);
        PlayerPrefs.SetInt("NoAds", noAds ? 1 : 0);
        PlayerPrefs.SetInt("InterstitialCounter", interstitialCounter);

        PlayerPrefs.SetInt("FirstOpenChallenges", m_isFirstOpenChallenges ? 1 : 0);
        // 用户信息        
        PlayerPrefs.SetInt("UserGold", m_userInfo.Gold);
        PlayerPrefs.SetInt("UserDiamond", m_userInfo.Diamond);
        PlayerPrefs.SetString("UserName", m_userInfo.Name);
    }

    public void Load()
    {
        foreach (var item in blobs)
            if (item.Index != 0)
            {
                item.Unlocked = PlayerPrefs.GetInt("UnlockedBlob" + item.Name, 0) == 1 ? true : false;
                item.unlockInfo.CurrentVideoUnlockCount = PlayerPrefs.GetInt("CurrentVideoUnlockCountBlob" + item.Name, 0);
            }
        foreach (var item in enviroments)
            if (item.Index != 0)
            {
                item.Unlocked = PlayerPrefs.GetInt("UnlockedEnviroments" + item.Name, 0) == 1 ? true : false;
                item.unlockInfo.CurrentVideoUnlockCount = PlayerPrefs.GetInt("CurrentVideoUnlockCountEnviroments" + item.Name, 0);
            }
        foreach (var item in musics)
            if (item.Index != 0)
            {
                item.Unlocked = PlayerPrefs.GetInt("UnlockedMusic" + item.Index) == 1 ? true : false;
                item.unlockInfo.CurrentVideoUnlockCount = PlayerPrefs.GetInt("CurrentVideoUnlockCountMusic" + item.Name, 0);
            }

        LoadChallenge();

        enviromentIndex = PlayerPrefs.GetInt("EnviromentIndex", 0);
        blobIndex = PlayerPrefs.GetInt("BlobIndex", 0);
        eMusic = (MusicArrayEnum)PlayerPrefs.GetInt("MusicIndex", 1);
        musicEnabled = PlayerPrefs.GetInt("Music", 1) == 1 ? true : false;
        soundEnabled = PlayerPrefs.GetInt("Sound", 1) == 1 ? true : false;
        firstPlay = PlayerPrefs.GetInt("FirstPlay", 1) == 1 ? true : false;
        record = PlayerPrefs.GetInt("HighScore", 0);
        playedGames = PlayerPrefs.GetInt("PlayedGames", 0);
        fullPerfectJump = PlayerPrefs.GetInt("FullPerfectJump", 0);
        secondChanceCount = PlayerPrefs.GetInt("SecondChanceCount", 0);
        dayInRow = PlayerPrefs.GetInt("DayInRow", 0);
        checkInDay = Convert.ToInt64(PlayerPrefs.GetString("CheckInDay", "0"));
        maxPlatform = PlayerPrefs.GetInt("MaxPlatform", 0);
        highJumpChallenges = PlayerPrefs.GetInt("HighSpeedChallenges", 0);
        perfetJumpChallenges = PlayerPrefs.GetInt("PerfectJumpChallenges", 0);
        jumpOverChallenges = PlayerPrefs.GetInt("JumpOverChallenges", 0);
        scalePlatformChallenges = PlayerPrefs.GetInt("ScalePlatformChallenges", 0);
        noAds = PlayerPrefs.GetInt("NoAds", 0) == 1 ? true : false;
        interstitialCounter = PlayerPrefs.GetInt("InterstitialCounter", 0);

        m_isFirstOpenChallenges = PlayerPrefs.GetInt("FirstOpenChallenges", 1) == 1 ? true : false;
        // 用户信息        
        m_userInfo.Gold = PlayerPrefs.GetInt("UserGold", 0);
        m_userInfo.Diamond = PlayerPrefs.GetInt("UserDiamond", 0);
        m_userInfo.Name = PlayerPrefs.GetString("UserName", "");
    }

    // 用户信息修改
    public int Gold
    {
        get { return m_userInfo.Gold; }
        set
        {
            if (value < 0)
            {
                value = 0;
            }

            m_userInfo.Gold = value;
            // 广播金币发生改变
            EventDispatcher.Instance.Dispatch(EventKey.OnGoldChange);
        }
    }
    public int Diamond { set { m_userInfo.Diamond = value; } get { return m_userInfo.Diamond; } }
    public string Name { set { m_userInfo.Name = value; } get { return m_userInfo.Name; } }

    private void OnApplicationQuit()
    {
        Save();
        DebugManager.LogInfo("######### QUIT #########");
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
            DebugManager.LogInfo("######### PAUSED #########");
        }
    }
}
