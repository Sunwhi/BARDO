using UnityEngine;
using DG.Tweening;
using System;
public class Padma : MonoBehaviour
{
    [SerializeField] private SpriteRenderer padmaSprite;

    private void Awake()
    {
        if (padmaSprite == null)
            padmaSprite = GetComponent<SpriteRenderer>();
    }

    // 파드마 페이드 인
    public void Show()
    {
        padmaSprite.DOFade(1f, 2f);
    }
    // 파드마 페이드 아웃
    public void Hide(Action onComplete = null)
    {
        padmaSprite.DOFade(0f, 2f) // 2초 동안 투명하게
          .OnComplete(() =>
          {
              onComplete?.Invoke(); // 다 끝나고 나면 콜백 실행
          });
    }

    public void FlipX()
    {
        Vector3 scale = transform.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;
    }

    /// <summary>
    /// 파드마 오른쪽으로 날아감.
    /// </summary>
    public void FlyRight(float distance, float flyTime, Action onComplete = null)
    {
        FlipX();
        Vector3 targetPos = transform.position + Vector3.right * distance;
        transform.DOMove(targetPos, flyTime)
            //.SetEase(Ease.Linear)
            .OnComplete(() => { onComplete?.Invoke(); });
    }
}