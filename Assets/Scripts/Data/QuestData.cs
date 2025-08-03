using System;
using System.Collections.Generic;

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
    public int Id
    {
        get { return QuestID; }
        set { QuestID = value; }
    }

    public int QuestID;
    public string QuestTitle;
    public List<SubQuestData> SubQuests;
}