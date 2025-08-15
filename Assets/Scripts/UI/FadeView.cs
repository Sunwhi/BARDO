using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Fadeview : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    
    public IEnumerator FillWhite()
    {
        canvasGroup.alpha = 1;
        return null;
    }
    public IEnumerator FadeOut(float duration = 1f)
    { 
        canvasGroup.DOFade(1f, duration);
        yield return new WaitForSeconds(duration);
    }
    
    public IEnumerator FadeIn(float duration = 1f)
    {
        canvasGroup.DOFade(0f, duration);
        yield return new WaitForSeconds(duration);
    }
}