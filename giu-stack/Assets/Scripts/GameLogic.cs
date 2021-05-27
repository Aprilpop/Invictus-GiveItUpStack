using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{

    [Header("Platform")]
    public float spawnerStep;

    public float maxPlatformSwing;

    [HideInInspector]
    public float minPlatformSpeed;
    [HideInInspector]
    public float maxPlatformSpeed;

    [SerializeField]
    float minDelayTime;
    public float MinDelayTime { get { return minDelayTime; } }

    [SerializeField]
    float maxDelayTime;
    public float MaxDelayTime { get { return maxDelayTime; } }

    int platformCount = 0;
    public int PlatformCount { get { return platformCount; } set { platformCount = value; } }

    int challengePlatformCount = 0;
    public int ChallengePlatformCount { get { return challengePlatformCount; } set { challengePlatformCount = value; } }

    int currentPlatfomCount;
    public int CurrentPlatformCount { get { return currentPlatfomCount; } set { currentPlatfomCount = value; } }

    bool inGame;
    public bool InGame { get { return inGame; } set { inGame = value; } }

    bool firstDeath = true;
    public bool FirstDeath { get { return firstDeath; } set { firstDeath = value; } }

    private int score;
    public int Score { get { return score; } set { score = value; } }

    private bool firstStart = true;
    public bool FirstStart { get { return firstStart; } set { firstStart = value; } }

    // 是否显示过危险提示
    private bool m_isShowStabilizationTip;
    public bool IsShowStabilizationTip { get { return m_isShowStabilizationTip; } set { m_isShowStabilizationTip = value; } }

    // 当前
    [HideInInspector]
    public Platform currentStack;

    // 以前
    [HideInInspector]
    public Platform previousStack;

    [HideInInspector]
    public Platform stack;

    [SerializeField]
    float speedMultiplier;

    float afterAd;

    bool isAfterAd;
    public bool IsAfterAd { get { return isAfterAd; } set { isAfterAd = value; } }

    [SerializeField]
    GameObject enviromentPoint;
    public GameObject EnviromentPoint { get { return enviromentPoint; } }
    public Material succes { get; set; }
    public Material gGrid { get; set; }
    public Material gSplash { get; set; }
    public Material normal { get; set; }
    public Material splash { get; set; }

    public Material white { get; set; }

    [Header("Character")]

    [SerializeField]
    float force;
    public float Force { get { return force; } }

    [SerializeField]
    float maxJump;
    public float MaxJump { get { return maxJump; } }

    [SerializeField]
    float globalGravity;
    public float GlobalGravity { get { return globalGravity; } }

    [SerializeField]
    float gravityScale;
    public float GravityScale { get { return gravityScale; } }

    [SerializeField]
    float maxJumpRotate;
    public float MaxJumpRotate { get { return maxJumpRotate; } }

    [SerializeField]
    float minJumpRotate;
    public float MinJumpRotate { get { return minJumpRotate; } }

    [SerializeField]
    float rotationSpeed;
    public float RotationSpeed { get { return rotationSpeed; } }

    [SerializeField]
    GameObject characterStartPoint;
    public GameObject CharacterStartPoint { get { return characterStartPoint; } }

    // 发生坍塌
    bool m_isCollapse = false;
    public bool IsCollapse { get { return m_isCollapse; } set { m_isCollapse = value; } }

    // 障碍物撞死
    public bool m_isBarrierDie = false;
    public bool IsBrarrierDie { get { return m_isBarrierDie; } set { m_isBarrierDie = value; } }

    public bool m_isShowGreenHandTipOffset = false;

    [Header("Saw")]
    [SerializeField]
    float sawMinSpeed;

    [SerializeField]
    float sawMaxSpeed;

    [HideInInspector]
    public Obstacle saw;

    public float SawMinSpeed { get { return sawMinSpeed; } }
    public float SawMaxSpeed { get { return sawMaxSpeed; } }

    public eChallengeType challengeType;

    public int challengeLevelNumber;

    private static GameLogic instance;
    /// <summary>
    /// 连击数
    /// </summary>
    public static int comboCount { get; set; }
    /// <summary>
    /// 跳跃计数
    /// </summary>
    public static int jumpIndex { get; set; }
    /// <summary>
    /// 跳跃状态 0 等待 1 完成 2 跳跃中
    /// </summary>
    public static int jumpState { get; private set; }

    private Vector3 zoomIn { get; set; }

    public static GameLogic Instance
    {
        get
        {
            if (instance == null)
            {
                UnityEngine.Object obj = Resources.Load("GameLogic");
                if (obj)
                {
                    Instantiate(obj);
                }
            }
            return instance;

        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        inGame = false;
        zoomIn = new Vector3(1.3f, 1f, 1.3f);

        Time.timeScale = 0;

        ProfileManager.Instance.SetEnviroment(ProfileManager.Instance.EnviromentIndex, enviromentPoint.transform);
        ProfileManager.Instance.SetBlob(ProfileManager.Instance.BlobIndex, characterStartPoint.transform);

        succes = ProfileManager.Instance.SuccesMaterial();
        gGrid = ProfileManager.Instance.GoldGridMaterial();
        gSplash = ProfileManager.Instance.GoldSplashMaterial();
        normal = ProfileManager.Instance.NormalMaterial();
        splash = ProfileManager.Instance.NormalSplashMaterial();
        white = ProfileManager.Instance.WhiteMaterial();

        afterAd = speedMultiplier;

        if (ProfileManager.Instance.IsRestart)
        {
            challengeType = ProfileManager.Instance.ChallengeType;
            challengeLevelNumber = ProfileManager.Instance.challengeNumber;
        }

    }

    private void Update()
    {
        //if (false == inGame) return;
        if (1 != jumpState) return;
        jumpState = 0;
        GameObject tmpObj = Spawner.Instance.tower;
        if (null == tmpObj) return;
//         int len = tmpObj.transform.GetChildCount();
//         for(int i = 0; i < len; i++)
//         {
//            int tmpObj.transform.GetChild()
//         }
    }

    public void SetDifficulty()
    {
        ProfileManager.Instance.SetSpeed(platformCount);
        if (challengeType == eChallengeType.None)
        {
            if (isAfterAd)
            {
                minPlatformSpeed = ProfileManager.Instance.MinSpeed * afterAd;
                maxPlatformSpeed = ProfileManager.Instance.MaxSpeed * afterAd;
                afterAd += 0.1f;
                if (afterAd >= 1f)
                {
                    isAfterAd = false;
                    afterAd = speedMultiplier;

                }
            }
            else
            {
                minPlatformSpeed = ProfileManager.Instance.MinSpeed;
                maxPlatformSpeed = ProfileManager.Instance.MaxSpeed;
            }
        }
        else
        {
            minPlatformSpeed = ProfileManager.Instance.MinSpeed;
            maxPlatformSpeed = ProfileManager.Instance.MaxSpeed;
        }
    }

    public bool PerfectJump(Platform platform, CharacterController character)
    {
        float distance;

        if (challengeType != eChallengeType.None)
            distance = 0.8f;
        else
            distance = 0.785f;

        if (currentStack != null && previousStack != null)
        {
            if (Vector3.Distance(currentStack.transform.position, previousStack.transform.position) < 0.342f || Vector3.Distance(platform.transform.position, character.transform.position) < distance)
            {
                SetPerfectPlatform(platform);
                return true;
            }
            return false;
        }
        if (Vector3.Distance(platform.transform.position, character.transform.position) < distance)
        {
            SetPerfectPlatform(platform);
            return true;
        }
        return false;

    }


    public bool GoalPlatform()
    {
        if (ProfileManager.Instance.GetGoalPlatform(challengeType, challengeLevelNumber) != 0)
            return ProfileManager.Instance.GetGoalPlatform(challengeType, challengeLevelNumber) != challengePlatformCount;
        else
            return false;
    }
    //设置完美跳跃
    public void SetPerfectPlatform(Platform platform)
    {
        score++;
        ProfileManager.Instance.UnlockItems(eUnlockType.Perfect);
        //Hack
        MenuManager.Instance.perfectBonus.SetActive(false);
        MenuManager.Instance.perfectBonus.SetActive(true);
        MenuManager.Instance.perfectBonus.GetComponent<Animator>().Play("IngamePerfect");
        comboCount++;//连击数
        if(comboCount <= 5)
        {
            platform.renderer.sharedMaterial = succes;
        }
        else
        {
            ProfileManager.Instance.Gold++;
            if (comboCount == 12)
                ProfileManager.Instance.Gold += 5;
            platform.renderer.sharedMaterial = gGrid;
        }
    }

    public bool SuccesJump(Platform platform)
    {
        bool isFerfectJumpFail = false;
        previousStack = currentStack;
        currentStack = platform;
        if (challengeType == eChallengeType.None)
        {
            jumpIndex++;
            bool isClear = PerfectJump(platform, ProfileManager.Instance.CharacterController);
            if (!isClear)
                jumpIndex = 0;
            score++;
            MenuManager.Instance.scoreText.text = score.ToString();
            if (comboCount >= 12 || jumpIndex != comboCount)
            {
                //Debug.LogError("结束 连击===================>> clear" + comboCount);
                comboCount = 0;
                jumpIndex = 0;
            }
            //Debug.LogError("【prefect jump】 ====>> " + comboCount + " 【jumpIndex】 ==>>" + jumpIndex);
        }
        if (challengeType == eChallengeType.PerfectJump)
        {
            if (!PerfectJump(platform, ProfileManager.Instance.CharacterController))
            {
                ProfileManager.Instance.CharacterController.Death();
                isFerfectJumpFail = true;
            }
            
        }
        //flowLight = (int)(platformCount / Spawner.Instance.indexArr.Length);
        FindOverPoint(platform.name);
        UpdateGridVFX(platform);
       
        return isFerfectJumpFail;
    }

    public float Swing()
    {
        return Mathf.Sin(Time.fixedTime * Mathf.PI * 0.6f + previousStack.SinY) * previousStack.transform.position.y / (maxPlatformSwing * spawnerStep);
    }

    public float Swing(float sinY, float positionY)
    {
        return Mathf.Sin(Time.fixedTime * Mathf.PI * 0.6f + sinY) * positionY / (maxPlatformSwing * spawnerStep);
    }

    private Platform currZoomIn;
    private Platform nextName;
    private int flowLight;

    private string  endPoint;

    private void  FindOverPoint(string name)
    {
        //Debug.Log("count:" + CurrentPlatformCount + "  name;" + name);
        GameObject[] tmpArr = Spawner.Instance.indexArr;
        int len = tmpArr.Length - 1;
        if((platformCount - 1) < len)
            return;
        for (int i = len; i >= 0; i--)
        {
            GameObject tmpObj = tmpArr[i];
            if (tmpObj.activeSelf && name.Equals(tmpObj.name))//显示中
            {
                if(i == len)
                    endPoint = tmpArr[1].name;
                else if(i+1 == len)
                    endPoint = tmpArr[0].name;
                else
                    endPoint = tmpArr[i+2].name;
            }
        }
        //Debug.LogError("end point ===>>" + endPoint);
    }
    private Transform FindTF(string name, bool isNext = false)
    {
        Transform tmpTF = null;
        GameObject[] tmpArr = Spawner.Instance.indexArr;
        int len = tmpArr.Length - 1;
        for (int i = len; i >= 0; i--)
        {
            GameObject tmpObj = tmpArr[i];
            if (tmpObj.activeSelf)//显示中
            {
                tmpTF = tmpObj.transform; //   当前显示
                if (name == tmpObj.name && false == isNext)
                {// 查找下一个显示对象
                    if (i > 0)
                        nextName = tmpArr[i - 1].GetComponent<Platform>();
                    else if(i == 0 && platformCount > len)
                        nextName = tmpArr[len].GetComponent<Platform>();
                    else
                        nextName = null;
                    break;
                }
            }
        }
        return tmpTF;
    }
    public void UpdateGridVFX(Platform tmpObj)
    {
        //         currZoomIn = FindTF(name);
        //         if(null != currZoomIn)
        //         {
        //             //currZoomIn.localScale = zoomIn;
        //             currZoomIn.GetChild(0).gameObject.SetActive(false);
        //             currZoomIn.GetChild(1).gameObject.SetActive(true);
        //         }
        
        FindTF(tmpObj.name);
        tmpObj.PlayVFX();
        Invoke("recover", 0.1f);
    }
    
    private void recover()
    {
//         if (null == currZoomIn) 
//             return;
        //currZoomIn.localScale = Vector3.one;
//         currZoomIn.GetChild(0).gameObject.SetActive(true);
//         currZoomIn.GetChild(1).gameObject.SetActive(false);
        if (null != nextName && string.IsNullOrEmpty(endPoint) ||
            null != nextName && !endPoint.Equals(nextName.name))
            {
                UpdateGridVFX(nextName);
            }
    }
}
