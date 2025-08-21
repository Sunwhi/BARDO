using UnityEngine;

public class NextStageEvent : IGameEvent
{
    public int stageId;
    public Vector3 playerTransform;

    public NextStageEvent(int id, Vector3 transform = default)
    {
        stageId = id;
        playerTransform = transform;

        SaveManager.Instance.SetSaveData(nameof(SaveData.stageIdx), stageId);
        SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), 0);
        UIManager.Instance.ShowPanelWithParam<RoundTransition>(stageId);
    }
}
