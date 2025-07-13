using System;

public class SaveData
{
    public string saveName = "New Save";
    public long lastSaveTime = DateTime.Now.Ticks;
    public int storyIdx = 0; //0 : 시작. 1~ : 스테이지 단계
}