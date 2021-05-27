using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SignIn : MonoBehaviour
{
    [Header("签到需要的控制变量")]
    public GameObject SignIn_bg;//签到界面
    public Text[] TiShi_Txt;//签到内容解释
    public Image[] Icon;//奖励icon背景
    public Sprite[] icon_image;//奖励的图标
    public Text[] time;//奖励时间显示
    public GameObject[] grayButton;//领取灰色按钮
    public GameObject[] HongDian;//可以领取红点提示
    public GameObject[] Linqu;//已经领取提示

    public DateTime today;
    public DateTime lastday;
    public TimeSpan Hour= TimeSpan.Zero;
    public DateTime countDown;

    [Header("随机奖励的变量")]
    Blob blob;
    Music Music;
    Enviroment Enviroment;
    private int[] role = new int[12];
    private int[] map = new int[4];
    private int[] music = new int[9];
    private int i, j = 0;

    /// 获取上次签到日期
    public string GetSignData()
    {
        if (PlayerPrefs.HasKey("signData"))
            return PlayerPrefs.GetString("signData");
        return DateTime.MinValue.ToString();
    }

    /// 设置上次签到日期
    public void SetSignData(DateTime data)
    {
        PlayerPrefs.SetString("signData", data.ToString());
    }

    /// 获取倒计时最终时间
    public string GetCountDownData()
    {
        if (PlayerPrefs.HasKey("CountDownData"))
            return PlayerPrefs.GetString("CountDownData");
        return DateTime.MinValue.ToString();
    }

    /// 保存倒计时最终时间
    public void SetCountDownData(DateTime data)
    {
        PlayerPrefs.SetString("CountDownData", data.ToString());
    }

    public static int SignInNumber//签到天数
    {
        get { return PlayerPrefs.GetInt("SignInNumber", 0); }
        set { PlayerPrefs.SetInt("SignInNumber", value); }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        today = DateTime.Now;
        lastday = DateTime.Parse(GetSignData());
        if(SignInNumber==0)
        {
            for (int i = 0; i < 5; i++)
            {
                PlayerPrefs.SetInt("StateControl" + i, 1);
            }     
        }
        if(SignInNumber<5)
        {
            if (!IsOneDay(today, lastday))
            {
                StartCoroutine(CheckForResettingQuestData());
            }          
        }

    }

    public void OnSignIn()//点击领取按钮
    {
        lastday= DateTime.Parse(GetSignData());
        if (!IsOneDay(today, lastday))
        {
            OnDaySign(SignInNumber);
            SignState(SignInNumber);
            SignInNumber++;
            SignXianShi();
            if (SignInNumber==1|| SignInNumber==3)
            {
                today = DateTime.Now;
                SetCountDownData(today.AddHours(2));
                StartCoroutine(CheckForResettingQuestData());
            }          
            Debug.Log("当前签到时间是：" + SignInNumber);
        }
    }

    public void OnDaySign(int dayNumber)//奖励领取
    {
        PlayerPrefs.SetInt("StateControl" + dayNumber, 0);
        switch (dayNumber)
        {
            case 0:
                    ProfileManager.Instance.Gold += 100;
           
                break;
            case 1:
                foreach (var item in ProfileManager.Instance.Musics)
                {
                    if (item.Unlocked == false)
                    {
                        music[i] = j;
                        i++;
                    }
                    j++;
                }
                Music = ProfileManager.Instance.Musics[UnityEngine.Random.Range(music[0], music[i - 1] + 1)];
                Music.Unlocked = true;
                i = 0; j = 0;
                SetSignData(DateTime.Now);
                break;
            case 2:
                foreach (var item in ProfileManager.Instance.blobs)
                {
                    if (item.Unlocked == false)
                    {
                        role[i] = j;
                        i++;
                    }
                    j++;
                }
                blob = ProfileManager.Instance.blobs[UnityEngine.Random.Range(role[0], role[i - 1] + 1)];
                blob.Unlocked = true;
                i = 0; j = 0;
                break;
            case 3:
                ProfileManager.Instance.Gold += 200;
                SetSignData(DateTime.Now);
                break;
            case 4:
                foreach (var item in ProfileManager.Instance.enviroments)
                {
                    if (item.Unlocked == false)
                    {
                        map[i] = j;
                        Debug.Log("当前bool得值是：" + i + "      " + j);
                        i++;
                    }
                    j++;
                }
                Enviroment = ProfileManager.Instance.enviroments[UnityEngine.Random.Range(map[0], map[i - 1] + 1)];
                Enviroment.Unlocked = true;
                i = 0; j = 0;
                SetSignData(DateTime.Now);
                break;

        }
    }

    public void SignState(int day)//签到状态
    {
        if (PlayerPrefs.GetInt("StateControl" + day)==0)
        {
            grayButton[day].SetActive(true);
            HongDian[day].SetActive(false);
            Debug.Log("这里来了没有：" + day);
        }
        switch (day)
        {
            case 0:
                Icon[0].sprite = icon_image[1];
                TiShi_Txt[0].text = "2小时后领取随机音乐";
                break;
            case 2:
                Icon[1].sprite = icon_image[0];
                TiShi_Txt[1].text = "2小时后领取200金币";
                break;
            case 4:
                break;
        }
    }

    IEnumerator CheckForResettingQuestData()//检查重置广告的时间
    {
        countDown = DateTime.Parse(GetCountDownData());
        bool asadas = true;
        while (asadas)
        {
            Hour = countDown - DateTime.Now;
         
            if (Hour.TotalSeconds <= 0)
            {
                asadas = false;   
                yield return Hour = TimeSpan.Zero;
                grayButton[SignInNumber].SetActive(false);
                HongDian[SignInNumber].SetActive(true);
            }
            Debug.Log("当前时间是：" + Hour);
            time[SignInNumber].text = string.Format("{0}:{1}:{2}", Hour.Hours.ToString("00"), Hour.Minutes.ToString("00"), Hour.Seconds.ToString("00"));
            yield return new WaitForSecondsRealtime(1);
        }
    }

    public void SignXianShi()
    {
        for (int i = 0; i <= SignInNumber; i++)
        {
            switch (i)
            {
                case 2:
                    Linqu[0].SetActive(true);
                    break;
                case 4:
                    Linqu[1].SetActive(true);
                    break;
                case 5:
                    Linqu[2].SetActive(true);
                    break;
            }
        }
    }

    bool IsOneDay(DateTime t1, DateTime t2)
    {
        return (t1.Year == t2.Year &&
         t1.Month == t2.Month &&
          t1.Day == t2.Day);
    }

    public void OpenSignIn()
    {
        SignIn_bg.SetActive(true);
        if (SignInNumber == 1|| SignInNumber == 3)
        {
            if (!IsOneDay(today, lastday))
            {
                StartCoroutine(CheckForResettingQuestData());
            }
        }
            SignXianShi();
    }
    public void CloseSignIn()
    {
        SignIn_bg.SetActive(false);
    }
}
