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
    [SerializeField] private Transform stage3PlayerPos;

    private void Start()
    {
        SaveData saveData = SaveManager.Instance.MySaveData;
        gameObject.SetActive(!saveData.quest1ItemAcquired[(int)itemType]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaveManager.Instance.SetSaveData(nameof(SaveData.quest1ItemAcquired), true, (int)itemType);
            SaveManager.Instance.SaveSlot();

            switch (itemType)
            {
                case eQuestItems.Karmic_Shard:
                    QuestManager.Instance.ClearSubQuest(1);
                    UIManager.Show<ItemDetailPanel>(eItemPanelType.Karmic_Shard, stage3PlayerPos);
                    break;
                case eQuestItems.Memory_Lamp:
                    QuestManager.Instance.ClearSubQuest(2);
                    UIManager.Show<ItemDetailPanel>(eItemPanelType.Memory_Lamp, stage3PlayerPos);
                    break;
                case eQuestItems.Soul_Thread:
                    QuestManager.Instance.ClearSubQuest(3);
                    UIManager.Show<ItemDetailPanel>(eItemPanelType.Soul_Thread, stage3PlayerPos);
                    break;
            }
        }

        gameObject.SetActive(false);
    }
}