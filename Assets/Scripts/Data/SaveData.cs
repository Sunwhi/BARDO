using System;
using System.Collections.Generic;

public enum SelectCard
{
    Default,
    BlackLantern,
    Kiln,
    Shoes,
    DimLantern
}

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
    public string stageName = "";
    public string checkPointName = "empty";
    public long lastSaveTime = 0;
    public bool dataSaved = false;
    
    public int stageIdx = 0; //0 : 시작. 1~ : N주차.
    public int storyIdx = 1; //Stage 내부 Idx
    public SerializableVector3 savedPosition = new SerializableVector3(0,0,0);
    public CamState savedCamState = CamState.v1;

    public bool stage1PadmaActive = true;
    public bool tutoDirectionComplete = false;
    public bool tutoJumpComplete = false;

    public bool isQuestActive = false;
    public QuestData currentQuest = null;

    public bool[] quest1ItemAcquired = new bool[3]; //0: Karma Shard, 1: Memory Lamp, 2: Soul Thread
    public bool[] quest1ItemSet = new bool[3]; //0: Karma Shard, 1: Memory Lamp, 2: Soul Thread
    public bool quest1Completed = false;
    public bool threadEnabled = false;
    public SelectCard selectedCard = SelectCard.Default;

    //Stage4 기믹 저장
    public bool isElevatorUp = true; //엘리베이터 위치
    public List<int> teleportIdxs;

    //Endings
    public bool firstJudgement = true; //true = 선. false = 악.
    public bool secondJudgement = true; //true = 선. false = 악.
    public EEnding ending = EEnding.Enlightenment; // Ending.
}