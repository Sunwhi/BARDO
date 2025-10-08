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
    [SerializeField] VideoPlayer loop3_Thread;

    [Header("item off")]
    float disableAtSeconds = 0.7f;
    [SerializeField] GameObject[] itemsToDisable;

    private void Awake()
    {
        startThread_short.gameObject.SetActive(false);
        startThread_long.gameObject.SetActive(false);

        loop1_Thread.gameObject.SetActive(SaveManager.Instance.MySaveData.threadEnabled);
        loop2_Thread.gameObject.SetActive(SaveManager.Instance.MySaveData.threadEnabled);
        loop3_Thread.gameObject.SetActive(SaveManager.Instance.MySaveData.threadEnabled);
    }

    public void PlayThreadVideo()
    {
        startThread_short.gameObject.SetActive(true);
        startThread_short.Play();
        startThread_short.loopPointReached += _ => startThread_short.gameObject.SetActive(false);

        startThread_long.gameObject.SetActive(true);
        startThread_long.Play();
        startThread_long.loopPointReached += _ => PlayLoopVideo();

        StartCoroutine(DisableItems());

        SaveManager.Instance.SetSaveData(nameof(SaveData.threadEnabled), true);
        SaveManager.Instance.SaveSlot();
    }

    private void PlayLoopVideo()
    {
        loop1_Thread.gameObject.SetActive(true);
        loop2_Thread.gameObject.SetActive(true);
        loop3_Thread.gameObject.SetActive(true);

        loop1_Thread.Play();
        loop2_Thread.Play();
        loop3_Thread.Play();

        startThread_long.gameObject.SetActive(false);
    }

    private IEnumerator DisableItems()
    {
        yield return new WaitForSeconds(disableAtSeconds);
        for (int i = 0; i < itemsToDisable.Length; i++)
            if (itemsToDisable[i]) itemsToDisable[i].SetActive(false);
    }

    public void StopThreadVideo()
    {
        loop1_Thread.gameObject.SetActive(false);
        loop2_Thread.gameObject.SetActive(false);
        loop3_Thread.gameObject.SetActive(false);

        SaveManager.Instance.SetSaveData(nameof(SaveData.threadEnabled), false);
        SaveManager.Instance.SaveSlot();
    }
}