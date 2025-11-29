using System;
using UnityEngine;

public class QuestPanelTrigger : TriggerBase
{
    protected override void OnTriggered()
    {
        StoryManager.Instance.S3_EnterStage();
    }
}
