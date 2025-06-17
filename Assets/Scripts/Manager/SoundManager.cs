using UnityEngine;
using UnityEngine.Audio;

public enum eBGM
{
    Stage1,
}

public enum eSFX
{
    Background_Wind,
    Opening_Door,
    Character_Walk,
    Character_Jump,
    Character_Death,
    UI_Txt_Scroll,
    UI_Button_Txt,
    UI_Btn_Open_Settings,
    UI_Button_Select_Settings,
    Stage_Transition,
    UI_Button_Hover,
}

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    void Start()
    {
        PlayBGM(eBGM.Stage1);
    }


    public void PlayBGM(eBGM bgmType)
    {
        int index = (int)bgmType;
        if (index >= 0 && index < bgmClips.Length)
        {
            bgmSource.clip = bgmClips[index];
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlaySFX(eSFX sfxType)
    {
        int index = (int)sfxType;
        if (index >= 0 && index < sfxClips.Length)
        {
            sfxSource.PlayOneShot(sfxClips[index]);
        }
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Clamp01(volume)) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp01(volume)) * 20);
    }
}
