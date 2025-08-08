using UnityEngine;
using TMPro;
using UnityEngine.Events;

/*
 * SavePanel에서 클릭으로 save data 저장
 * Auto 슬롯의 데이터를 Load해서 저장한다.
 */
public class ClickSaveSlot : MonoBehaviour
{
    private TMP_Text slotText;
    private string slotName;
    private bool hasSaveSlot;

    private void Start()
    {
        slotText = GetComponentInChildren<TMP_Text>();
    }

    public void OnClickSaveSlot()
    {
        SaveManager.Instance.LoadSlot(ESaveSlot.Auto);

        switch (gameObject.name)
        {
            case "Slot1":
                hasSaveSlot = SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot1);

                if(!hasSaveSlot)
                {
                    SaveManager.Instance.SaveSlot(ESaveSlot.Slot1);
                    SaveManager.Instance.currentSaveSlot = 1;
                    slotName = "[슬롯1]";
                    changeSlotText();
                }
                else
                {
                    UIManager.Instance.ShowPanelWithParam<YesNoPanel>("YesNoPanel", new object[] {
                    EYesNoPanelType.Save,
                    new UnityAction(() =>
                    {
                        SaveManager.Instance.SaveSlot(ESaveSlot.Slot1);
                        SaveManager.Instance.currentSaveSlot = 1;
                        slotName = "[슬롯1]";
                        changeSlotText();
                    })
                    });
                }

                break;
            case "Slot2":
                hasSaveSlot = SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot2);

                if (!hasSaveSlot)
                {
                    SaveManager.Instance.SaveSlot(ESaveSlot.Slot2);
                    SaveManager.Instance.currentSaveSlot = 2;
                    slotName = "[슬롯2]";
                    changeSlotText();
                }
                else
                {
                    UIManager.Instance.ShowPanelWithParam<YesNoPanel>("YesNoPanel", new object[] {
                    EYesNoPanelType.Save,
                    new UnityAction(() =>
                    {
                        SaveManager.Instance.SaveSlot(ESaveSlot.Slot2);
                        SaveManager.Instance.currentSaveSlot = 2;
                        slotName = "[슬롯2]";
                        changeSlotText();
                    })
                    });
                }
                break;
            case "Slot3":
                hasSaveSlot = SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot3);

                if (!hasSaveSlot)
                {
                    SaveManager.Instance.SaveSlot(ESaveSlot.Slot3);
                    SaveManager.Instance.currentSaveSlot = 3;
                    slotName = "[슬롯3]";
                    changeSlotText();
                }
                else
                {
                    UIManager.Instance.ShowPanelWithParam<YesNoPanel>("YesNoPanel", new object[] {
                    EYesNoPanelType.Save,
                    new UnityAction(() =>
                    {
                        SaveManager.Instance.SaveSlot(ESaveSlot.Slot3);
                        SaveManager.Instance.currentSaveSlot = 3;
                        slotName = "[슬롯3]";
                        changeSlotText();
                    })
                    });
                }
                break;
            case "Slot4":
                hasSaveSlot = SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot4);

                if (!hasSaveSlot)
                {
                    SaveManager.Instance.SaveSlot(ESaveSlot.Slot4);
                    SaveManager.Instance.currentSaveSlot = 4;
                    slotName = "[슬롯4]";
                    changeSlotText();
                }
                else
                {
                    UIManager.Instance.ShowPanelWithParam<YesNoPanel>("YesNoPanel", new object[] {
                    EYesNoPanelType.Save,
                    new UnityAction(() =>
                    {
                        SaveManager.Instance.SaveSlot(ESaveSlot.Slot4);
                        SaveManager.Instance.currentSaveSlot = 4;
                        slotName = "[슬롯4]";
                        changeSlotText();
                    })
                    });
                }
                break;
            case "Slot5":
                hasSaveSlot = SaveManager.Instance.HasSaveSlot(ESaveSlot.Slot5);

                if (!hasSaveSlot)
                {
                    SaveManager.Instance.SaveSlot(ESaveSlot.Slot5);
                    SaveManager.Instance.currentSaveSlot = 5;
                    slotName = "[슬롯5]";
                    changeSlotText();
                }
                else
                {
                    UIManager.Instance.ShowPanelWithParam<YesNoPanel>("YesNoPanel", new object[] {
                    EYesNoPanelType.Save,
                    new UnityAction(() =>
                    {
                        SaveManager.Instance.SaveSlot(ESaveSlot.Slot5);
                        SaveManager.Instance.currentSaveSlot = 5;
                        slotName = "[슬롯5]";
                        changeSlotText();
                    })
                    });
                }
                break;
        }
        hasSaveSlot = false;
    }

    private void changeSlotText()
    {
        slotText.text = slotName + SaveManager.Instance.MySaveData.saveName.ToString();
    }
}
