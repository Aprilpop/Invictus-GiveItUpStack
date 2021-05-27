using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    //private bool isJump;
    //public bool IsJump { get { return isJump; } set { isJump = value; } }
    public bool IsJump { get; set; }

    // 点击点是否在UI上
    private bool m_bIsInUI = false;

    private static InputManager instance;
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
                Instantiate(Resources.Load("InputManager"));
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        m_bIsInUI = false;
        Vector2 v2 = Vector2.zero;
        //移动端
        if (Application.platform == RuntimePlatform.Android ||
                    Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                v2 = Input.GetTouch(0).position;
                DebugManager.LogInfo("点击到UI => mobile");
            }
        }
        //其它平台
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                v2 = Input.mousePosition;
                DebugManager.LogInfo("点击到UI => other");
            }
        }
        if (IsPointerOverGameObject(v2))
        {
            //Debug.Log("onClick UI Time ========>>" + Time.realtimeSinceStartup * 1000);
            m_bIsInUI = true;
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) && GameLogic.Instance.InGame && !m_bIsInUI)
            IsJump = true;
#elif UNITY_ANDROID
        if(Input.GetMouseButtonDown(0) && !IsJump && GameLogic.Instance.InGame && !m_bIsInUI)
            IsJump = true;
#elif UNITY_IOS
        if(Input.GetMouseButtonDown(0) && !IsJump && GameLogic.Instance.InGame && !m_bIsInUI)
            IsJump = true;
#elif UNITY_WEBGL
        if(Input.GetMouseButtonDown(0) && !IsJump && GameLogic.Instance.InGame && !m_bIsInUI)
            IsJump = true;
#endif
    }

    /// <summary>
    /// 检测是否点击UI
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <returns></returns>
    private bool IsPointerOverGameObject(Vector2 mousePosition)
    {
        //创建一个点击事件
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        //向点击位置发射一条射线，检测是否点击UI
        EventSystem.current.RaycastAll(eventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
