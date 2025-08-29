using System;
using UnityEngine;

/*
 * checkpoint마다 자동으로 SaveData에 데이터 저장
 */
public class AutoSaveData : Singleton<AutoSaveData>
{
    private string checkPointName;
    /*
     * 슬롯이름형식:[슬롯네이밍]스테이지넘버-퀘스트명칭-저장시간
     * [슬롯1]7일차/스테이지1-파드마와의만남-2025.07.11.금17:42
     */
    private string saveName;    // 플레이어가 입력하는 슬롯이름 

    private void OnEnable()
    {
        GameEventBus.Subscribe<CheckPointEvent>(OnCheckPointSave);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe<CheckPointEvent>(OnCheckPointSave);
    }

    // CheckPointTrigger마다 새로운 SaveData 생성
    private void OnCheckPointSave(CheckPointEvent ev)
    {
        SetCheckpointName(ev.checkpointID);
        SaveManager.Instance.SetSaveData(nameof(SaveData.checkPointName), this.checkPointName);
        SaveManager.Instance.SaveSlot(ESaveSlot.Auto);
        SaveManager.Instance.SaveSlot((ESaveSlot)SaveManager.Instance.currentSaveSlot); // 자동 저장
    }

    private void SetCheckpointName(string checkpointID)
    {
        switch (checkpointID)
        {
            case "stage1-1":
                checkPointName = "파드마와의 만남";
                break;
            case "stage1-2":
                checkPointName = "미지의 세계로";
                break;
            case "stage1-3":
                checkPointName = "다음 스테이지로 가자";
                break;
            case "stage1-4":
                checkPointName = "클리어직전";
                break;
            case "stage2-0":
                checkPointName = "파드마와의 재회";
                break;
            case "stage2-1":
                checkPointName = "아이템을 찾아서";
                break;
        }
    }

}
