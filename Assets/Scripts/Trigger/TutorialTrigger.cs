using UnityEngine;

public class TutorialTrigger : TriggerBase
{
    [SerializeField] private string tutorialID;

    protected override void OnTriggered()
    {
        TriggerManager.Instance.HandleTrigger(tutorialID, TriggerType.Tutorial);
    }
}