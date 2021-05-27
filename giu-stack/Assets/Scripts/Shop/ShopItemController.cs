using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static ShopControllerPanel;
using Random = UnityEngine.Random;

public enum RARITY
{
    uncommon = 0,
    advanced,
    epic
}

public struct TreasureBoxInfo
{
    public eCurrencyType m_eType;
    public int m_iCout;
}

[System.Serializable]
// 商品信息结构
public class ShopInfo
{
    [HideInInspector]
    public int m_iIndex;            // 当前商品号（商品下标 1为起点）
    public eCurrencyType m_eType;             // 当前货币类型
    public int m_iGive;
    public int m_iPrice;
    public int m_iCout;           // 货币数量     
    public string m_strProductId;    // 订单号
    public eCurrencyType m_eTreasureBoxPriceType;      // 宝箱
    public RARITY m_eRarity;        // 宝箱稀有程度
    public string m_strDescribe;    // 描述    
    public string m_strProperty;    // 属性（宝箱用）
    
}

public class ShopItemController : MonoBehaviour
{
    // 他爹商城
    [HideInInspector]
    public ShopControllerPanel m_parent;


    public string m_strPath = "image/shop/";
    // Start is called before the first frame update

    public Button btnGoldBuy;
    public Button btnDiamondBuy;
    public Button btnTreasureBoxBuy;

    public GameObject textGold;
    public GameObject textDiamond;
    public GameObject textMoney;


    public ShopInfo m_shopInfo;

    // 
    private GameObject img_price;
    private Image m_imgItem;
    private Text m_txtGold;
    private Text m_txtDiamond;
    private Text m_txtMoney;

    private Text m_textNum;
    private Text m_textPrice;


