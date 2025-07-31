using System;

[Serializable]
public class SubQuestData
{
    public int SubQuestID;
    public string SubQuestName;
    public bool isCompleted = false;
}

[Serializable]
public class QuestData : IData
{
    public int QuestID;
    public int Id { get; set; } // Implementing IData interface
    public string QuestTitle;
    public SubQuestData[] SubQuests;

    QuestData()
    {
        Id = QuestID;
    }
}