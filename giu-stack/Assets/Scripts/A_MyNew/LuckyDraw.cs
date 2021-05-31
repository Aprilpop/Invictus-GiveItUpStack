using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class LuckyDraw : MonoBehaviour
{
    [Header("抽奖的变量")]
    public static bool _isRotate = false;//是否旋转
    private float ContinuousTime = 4;//旋转时间
    public float Speed = 500;//旋转速度
    [SerializeField]
    private float Angle = 0; // 这个是设置停止的角度
    private float _time;
    [SerializeField]
    private int Weight_value;//奖励的权重
    [Header("物品的定义")]
    public GameObject BonusDisc;//旋转的imagn
    public GameObject Lucky_bg;//背景黑慕
    public GameObject An_interface;//领奖界面
    DateTime today;
    DateTime lastDay;

    public int Double_ad = 1;

    [Header("角色和地图的随机变量")]
    Enviroment Enviroment;
    Blob blob;
    private int[] role = new int[12];
    private int[] map = new int[4];
    private int i, j = 0;

    public GameObject RewardInterface;//奖励界面
    public Image zsssss;
    public Text RewardTxt;//显示奖励数量
    public Sprite[] RewardIcon;//奖励的图标
    public GameObject[] TiShi_rad;//红点提示


    /// 获取上次签到日期
    public string GetLuckyData()
    {
        if (PlayerPrefs.HasKey("LuckyData"))
            return PlayerPrefs.GetString("LuckyData");
        return DateTime.MinValue.ToString();
    }

    /// 设置上次签到日期
    public void SetLuckyData(DateTime data)
    {
        PlayerPrefs.SetString("LuckyData", data.ToString());
    }

    public static int Ad_Number
    {
        get { return PlayerPrefs.GetInt("Ad_Number", 0); }
        set { PlayerPrefs.SetInt("Ad_Number", value); }
    }
    private void Start()
    {
        today = DateTime.Now;
        lastDay = DateTime.Parse(GetLuckyData());
        if (!IsOneDay(today, lastDay))
        {
            if (Ad_Number > 2)
            {
                Ad_Number = 0;
            }
            TiShi_rad[0].SetActive(false);
        }
        else
        {
            TiShi_rad[0].SetActive(true);
            if (Ad_Number <= 0 && Ad_Number > 3)
            {
                TiShi_rad[1].SetActive(false);
            }
            else
            {
                TiShi_rad[1].SetActive(true);
            }
                 Debug.Log("抽个奖的初始值是：" + Ad_Number);       
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (!_isRotate) return; //不旋转结束

        if (Time.time < _time) // 没结束
        {
            BonusDisc.transform.Rotate(Vector3.forward * -Speed * Time.deltaTime);
        }
        else
        {
            //结束，使用DoTween旋转到结束角度，耗时1秒
            //这里有个360，使用来防止指针回转的，如果不加这个360，你会看到指针倒退
            BonusDisc.transform.DORotate(new Vector3(0, 0, -360 - Angle), 5f, RotateMode.FastBeyond360);
            _isRotate = false; // 设置不旋转
            StartCoroutine(sadkjashd(5));
        }


    }

    IEnumerator sadkjashd(int time)
    {
        yield return new WaitForSeconds(time);
        if (_isRotate == false)
        {
            RewardInterface.SetActive(true);
            Debug.Log("停止抽奖旋转！");
        }
    }

    //外部调用，初始化时间和打开旋转
    public void SetTime(int id)
    {
        switch (id)
        {
            case 0:
                lastDay = DateTime.Parse(GetLuckyData());
                if (!IsOneDay(today, lastDay))
                {
                    //保存本次抽奖时间
                    SetLuckyData(today);
                    Weight_value = UnityEngine.Random.Range(1, 101);
                    Double_ad = 1;
                    SetAngle(Weight_value);
                    TiShi_rad[0].SetActive(true);
                    _time = Time.time + ContinuousTime;
                    _isRotate = true;
                }
                break;
            case 1:
                // 注册广告加载成功的回调
                if (Ad_Number < 3)
                {
                    Ad_Number++;
                    EventDispatcher.Instance.AddEventListener(EventKey.AdShowSuccessCallBack, onAdShowSuccessCallBack);
                    Debug.LogError("广告抽奖初始化!");
                    PluginMercury.Instance.ActiveRewardVideo();
                    if(Ad_Number==3)
                    {
                        TiShi_rad[1].SetActive(true);
                    }
                }
                else
                {
                    TiShi_rad[1].SetActive(true);
                }
                break;
        }


    }
    //外部调用，设置停止角度
    public void SetAngle(int angle)
    {
        if (angle >= 1 && angle < 51)
        {
            Angle = 0;
            zsssss.sprite = RewardIcon[0];
            RewardTxt.text = "恭喜获得金币*"+100* Double_ad;
        }
        if (angle >= 51 && angle < 71)
        {
            Angle = UnityEngine.Random.Range(-40, -80);
            zsssss.sprite = RewardIcon[0];
            RewardTxt.text = "恭喜获得金币*"+ 200 * Double_ad;
        }
        if (angle >= 71 && angle < 81)
        {
            Angle = UnityEngine.Random.Range(-100, -140);
            zsssss.sprite = RewardIcon[0];
            RewardTxt.text = "恭喜获得金币*"+ 400 * Double_ad;
        }
        if (angle >= 81 && angle < 91)
        {
            Angle = UnityEngine.Random.Range(-160, -200);
            zsssss.sprite = RewardIcon[0];
            RewardTxt.text = "恭喜获得金币*"+ 800 * Double_ad;
        }
        if (angle >= 91 && angle < 96)
        {
            foreach (var item in ProfileManager.Instance.blobs)
            {
                if (item.Unlocked == false)
                {
                    role[i] = j;
                    Debug.Log("当前bool得值是：" + i + "      " + j);
                    i++;
                }
                j++;
            }
            Angle = UnityEngine.Random.Range(-220, -260);
            zsssss.sprite = RewardIcon[1];
            RewardTxt.text = "恭喜随机获得神秘角色";
        }
        if (angle >= 96 && angle <= 100)
        {
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
            Angle = UnityEngine.Random.Range(-280, -320);
            zsssss.sprite = RewardIcon[2];
            RewardTxt.text = "恭喜随机获得神秘地图";
        }

    }

    public void GetTheRewards()
    {
        if (Angle == 0)
        {
            ProfileManager.Instance.Gold += 100 * Double_ad;
        }
        if (Angle <= -40 && Angle >= -80)
        {
            ProfileManager.Instance.Gold += 200 * Double_ad;
        }
        if (Angle <= -100 && Angle >= -140)
        {
            ProfileManager.Instance.Gold += 400 * Double_ad;
        }

        if (Angle <= -160 && Angle >= -200)
        {
            ProfileManager.Instance.Gold += 800 * Double_ad;
        }
        if (Angle <= -220 && Angle >= -260)
        {
            if (role[0] != 0)
            {
                blob = ProfileManager.Instance.blobs[UnityEngine.Random.Range(role[0], role[i - 1] + 1)];
                blob.Unlocked = true;
            }
            else
            {
                ProfileManager.Instance.Gold += 300;
            }
        }
        if (Angle <= -280 && Angle >= -320)
        {
            if (role[0] != 0)
            {
                Enviroment = ProfileManager.Instance.enviroments[UnityEngine.Random.Range(map[0], map[i - 1] + 1)];
                Enviroment.Unlocked = true;
            }
            else
            {
                ProfileManager.Instance.Gold += 300;
            }
        }
        RewardInterface.SetActive(false);
        i = 0;
        j = 0;
    }

public void OpenLuckyDraw()
    {
        Lucky_bg.SetActive(true);
    }
    public void CloseLuckyDraw()
    {
        if (_isRotate == false)
        {
            Lucky_bg.SetActive(false);
            PlaqueAdvertising();
        }
    }

    bool IsOneDay(DateTime t1, DateTime t2)
    {
        return (t1.Year == t2.Year &&
         t1.Month == t2.Month &&
          t1.Day == t2.Day);
    }

    public void onAdShowSuccessCallBack(string p)
    {
        Debug.Log("看视频抽奖！");
        Weight_value = UnityEngine.Random.Range(1, 101);
        Double_ad = 1;
        SetAngle(Weight_value);
        _time = Time.time + ContinuousTime;
        _isRotate = true;
        // 注销广告加载成功事件
        EventDispatcher.Instance.RemoveEventListener(EventKey.AdShowSuccessCallBack, onAdShowSuccessCallBack);
    }

    public void CoinDouble_AD()
    {
        EventDispatcher.Instance.AddEventListener(EventKey.AdShowSuccessCallBack, onAdCoinDouble);
        PluginMercury.Instance.ActiveRewardVideo();
    }


    public void onAdCoinDouble(string p)
    {
        Double_ad = 2;
        GetTheRewards();
        // 注销广告加载成功事件
        EventDispatcher.Instance.RemoveEventListener(EventKey.AdShowSuccessCallBack, onAdCoinDouble);
        Debug.Log("金币奖励，广告双倍领取！");
    }


    public void FollowDouyinUser()//关注抖音号
    {
        ByteDanceSDKManager.Instance.FollowDouyin();
    }
    public void FollowDesktopUser()//添加到桌面
    {
        ByteDanceSDKManager.Instance.SaveToDesktop();
    }
    public void PlaqueAdvertising()//退出转盘抽奖时的插屏广告
    {
        ByteDanceSDKManager.Instance.ShowInterstitial();
    }
}
