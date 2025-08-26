using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestPanel : UIBase
{
    [SerializeField] private RectTransform questParent;
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private RectTransform contentParent;
    [SerializeField] private GameObject subQuestTxtPrefab;
    private List<TextMeshProUGUI> subQuestTxts = new();

    private float questBoxSize = 0f;
    private bool isSlided = false;
    private Tween slideTween;

    private QuestData currentQuestData;

    protected override void Awake()
    {
        base.Awake();
        questBoxSize = contentParent.rect.width - 100f;
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe<DataChangeEvent<QuestData>>(OnQuestDataChanged);
        currentQuestData = SaveManager.Instance.MySaveData.currentQuest;
        SetQuestData();
        SetSlide(true);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<DataChangeEvent<QuestData>>(OnQuestDataChanged);
    }

    public void SetSlide(bool alwaysOpen = false)
    {
        if (alwaysOpen) isSlided = false;
        else if (slideTween != null 
            && slideTween.IsActive() 
            && slideTween.IsPlaying())
            return;

        float moveX = isSlided ? questBoxSize : -questBoxSize;
        slideTween = questParent.DOMoveX(questParent.position.x + moveX, 1f)
            .SetEase(Ease.InOutBack)
            .OnComplete(() => slideTween = null);

        isSlided = !isSlided;
    }

    public void CompleteSubQuest(int id)
    {
        subQuestTxts[id].transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnQuestDataChanged(DataChangeEvent<QuestData> @event)
    {
        currentQuestData = @event.NewValue;
        SetQuestData();
    }

    private void SetQuestData()
    {
        if (currentQuestData == null)
        {
            titleTxt.text = "No Active Quest";
            return;
        }
        titleTxt.text = currentQuestData.QuestTitle;

        foreach (var items in subQuestTxts)
        {
            Destroy(items.gameObject);
        }
        subQuestTxts.Clear();

        for (int i = 0; i < currentQuestData.SubQuests.Count; i++)
        {
            var subQuest = currentQuestData.SubQuests[i];
            TextMeshProUGUI newTmp = Instantiate(subQuestTxtPrefab, contentParent)
                .GetComponent<TextMeshProUGUI>();
            subQuestTxts.Add(newTmp);
            subQuestTxts[i].text = $"({subQuest.SubQuestID}) {subQuest.SubQuestName}";
        }
    }
}