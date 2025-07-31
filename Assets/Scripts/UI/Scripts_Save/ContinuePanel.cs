using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContinuePanel : MonoBehaviour
{
    public List<GameObject> Slots = new List<GameObject>();
    private TMP_Text slotText;

    private void Awake()
    {
        if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot1))
        {
            slotText = Slots[0].GetComponentInChildren<TMP_Text>();
            SaveManager.Instance.LoadSlot(ESaveSlot.Slot1);
            slotText.text = SaveManager.Instance.MySaveData.saveName.ToString();
        }
        if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot2))
        {
            slotText = Slots[1].GetComponentInChildren<TMP_Text>();
            SaveManager.Instance.LoadSlot(ESaveSlot.Slot2);
            slotText.text = SaveManager.Instance.MySaveData.saveName.ToString();
        }
        if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot3))
        {
            slotText = Slots[2].GetComponentInChildren<TMP_Text>();
            SaveManager.Instance.LoadSlot(ESaveSlot.Slot3);
            slotText.text = SaveManager.Instance.MySaveData.saveName.ToString();
        }
        if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot4))
        {
            slotText = Slots[3].GetComponentInChildren<TMP_Text>();
            SaveManager.Instance.LoadSlot(ESaveSlot.Slot4);
            slotText.text = SaveManager.Instance.MySaveData.saveName.ToString();
        }
        if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot5))
        {
            slotText = Slots[4].GetComponentInChildren<TMP_Text>();
            SaveManager.Instance.LoadSlot(ESaveSlot.Slot5);
            slotText.text = SaveManager.Instance.MySaveData.saveName.ToString();
        }
    }
    public void OnClickContinueExitBtn()
    {
        SoundManager.Instance.PlaySFX(eSFX.UI_Button_Select_Settings);
        UIManager.Instance.HidePanel("ContinuePanel");
    }
}
