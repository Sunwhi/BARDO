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
                    break;
                case eQuestItems.Memory_Lamp:
                    QuestManager.Instance.ClearSubQuest(1);
                    break;
                case eQuestItems.Soul_Thread:
                    QuestManager.Instance.ClearSubQuest(2);
                    break;
            }
        }

        gameObject.SetActive(false);
    }
}