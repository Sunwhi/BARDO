using System.Collections;
using UnityEngine;

public class Altar : MonoBehaviour
{
    public int altarIdx;

    [SerializeField] private GameObject item;
    [SerializeField] private GameObject guide;
    [SerializeField] private GameObject thread;

    private Coroutine altarCoroutine;

    private IEnumerator AltarCoroutine()
    {
        yield return new WaitUntil(() =>
    Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter));

        SaveData curData = SaveManager.Instance.MySaveData;

        switch (altarIdx)
        {
            case 1:
                //임시 코드
                UIManager.Instance.ShowPanelWithParam<ItemDetailPanel>(nameof(ItemDetailPanel), new object[] { eItemPanelType.Karmic_Shard });
                SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), 2);
                break;
            case 2:
                //애니메이션 재생
                SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), 3);
                break;
            case 3:
                thread.SetActive(true); //혼의 실
                SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), 4);
                break;
        }

        guide.SetActive(false);
        item.SetActive(true);
        altarCoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SaveManager.Instance.MySaveData.stageIdx == 3
            && SaveManager.Instance.MySaveData.storyIdx == altarIdx)
        {
            guide.SetActive(true);
            altarCoroutine ??= StartCoroutine(AltarCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        guide.SetActive(false);
        if (altarCoroutine != null)
        {
            StopCoroutine(altarCoroutine);
            altarCoroutine = null;
        }
    }
}
