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
    // �ĵ帶 ���̵� ��
    public void ShowPadma()
    {
        padmaSprite.DOFade(1f, 2f);
    }
    // �ĵ帶 ���̵� �ƿ�
    public void HidePadma(Action onComplete = null)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.DOFade(0f, 2f) // 2�� ���� �����ϰ�
          .OnComplete(() =>
          {
              onComplete?.Invoke(); // �� ������ ���� �ݹ� ����
          });
    }
    // �ĵ帶 ���������� ����.
    public void FlyRightPadma(Action onComplete = null)
    {
        this.transform.localScale = new Vector3(0.4f, 0.4f, 1);
        this.transform.DOMove(targetPos, 3f)
            .SetEase(Ease.Linear)
            .OnComplete(() => { onComplete?.Invoke(); });
    }
}
