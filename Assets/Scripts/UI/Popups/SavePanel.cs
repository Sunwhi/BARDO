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
            //TODO : 만약 SaveSlot이 비어있다면 SetSlot 스킵.
            if (slotData.dataSaved) 
            {
                Debug.Log(slot.name);
                slot.SetSlot(slotData, OnSaveSlotSlicked);
            }
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
        slots[(int)slotName].SetSlot(slotData, OnSaveSlotSlicked);
    }

    public override void OnUICloseBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Select_Settings);
        base.OnUICloseBtn();
    }
    private void OnSaveSlotSlicked(int idx)
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Select_Settings);

        ESaveSlot slot = (ESaveSlot)idx;
        bool hasSaveSlot = SaveManager.Instance.HasSaveSlot(slot);

        if (hasSaveSlot)
        {
            UIManager.Show<YesNoPanel>(
            EYesNoPanelType.Save,
            new UnityAction(() =>
            {
                SaveManager.Instance.SaveSlot(slot);
            }
            ));
        }
        else
        {
            SaveManager.Instance.SaveSlot(slot);
        }
    }   
}