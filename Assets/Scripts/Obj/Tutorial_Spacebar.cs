using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Tutorial_Spacebar : MonoBehaviour
{
    private SpriteRenderer spaceBar;

    private void Start()
    {
        spaceBar = GetComponent<SpriteRenderer>();
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
        switch (ev.tutorialId)
        {
            case "Jump_On":
                if(!SaveManager.Instance.MySaveData.tutoJumpComplete)
                    spaceBar.DOFade(1f, 2f);
                break;
            case "Jump_Off":
                QuestManager.Instance.ClearSubQuest(1);
                SaveManager.Instance.SetSaveData(nameof(SaveData.tutoJumpComplete), true);
                spaceBar.DOFade(0f, 2f);
                break;
        }
    }
}
