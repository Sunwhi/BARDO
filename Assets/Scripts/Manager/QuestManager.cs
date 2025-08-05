using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public override void Awake()
    {
        base.Awake();
        //Init();
    }

    //테스트 코드
    private void Start()
    {
        ShowQuestUI();
    }

    public void Init()
    {
        if (SaveManager.Instance.MySaveData.isQuestActive)
        {
            ShowQuestUI();
        }
    }

    public void ShowQuestUI()
    {
        if (!SaveManager.Instance.MySaveData.isQuestActive)
        {
            SaveManager.Instance.SetSaveData(nameof(SaveData.isQuestActive), true);
            SetNewQuest();
        }

        UIManager.Instance.ShowPanel(nameof(QuestPanel));
    }

    public void SetNewQuest()
    {
        if (SaveManager.Instance.MySaveData.currentQuest == null)
        {
            var curQuest = DataManager.Instance.GetObj<QuestData>(1);
            SaveManager.Instance.SetSaveData(nameof(SaveData.currentQuest), curQuest);
        }
        else
        {
            var curQuest = DataManager.Instance.GetObj<QuestData>(SaveManager.Instance.MySaveData.currentQuest.QuestID + 1);

            if (curQuest != null)
            {
                SaveManager.Instance.SetSaveData(nameof(SaveData.currentQuest), curQuest);
            }
            else
            {
                //TODO : 마지막 퀘스트 완료시 이벤트.
            }
        }
    }

    public void ClearSubQuest(int subQuestID)
    {
        var curQuest = SaveManager.Instance.MySaveData.currentQuest;
        if (curQuest != null && curQuest.SubQuests.Count > subQuestID)
        {
            curQuest.SubQuests[subQuestID].isCompleted = true;
            SaveManager.Instance.SetSaveData(nameof(SaveData.currentQuest), curQuest);
        }
        else
        {
            Debug.LogWarning($"SubQuest with ID {subQuestID} does not exist in the quest {curQuest.QuestID}");
        }

        //TODO : SubQuest 모두 완료 시 처리.
    }
}