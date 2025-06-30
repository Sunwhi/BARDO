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
                directionSprite.DOFade(1f, 1f);
                break;
            case "Move_Off":
                directionSprite.DOFade(0f, 1f);
                break;  
        }
    }
}
