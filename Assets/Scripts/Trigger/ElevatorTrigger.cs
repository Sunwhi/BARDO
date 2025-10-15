using UnityEngine;
using System.Collections;
public class ElevatorTrigger : TriggerBase
{
    protected override void OnTriggered()
    {
        StartCoroutine(TriggerEffect());
    }

    private IEnumerator TriggerEffect()
    {
        UIManager.Show<ElevatorScene>();
        StoryManager.Instance.S4_EnterStage();
        yield return null;
    }
}
