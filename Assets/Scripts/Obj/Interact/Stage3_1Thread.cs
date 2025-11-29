using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class Stage3_1Thread : MonoBehaviour
{
    [Header("videos")]
    [SerializeField] VideoPlayer startThread_short;
    [SerializeField] VideoPlayer startThread_long;
    [SerializeField] VideoPlayer loop1_Thread;
    [SerializeField] VideoPlayer loop2_Thread;
    [SerializeField] VideoPlayer last_Thread;

    [Header("item off")]
    float disableAtSeconds = 0.7f;
    [SerializeField] GameObject[] itemsToDisable;

    private void Awake()
    {
        startThread_short.gameObject.SetActive(false);
        startThread_long.gameObject.SetActive(false);
        last_Thread.gameObject.SetActive(false);

        loop1_Thread.gameObject.SetActive(SaveManager.Instance.MySaveData.threadEnabled);
        loop2_Thread.gameObject.SetActive(SaveManager.Instance.MySaveData.threadEnabled);
    }

    public void PlayThreadVideo()
    {
        startThread_short.gameObject.SetActive(true);
        startThread_short.Play();
        startThread_short.loopPointReached += _ => { 
            startThread_short.gameObject.SetActive(false);
            PlayLongVideo();
        };


        startThread_long.loopPointReached += _ => PlayLoopVideo();

        StartCoroutine(DisableItems());

        SaveManager.Instance.SetSaveData(nameof(SaveData.threadEnabled), true);
        SaveManager.Instance.SaveSlot();
    }
    private void PlayLongVideo()
    {
        startThread_long.gameObject.SetActive(true);
        startThread_long.Play();

        // ★ 중요 1: Long 영상이 재생되는 동안, 다음에 나올 Loop 영상들을 미리 로딩(Prepare) 합니다.
        // 미리 준비해두지 않으면 Play() 할 때 버벅거립니다.
        PrepareLoopVideos();

        startThread_long.loopPointReached += _ => PlayLoopVideo();
    }

    private void PrepareLoopVideos()
    {
        loop1_Thread.Prepare();
        loop2_Thread.Prepare();
    }

    private void PlayLoopVideo()
    {
        StartCoroutine(PlayRound3BGM());
        loop1_Thread.gameObject.SetActive(true);
        loop2_Thread.gameObject.SetActive(true);

        loop1_Thread.Play();
        loop2_Thread.Play();

        // ★ 중요 2: 여기서 바로 startThread_long.SetActive(false)를 하지 않습니다!
        // Loop 영상이 '진짜로' 재생 시작된 순간(started 이벤트)에 이전 영상을 끕니다.
        // 이렇게 해야 중간에 검은 화면(Gap)이 생기지 않습니다.
        //startThread_long.gameObject.SetActive(false);
        loop1_Thread.started += OnLoopVideoStarted;
    }

    private void OnLoopVideoStarted(VideoPlayer source)
    {
        startThread_long.gameObject.SetActive(false);

        loop1_Thread.started -= OnLoopVideoStarted;
    }
    private IEnumerator DisableItems()
    {
        yield return new WaitForSeconds(disableAtSeconds);
        for (int i = 0; i < itemsToDisable.Length; i++)
            if (itemsToDisable[i]) itemsToDisable[i].SetActive(false);
    }

    public void LastThreadVideo()
    {
        last_Thread.gameObject.SetActive(true);
        last_Thread.Play();
        last_Thread.loopPointReached += _ => StopThreadVideo();

        SaveManager.Instance.SetSaveData(nameof(SaveData.threadEnabled), false);
        SaveManager.Instance.SaveSlot();
    }

    private void StopThreadVideo()
    {
        loop1_Thread.gameObject.SetActive(false);
        loop2_Thread.gameObject.SetActive(false);
        last_Thread.gameObject.SetActive(false);
    }

    IEnumerator PlayRound3BGM()
    {
        yield return new WaitForSeconds(1f);

        SoundManager.Instance.PlayBGM(EBGM.Stage3);
    }
}