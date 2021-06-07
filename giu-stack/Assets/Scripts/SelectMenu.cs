using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour
{
    public static SelectMenu Instance;

    [Header("替换文本")]
    public Text m_textToggleCharacters;
    public Text m_textToggleThemes;
    public Text m_textToggleMusic;

    [SerializeField]
    RectTransform blobList;

    [SerializeField]
    RectTransform themeList;

    [SerializeField]
    RectTransform musicList;

    public Toggle characterToggle;
    public Toggle themeToggle;
    public Toggle musicToggle;

    public GameObject characterChoose;
    public GameObject themeChoose;
    public GameObject musicChoose;

    private void Awake()
    {

        characterToggle.onValueChanged.AddListener((isOn) =>
        {
            characterChoose.SetActive(true);
            themeChoose.SetActive(false);
            musicChoose.SetActive(false);
        });

        themeToggle.onValueChanged.AddListener((isOn) =>
        {
            characterChoose.SetActive(false);
            themeChoose.SetActive(true);
            musicChoose.SetActive(false);
        });
        musicToggle.onValueChanged.AddListener((isOn) =>
        {
            characterChoose.SetActive(false);
            themeChoose.SetActive(false);
            musicChoose.SetActive(true);
        });


        if (Instance == null)
            Instance = this;

        foreach (var item in ProfileManager.Instance.blobs)
        {
            SelectBlob blobButton = Instantiate(Resources.Load<SelectBlob>("Ui/SelectBlob"), blobList);
            //            blobButton.name = item.Name;
            blobButton.name = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Character.Name." + item.Name) );
            blobButton.index = item.Index;
            blobButton.image.sprite = item.Image;
            blobButton.unlocked = item.Unlocked;
        }

        foreach (var item in ProfileManager.Instance.enviroments)
        {
            SelectEnviroment enviromentButton = Instantiate(Resources.Load<SelectEnviroment>("Ui/SelectEnviroment"), themeList);
            // enviromentButton.name = item.Name;
            enviromentButton.name = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Enviroments.Name." + item.Name) );
            enviromentButton.index = item.Index;
            enviromentButton.image.sprite = item.Image;
            enviromentButton.unlocked = item.Unlocked;
        }

        foreach (var item in ProfileManager.Instance.Musics)
        {
            SelectMusic musicButton = Instantiate(Resources.Load<SelectMusic>("Ui/SelectMusic"), musicList);
            // musicButton.name = item.Name;
            musicButton.name = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Music.Name." + item.Name) );
            musicButton.index = item.Index;
            musicButton.image.sprite = item.UnlockImage;
            musicButton.unlocked = item.Unlocked;
        }

        // 文本替换
        m_textToggleCharacters.text = String.Format( SmartLocalization.LanguageManager.Instance.GetTextValue("Characters") );;
        m_textToggleThemes.text = String.Format( SmartLocalization.LanguageManager.Instance.GetTextValue("Enviroment") );;
        m_textToggleMusic.text = String.Format( SmartLocalization.LanguageManager.Instance.GetTextValue("Music") );;        
    }

    bool isOpen = false;

    protected void OnEnable()
    {
        //characterToggle.isOn = true;
        isOpen = true;
        ChangeImage();
        ChangeThemeImage();
        ChangeMusicImage();

        // 注册广告加载成功的回调
        EventDispatcher.Instance.AddEventListener(EventKey.AdShowSuccessCallBack, onAdShowSuccessCallBack);
        Debug.Log("初始化成功：");
    }



    protected void OnDisable()
    {
        // 注销广告加载成功事件
        EventDispatcher.Instance.RemoveEventListener(EventKey.AdShowSuccessCallBack, onAdShowSuccessCallBack);
        Debug.Log("这里来过没有！");
    }

    private void Update()
    {
        if (isOpen)
        {
            characterToggle.isOn = true;
            characterChoose.SetActive(true);
            musicChoose.SetActive(false);
            themeChoose.SetActive(false);
            isOpen = false;
        }

    }

    public void ChangeImage()
    {
        foreach (Transform item in blobList)
        {
            SelectBlob curBlob = item.GetComponent<SelectBlob>();

            if (ProfileManager.Instance.BlobIndex != curBlob.index && curBlob.unlocked)
                item.GetComponent<Image>().sprite = ProfileManager.Instance.characterOn;
            else if (ProfileManager.Instance.BlobIndex != curBlob.index && !curBlob.unlocked)
                item.GetComponent<Image>().sprite = ProfileManager.Instance.characterOff;
            else
                item.GetComponent<Image>().sprite = ProfileManager.Instance.characterSelected;
        }
    }

    public void ChangeThemeImage()
    {
        foreach (Transform item in themeList)
        {
            SelectEnviroment curEnviroment = item.GetComponent<SelectEnviroment>();

            if (ProfileManager.Instance.EnviromentIndex != curEnviroment.index && curEnviroment.unlocked)
                item.GetComponent<Image>().sprite = ProfileManager.Instance.characterOn;
            else if (ProfileManager.Instance.EnviromentIndex != curEnviroment.index && !curEnviroment.unlocked)
                item.GetComponent<Image>().sprite = ProfileManager.Instance.characterOff;
            else
                item.GetComponent<Image>().sprite = ProfileManager.Instance.characterSelected;
        }
    }

    public void ChangeMusicImage()
    {
        foreach (Transform item in musicList)
        {
            SelectMusic curMusic = item.GetComponent<SelectMusic>();

            if ((int)ProfileManager.Instance.EMusic - 1 != curMusic.index && curMusic.unlocked)
                item.GetComponent<Image>().sprite = ProfileManager.Instance.characterOn;
            else if ((int)ProfileManager.Instance.EMusic - 1 != curMusic.index && !curMusic.unlocked)
                item.GetComponent<Image>().sprite = ProfileManager.Instance.characterOff;
            else
                item.GetComponent<Image>().sprite = ProfileManager.Instance.characterSelected;

        }
    }

    /// <summary>
    /// 当广告加载成功之后
    /// </summary>
    /// 
    public void onAdShowSuccessCallBack(string p)
    {
        UIManager.Instance.ShowTipMsg("金币领取成功！");
        // 加金币
        ProfileManager.Instance.Gold += ProfileManager.Instance.AdGold;
    }
}
