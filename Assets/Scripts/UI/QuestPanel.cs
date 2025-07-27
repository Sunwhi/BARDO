using DG.Tweening;
using TMPro;
using UnityEngine;

public class QuestPanel : UIBase
{
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private RectTransform contentParent;

    private bool isSlided = false;
    private bool isOpened = true;
    private Tween slideTween;
    private Tween openTween;

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
}
