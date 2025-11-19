using UnityEngine;

public class CheckpointTrigger : TriggerBase
{
    [SerializeField] private string checkpointID;
    [SerializeField] private int stageIdx;
    [SerializeField] private int storyIdx;

    protected override void OnTriggered()
    {
        Vector3 pos = transform.position;

        SaveManager.Instance.SetSaveData(nameof(SaveData.stageIdx), stageIdx);
        SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), storyIdx);

        TriggerManager.Instance.RegisterCheckpoint(checkpointID, pos);
        TriggerManager.Instance.HandleTrigger(checkpointID, TriggerType.Checkpoint);


        ContinueManager.Instance.loadedByContinue = false;
    }
}