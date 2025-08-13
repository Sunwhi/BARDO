using UnityEngine;

public class CheckpointTrigger : TriggerBase
{
    [SerializeField] private string checkpointID;
    [SerializeField] private int stageId = -1;
    [SerializeField] private int storyId = -1;

    protected override void OnTriggered()
    {
        Vector3 pos = transform.position;

        if (stageId != -1)
            SaveManager.Instance.SetSaveData(nameof(SaveData.stageIdx), stageId);
        if (storyId != -1)
            SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), storyId);

        TriggerManager.Instance.RegisterCheckpoint(checkpointID, pos);
        TriggerManager.Instance.HandleTrigger(checkpointID, TriggerType.Checkpoint);
    }
}