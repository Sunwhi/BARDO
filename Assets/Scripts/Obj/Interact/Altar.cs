using System.Collections;
using UnityEngine;

public class Altar : InteractEnter
{
    [SerializeField] private GameObject item;
    [SerializeField] private Stage3_1Thread thread;

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
                    SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), 1);
                    UIManager.Show<ItemDetailPanel>(eItemPanelType.Karmic_Shard);
                    break;
                case 1:
                    SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), 2);
                    //TODO : 애니메이션 재생.
                    break;
                case 2:
                    SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), 3);
                    thread.PlayThreadVideo();
                    break;
            }
        }
        
        yield return null;
    }
}
