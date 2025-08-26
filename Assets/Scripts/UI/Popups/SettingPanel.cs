using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : UIBase
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    public void SetBgmVolume()
    {
        float bgmVolume = bgmSlider.value;
        SoundManager.Instance.SetBGMVolume(bgmVolume);
    }
    public void SetSFXVolume()
    {
        float sfxVolume = sfxSlider.value;
        SoundManager.Instance.SetSFXVolume(sfxVolume);
    }

    public override void OnUICloseBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Select_Settings);
        base.OnUICloseBtn();
    }
}
