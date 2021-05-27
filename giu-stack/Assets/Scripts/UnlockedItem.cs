using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnlockedItem : MonoBehaviour
{

    public Text name;
    public Image image;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (ProfileManager.Instance.unlockedItems.Count > 0)
        {
            name.text = ProfileManager.Instance.unlockedItems[0].Name;
            image.sprite = ProfileManager.Instance.unlockedItems[0].UnlockImage;
            GameAnalyticsManager.LogDesignEvent("Unlocked" + ":" + ProfileManager.Instance.unlockedItems[0].Name + ":InGame");
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void NextItem()
    {
        ProfileManager.Instance.unlockedItems.RemoveAt(0);
        Init();
    }
}
