using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGamePopup : MonoBehaviour
{

    GameObject m_goContent;
    GameObject m_goTextTime;
    Text m_textTime;
    Button m_btnMainMenu;
    Button m_btngoOnGame;

    // 3s
    public float m_fDelayTime = 3.0f;

    private bool m_bGoOnGame = false;

    private float m_realtimeSinceStartup;
    private void Awake()
    {

        m_goTextTime = Global.FindChild(this.transform, "text_time").gameObject;
        m_goTextTime.SetActive(false);  // 先隐藏
        m_textTime = m_goTextTime.GetComponent<Text>();

        m_goContent = Global.FindChild(this.transform, "Panel").gameObject;

        m_btnMainMenu = Global.FindChild(this.m_goContent.transform, "btn_MainMenu").GetComponent<Button>();
        m_btngoOnGame = Global.FindChild(this.m_goContent.transform, "btn_goOnGame").GetComponent<Button>();

        m_btnMainMenu.onClick.AddListener(this.onCallBackMainMenu);
        m_btngoOnGame.onClick.AddListener(this.onCallBackGoOnGame);

        // 点击广告
        Global.FindChild(transform, "btn_close").GetComponent<Button>().onClick.AddListener(this.onCallClose);
    }

    private void OnEnable()
    {
        m_bGoOnGame = false;
        m_goContent.SetActive(true);
        m_goTextTime.SetActive(false);
        // m_realtimeSinceStartup = Time.realtimeSinceStartup;
    }

    // 返回主菜单
    void onCallBackMainMenu()
    {
        MenuManager.Instance.BackMenu();
    }

    // 继续游戏
    void onCallBackGoOnGame()
    {
        // 隐藏
        m_goContent.SetActive(false);

        // 倒计时
        m_goTextTime.SetActive(true);

        m_bGoOnGame = true;

        m_realtimeSinceStartup = Time.realtimeSinceStartup;
        //m_bIsGamePaused
    }


    private void Update()
    {
        m_textTime.text = (m_fDelayTime - (int)(Time.realtimeSinceStartup - m_realtimeSinceStartup)).ToString();
        // 3s时间到
        if (m_bGoOnGame && (Time.realtimeSinceStartup - m_realtimeSinceStartup >= m_fDelayTime))
        {
            m_goTextTime.SetActive(false);

            // 继续游戏                                                            
            MenuManager.Instance.PauseGame(false);
        }
    }

    /// <summary>
    /// 关闭
    /// </summary>
    private void onCallClose()
    {
        onCallBackGoOnGame();                
    }

}
