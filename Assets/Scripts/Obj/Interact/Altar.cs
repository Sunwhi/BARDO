using System.Collections;
using UnityEngine;

public class Altar : InteractEnter
{
    [SerializeField] private GameObject item;
    [SerializeField] private Stage3_1Thread thread;
    [SerializeField] private VideoController VideoController;

    private void Start()
    {
        SaveData data = SaveManager.Instance.MySaveData;
        if (data.stageIdx != 3 || data.stageIdx == 3 && data.storyIdx > 2)
        {
            item.SetActive(false);
            return;
        }

        if (data.quest1ItemSet[storyIdx] && !SaveManager.Instance.MySaveData.threadEnabled)
            item.SetActive(true);
        else
            item.SetActive(false);
    }

    protected override IEnumerator InteractCoroutine()
    {
        yield return base.InteractCoroutine();
        item.SetActive(true);
        SaveManager.Instance.SetSaveData(nameof(SaveData.quest1ItemSet), true, storyIdx);

        if (stageIdx == 3)
        {
            switch (storyIdx)
            {
                case 0:
                    SoundManager.Instance.PlaySFX(ESFX.Karmic_Shard);
                    SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), 1);
                    QuestManager.Instance.ClearSubQuest(0);
                    UIManager.Show<MapHintPanel>();
                    break;
                case 1:
                    SoundManager.Instance.PlaySFX(ESFX.Memory_Lamp);
                    SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), 2);
                    QuestManager.Instance.ClearSubQuest(1);
                    if(VideoController != null)
                    {
                        VideoController.PlayVideo(VideoType.Stage3);
                    }
                    break;
                case 2:
                    SoundManager.Instance.PlaySFX(ESFX.Soul_Thread);
                    SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), 3);
                    QuestManager.Instance.ClearSubQuest(2);
                    thread.PlayThreadVideo();
                    break;
            }
        }
        
        yield return null;
    }
}
