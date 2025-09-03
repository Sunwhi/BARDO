using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SavePanel : UIBase
{
    [SerializeField] private List<SaveSlot> slots = new();
    
    private void OnEnable()
    {
        SaveManager.Instance.OnSaveSlotUpdated += UpdateSaveSlot;

        int slotNum = 0;
        foreach (var slot in slots)
        {
            //TODO : SaveManager 완성되면 거기서 개별 슬롯의 이름 불러오기
            SaveData slotData = new();
            slotData = SaveManager.Instance.SaveSlots[slotNum++];
            //TODO : 만약 SaveSlot이 비어있다면 SetSlot 스킵. -> button event 연결 위해 취소
            slot.SetSlot(slotData, OnSaveSlotClicked);
        }
    }
    private void OnDisable()
    {
        SaveManager.Instance.OnSaveSlotUpdated -= UpdateSaveSlot;
    }

    private void UpdateSaveSlot(ESaveSlot slotName)
    {
        SaveData slotData = new();
        slotData = SaveManager.Instance.SaveSlots[(int)slotName];

        slots[(int)slotName].SetSlot(slotData, OnSaveSlotClicked);
    }

    private void OnSaveSlotClicked(int idx)
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Select_Settings);
        ESaveSlot slot = (ESaveSlot)idx;
        bool hasSaveSlot = SaveManager.Instance.HasSaveSlot(slot);

        // 현재 저장슬롯 데이터를 복사해서 선택한 슬롯의 데이터에 붙여넣는다.
        SaveManager.Instance.copySaveData(idx);

        if (hasSaveSlot)
        {
            UIManager.Show<YesNoPanel>(
            EYesNoPanelType.Save,
            new UnityAction(() =>
            {
                SetSaveSlotName(); // 슬롯이름 저장 패널을 띄운다.
                SaveManager.Instance.SaveSlot(slot);
                UpdateSaveSlot(slot);
            }
            ));
        }
        else
        {
            SetSaveSlotName(); // 슬롯이름 저장 패널을 띄운다.
            SaveManager.Instance.SaveSlot(slot);
            UpdateSaveSlot(slot);
        }
    }

    public override void OnUICloseBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Select_Settings);
        base.OnUICloseBtn();
    }

    // 슬롯이름 저장 패널을 띄운다
    private void SetSaveSlotName()
    {
        UIManager.Show<SaveSlotNamePanel>(
            new UnityAction<string>((inputText) =>
            {
                SaveManager.Instance.SetSaveData(nameof(SaveData.saveName), inputText);
            }
            ));
    }

}