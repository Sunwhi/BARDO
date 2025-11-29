using UnityEngine;
using UnityEngine.Video;
using System;
using System.Collections.Generic;

public enum VideoType
{
    Intro,
    Stage3,
    Ending
}
public class VideoController : MonoBehaviour
{
    [Serializable]
    public struct VideoMapping
    {
        public VideoType Type;
        public VideoClip clip;
    }

    [Header("Components")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoDisplayUI;
    [SerializeField] private GameObject skipBtn;
    [SerializeField] private GameObject blackPanel;

    [Header("Video Library")]
    [SerializeField] private List<VideoMapping> videoList;

    public event Action<VideoType> OnVideoFinished;
    private VideoType curVideoType;
    private EBGM stoppedBgm; // 비디오 재생으로 멈춰진 현재 bgm 저장

    private void OnEnable()
    {
        if(videoPlayer != null)
            videoPlayer.loopPointReached += HandleVIdeoFinished;

        blackPanel.SetActive(false); // continue시에 나타나지 않게
    }
    private void OnDisable()
    {
        if(videoPlayer != null)
            videoPlayer.loopPointReached -= HandleVIdeoFinished;
    }
    private void HandleVIdeoFinished(VideoPlayer vp)
    {
        StopVideo();

        if(stoppedBgm != EBGM.Title) // 타이틀만 제외하고 이어서 재생
        {
            SoundManager.Instance.PlayBGM(stoppedBgm);
        }

        videoDisplayUI.SetActive(false);
        skipBtn.SetActive(false);
        if (blackPanel.activeSelf)
        {
            blackPanel.SetActive(false);
        }
  

        OnVideoFinished?.Invoke(curVideoType);
    }
    public void PlayVideo(VideoType type)
    {
        curVideoType = type;
        blackPanel.SetActive(true); //video 시작하면 blackPanel 뒤로 깐다.
        SoundManager.Instance.StopBGM();
        stoppedBgm = SoundManager.Instance.GetCurrentBGM();

        VideoMapping target = videoList.Find(x => x.Type == type);

        if(target.clip != null && videoPlayer != null)
        {
            videoPlayer.clip = target.clip;
            videoDisplayUI.SetActive(true);
            skipBtn?.SetActive(true);

            videoPlayer.Play();
        }
    }
    public void StopVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }
    }
    public void OnClickedVideoSkip()
    {
        HandleVIdeoFinished(videoPlayer);
    }
}
