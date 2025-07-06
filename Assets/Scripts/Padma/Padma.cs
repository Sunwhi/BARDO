using UnityEngine;
using DG.Tweening;
using System;
public class Padma : MonoBehaviour
{
    private SpriteRenderer padmaSprite;
    private Vector2 targetPos = new Vector2(-35, 0.43f);
    private void Start()
    {
        padmaSprite = GetComponent<SpriteRenderer>();
        if (padmaSprite == null) Debug.Log("warning~!!!!!!!!!!!!!!!!!!!");
    }
    // 파드마 페이드 인
    public void ShowPadma()
    {
        padmaSprite.DOFade(1f, 2f);
    }
    // 파드마 페이드 아웃
    public void HidePadma(Action onComplete = null)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.DOFade(0f, 2f) // 2초 동안 투명하게
          .OnComplete(() =>
          {
              onComplete?.Invoke(); // 다 끝나고 나면 콜백 실행
          });
    }
    // 파드마 오른쪽으로 날라감.
    public void FlyRightPadma(Action onComplete = null)
    {
        this.transform.localScale = new Vector3(0.4f, 0.4f, 1);
        this.transform.DOMove(targetPos, 3f)
            .SetEase(Ease.Linear)
            .OnComplete(() => { onComplete?.Invoke(); });
    }
}
