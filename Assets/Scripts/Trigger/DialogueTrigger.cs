using UnityEngine;

public class DialogueTrigger : TriggerBase
{
    [SerializeField] private string dialogueID;

    protected override void OnTriggered()
    {
        TriggerManager.Instance.HandleTrigger(dialogueID, TriggerType.Dialogue);
    }
}