using Unity.Cinemachine;
using UnityEngine;

public class Camera2_0 : MonoBehaviour
{
    public CinemachineCamera playerCam;
    public CinemachineCamera padmaCam;

    public void FocusOnPlayer()
    {
        playerCam.Priority = 20;
        padmaCam.Priority = 10;
    }
    public void FocusOnPadma()
    {
        padmaCam.Priority = 20;
        playerCam.Priority = 10;
    }
}
