using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 环境背景
/// </summary>
public class SelectEnviroment : SelectItem
{
    Enviroment enviroment;

    PopUp popUp;

    public override void Start()
    {
        base.Start();
        button.onClick.AddListener(EnviromentSelect);

        enviroment = ProfileManager.Instance.enviroments[index];
        popUp = MenuManager.Instance.PopUp;
    }
    // 解锁
    public override void BuySucceed()
    {
        base.BuySucceed();
        enviroment.Unlocked = true;
        this.EnviromentSelect();  // 选中当前块
    }
    private void EnviromentSelect()
    {
        if (unlocked)
        {
            ProfileManager.Instance.SetEnviroment(index, GameLogic.Instance.EnviromentPoint.transform);
            SelectMenu.Instance.ChangeThemeImage();
        }
        else
        {
            SetPopUp();
        }
    }

    private void SetPopUp()
    {
        //popUp.name = enviroment.Name;
        popUp.name = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Enviroments.Name." + enviroment.Name) );
        popUp.unlockType = enviroment.unlockInfo.UnlockType;
        popUp.count = enviroment.unlockInfo.UnlockCount;
        Debug.Log("环境解锁金币赋值地点：" + popUp.count);
        popUp.videoUnlockable = enviroment.unlockInfo.VideoUnlockable;
        popUp.videoCount = enviroment.unlockInfo.VideoCount;
        popUp.image.sprite = enviroment.UnlockImage;
        popUp.selectItem = this;

        popUp.gameObject.SetActive(true);
    }
}
