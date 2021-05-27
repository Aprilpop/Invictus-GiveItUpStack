using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPhoneXFix : MonoBehaviour
{

    const float topOffset = 70;

    public static bool IsiPhoneX
    {
        get
        {
            int width = Screen.width;
            int height = Screen.height;
            float aspect = (height > width) ? (float)height / width : (float)width / height;
            return (aspect > 2);
        }
    }

    private void Awake()
    {
        if (IsiPhoneX)
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.offsetMin = new Vector2(rect.offsetMin.x, 200);
            if (transform.name == "MainMenu")
                rect.offsetMax = new Vector2(rect.offsetMax.x, -100);

        }
      
    }

}
