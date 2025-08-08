using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Parsed;
using UnityEngine.EventSystems;

/*
 * 자동저장을 SavePanel UI에 표시한다.
 */
public class AutoSaveSlotUI : MonoBehaviour
{
    public List<GameObject> Slots = new List<GameObject>();
    private TMP_Text slotText;
    private int currentSaveSlot;
    private void OnEnable()
    {
        GameEventBus.Subscribe<CheckPointEvent>(OnCheckPointAutoSave);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe<CheckPointEvent>(OnCheckPointAutoSave);
    }

    // 자동저장 text 표시
    private void OnCheckPointAutoSave(CheckPointEvent ev)
    {
        //Debug.Log("oncheckpointautosave");
        currentSaveSlot =  SaveManager.Instance.currentSaveSlot; // 현재 저장되고 있는 슬롯 불러오기
        Debug.Log(currentSaveSlot);
        foreach (var slot in Slots)
        {
            switch (currentSaveSlot)
            {
                case 1:
                    SaveManager.Instance.SaveSlot(ESaveSlot.Slot1);
                    break;
                case 2:
                    SaveManager.Instance.SaveSlot(ESaveSlot.Slot2);
                    break;
                case 3:
                    SaveManager.Instance.SaveSlot(ESaveSlot.Slot3);
                    break;
                case 4:
                    SaveManager.Instance.SaveSlot(ESaveSlot.Slot4);
                    break;
                case 5:
                    SaveManager.Instance.SaveSlot(ESaveSlot.Slot5);
                    break;

            }
                
            if (slot.name == "Slot" + currentSaveSlot)
            {
                slotText = slot.GetComponentInChildren<TMP_Text>();
                slotText.text = "[슬롯" + currentSaveSlot + "]" + SaveManager.Instance.MySaveData.saveName.ToString();
            }
        }
    }
}
