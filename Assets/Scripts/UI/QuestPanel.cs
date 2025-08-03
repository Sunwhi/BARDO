using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestPanel : UIBase
{
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private RectTransform contentParent;
    [SerializeField] private GameObject subQuestTxtPrefab;
    private List<TextMeshProUGUI> subQuestTxts;

    private bool isSlided = false;
    private bool isOpened = true;
    private Tween slideTween;
    private Tween openTween;

    private QuestData currentQuestData;

    private void OnEnable()
    {
        GameEventBus.Subscribe<DataChangeEvent<QuestData>>(OnQuestDataChanged);
        currentQuestData = SaveManager.Instance.MySaveData.currentQuest;
        SetQuestData();
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<DataChangeEvent<QuestData>>(OnQuestDataChanged);
    }

    public void SetSlide()
    {
        if (slideTween != null && slideTween.IsActive() && slideTween.IsPlaying())
            return;

        float moveX = isSlided ? 500f : -500f;
        slideTween = transform.DOMoveX(transform.position.x + moveX, 1f)
            .SetEase(isSlided ? Ease.InOutBack : Ease.InOutBack)
            .OnComplete(() => slideTween = null);

        isSlided = !isSlided;
    }

    public void SetOpen()
    {
        if (openTween != null && openTween.IsActive() && openTween.IsPlaying())
            return;

        float moveY = isOpened ? contentParent.rect.height : -contentParent.rect.height;
        openTween = contentParent.DOMoveY(contentParent.position.y + moveY, 1f)
            .SetEase(isOpened ? Ease.InBack : Ease.OutBack)
            .OnComplete(() => openTween = null);

        isOpened = !isOpened;
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
        for (int i = 0; i < currentQuestData.SubQuests.Count; i++)
        {
            var subQuest = currentQuestData.SubQuests[i];
            subQuestTxts.Add(
                Instantiate(subQuestTxtPrefab.gameObject, contentParent)
                .GetComponent<TextMeshProUGUI>()
                );
            subQuestTxts[i].text = $"({subQuest.SubQuestID}) {subQuest.SubQuestName}";
        }
    }
}
