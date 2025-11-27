using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : UIBase
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    private GameObject bgBlurImg;
    private void Awake()
    {
        base.Awake();
        opened += OnPanelOpened;
        closed += OnPanelClosed;
    }
    private void OnEnable()
    {
        bgmSlider.value = SoundManager.Instance.GetBGMVolume();
        sfxSlider.value = SoundManager.Instance.GetSFXVolume();
    }

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
        bgBlurImg.SetActive(false);

        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Select_Settings);
        base.OnUICloseBtn();
    }

    private void OnPanelOpened(object[] parameters)
    {
        if(parameters != null && parameters.Length > 0)
        {
            if (parameters[0] is GameObject bgBlur)
            {
                bgBlurImg = bgBlur;
            }
        }
    }
    private void OnPanelClosed(object[] parameters)
    {
        if(bgBlurImg != null)
            bgBlurImg.SetActive(false);
    }
}