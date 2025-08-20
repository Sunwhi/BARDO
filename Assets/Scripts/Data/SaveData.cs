using System;

public enum EEnding
{
    Infernal,
    Atonement,
    Defilement,
    Enlightenment,
    Reincarnation
}

[Serializable]
public class SaveData
{
    public string slotNumber = "[슬롯0]";
    public string saveName = "New Save";
    public long lastSaveTime = 0;
    
    public int stageIdx = 0; //0 : 시작. 1~ : N주차.
    public int storyIdx = 0; //Stage 내부 Idx
    public SerializableVector3 savedPosition = new SerializableVector3(0,0,0);

    public bool isQuestActive = false;
    public QuestData currentQuest = null;
    public bool[] quest1ItemAcquired = new bool[3]; //0: Karma Shard, 1: Memory Lamp, 2: Soul Thread

    //Endings
    public bool firstJudgement = true; //true = 선. false = 악.
    public bool secondJudgement = true; //true = 선. false = 악.
    public EEnding ending = EEnding.Enlightenment; // Ending.
}