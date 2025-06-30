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
                spaceBar.DOFade(1f, 1f);
                break;
            case "Jump_Off":
                spaceBar.DOFade(0f, 1f);
                break;
        }
    }
}
