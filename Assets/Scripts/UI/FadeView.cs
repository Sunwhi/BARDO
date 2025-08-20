using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Fadeview : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image img;
    public IEnumerator imgAlpha1()
    {
        img.color = new Color(1f, 1f, 1f, 1f);
        return null;
    }
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