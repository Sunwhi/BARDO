using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public class ClickContinueSlot : MonoBehaviour
{
    private int stageIdx;
    private bool hasSaveSlot = false;

    public void OnClickContinueSlot(GameObject clickedSlot)
    {
        switch (clickedSlot.name)
        {
            case "Slot1":
                if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot1))
                {
                    SaveManager.Instance.LoadSlot(ESaveSlot.Slot1);
                    SaveManager.Instance.currentSaveSlot = 1;
                    hasSaveSlot = true;
                }
                break;
            case "Slot2":
                if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot2))
                {
                    SaveManager.Instance.LoadSlot(ESaveSlot.Slot2);
                    SaveManager.Instance.currentSaveSlot = 2;
                    hasSaveSlot = true;
                }
                break;
            case "Slot3":
                if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot3))
                {
                    SaveManager.Instance.LoadSlot(ESaveSlot.Slot3);
                    SaveManager.Instance.currentSaveSlot = 3;
                    hasSaveSlot = true;
                }
                break;
            case "Slot4":
                if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot4))
                {
                    SaveManager.Instance.LoadSlot(ESaveSlot.Slot4);
                    SaveManager.Instance.currentSaveSlot = 4;
                    hasSaveSlot = true;
                }
                break;
            case "Slot5":
                if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot5))
                {
                    SaveManager.Instance.LoadSlot(ESaveSlot.Slot5);
                    SaveManager.Instance.currentSaveSlot = 5;
                    hasSaveSlot = true;
                }
                break;
        }
        if (hasSaveSlot)
        {
            UIManager.Instance.ShowPanelWithParam<YesNoPanel>(
            EYesNoPanelType.Continue,
            new UnityAction(() =>
            {
                ContinueGame();
                SoundManager.Instance.PlayBGM(eBGM.Stage1);
            })
            );
            hasSaveSlot = false;
        }
    }
    private void ContinueGame()
    {
        MySceneManager.Instance.LoadScene(SceneType.MainScene);

        GameEventBus.Raise(new ClickContinueEvent());
    }
}

