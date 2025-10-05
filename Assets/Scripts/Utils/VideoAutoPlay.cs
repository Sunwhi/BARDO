using UnityEngine;
using UnityEngine.Video;

public class VideoAutoPlay : MonoBehaviour
{
    public VideoPlayer vp;

    void Start()
    {
        vp.waitForFirstFrame = true;
        vp.Prepare();
        vp.prepareCompleted += _ => vp.Play();
    }
}
