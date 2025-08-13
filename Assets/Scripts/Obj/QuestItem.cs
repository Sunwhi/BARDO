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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (itemType)
            {
                case eQuestItems.Karmic_Shard:
                    QuestManager.Instance.ClearSubQuest(0);
                    UIManager.Instance.ShowPanelWithParam<ItemDetailPanel>(nameof(ItemDetailPanel), new object[] { eItemPanelType.Karmic_Shard });
                    break;
                case eQuestItems.Memory_Lamp:
                    QuestManager.Instance.ClearSubQuest(1);
                    UIManager.Instance.ShowPanelWithParam<ItemDetailPanel>(nameof(ItemDetailPanel), new object[] { eItemPanelType.Memory_Lamp });
                    break;
                case eQuestItems.Soul_Thread:
                    QuestManager.Instance.ClearSubQuest(2);
                    UIManager.Instance.ShowPanelWithParam<ItemDetailPanel>(nameof(ItemDetailPanel), new object[] { eItemPanelType.Soul_Thread });
                    break;
            }
        }

        gameObject.SetActive(false);
    }
}