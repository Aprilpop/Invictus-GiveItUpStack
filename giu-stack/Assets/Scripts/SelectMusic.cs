using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 选择音乐
/// </summary>
public class SelectMusic : SelectItem
{

    Music music;

    PopUp popUp;

    public override void Start()
    {
        base.Start();
        button.onClick.AddListener(MusicSelect);

        music = ProfileManager.Instance.Musics[index];
        popUp = MenuManager.Instance.PopUp;
    }
    // 解锁
    public override void BuySucceed()
    {
        base.BuySucceed();
        music.Unlocked = true;
        this.MusicSelect();  // 选中当前块
    }

    private void MusicSelect()
    {
        if (unlocked)
        {
            ProfileManager.Instance.EMusic = music.eMusic;
            MusicManager.Instance.PlayMusic(music.eMusic);
            SelectMenu.Instance.ChangeMusicImage();
        }
        else
        {
            SetPopUp();
        }
    }

    public void SetPopUp()
    {

        // popUp.name = music.Name;
        popUp.name = String.Format(SmartLocalization.LanguageManager.Instance.GetTextValue("Music.Name." + music.Name) );
        popUp.unlockType = music.unlockInfo.UnlockType;
        popUp.count = music.unlockInfo.UnlockCount;
        Debug.Log("音乐解锁金币赋值地点：" + popUp.count);
        popUp.videoUnlockable = music.unlockInfo.VideoUnlockable;
        popUp.videoCount = music.unlockInfo.VideoCount;
        popUp.image.sprite = music.UnlockImage;
        popUp.selectItem = this;


        popUp.gameObject.SetActive(true);
    }

}
