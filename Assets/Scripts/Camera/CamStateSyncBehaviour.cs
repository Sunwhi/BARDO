using UnityEngine;

// Animator 스테이트 진입 시 해시로 동기화
public class CamStateSyncBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CameraManager.Instance?.SyncStateByHash(stateInfo.shortNameHash);
    }
}