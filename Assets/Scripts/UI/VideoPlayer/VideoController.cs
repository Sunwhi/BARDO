using UnityEngine;
using UnityEngine.Video;
using System;

public class VideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoDisplayUI;
    [SerializeField] private GameObject skipBtn;

    public event Action OnVideoFinished;

    private void OnEnable()
    {
        if(videoPlayer != null)
            videoPlayer.loopPointReached += HandleVIdeoFinished;
    }
    private void OnDisable()
    {
        if(videoPlayer != null)
            videoPlayer.loopPointReached -= HandleVIdeoFinished;
    }
    private void HandleVIdeoFinished(VideoPlayer vp)
    {
        StopVideo();
        Destroy(videoDisplayUI);
        skipBtn.SetActive(false);
        OnVideoFinished?.Invoke();
    }
    public void PlayVideo()
    {
        if (videoPlayer != null)
        {
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
