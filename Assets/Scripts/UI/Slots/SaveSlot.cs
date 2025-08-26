using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int slotIdx = 0; // 1~5

    [Header("Components")]
    [SerializeField] private Image slotImg;
    [SerializeField] private Button slotBtn;

    [Header("Slot Parent")]
    [SerializeField] private GameObject disableSlot;
    [SerializeField] private GameObject enableSlot;

    [Header("Slot Enabled")]
    [SerializeField] private TextMeshProUGUI slotNameTxt;
    [SerializeField] private TextMeshProUGUI slotDetailTxt;
    [SerializeField] private TextMeshProUGUI slotDateTxt;
    [SerializeField] private Image slotLineImg;

    [Header("Sprites")] //0 : black(default), 1 : white(hover).
    [SerializeField] private Sprite[] slotImgSprites;
    [SerializeField] private Sprite[] slotLineSprites;

    public void OnPointerEnter(PointerEventData eventData)
    {
            
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void SetSlot(SaveData data, Action<int> onSlotClicked)
    {
        disableSlot.SetActive(false);
        enableSlot.SetActive(true);

        slotNameTxt.text = $"[{data.saveName}] {data.stageIdx}주차";
        //TODO : slotDetailTxt
        slotDateTxt.text = new DateTime(data.lastSaveTime).ToString("yyyy/MM/dd (ddd) HH:mm");

        slotBtn.enabled = true; 
        slotBtn.onClick.RemoveAllListeners();
        slotBtn.onClick.AddListener(() => onSlotClicked?.Invoke(slotIdx));
    }
}