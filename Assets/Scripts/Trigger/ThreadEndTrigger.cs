using UnityEngine;

public class ThreadEndTrigger : TriggerBase
{
    [SerializeField] private Stage3_1Thread thread;

    protected override void OnTriggered()
    {
        if (SaveManager.Instance.MySaveData.stageIdx == 3 && SaveManager.Instance.MySaveData.storyIdx == 3)
        {
            thread.LastThreadVideo();
            SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), 4); 
            SaveManager.Instance.SaveSlot();
        }
    }
}