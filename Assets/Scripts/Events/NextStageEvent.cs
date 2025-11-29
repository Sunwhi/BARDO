using UnityEngine;

public class NextStageEvent : IGameEvent
{
    public int stageId;
    public Vector3 playerTransform;

    public NextStageEvent(int id, Vector3 transform = default)
    {
        stageId = id;
        if (transform != default) playerTransform = transform;
        UIManager.Show<RoundTransition>(stageId);
        SoundManager.Instance.PlaySFX(ESFX.Stage_Transition);

        switch (stageId)
        {
            case 3:
                CameraManager.Instance.JumpAndCut(CamState.v3_1);
                break;
            case 4:
                CameraManager.Instance.JumpAndCut(CamState.v4_0);
                break;
        }

        Vector3 pos = playerTransform;
        Player p = StoryManager.Instance.Player;
        p.controller.ResetInput();
        p.transform.position = pos;
    }
}
