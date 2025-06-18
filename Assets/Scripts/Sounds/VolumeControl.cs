using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {

    }
    private void Update()
    {
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
}
