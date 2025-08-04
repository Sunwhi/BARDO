using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * SavePanel, ContinuePanel의 5개 슬롯 UI를 최근 저장 데이터로 표시
 */
public class SaveSlotInit : MonoBehaviour
{

    public List<GameObject> Slots = new List<GameObject>();
    private TMP_Text slotText;
    private void Start()
    {
        if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot1))
        {
            slotText = Slots[0].GetComponentInChildren<TMP_Text>();
            SaveManager.Instance.LoadSlot(ESaveSlot.Slot1);
            slotText.text = "[슬롯1]" + SaveManager.Instance.MySaveData.saveName.ToString();
        }
        if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot2))
        {
            slotText = Slots[1].GetComponentInChildren<TMP_Text>();
            SaveManager.Instance.LoadSlot(ESaveSlot.Slot2);
            slotText.text = "[슬롯2]" + SaveManager.Instance.MySaveData.saveName.ToString();
        }
        if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot3))
        {
            slotText = Slots[2].GetComponentInChildren<TMP_Text>();
            SaveManager.Instance.LoadSlot(ESaveSlot.Slot3);
            slotText.text = "[슬롯3]" + SaveManager.Instance.MySaveData.saveName.ToString();
        }
        if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot4))
        {
            slotText = Slots[3].GetComponentInChildren<TMP_Text>();
            SaveManager.Instance.LoadSlot(ESaveSlot.Slot4);
            slotText.text = "[슬롯4]" + SaveManager.Instance.MySaveData.saveName.ToString();
        }
        if (SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot5))
        {
            slotText = Slots[4].GetComponentInChildren<TMP_Text>();
            SaveManager.Instance.LoadSlot(ESaveSlot.Slot5);
            slotText.text = "[슬롯5]" + SaveManager.Instance.MySaveData.saveName.ToString();
        }
    }
}
