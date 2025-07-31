using UnityEngine;
using TMPro;

public class ClickSaveSlot : MonoBehaviour
{
    private TMP_Text slotText;

    private void Start()
    {
        slotText = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        //Debug.Log(SaveManager.Instance.MySaveData.saveName.ToString());
    }
    public void OnClickSaveSlot()
    {
        SaveManager.Instance.LoadSlot(ESaveSlot.Auto);
        //Debug.Log(SaveManager.Instance.MySaveData.saveName.ToString());
        slotText.text = SaveManager.Instance.MySaveData.saveName.ToString();

        switch (gameObject.name)
        {
            case "Slot1":
                SaveManager.Instance.SaveSlot(ESaveSlot.Slot1);
                break;
            case "Slot2":
                SaveManager.Instance.SaveSlot(ESaveSlot.Slot2);
                break;
            case "Slot3":
                SaveManager.Instance.SaveSlot(ESaveSlot.Slot3);
                break;
            case "Slot4":
                SaveManager.Instance.SaveSlot(ESaveSlot.Slot4);
                break;
            case "Slot5":
                SaveManager.Instance.SaveSlot(ESaveSlot.Slot5);
                break;
        }
    }
}
