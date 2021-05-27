using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using GameAnalyticsSDK.Setup;
using System;

public class MenuManager : MonoBehaviour
{

    private static MenuManager instance;

    public static MenuManager Instance
    {
        get
        {
            if (instance == null)
                Instantiate(Resources.Load("MenuManager"));
            return instance;
        }
    }

    [Header("按钮文本显示")]
    public Text m_textHieghtScore;
    public Text m_textCharaterChooseButton;
    public Text m_textPlay;
    public Text m_textChallengesSelect;

    [Header("Ingame")]

    public Text scoreText;
    public Text gameOverScoreText;

    public GameObject gameOver;
    public GameObject newRecord;

    public GameObject inGameNewRecord;

    public GameObject perfectBonus;

    [Header("Menu")]

    public GameObject menu;
    public Text menuScore;
    public Text promptDebug;

    public GameObject noAds;

    [Header("RestoreFeedback")]
    [SerializeField]
    GameObject restoreFeedback;

    public GameObject RestoreFeedbackObj { get { return restoreFeedback; } }

    [SerializeField]
    Text feedbackText;

    public Text FeedbackText { get { return feedbackText; } }

    [Header("SelectMenu")]
    public GameObject selectMenu;

    [Header("CreditsMenu")]
    public GameObject creditsMenu;

    [Header("Second Chance")]

    public GameObject secondChance;

    [SerializeField]
    Button secondChanceButton;

    [Header("Challenges")]
    public GameObject challenges;

    public Text challengeLevel;

    public GameObject winPanel;
    public Text winText;

    public GameObject failPanel;

    public Button nextButton;

    [Header("Unlock")]
    [SerializeField]
    PopUp popUp;
    public PopUp PopUp { get { return popUp; } }

    [SerializeField]
    UnlockedItem unlockedItem;

    float currentTime;

    [Header("Logo")]
    [SerializeField]
    Image logo;
    [SerializeField]
    Sprite normalLogo;
    [SerializeField]
    Sprite chineseLogo;


    [Header("shop")]
    [SerializeField]
    ShopControllerPanel m_shopControllerPanel;

    [Header("TipPopup")]
    public TipPopup m_tipPopup;

    bool succes = false;

    public static int gameCount { get; set; }

    // 游戏暂停
    GameObject m_PauseGamePopup;
    GameObject m_pauseGame;
    Button m_btnPauseGame;
    Text m_textPauseGame;

    [HideInInspector]
    public bool m_bIsGamePaused = false;

    [Header("新手引导")]
    // 新手引导
    [SerializeField]
    private GameObject m_GreenHand;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        m_PauseGamePopup = this.transform.Find("Canvas/PauseGamePopup").gameObject;

        // c
        m_pauseGame = this.transform.Find("Canvas/btn_PauseGame").gameObject;
        m_btnPauseGame = this.m_pauseGame.GetComponent<Button>();

        m_textPauseGame = m_pauseGame.transform.Find("text_PauseGame").gameObject.GetComponent<Text>();
        if (m_btnPauseGame)
        {
            // 点击
            m_btnPauseGame.onClick.AddListener(this.onCallBackPauseGame);
        }

        if(null != promptDebug)
            promptDebug.text = "";//Azerion内部测试专用

        if (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified || Application.systemLanguage == SystemLanguage.ChineseTraditional)
            logo.sprite = chineseLogo;
        else
            logo.sprite = normalLogo;

        // if (Application.internetReachability == NetworkReachability.NotReachable)
        //     InvictusMoreGames.MoreGamesBoxController.Instance.Hide();

        // InvictusMoreGames.MoreGamesBoxController.Instance.onJsonReadSuccess += (bool succes) => ShowMoreGames();

        // InvictusMoreGames.MoreGamesBoxController.Instance.onClicked += (string success) =>
        //     GameAnalyticsManager.LogDesignEvent("MoreGamesClicked" + ":" + InvictusMoreGames.MoreGamesBoxController.Instance.gameBox.gameName.GetLanguageElement(SystemLanguage.English).value);

        menuScore.text = ProfileManager.Instance.Record.ToString();
        scoreText.text = "0";

