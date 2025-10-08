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
    }
}
