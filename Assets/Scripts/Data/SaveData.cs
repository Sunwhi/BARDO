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
    public string saveName = "New Save";
    public long lastSaveTime = 0;
    public int stageIdx = 0; //0 : ����. 1~ : N����.
    public int storyIdx = 0; //Stage ���� Idx

    public bool firstJudgement = true; //true = ��. false = ��.
    public bool secondJudgement = true; //true = ��. false = ��.
    public EEnding ending = EEnding.Enlightenment; // Ending.
}