        // 修改按钮文本显示
        m_textHieghtScore.text = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("HighScore"));
        m_textChallengesSelect.text = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Challenges"));
        m_textPlay.text = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Play")); ;
        m_textCharaterChooseButton.text = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("ChangeCharacters")); ;

        // if (!ProfileManager.Instance.NoAds)
        //     noAds.SetActive(true);
        // else
        //     noAds.SetActive(false);

        if (ProfileManager.Instance.IsFirstEnterMenu && 
            !ProfileManager.Instance.FirstPlay)
       {            
            // PluginMercury.Instance.ActiveRewardVideo();
        }

        ProfileManager.Instance.IsFirstEnterMenu = false;
        Time.timeScale = 1;
        // 新手引导        
        if (ProfileManager.Instance.PlayedGames <= 0)
        {            
            m_GreenHand.SetActive(true);
        }
    }

    private void ShowMoreGames()
    {        
        if (Application.internetReachability != NetworkReachability.NotReachable && InvictusMoreGames.MoreGamesBoxController.Instance.JsonReadSuccess && InvictusMoreGames.MoreGamesBoxController.Instance.IsActive)
        {
            InvictusMoreGames.MoreGamesBoxController.Instance.Show();
            InvictusMoreGames.MoreGamesBoxController.Instance.ShowNewGame();
            succes = true;
        }
        else
            InvictusMoreGames.MoreGamesBoxController.Instance.Hide();
    }

    private void Start()
    {
        if (ProfileManager.Instance.IsRestart)
        {
            Replay();
        }
        currentTime = 1f / ProfileManager.Instance.SecondChanceTime;
    }

    private void Update()
    {
        if (secondChance.activeInHierarchy && SecondChance.isNotBlock)
        {
            secondChanceButton.GetComponent<Image>().fillAmount -= currentTime * Time.deltaTime;
            if (secondChanceButton.GetComponent<Image>().fillAmount == 0)
            {
                if (GameLogic.Instance.challengeType == eChallengeType.None)
                    GameOverMenu();
                else
                    Fail();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu.activeInHierarchy)
                Application.Quit();

            if (selectMenu.activeInHierarchy && !popUp.gameObject.activeInHierarchy)
                Back(selectMenu);
            else if (selectMenu.activeInHierarchy && popUp.gameObject.activeInHierarchy)
                popUp.gameObject.SetActive(false);


            if (challenges.activeInHierarchy)
                Back(challenges);
        }
    }

    public void NextChallenge()
    {
        ProfileManager.Instance.SavePlatformSpeedChallange(GameLogic.Instance.challengeType, GameLogic.Instance.challengeLevelNumber + 1);
        ProfileManager.Instance.SetSpeed(1);
        GameLogic.Instance.SetDifficulty();
        GameLogic.Instance.challengeLevelNumber = GameLogic.Instance.challengeLevelNumber + 1;
        ProfileManager.Instance.SetMaxPlatform(GameLogic.Instance.challengeType, GameLogic.Instance.challengeLevelNumber);
        RestartGame();
    }

    public void Play()
    {
        gameCount++;
        // 关闭新手引导
        m_GreenHand.SetActive(false);
        // InvictusMoreGames.MoreGamesBoxController.Instance.gameObject.SetActive(false);

        menu.gameObject.SetActive(false);
        if (GameLogic.Instance.challengeType == eChallengeType.None)
        {
            scoreText.transform.parent.gameObject.SetActive(true);
            scoreText.transform.gameObject.SetActive(true);
        }
        else
            scoreText.transform.gameObject.SetActive(false);

        Time.timeScale = 1;
        GameLogic.Instance.InGame = true;
        ProfileManager.Instance.UnlockItems(eUnlockType.Play);
        GameLogic.Instance.challengeType = eChallengeType.None;
        Spawner.Instance.CreatePlatform();
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, GameLogic.Instance.challengeType.ToString(), GameLogic.Instance.challengeLevelNumber.ToString());
        //GameAnalyticsManager.LogProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Start, GameLogic.Instance.challengeType.ToString(), GameLogic.Instance.challengeLevelNumber.ToString());
        
        m_pauseGame.SetActive(true);
        GameLogic.Instance.IsCollapse = false;
        GameLogic.Instance.IsBrarrierDie = false;
        // 新手提示
        GreenHandTip();
        UIManager.Instance.ShowStabilizationTip(false);
        StartRecorder();
        Debug.Log("游戏开始！");
    }

    public void Play(eChallengeType challengeType)
    {
        gameCount++;
        m_GreenHand.SetActive(false);

        menu.gameObject.SetActive(false);
        if (GameLogic.Instance.challengeType == eChallengeType.None)
        {
            scoreText.transform.parent.gameObject.SetActive(true);
            scoreText.transform.gameObject.SetActive(true);
        }
        else
            scoreText.transform.gameObject.SetActive(false);

        Time.timeScale = 1;
        GameLogic.Instance.InGame = true;
        ProfileManager.Instance.UnlockItems(eUnlockType.Play);

        GameLogic.Instance.challengeType = challengeType;
        ProfileManager.Instance.SavePlatformSpeedChallange(challengeType, GameLogic.Instance.challengeLevelNumber);
        if (GameLogic.Instance.challengeType == eChallengeType.None)
        {
            scoreText.transform.parent.gameObject.SetActive(true);
            scoreText.transform.gameObject.SetActive(true);
        }
        else
        {
            int challengeLevelNumber = GameLogic.Instance.challengeLevelNumber + 1;
            scoreText.transform.gameObject.SetActive(false);
            challengeLevel.text = "关卡 " + challengeLevelNumber;
            challengeLevel.transform.gameObject.SetActive(true);
        }
        Spawner.Instance.CreatePlatform();
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, challengeType.ToString(), GameLogic.Instance.challengeLevelNumber.ToString());
        //GameAnalyticsManager.LogProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Start, challengeType.ToString(), GameLogic.Instance.challengeLevelNumber.ToString());        
        m_pauseGame.SetActive(true);
        GameLogic.Instance.IsCollapse = false;
        GameLogic.Instance.IsBrarrierDie = false;
        // 新手提示
        GreenHandTip();
        UIManager.Instance.ShowStabilizationTip(false);
    }

    public void Replay()
    {
        m_GreenHand.SetActive(false);
        menu.gameObject.SetActive(false);
        // InvictusMoreGames.MoreGamesBoxController.Instance.Hide();

        if (GameLogic.Instance.challengeType == eChallengeType.None)
        {
            scoreText.transform.parent.gameObject.SetActive(true);
            scoreText.transform.gameObject.SetActive(true);
        }
        else
        {
            int challengeLevelNumber = GameLogic.Instance.challengeLevelNumber + 1;
            scoreText.transform.gameObject.SetActive(false);
            challengeLevel.text = "关卡 " + challengeLevelNumber;
            challengeLevel.transform.gameObject.SetActive(true);
        }

        Time.timeScale = 1;
        GameLogic.Instance.InGame = true;
        ProfileManager.Instance.UnlockItems(eUnlockType.Play);
        GameLogic.Instance.challengeType = ProfileManager.Instance.ChallengeType;
        GameLogic.Instance.challengeLevelNumber = ProfileManager.Instance.challengeNumber;

        ProfileManager.Instance.SetMaxPlatform(GameLogic.Instance.challengeType, GameLogic.Instance.challengeLevelNumber);
        Spawner.Instance.CreatePlatform();

        m_pauseGame.SetActive(true);
        GameLogic.Instance.IsCollapse = false;
        GameLogic.Instance.IsBrarrierDie = false;
        UIManager.Instance.ShowStabilizationTip(false);
    }

    public void ResetScore()
    {
        PlayerPrefs.DeleteAll();
        menuScore.text = "0";
        ProfileManager.Instance.Record = 0;
        ProfileManager.Instance.FirstPlay = true;
        ProfileManager.Instance.PlayedGames = 0;
        ProfileManager.Instance.FullPerfectJump = 0;
        ProfileManager.Instance.SecondChanceCount = 0;
        ProfileManager.Instance.DayInRow = 0;
        ProfileManager.Instance.CheckInDay = 0;
        ProfileManager.Instance.HighJumpChallenges = 0;
        ProfileManager.Instance.PerfectJumpChallenges = 0;
        ProfileManager.Instance.JumpOverChallenges = 0;
        ProfileManager.Instance.ScalePlatformChallenges = 0;

        ProfileManager.Instance.SetBlob(0, GameLogic.Instance.CharacterStartPoint.transform);
        ProfileManager.Instance.SetEnviroment(0, GameLogic.Instance.EnviromentPoint.transform);

        foreach (var item in ProfileManager.Instance.blobs)
        {
            if (item.Name != "Blob")
                item.Unlocked = false;
        }

        foreach (var item in ProfileManager.Instance.enviroments)
        {
            if (item.Name != "Child's room")
                item.Unlocked = false;
        }

        foreach (var item in ProfileManager.Instance.Musics)
        {
            if (item.Index != 0)
                item.Unlocked = false;
        }

        ProfileManager.Instance.ResetChallenge();
    }

    public void UnlockAll()
    {
        foreach (var item in ProfileManager.Instance.blobs)
            item.Unlocked = true;

        foreach (var item in ProfileManager.Instance.enviroments)
            item.Unlocked = true;

        foreach (var item in ProfileManager.Instance.Musics)
            item.Unlocked = true;

        foreach (var item in ProfileManager.Instance.Challenges)
            foreach (var item2 in item.challengesLevels)
                item2.unlocked = true;


    }


    public void GameOverMenu()
    {
        if (!ProfileManager.Instance.NoAds && ProfileManager.Instance.IntersitialCounter == 3)
        {
            // AdManagerIronsrc.Instance.ShowInterstitialAd();
            ProfileManager.Instance.IntersitialCounter = 0;
        }

        ProfileManager.Instance.IntersitialCounter++;

        GameLogic.Instance.InGame = false;
        int score = GameLogic.Instance.Score;
        int highScore = ProfileManager.Instance.Record;
        secondChance.SetActive(false);
        gameOver.SetActive(true);
        scoreText.transform.parent.gameObject.SetActive(false);
        gameOverScoreText.text = score.ToString();
        if (score > highScore)
        {
            ProfileManager.Instance.Record = score;
            newRecord.SetActive(true);
        }
        ProfileManager.Instance.FirstPlay = false;

        if (ProfileManager.Instance.unlockedItems.Count > 0)
            unlockedItem.gameObject.SetActive(true);

        if(gameCount >= 3)
        {
            PluginMercury.Instance.ActiveRewardVideo();
            gameCount = 0;
        }
    }

    public void GameOver(Collision collision)
    {
        UIManager.Instance.ShowStabilizationTip(false);
        m_pauseGame.SetActive(false);
        if (!GameLogic.Instance.FirstDeath)
        {

            if (GameLogic.Instance.challengeType == eChallengeType.None)
                GameOverMenu();
            else
                Fail();

            /*if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
                GameLogic.Instance.stack.gameObject.SetActive(false);
            else
            {
                GameLogic.Instance.stack.gameObject.SetActive(false);
                GameLogic.Instance.saw.gameObject.SetActive(false);
            }*/
            if (GameLogic.Instance.stack)
                GameLogic.Instance.stack.gameObject.SetActive(false);
            if (GameLogic.Instance.saw)
                GameLogic.Instance.saw.gameObject.SetActive(false);

        }
        else
        {
            secondChance.SetActive(true);
            GameLogic.Instance.InGame = false;
            GameLogic.Instance.FirstDeath = false;

            /*if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
                GameLogic.Instance.stack.gameObject.SetActive(false);

            else
            {
                GameLogic.Instance.stack.gameObject.SetActive(false);
                GameLogic.Instance.saw.gameObject.SetActive(false);
            }*/

            if (GameLogic.Instance.stack)
                GameLogic.Instance.stack.gameObject.SetActive(false);
            if (GameLogic.Instance.saw)
                GameLogic.Instance.saw.gameObject.SetActive(false);
        }
    }


    public void Win(int index)
    {
        UIManager.Instance.ShowStabilizationTip(false);
        m_pauseGame.SetActive(false);

        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, GameLogic.Instance.challengeType.ToString(), GameLogic.Instance.challengeLevelNumber.ToString());
        //GameAnalyticsManager.LogProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Complete, GameLogic.Instance.challengeType.ToString(), GameLogic.Instance.challengeLevelNumber.ToString());
        if (GameLogic.Instance.saw)
            GameLogic.Instance.saw.gameObject.SetActive(false);

        // 完美模式下  通过加金币
        WinAward winAward = ProfileManager.Instance.GetWinAward(GameLogic.Instance.challengeType, index - 1);
        if (winAward != null)
        {
            switch (winAward.m_eAward)
            {
                case eCurrencyType.diamond:
                    ProfileManager.Instance.Diamond += winAward.m_iAwardCount;
                    break;
                case eCurrencyType.gold:
                    // 加金币
                    ProfileManager.Instance.Gold += winAward.m_iAwardCount;
                    break;
                default:
                    break;
            }
        }

        scoreText.transform.parent.gameObject.SetActive(false);
        GameLogic.Instance.InGame = false;
        winText.text = "关卡 " + index + " 完成";
        // 将下标传入到GameOver组件
        winPanel.GetComponent<WinPanel>().m_index = index - 1;
        winPanel.SetActive(true);

        ProfileManager.Instance.UnlockItemsByChallenge(GameLogic.Instance.challengeType, index - 1);

        if (ProfileManager.Instance.unlockedItems.Count > 0)
            unlockedItem.gameObject.SetActive(true);

        if (GameLogic.Instance.challengeLevelNumber == 11)
            nextButton.interactable = false;

        ProfileManager.Instance.CompleteChallenge(GameLogic.Instance.challengeType, index - 1);
        ParticleManager.Instance.confetti.Play();
        if(gameCount >= 3)
        {
            PluginMercury.Instance.ActiveRewardVideo();
            gameCount = 0;
        }
    }


    public void Fail()
    {
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, GameLogic.Instance.challengeType.ToString(), GameLogic.Instance.challengeLevelNumber.ToString());
        //GameAnalyticsManager.LogProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Fail, GameLogic.Instance.challengeType.ToString(), GameLogic.Instance.challengeLevelNumber.ToString());

        GameLogic.Instance.InGame = false;
        secondChance.SetActive(false);
        failPanel.SetActive(true);
        if (ProfileManager.Instance.unlockedItems.Count > 0)
            unlockedItem.gameObject.SetActive(true);

        if (!ProfileManager.Instance.NoAds && ProfileManager.Instance.IntersitialCounter == 3)
        {
            //AdManagerIronsrc.Instance.ShowInterstitialAd();
            ProfileManager.Instance.IntersitialCounter = 0;
        }

        ProfileManager.Instance.IntersitialCounter++;
        if(gameCount >= 3)
        {
            PluginMercury.Instance.ActiveRewardVideo();
            gameCount = 0;
        }
    }

    public void RestartGame()
    {
        gameCount++;
        ProfileManager.Instance.IsRestart = true;
        ProfileManager.Instance.ChallengeType = GameLogic.Instance.challengeType;
        ProfileManager.Instance.challengeNumber = GameLogic.Instance.challengeLevelNumber;
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, GameLogic.Instance.challengeType.ToString(), GameLogic.Instance.challengeLevelNumber.ToString());
        //GameAnalyticsManager.LogProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Start, GameLogic.Instance.challengeType.ToString(), GameLogic.Instance.challengeLevelNumber.ToString());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        ProfileManager.Instance.IsRestart = false;
        ProfileManager.Instance.ChallengeType = eChallengeType.None;
        ProfileManager.Instance.challengeNumber = 0;
        ProfileManager.Instance.SavePlatformSpeed();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CreditsMenu()
    {
        // InvictusMoreGames.MoreGamesBoxController.Instance.Hide();
        menu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void SelectMenu()
    {
        // InvictusMoreGames.MoreGamesBoxController.Instance.Hide();
        menu.SetActive(false);
        selectMenu.SetActive(true);
        Banner_AD_Store();
    }

    public void ChallengesMenu()
    {
        // InvictusMoreGames.MoreGamesBoxController.Instance.Hide();
        menu.SetActive(false);
        challenges.SetActive(true);
        Banner_AD_Challenge();

        // 首次进入新手提示
        if (ProfileManager.Instance.FirstOpenChallenges)
        {
            UIManager.Instance.ShowGreenHandTipMsg("在挑战关卡通关后，可以获得金币哟！", false);
        }

        ProfileManager.Instance.FirstOpenChallenges = false;

    }

    public void Back(GameObject deactive)
    {
        if (succes)
        {
            // InvictusMoreGames.MoreGamesBoxController.Instance.Show();
            // InvictusMoreGames.MoreGamesBoxController.Instance.ShowNewGame();
        }
        menu.SetActive(true);
        deactive.SetActive(false);
    }

    public void ClosePopUp()
    {
        popUp.gameObject.SetActive(false);
    }

    public void CloseRestoreFeedback()
    {
        restoreFeedback.SetActive(false);
    }

    public void CloseRestoreFeedback(string response)
    {
        feedbackText.text = response;
    }

    public void Skip()
    {
        secondChance.SetActive(false);
        if (GameLogic.Instance.challengeType == eChallengeType.None)
            GameOverMenu();
        else
            Fail();
        Plaque_AD();
        StopRecorder();
        Debug.Log("闯关结束！");
    }

    public void Continue()
    {
        Time.timeScale = 0;
        // AdManagerIronsrc.Instance.ShowVideoAd((succes) => {
        //     if (succes)
        //     {
        //         Time.timeScale = 1;
        //         StopCoroutine(ParticleManager.Instance.StopParticle(3));
        //         secondChance.SetActive(false);
        //         ParticleManager.Instance.death.Play();
        //         ParticleManager.Instance.death.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        //         GameLogic.Instance.InGame = true;
        //         GameLogic.Instance.IsAfterAd = true;
        //         Spawner.Instance.CreatePlatform();
        //         ProfileManager.Instance.UnlockItems(eUnlockType.SecondChance);
        //     }
        //     else
        //     {
        //         gameOver.SetActive(true);
        //         secondChance.SetActive(false);
        //     }
        // });

    }

    public void BuyNoAds()
    {
        //IAPManager.Instance.PurchaseNonConsumableProduct(eIAP.noAds);
    }

    public void RestorePurchase()
    {
        //IAPManager.Instance.Restore();
    }

    public void RestoreFeedback(bool succes)
    {
        if (succes)
        {
            restoreFeedback.SetActive(true);
            feedbackText.text = "Restore Successful";
        }
        else
        {
            restoreFeedback.SetActive(true);
            feedbackText.text = "Failed Restore";
        }

    }

    public void BuyNoadsResponse(bool succes)
    {
        if (succes)
        {
            restoreFeedback.SetActive(true);
            feedbackText.text = "Please Wait...";
        }
        else
        {
            restoreFeedback.SetActive(true);
            feedbackText.text = "Failed Purchase";
        }

    }

    // 游戏暂停
    public void onCallBackPauseGame()
    {
        // 暂停
        PauseGame(true);
    }

    public void PauseGame(bool isGamePaused)
    {
        if (isGamePaused)
        {
            m_bIsGamePaused = true;
            // 游戏暂停
            Time.timeScale = 0;
            // 显示和值是反的
            m_textPauseGame.text = "继续";

            m_PauseGamePopup.SetActive(true);
            m_pauseGame.SetActive(false);
        }
        else
        {

            m_bIsGamePaused = false;
            // 继续游戏
            Time.timeScale = 1;
            // 显示和值是反的
            m_textPauseGame.text = "暂停";

            m_PauseGamePopup.SetActive(false);
            m_pauseGame.SetActive(true);
        }
    }


    // 游戏暂停时 返回主菜单
    public void BackMenu()
    {
        // 返回之前先清理游戏数据
        GameLogic.Instance.InGame = false;
        GameLogic.Instance.FirstDeath = false;
        m_PauseGamePopup.SetActive(false);
        this.Menu();
    }

    // 打开商城
    public void OpenShop()
    {
        // 返回
        if (this.m_shopControllerPanel)
        {
            this.m_shopControllerPanel.gameObject.SetActive(true);
        }
    }

    // 测试
    public void AddGoldTest()
    {
        ProfileManager.Instance.Gold += 1000;
    }

    void GreenHandTip()
    {
        // 首次玩游戏
        if (ProfileManager.Instance.PlayedGames <= 1)
        {
            DebugManager.LogInfo("==========显示新手提示 跳跃");
            //UIManager.Instance.ShowGreenHandTipMsg("点击屏幕可以使点点跳起来！", true, ()=>{
            //    UIManager.Instance.ShowGreenHandTipMsg("当飞盘移动过来，跳跃点点，飞盘将移动至点点下方并堆叠起来");
            //});
        }
    }

    public void Plaque_AD()//结算时的插屏广告
    {

    }

    public void Banner_AD_Store()//打开商店调用Banner广告
    {

    }
    public void Banner_AD_Challenge()//打开挑战调用Banner广告
    {

    }

    public void StartRecorder()//游戏录屏开始
    {

    }

    public void StopRecorder()//游戏录屏结束
    {

    }

    public void Share()//分享领取
    {
        Debug.Log("分享成功！");
    }

    private void ShareReward()
    {
        Debug.Log("获得奖励！");
        ProfileManager.Instance.Gold += 100;
    }
}
