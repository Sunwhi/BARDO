using UnityEngine;
using UnityEngine.Audio;
using System;
public enum EBGM
{
    Title,
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage4Maze,
}

public enum ESFX
{
    Background_Wind,
    Opening_Door,
    Character_Walk,
    Character_Jump,
    Character_land,
    Character_Death,
    UI_Txt_Scroll,
    UI_Button_Txt,
    UI_Btn_Open_Settings,
    UI_Button_Select_Settings,
    Stage_Transition,
    UI_Button_Hover,
    UI_Mouse_Click,
    Item_Get, 
    Card,
    Karmic_Shard,
    Memory_Lamp,
    Soul_Thread
}
[Serializable]
public class SFXData
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
}
public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource ambientSource;

    [Header("Audio Clips")]
    public AudioClip[] bgmClips;
    public SFXData[] sfxClips;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    private EBGM currentBGM;
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

        PlayBGM(EBGM.Title);

        sfxSource.ignoreListenerPause = true;
    }

    public void PlayBGM(EBGM bgmType)
    {
        int index = (int)bgmType;
        currentBGM = bgmType;
        if (index >= 0 && index < bgmClips.Length)
        {
            if (bgmSource.clip != null) StopBGM();

            bgmSource.clip = bgmClips[index];
            bgmSource.Play();
        }
    }

    public void PlaySFX(ESFX sfxType)
    {
        int index = (int)sfxType;
        if (index >= 0 && index < sfxClips.Length && sfxClips[index].clip != null)
        {
            sfxSource.PlayOneShot(sfxClips[index].clip, sfxClips[index].volume);
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
        //bgmSource.loop = false;
    }
    public void StopSFX()
    {
        sfxSource.Stop();
        sfxSource.loop = false;
    }

    // 현재 재생되고 있는 bgm을 가져온다. 끊겼을 때 다시 이어서 틀기 위해 사용
    public EBGM GetCurrentBGM()
    {
        return currentBGM;
    }
    public void PlayAmbientSound(ESFX sfxType)
    {
        int index = (int)sfxType;
        if (index >= 0 && index < sfxClips.Length && sfxClips[index].clip != null)
        {
            ambientSource.clip = sfxClips[index].clip;
            ambientSource.volume = sfxClips[index].volume;
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
