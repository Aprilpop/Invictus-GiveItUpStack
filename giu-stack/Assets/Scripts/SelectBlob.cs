using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 选择角色
/// </summary>
public class SelectBlob : SelectItem
{

    Blob blob;

    PopUp popUp;

    public override void Start()
    {
        
        base.Start();
        button.onClick.AddListener(BlobSelect);

        blob = ProfileManager.Instance.blobs[index];
        popUp = MenuManager.Instance.PopUp;
    }

    // 解锁
    public override void BuySucceed()
    {
        base.BuySucceed();
        blob.Unlocked = true;
        this.BlobSelect();  // 选中当前块
    }

    public void BlobSelect()
    {
        if (unlocked)
        {
            ProfileManager.Instance.SetBlob(index, GameLogic.Instance.CharacterStartPoint.transform);
            SelectMenu.Instance.ChangeImage();
        }
        else
        {
            SetPopUp();
        }
    }

    private void SetPopUp()
    {
        // popUp.name = blob.Name;
        popUp.name = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Character.Name." + blob.Name) );
        popUp.unlockType = blob.unlockInfo.UnlockType;
        popUp.count = blob.unlockInfo.UnlockCount;
        Debug.Log("角色解锁金币赋值地点：" + popUp.count);
        popUp.videoUnlockable = blob.unlockInfo.VideoUnlockable;
        popUp.videoCount = blob.unlockInfo.VideoCount;
        popUp.image.sprite = blob.Image;
        popUp.selectItem = this;

        popUp.gameObject.SetActive(true);
    }
}
