using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private Button slotBtn;
    [SerializeField] private TextMeshProUGUI slotNameTxt;
    [SerializeField] private int slotIdx = 0; // 1~5

    public void SetSlot(string name, Action<int> onSlotClicked)
    {
        slotNameTxt.text = name;
        slotBtn.onClick.RemoveAllListeners();
        slotBtn.onClick.AddListener(() => onSlotClicked?.Invoke(slotIdx));
    }
}