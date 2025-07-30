using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CinemachineBrain brain;
    private CinemachineCamera currentVCam;

    public void SetCamera(CinemachineCamera newVCam)
    {
        if (newVCam == null || newVCam == currentVCam)
            return;

        // 우선순위 변경: 새 카메라를 우선으로
        if (currentVCam != null)
            currentVCam.Priority = 0;

        newVCam.Priority = 10;
        currentVCam = newVCam;
    }
}
