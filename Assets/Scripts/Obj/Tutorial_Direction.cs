using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour
{
    private SpriteRenderer directionSprite;

    private void Start()
    {
        directionSprite = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        GameEventBus.Subscribe<TutorialEvent>(Show);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<TutorialEvent>(Show);
    }

    private void Show(TutorialEvent ev)
    {
        switch(ev.tutorialId)
        {
            case "Move_On":
                if (!SaveManager.Instance.MySaveData.tutoDirectionComplete)
                    directionSprite.DOFade(1f, 3f);
                break;
            case "Move_Off":
                QuestManager.Instance.ClearSubQuest(0);
                SaveManager.Instance.SetSaveData(nameof(SaveData.tutoDirectionComplete), true);
                directionSprite.DOFade(0f, 2f);
                break;  
        }
    }
}