    private string m_strCurCurrencyType = "";
    void Start()
    {
        img_price = Global.FindChild(transform, "img_price");
        m_imgItem = Global.FindChild(transform, "img_item").GetComponent<Image>();
        m_txtGold = Global.FindChild(transform, "txt_gold").GetComponent<Text>();
        m_txtDiamond = Global.FindChild(transform, "txt_diamond").GetComponent<Text>();
        m_txtMoney = Global.FindChild(transform, "txt_money").GetComponent<Text>();

        m_textNum = Global.FindChild(transform, "text_num").GetComponent<Text>();

        m_textPrice = img_price.transform.Find("text_price").GetComponent<Text>();

        this.checkSelect();

        this.btnGoldBuy.onClick.AddListener(this.onCallBackgBuy);
        this.btnDiamondBuy.onClick.AddListener(this.onCallBackgBuy);
        this.btnTreasureBoxBuy.onClick.AddListener(this.onCallBackgBuy);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public eCurrencyType curSelect;

    public void checkSelect()
    {
        this.img_price.SetActive(this.m_shopInfo != null && this.m_shopInfo.m_iGive > 0);

        if (this.img_price != null && this.m_shopInfo != null && this.m_shopInfo.m_iGive > 0)
        {
            this.m_textPrice.text = this.m_shopInfo.m_iGive.ToString();
        }


        this.btnGoldBuy.gameObject.SetActive(eCurrencyType.gold == this.m_shopInfo.m_eType);
        this.btnDiamondBuy.gameObject.SetActive(eCurrencyType.diamond == this.m_shopInfo.m_eType);
        this.btnTreasureBoxBuy.gameObject.SetActive(eCurrencyType.treasureBox == this.m_shopInfo.m_eType);




        this.textGold.gameObject.SetActive(false);
        this.textDiamond.gameObject.SetActive(eCurrencyType.gold == this.m_shopInfo.m_eType);
        this.textMoney.gameObject.SetActive(eCurrencyType.diamond == this.m_shopInfo.m_eType);


        // 宝箱
        if (eCurrencyType.treasureBox == this.m_shopInfo.m_eType)
        {
            this.textGold.gameObject.SetActive(this.m_shopInfo.m_eTreasureBoxPriceType == eCurrencyType.gold);
            this.textDiamond.gameObject.SetActive(eCurrencyType.diamond == this.m_shopInfo.m_eTreasureBoxPriceType);
        }


        switch (this.m_shopInfo.m_eType)
        {
            case eCurrencyType.gold:
                m_strCurCurrencyType = "钻石";
                this.selectGold();
                break;
            case eCurrencyType.diamond:
                m_strCurCurrencyType = "美元";
                this.selectDiamaond();
                break;
            case eCurrencyType.treasureBox:
                m_strCurCurrencyType = "金币";
                if (m_shopInfo.m_eTreasureBoxPriceType == eCurrencyType.diamond)
                {
                    m_strCurCurrencyType = "钻石";
                }
                this.selectTreasureBox();
                break;
            default:
                break;
        }



    }


    public void selectGold()
    {
        m_textNum.text = "X" + this.m_shopInfo.m_iCout.ToString();
        // 金币用砖石买
        m_txtDiamond.text = this.m_shopInfo.m_iPrice.ToString();
        m_imgItem.overrideSprite = Resources.Load(m_strPath + "shop_icon_coins0" + m_shopInfo.m_iIndex.ToString(), typeof(Sprite)) as Sprite;

    }

    public void selectDiamaond()
    {
        m_textNum.text = "X" + this.m_shopInfo.m_iCout.ToString();
        // 钻石用美元买   

        m_txtMoney.text = moneyToString(this.m_shopInfo.m_iPrice);
        m_imgItem.overrideSprite = Resources.Load(m_strPath + "shop_icon_gems0" + m_shopInfo.m_iIndex.ToString(), typeof(Sprite)) as Sprite;
    }

    public void selectTreasureBox()
    {
        m_textNum.text = "X" + this.m_shopInfo.m_strProperty;

        m_txtGold.text = this.m_shopInfo.m_iPrice.ToString();
        m_txtDiamond.text = this.m_shopInfo.m_iPrice.ToString();

        m_imgItem.overrideSprite = Resources.Load(m_strPath + "league_chest_level" + m_shopInfo.m_iIndex.ToString(), typeof(Sprite)) as Sprite;

    }


    public void onCallBackgBuy()
    {
        string strHeader = this.m_shopInfo.m_iPrice.ToString() + "" + m_strCurCurrencyType;
        if (this.m_shopInfo.m_eType == eCurrencyType.diamond)
        {
            strHeader = moneyToString(this.m_shopInfo.m_iPrice) + m_strCurCurrencyType; ;
        }

        // 购买确定提示框
        string strTip = "是否确定购买 ";

        // TipPopup.Instance.Open(ePopupType.general, strTip, buy, null);

        buy();

        //OnOkPress
    }

    public void buy()
    {

        switch (this.m_shopInfo.m_eType)
        {
            case eCurrencyType.gold:
                this.onGoldBuy();
                break;
            case eCurrencyType.diamond:
                m_strCurCurrencyType = "美元";
                this.onDiamaondBuy();
                break;
            case eCurrencyType.treasureBox:
                m_strCurCurrencyType = "金币";
                this.onTreasureBoxBuy();
                break;
            default:
                break;
        }

    }

    public void onGoldBuy()
    {
        // 当前钻石数量 - 需要的

        int balanceCount = ProfileManager.Instance.Diamond - this.m_shopInfo.m_iPrice;

        // // 购买货币不够
        // if (balanceCount<0)
        // {
        //     ShopInfo needRecharge = this.m_parent.getRechargeDiaGrade(balanceCount, eCurrencyType.diamond );

        //     // 钻石告急谈框

        //     return;
        // }


        // 加金币
        // ProfileManager.Instance.Gold  += this.m_shopInfo.m_iCout;

        // 减钻石
        // ProfileManager.Instance.Diamond -= this.m_shopInfo.m_iPrice;        

        // 购买成功弹窗
        DebugManager.LogInfo("-------------调用购买"+m_shopInfo.m_strProductId);
        PluginMercury.Instance.Purchase(m_shopInfo.m_strProductId);
    }



    public void onDiamaondBuy()
    {
        // 当前钻石数
        int count = ProfileManager.Instance.Gold;

        // 充钱

    }

    public void onTreasureBoxBuy()
    {
        // 当前金币数
        int count = ProfileManager.Instance.Diamond;


        // 当前货币数量 - 需要的
        int balanceCount = ProfileManager.Instance.Gold - this.m_shopInfo.m_iPrice;


        // 钻石类型
        if (m_shopInfo.m_eTreasureBoxPriceType == eCurrencyType.diamond)
        {
            balanceCount = ProfileManager.Instance.Diamond - this.m_shopInfo.m_iPrice;
        }

        // 购买货币不够
        if (balanceCount < 0)
        {
            switch (m_shopInfo.m_eTreasureBoxPriceType)
            {
                case eCurrencyType.gold:
                // 金币不够

                //break;
                case eCurrencyType.diamond:
                    // 钻石不够
                    ShopInfo needRecharge = this.m_parent.getRechargeDiaGrade(balanceCount, eCurrencyType.diamond);
                    // 购买成功                


                    break;
                default:
                    break;
            }

            return;
        }



        TreasureBoxInfo[] arrTreasureBoxInfo;
        switch (this.m_shopInfo.m_eRarity)
        {
            case RARITY.uncommon:
                arrTreasureBoxInfo = GenerateTreasureBox(50, 150, 1, 5);
                break;
            case RARITY.advanced:
                arrTreasureBoxInfo = GenerateTreasureBox(200, 500, 10, 30);
                break;
            case RARITY.epic:
                arrTreasureBoxInfo = GenerateTreasureBox(400, 1000, 20, 60);
                break;
            default:
                return;
        }

        // 往账户上添加货币

        // 加开宝箱获得的货币
        foreach (var item in arrTreasureBoxInfo)
        {
            switch (item.m_eType)
            {
                case eCurrencyType.gold:
                    ProfileManager.Instance.Gold += item.m_iCout;
                    break;
                case eCurrencyType.diamond:
                    ProfileManager.Instance.Diamond += item.m_iCout;
                    break;
                default:
                    break;
            }

        }

        // 减开宝箱消耗的货币
        switch (m_shopInfo.m_eTreasureBoxPriceType)
        {
            case eCurrencyType.gold:
                ProfileManager.Instance.Gold -= this.m_shopInfo.m_iPrice;
                break;
            case eCurrencyType.diamond:
                ProfileManager.Instance.Diamond -= this.m_shopInfo.m_iPrice;
                break;
            default:
                break;
        }

        // 打开宝箱        
    }

    TreasureBoxInfo[] GenerateTreasureBox(int minGold, int maxGold, int minDia, int maxDia)
    {

        TreasureBoxInfo[] arrTemp = new TreasureBoxInfo[3];

        for (int i = 0; i < arrTemp.Length; i++)
        {
            arrTemp[i] = this.RandomTreasureBox(minGold, maxGold, minDia, maxDia);
        }

        return arrTemp;
    }

    TreasureBoxInfo RandomTreasureBox(int minGold, int maxGold, int minDia, int maxDia)
    {
        TreasureBoxInfo tempInfo = new TreasureBoxInfo();

        int type = Random.Range(0, 2);           // 生成金币还是钻石
        tempInfo.m_eType = (eCurrencyType)type;

        if ((eCurrencyType)type == eCurrencyType.gold)
        {
            tempInfo.m_iCout = Random.Range(minGold, maxGold);
        }
        else
        {
            tempInfo.m_iCout = Random.Range(minDia, maxDia);
        }

        return tempInfo;
    }


    // 将美元转为字符串显示
    public static string moneyToString(int iMoney)
    {
        string strMoney;
        strMoney = (iMoney / 100).ToString();
        strMoney += "." + (iMoney % 100).ToString();
        return strMoney;
    }
}
