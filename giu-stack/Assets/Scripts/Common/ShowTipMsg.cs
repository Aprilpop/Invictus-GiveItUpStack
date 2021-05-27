using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowTipMsg : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public string m_strMsg;
    private Text m_textMsg;
    void Awake()
    {
        m_textMsg = Global.FindChild(transform, "txt_msg").GetComponent<Text>();

    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }


    /// <summary>
    ///  开始播放
    /// </summary>
    void onPlayStart()
    {
        m_textMsg.text = m_strMsg;
    }

    /// <summary>
    /// 播放完一次
    /// </summary>
    void onPlayStop()
    {
        Destroy(gameObject);
    }
}
