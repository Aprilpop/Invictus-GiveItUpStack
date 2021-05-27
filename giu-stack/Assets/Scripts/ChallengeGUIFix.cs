using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeGUIFix : MonoBehaviour
{

   const int topOffset = 80;

    GridLayoutGroup gridLayout;

    private bool isMobile()
    {
        int width = Screen.width;
        int height = Screen.height;
        float aspect = (height > width) ? (float)height / width : (float)width / height;

        return (aspect > 1.4);

    }

    private void Awake()
    {
        if (isMobile())
        {
            gridLayout = GetComponent<GridLayoutGroup>();
            gridLayout.padding.top = topOffset;
        }
    }

}
