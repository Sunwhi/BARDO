using System.Collections.Generic;
using UnityEngine.Events;

public class ContinuePanel : UIBase
{
    public List<SaveSlot> Slots = new();

    private void Start()
    {
        foreach (var slot in Slots)
        {
            //TODO : SaveManager 완성되면 거기서 개별 슬롯의 이름 불러오기
            string fakeName = "슬롯이름";

            slot.SetSlot(fakeName, OnContinueSlotClicked);
        }
    }

    public override void OnUICloseBtn()
    {
        SoundManager.Instance.PlaySFX(ESFX.UI_Button_Select_Settings);
        base.OnUICloseBtn();
    }

    private void OnContinueSlotClicked(int idx)
    {
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
                SaveManager.Instance.currentSaveSlot = idx;
                ContinueGame();
                SoundManager.Instance.PlayBGM(EBGM.Stage1);
            }));
        }
    }

    private void ContinueGame()
    {
        MySceneManager.Instance.LoadScene(ESceneType.MainScene);
        GameEventBus.Raise(new ClickContinueEvent());
    }
}