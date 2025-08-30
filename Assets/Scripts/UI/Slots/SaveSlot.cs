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

    public void SetSlot(SaveData data, Action<int> onSlotClicked)
    {
        Debug.Log("setslot");
        disableSlot.SetActive(false);
        enableSlot.SetActive(true);

        slotNameTxt.text = $"[{data.saveName}] {data.stageIdx}주차/스테이지{data.stageIdx}";
        //TODO : slotDetailTxt
        slotDetailTxt.text = data.checkPointName;

        slotDateTxt.text = new DateTime(data.lastSaveTime).ToString("yyyy/MM/dd (ddd) HH:mm");

        slotBtn.enabled = true; 
        slotBtn.onClick.RemoveAllListeners();
        slotBtn.onClick.AddListener(() => onSlotClicked?.Invoke(slotIdx));
    }


    private void SetCheckpointName(string checkpointID)
    {
        string checkPointName;

        switch (checkpointID)
        {
            case "stage1-1":
                checkPointName = "파드마와의 만남";
                break;
            case "stage1-2":
                checkPointName = "미지의 세계로";
                break;
            case "stage1-3":
                checkPointName = "다음 스테이지로 가자";
                break;
            case "stage1-4":
                checkPointName = "클리어직전";
                break;
            case "stage2-0":
                checkPointName = "파드마와의 재회";
                break;
            case "stage2-1":
                checkPointName = "아이템을 찾아서";
                break;
        }
    }
}