using UnityEngine;

public class RoundTrigger4_1 : TriggerBase
{
    protected override void OnTriggered()
    {
        StartCoroutine(StoryManager.Instance.S4_1EnterStage());
    }
}
