using System.Collections;
using UnityEngine;

public class Altar : InteractEnter
{
    [SerializeField] private GameObject item;
    [SerializeField] private GameObject thread;

    protected override IEnumerator InteractCoroutine()
    {
        yield return base.InteractCoroutine();
        item.SetActive(true);
        SaveManager.Instance.SetSaveData(nameof(SaveData.quest1ItemSet), true, storyIdx - 1);

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
                    thread.SetActive(true); //혼의 실 활성화
                    break;
            }
        }
        
        yield return null;
    }
}
