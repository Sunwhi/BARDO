using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Parsed;
using UnityEngine.EventSystems;

public class AutoSaveSlotUI : MonoBehaviour
{
    public List<GameObject> Slots = new List<GameObject>();
    private TMP_Text slotText;
    private int firstEmptySlotIdx;
    private void OnEnable()
    {
        GameEventBus.Subscribe<CheckPointEvent>(OnCheckPointAutoSave);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe<CheckPointEvent>(OnCheckPointAutoSave);
    }

    // 비어있는 첫번째 슬롯에 자동저장 text 표시
    private void OnCheckPointAutoSave(CheckPointEvent ev)
    {
        //Debug.Log("oncheckpointautosave");
        firstEmptySlotIdx =  SaveManager.Instance.FirstEmptySlot();

        foreach (var slot in Slots)
        {
            switch (firstEmptySlotIdx)
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
                
            if (slot.name == "Slot" + firstEmptySlotIdx)
            {
                slotText = slot.GetComponentInChildren<TMP_Text>();
                slotText.text = SaveManager.Instance.MySaveData.saveName.ToString();
            }
        }
    }
}
