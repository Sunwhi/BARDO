using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : UIBase
{
    [SerializeField] private Image questBtnImg;
    
    [SerializeField] private RectTransform questParent;
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private RectTransform contentParent;
    [SerializeField] private GameObject subQuestTxtPrefab;
    private List<TextMeshProUGUI> subQuestTxts = new();
    [SerializeField] private Button questListBtn;

    private QuestData currentQuestData;
    Sequence seq = null;
    private const float slideDuration = 1f;
    private const float fadeDuration = 0.25f;

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

    public void SetSlide(bool isOpen)
    {
        if (seq != null) return;

        float targetW = isOpen ? 437f : 0f;
        float fadeTo = isOpen ? 0f : 1f;

        seq = DOTween.Sequence();
        if (isOpen)
        {
            seq.Append(questBtnImg.DOFade(fadeTo, fadeDuration).SetEase(Ease.InOutSine));
            seq.Append(questParent
                .DOSizeDelta(new Vector2(targetW, questParent.sizeDelta.y), slideDuration)
                .SetEase(Ease.InOutBack));
            questListBtn.enabled = true;
        }
        else
        {
            questListBtn.enabled = false;
            seq.Append(questParent
                .DOSizeDelta(new Vector2(targetW, questParent.sizeDelta.y), slideDuration)
                .SetEase(Ease.InOutBack));
            seq.Append(questBtnImg.DOFade(fadeTo, fadeDuration).SetEase(Ease.InOutSine));
        }

        seq.OnComplete(() =>
        {
            seq = null;
        }).OnKill(() => seq = null);
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
        currentQuestData = SaveManager.Instance.MySaveData.currentQuest;

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
            subQuestTxts[i].text = $"{subQuest.SubQuestName}";

            if (subQuest.isCompleted)
            {
                CompleteSubQuest(i);
            }
        }
    }
}