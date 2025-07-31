using System;
using System.Collections.Generic;

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
    public string saveName = "New Save";
    public long lastSaveTime = 0;
    public int stageIdx = 0; //0 : 시작. 1~ : N주차.
    public int storyIdx = 0; //Stage 내부 Idx

    public bool isQuestActive = false;
    public QuestData currentQuest = null;

    public bool firstJudgement = true; //true = 선. false = 악.
    public bool secondJudgement = true; //true = 선. false = 악.
    public EEnding ending = EEnding.Enlightenment; // Ending.
}