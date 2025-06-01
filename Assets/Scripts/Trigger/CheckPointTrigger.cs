using UnityEngine;

public class CheckpointTrigger : TriggerBase
{
    [SerializeField] private string checkpointID;

    protected override void OnTriggered()
    {
        Vector3 pos = transform.position;
        TriggerManager.Instance.RegisterCheckpoint(checkpointID, pos);
    }
}