using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    public VideoPlayer vp;

    public void OnVideoAction()
    {
        vp.waitForFirstFrame = true;
        vp.Prepare();
        vp.prepareCompleted += _ => vp.Play();
    }
}