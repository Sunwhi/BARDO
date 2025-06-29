using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialGuide : MonoBehaviour
{
    public Image image;

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
            case "Move_On":
                image.DOFade(1f, 1f);
                break;
            case "Move_Off":
                image.DOFade(0f, 1f);
                break;
        }
    }
}
