using System.Collections;
using UnityEngine;

public class Altar : InteractEnter
{
    [SerializeField] private GameObject item;
    [SerializeField] private GameObject thread;

    protected override IEnumerator InteractCoroutine()
    {
        yield return base.InteractCoroutine();

        if (stageIdx == 3)
        {
            switch (storyIdx)
            {
                case 1:
                    //임시 코드
                    UIManager.Instance.ShowPanelWithParam<ItemDetailPanel>(eItemPanelType.Karmic_Shard);
                    break;
                case 2:
                    //애니메이션 재생
                    break;
                case 3:
                    thread.SetActive(true); //혼의 실 활성화
                    break;
            }
        }
        
        yield return null;
    }
}
