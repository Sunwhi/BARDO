using UnityEngine;
using UnityEngine.Audio;

public enum EBGM
{
    Title,
    Stage1,
    Stage2,
    Stage3,
}

public enum ESFX
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
    UI_Mouse_Click,
    Item_Get, 
}

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource ambientSource;

    [Header("Audio Clips")]
    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    private const string bgmVolumeParam = "BGM";
    private const string sfxVolumeParam = "SFX";

    public float GetBGMVolume() => PlayerPrefs.GetFloat(bgmVolumeParam);
    public float GetSFXVolume() => PlayerPrefs.GetFloat(sfxVolumeParam);

    public override void Awake()
    {
        base.Awake();

        if (PlayerPrefs.HasKey(bgmVolumeParam))
        {
            float bgmVolume = GetBGMVolume();
            SetBGMVolume(bgmVolume);
        }
        else
        {
            SetBGMVolume(0.5f); // 기본값 설정
        }

        if (PlayerPrefs.HasKey(sfxVolumeParam))
        {
            float sfxVolume = GetSFXVolume();
            SetSFXVolume(sfxVolume);
        }
        else
        {
            SetSFXVolume(0.5f); // 기본값 설정
        }
    }

    public void PlayBGM(EBGM bgmType)
    {
        int index = (int)bgmType;
        if (index >= 0 && index < bgmClips.Length)
        {
            bgmSource.clip = bgmClips[index];
            bgmSource.Play();
        }
    }

    public void PlaySFX(ESFX sfxType)
    {
        Debug.Log(sfxType);
        int index = (int)sfxType;
        Debug.Log("index : " + index);
        Debug.Log("sfxClips.Length : " + sfxClips.Length);
        if (index >= 0 && index < sfxClips.Length)
        {
            if(index == 9)  Debug.Log("inside if moon");
            sfxSource.PlayOneShot(sfxClips[index]);
        }
    }

    public void StopSFX()
    {
        sfxSource.Stop();
        sfxSource.loop = false;
    }

    public void PlayAmbientSound(ESFX sfxType)
    {
        int index = (int)sfxType;
        if (index >= 0 && index < sfxClips.Length)
        {
            ambientSource.clip = sfxClips[index];
            ambientSource.Play();
        }
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Clamp01(volume)) * 20);
        PlayerPrefs.SetFloat(bgmVolumeParam, volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp01(volume)) * 20);
        PlayerPrefs.SetFloat(sfxVolumeParam, volume);
    }
}
