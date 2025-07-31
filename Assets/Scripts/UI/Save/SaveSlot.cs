using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SaveSlot : MonoBehaviour
{
    private void OnEnable()
    {
        //GameEventBus.Subscribe<CheckPointEvent>();
    }
    private void OnDisable()
    {
        
    }
    private string objName; // 현재 오브젝트의 슬롯 넘버
    private string slotName;  // saveName에 들어갈 슬롯 넘버 스트링

    private string sceneName;
    private string stageName;

    private string saveDate;
    /*
     * 슬롯이름형식:[슬롯네이밍]스테이지넘버-퀘스트명칭-저장시간
     * [슬롯1]7일차/스테이지1-파드마와의만남-2025.07.11.금17:42
     */
    private string saveName;    // 최종적으로 저장될 전체 이름 

    private TMP_Text slotText;

    private void Start()
    {
        slotText = transform.GetComponentInChildren<TMP_Text>();
    }
    public void OnClickSaveSlot()
    {
        SetSlotName();

        SaveCurentData();
        slotText.text = saveName;
    }

    private void SaveCurentData()
    {
        saveName = '[' + slotName + "]" + " " + stageName + " - " + "파드마와의 만남" + " - " + saveDate;
        SaveManager.Instance.SetSaveData("saveName", this.saveName);
    }

    private void SetSlotName()
    {
        objName = gameObject.name;

        switch (objName)
        {
            case "Slot1":
                slotName = "슬롯1";
                break;
            case "Slot2":
                slotName = "슬롯2";
                break;
            case "Slot3":
                slotName = "슬롯3";
                break;
            case "Slot4":
                slotName = "슬롯4";
                break;
            case "Slot5":
                slotName = "슬롯5";
                break;
        }
    }
    
    private void SetStageName()
    {
        sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Prototype":
                stageName = "7일차 / 스테이지 1";
                break;
            case "Stage2":
                stageName = "14일차 / 스테이지 2";
                break;
            case "Stage3":
                stageName = "21일차 / 스테이지 3";
                break;
            case "Stage4":
                stageName = "28일차 / 스테이지 4";
                break;
            case "Stage5":
                stageName = "35일차 / 스테이지 5";
                break;
            case "Stage6":
                stageName = "42일차 / 스테이지 6";
                break;
            case "Stage7":
                stageName = "49일차 / 스테이지 7";
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
}
