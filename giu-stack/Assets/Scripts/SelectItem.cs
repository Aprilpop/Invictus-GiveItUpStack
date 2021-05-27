using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{
    public string name;

    public int index;

    public Button button;

    [SerializeField]
    Text text;

    public Image image;

    public bool unlocked;

    public virtual void Start()
    {
        button = GetComponent<Button>();
        text.text = name;
    }

    // 购买成功之后解锁
    public virtual void BuySucceed()
    {
        unlocked = true;
    }
}
