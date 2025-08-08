using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * checkpoint마다 자동으로 SaveData에 데이터 저장
 */
public class AutoSaveData : Singleton<AutoSaveData>
{
    private string objName; // 현재 오브젝트의 슬롯 넘버
    private string slotName;  // saveName에 들어갈 슬롯 넘버 스트링

    private string sceneName;
    private string stageName;
    private int stageIdx;

    private string saveDate;

    private int storyIdx;

    private string questName;
    /*
     * 슬롯이름형식:[슬롯네이밍]스테이지넘버-퀘스트명칭-저장시간
     * [슬롯1]7일차/스테이지1-파드마와의만남-2025.07.11.금17:42
     */
    private string saveName;    // 최종적으로 저장될 전체 이름 

    private void OnEnable()
    {
        GameEventBus.Subscribe<CheckPointEvent>(OnCheckPointSave);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe<CheckPointEvent>(OnCheckPointSave);
    }
    private void Update()
    {
        //Debug.Log(SaveManager.Instance.MySaveData.saveName.ToString());
    }

    // CheckPointTrigger마다 새로운 SaveData 생성
    private void OnCheckPointSave(CheckPointEvent ev)
    {
        //SaveManager.Instance.CreateSaveData();
        SetQuestName(ev.checkpointID);

        SetStageNameIdx();
        SetSaveDate();

        SaveCurentData();
        Debug.Log(saveName);
    }

    
    private void SaveCurentData()
    {
        saveName = " " + stageName + " - " + questName + " - " + saveDate;
        SaveManager.Instance.SetSaveData("saveName", this.saveName); // saveName 저장
        //SaveManager.Instance.SetSaveData("lastSaveTime", DateTime.Now.Ticks); 시간은 자동으로 저장
        SaveManager.Instance.SetSaveData("stageIdx", stageIdx);  // stageIdx 저장
        SaveManager.Instance.SetSaveData("storyIdx", storyIdx);  // storyIdx 저장

        SaveManager.Instance.SaveSlot(ESaveSlot.Auto);
    }

    private void SetStageNameIdx()
    {
        sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "MainScene":
                stageName = "1주차 / 스테이지 1";
                stageIdx = 1;
                break;
            case "Stage2":
                stageName = "2주차 / 스테이지 2";
                stageIdx = 2;
                break;
            case "Stage3":
                stageName = "3주차 / 스테이지 3";
                stageIdx = 3;
                break;
            case "Stage4":
                stageName = "4주차 / 스테이지 4";
                stageIdx = 4;
                break;
            case "Stage5":
                stageName = "5주차 / 스테이지 5";
                stageIdx = 5;
                break;
            case "Stage6":
                stageName = "6주차 / 스테이지 6";
                stageIdx = 6;
                break;
            case "Stage7":
                stageName = "7주차 / 스테이지 7";
                stageIdx = 7;
                break;
        }
    }

    private void SetSaveDate()
    {
        long ticks = DateTime.Now.Ticks;

        // ticks → DateTime
        DateTime dateTime = new DateTime(ticks);

        // 요일을 한글로 변환
        string[] koreanDays = { "일", "월", "화", "수", "목", "금", "토" };
        string dayOfWeek = koreanDays[(int)dateTime.DayOfWeek];

        // 최종 문자열 조합
        string formatted = dateTime.ToString($"yyyy.MM.dd.{dayOfWeek}HH:mm");

        saveDate = formatted;
        //Debug.Log(formatted);  // 예: 2025.07.29.화16:23
    }

    private void SetQuestName(string checkpointID)
    {
        switch (checkpointID)
        {
            case "stage1-1":
                questName = "파드마와의 만남";
                storyIdx = 1;
                break;
            case "stage1-2":
                questName = "미지의 세계로";
                storyIdx = 2;
                break;
            case "stage1-3":
                questName = "다음 스테이지로 가자";
                storyIdx = 3;
                break;
            case "stage2-1":
                questName = "2스테이지 시작";
                break;
        }
    }

}
