using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;
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

    //코루틴 제어
    private Coroutine bgmFadeCoroutine;
    private float originSourceVolume = 1.0f;


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
        originSourceVolume = bgmSource.volume;
    }

    public void PlayBGM(EBGM bgmType, float fadeDuration = 1.0f)
    {
        // 현재 재생되고 있는 BGM을 또 재생하려 하면 return / 중복 재생 막기
        if(currentBGM == bgmType && bgmSource.isPlaying)
        {
            Debug.Log("bgm 중복 재생");
            return;
        }


        currentBGM = bgmType;
        int index = (int)bgmType;
        if (index < 0 || index >= bgmClips.Length) return;

        // 기존에 돌고 있던 페이드 작업 취소
        if (bgmFadeCoroutine != null) StopCoroutine(bgmFadeCoroutine);

        bgmFadeCoroutine = StartCoroutine(CoPlayBGM(bgmClips[index], fadeDuration));
        /*if (index >= 0 && index < bgmClips.Length)
        {
            if (bgmSource.clip != null) StopBGM();

            bgmSource.clip = bgmClips[index];
            bgmSource.Play();
        }*/
    }

    private IEnumerator CoPlayBGM(AudioClip nextClip, float duration)
    {
        if(bgmSource.isPlaying)
        {
            float startVolume = bgmSource.volume;

            float fadeOutTime = duration * 0.5f;
            float timer = 0f;

            while ((timer < fadeOutTime))
            {
                timer += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeOutTime);
                yield return null;
            }
            bgmSource.volume = 0f;
            bgmSource.Stop();
        }
        
        bgmSource.clip = nextClip;
        bgmSource.Play();

        float fadeInTime = duration * 0.5f;
        float timerIn = 0f;

        while (timerIn < fadeInTime)
        {
            timerIn += Time.deltaTime;

            bgmSource.volume = Mathf.Lerp(0, originSourceVolume, fadeInTime / timerIn);
            yield return null;
        }

        bgmSource.volume = originSourceVolume; // 볼륨 확실하게 고정
    }
    public void PlaySFX(ESFX sfxType)
    {
        int index = (int)sfxType;
        if (index >= 0 && index < sfxClips.Length && sfxClips[index].clip != null)
        {
            sfxSource.PlayOneShot(sfxClips[index].clip, sfxClips[index].volume);
        }
    }

    public void StopBGM(float fadeDuration = 1.0f)
    {
        // 이미 꺼져있으면 패스  
        if (!bgmSource.isPlaying) return;

        if (bgmFadeCoroutine != null) StopCoroutine(bgmFadeCoroutine);
        bgmFadeCoroutine = StartCoroutine(CoStopBGM(fadeDuration));
    }

    private IEnumerator CoStopBGM(float duration)
    {
        float startVolume = bgmSource.volume;
        float timer = 0f;

        // 서서히 줄이기
        while (timer < duration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        bgmSource.volume = 0f;
        bgmSource.Stop(); // 완전히 줄어들면 정지
        bgmSource.loop = false;

        // (중요) 다음에 틀 때를 대비해 볼륨 복구는 하지 않고, 
        // PlayBGM에서 0부터 키우도록 로직이 되어 있으므로 괜찮습니다.
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
