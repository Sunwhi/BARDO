using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int slotIdx = 0; // 0~4
    [SerializeField] private Dictionary<(int stageIdx, int storyId), string> checkpointNames = new()
    {
        {(1,0), "1스테이지" }, {(1,1), "1스테이지" }, {(1,2), "1스테이지" },
        {(2,0), "2스테이지" }, {(2,1), "2스테이지" }, {(2,2), "2스테이지" }, {(2,3), "2스테이지" }, {(2,4), "2스테이지" }, {(2,5), "2스테이지" },
        {(3,0), "3스테이지" }, {(3,1), "3스테이지" }, {(3,2), "3스테이지" }, {(3,3), "3스테이지" },
        {(4,0), "4스테이지" }
    };

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
        slotImg.sprite = slotImgSprites[1];
        slotLineImg.sprite = slotLineSprites[1];
        slotNameTxt.color = Color.black;
        slotDetailTxt.color = Color.black;
        slotDateTxt.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slotImg.sprite = slotImgSprites[0];
        slotLineImg.sprite = slotLineSprites[0];
        slotNameTxt.color = Color.white;
        slotDetailTxt.color = Color.white;
        slotDateTxt.color = Color.white;
    }

    // slot의 text를 저장 데이터로 변경
    public void SetSlot(SaveData data, Action<int> onSlotClicked)
    {
        if (data.dataSaved)
        {
            disableSlot.SetActive(false);
            enableSlot.SetActive(true);

            slotNameTxt.text = $"<{data.saveName}> {data.stageIdx}주차";
            slotDetailTxt.text = GetCheckpointName(data.stageIdx, data.storyIdx);

            slotDateTxt.text = new DateTime(data.lastSaveTime).ToString("yyyy/MM/dd (ddd) HH:mm");
        }

        slotBtn.enabled = true; 
        slotBtn.onClick.RemoveAllListeners();
        slotBtn.onClick.AddListener(() => onSlotClicked?.Invoke(slotIdx));
    }

    public string GetCheckpointName(int stageIdx, int storyId)
    {
        return checkpointNames.TryGetValue((stageIdx, storyId), out var name) ? name : "Unknown";
    }
}