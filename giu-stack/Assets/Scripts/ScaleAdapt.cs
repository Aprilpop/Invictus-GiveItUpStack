using UnityEngine;
using System.Collections;
/// <summary>
/// UI适配
/// </summary>
public class ScaleAdapt : MonoBehaviour
{

    [SerializeField]
    public enum HorizontalAdaptiveMode
    {
        NONE,
        LEFT,
        RIGHT
    }
    [SerializeField]
    public enum VerticalAdaptiveMode
    {
        NONE,
        UP,
        DOWN

    }
    public HorizontalAdaptiveMode horMode = HorizontalAdaptiveMode.NONE;
    public VerticalAdaptiveMode verMode = VerticalAdaptiveMode.NONE;
    Vector2 startVector;
    Vector2 lastVector;
    public Vector2 nowHW;

    //UIPanel panel;

    void Awake()
    {
        // panel = GameObject.FindGameObjectWithTag("GuiCamera").transform.parent.GetComponent<UIPanel>();
    }
    void Start()
    {

        startVector = transform.localScale;
        Adaptive();
    }
#if UNITY_EDITOR
    void Update()
    {
        Adaptive();
    }
#endif
    Vector3 finScale;
    void Adaptive()
    {

        nowHW = UIAdapt.Instance.GetNowHW();
        float ratio = UIAdapt.Instance.GetScale();

        if (lastVector == nowHW) return;

        lastVector = nowHW;
        finScale.x = startVector.x * ratio;
        finScale.y = startVector.y * ratio;
        finScale.z = finScale.x;
        transform.localScale = finScale;

        Vector3 offset = Vector3.zero;

        int dir = 0;

        if (horMode != HorizontalAdaptiveMode.NONE)
        {

            dir = horMode == HorizontalAdaptiveMode.LEFT ? -1 : 1;
            // offset += dir * Vector3.right * (panel.width - nowHW.x) / 2;
            offset += dir * Vector3.right * (Screen.width - nowHW.x) / 2;

        }

        if (verMode != VerticalAdaptiveMode.NONE)
        {

            dir = verMode == VerticalAdaptiveMode.DOWN ? -1 : 1;
            // offset += dir * Vector3.up * (panel.height - nowHW.y) / 2;
            offset += dir * Vector3.up * (Screen.height - nowHW.y) / 2;

        }
        // transform.localPosition = offset;
    }
}