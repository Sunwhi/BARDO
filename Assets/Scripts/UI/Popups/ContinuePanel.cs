using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContinuePanel : UIBase
{
    [SerializeField] private List<SaveSlot> slots = new();

    private void Start()
    {
        int slotNum = 0;
        foreach (var slot in slots)
        {
            //TODO : SaveManager 완성되면 거기서 개별 슬롯의 이름 불러오기
            SaveData slotData = new();
            slotData = SaveManager.Instance.SaveSlots[slotNum++];

            //TODO : 만약 SaveSlot이 비어있다면 SetSlot 스킵.
            if (slotData.dataSaved) slot.SetSlot(slotData, OnContinueSlotClicked);
        }
    }

    public override void OnUICloseBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Select_Settings);
        base.OnUICloseBtn();
    }

    private void OnContinueSlotClicked(int idx)
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Select_Settings);

        ESaveSlot slot = (ESaveSlot)idx;
        bool hasSaveSlot = false;

        if (SaveManager.Instance.HasSaveSlot(slot)) 
        {
            SaveManager.Instance.LoadSlot(slot);
            hasSaveSlot = true;
        }

        if (hasSaveSlot)
        {
            UIManager.Show<YesNoPanel>(
            EYesNoPanelType.Continue,
            new UnityAction(() =>
            {
                SaveManager.Instance.SetSaveSlotIdx(idx);
                ContinueGame();
                SoundManager.Instance.PlayBGM(EBGM.Stage1);
            }));
        }
        else
        {
            //TODO : 슬롯에 저장된게 없을 때 안내 팝업?
        }
    }

    private void ContinueGame()
    {
        MySceneManager.Instance.LoadScene(ESceneType.MainScene);
        GameEventBus.Raise(new ClickContinueEvent());
    }
}