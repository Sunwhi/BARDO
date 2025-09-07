using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fadeview : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image img;
    
    public IEnumerator FadeOut(float duration = 1f)
    {
        StoryManager.Instance.Player.playerInput.enabled = false;
        canvasGroup.blocksRaycasts = true;
        UIManager.EscDelay = true;

        canvasGroup.DOFade(1f, duration);
        yield return new WaitForSeconds(duration);
    }
    
    public IEnumerator FadeIn(float duration = 1f)
    {
        UIManager.Instance.HideAllPanels();
        canvasGroup.DOFade(0f, duration);
        yield return new WaitForSeconds(duration);
        StoryManager.Instance.Player.playerInput.enabled = true;
        canvasGroup.blocksRaycasts = false;
        UIManager.EscDelay = false;
    }
}