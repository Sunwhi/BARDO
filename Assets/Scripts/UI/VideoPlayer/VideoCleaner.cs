using UnityEngine;
using UnityEngine.Video;

public class VideoCleaner : MonoBehaviour
{
    [SerializeField] private VideoPlayer vp;

    private void OnDisable()
    {
        if(vp != null && vp.targetTexture != null)
        {
            vp.targetTexture.Release();
        }
    }
}
