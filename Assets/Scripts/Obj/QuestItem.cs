using UnityEngine;

public enum eQuestItems
{
    Karmic_Shard,
    Memory_Lamp,
    Soul_Thread

}

public class QuestItem : MonoBehaviour
{
    [SerializeField] private eQuestItems itemType;

    private void Start()
    {
        SaveData saveData = SaveManager.Instance.MySaveData;
        gameObject.SetActive(!saveData.questItemAcquired[(int)itemType]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaveManager.Instance.SetSaveData(nameof(SaveData.questItemAcquired), true, (int)itemType);

            switch (itemType)
            {
                case eQuestItems.Karmic_Shard:
                    QuestManager.Instance.ClearSubQuest(0);
                    UIManager.Instance.ShowPanelWithParam<ItemDetailPanel>(eItemPanelType.Karmic_Shard);
                    break;
                case eQuestItems.Memory_Lamp:
                    QuestManager.Instance.ClearSubQuest(1);
                    UIManager.Instance.ShowPanelWithParam<ItemDetailPanel>(eItemPanelType.Memory_Lamp);
                    break;
                case eQuestItems.Soul_Thread:
                    QuestManager.Instance.ClearSubQuest(2);
                    UIManager.Instance.ShowPanelWithParam<ItemDetailPanel>(eItemPanelType.Soul_Thread);
                    break;
            }
        }

        bool isAllItemsAcquired = true;
        foreach (var item in SaveManager.Instance.MySaveData.questItemAcquired)
        {
            if (!item)
            {
                isAllItemsAcquired = false;
                break;
            }
        }

        if (isAllItemsAcquired)
        {

        }

        gameObject.SetActive(false);
    }
}