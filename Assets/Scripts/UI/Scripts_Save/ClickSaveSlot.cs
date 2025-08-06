using UnityEngine;
using TMPro;

/*
 * SavePanel에서 클릭으로 save data 저장
 * Auto 슬롯의 데이터를 Load해서 저장한다.
 */
public class ClickSaveSlot : MonoBehaviour
{
    private TMP_Text slotText;
    private string slotName;

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

        switch (gameObject.name)
        {
            case "Slot1":
                SaveManager.Instance.SaveSlot(ESaveSlot.Slot1);
                slotName = "[슬롯1]";
                break;
            case "Slot2":
                SaveManager.Instance.SaveSlot(ESaveSlot.Slot2);
                slotName = "[슬롯2]";
                break;
            case "Slot3":
                SaveManager.Instance.SaveSlot(ESaveSlot.Slot3);
                slotName = "[슬롯3]";
                break;
            case "Slot4":
                SaveManager.Instance.SaveSlot(ESaveSlot.Slot4);
                slotName = "[슬롯4]";
                break;
            case "Slot5":
                SaveManager.Instance.SaveSlot(ESaveSlot.Slot5);
                slotName = "[슬롯5]";
                break;
        }

        slotText.text = slotName + SaveManager.Instance.MySaveData.saveName.ToString();
    }
}
