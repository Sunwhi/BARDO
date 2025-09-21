using System.Collections;
using UnityEngine;

public class Altar : InteractEnter
{
    [SerializeField] private GameObject item;
    [SerializeField] private Stage3_1Thread thread;

    private void Start()
    {
        if (SaveManager.Instance.MySaveData.quest1Completed)
        {
            gameObject.SetActive(false);
        }
        else if (SaveManager.Instance.MySaveData.quest1ItemSet[storyIdx])
        {
            gameObject.SetActive(true);
        }
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
                    //임시 코드
                    UIManager.Show<ItemDetailPanel>(eItemPanelType.Karmic_Shard);
                    break;
                case 1:
                    //애니메이션 재생
                    break;
                case 2:
                    thread.anim.SetBool("isMade", false);
                    break;
            }
        }
        
        yield return null;
    }
}
