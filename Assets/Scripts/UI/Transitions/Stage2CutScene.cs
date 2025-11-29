using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stage2CutScene : UIBase
{
    [SerializeField] private Animator cutsceneAnimator;

    //Closeup Type
    private Closeup previousCloseup = Closeup.None;
    private readonly string[] cutScenes = { "CutBardoAnim", "CutPadmaAnim", "CutDoubleAnim", "CutPadmaFlyAnim" };

    [Header("Rain For Cutscene")]
    [SerializeField] private Sprite[] rainSprites;
    [SerializeField] private Image rainSR1;
    [SerializeField] private Image rainSR2;
    private float scrollSpeed = 300f;
    Coroutine RainCoroutine;

    private void Update()
    {
        Closeup currentCloseup = DialogueManager.Instance.closeup;
        if (previousCloseup != currentCloseup)
        {
            SetNewCutscene(currentCloseup);
        }

        //if (Input.GetKeyDown(KeyCode.F)) CutsceneFinished();
        /*if (previousCloseup == Closeup.PadmaFly && currentCloseup == previousCloseup) 
        {
            CutsceneFinished();
            return; 
        }*/
    }

    private void OnDestroy()
    {
        KillRain();
    }

    public void CutsceneFinished()
    {
        StoryManager.Instance.S2_CutsceneFin();
        Destroy(gameObject);
    }

    private void SetNewCutscene(Closeup currentCloseup)
    {
        if (currentCloseup == Closeup.None) return;
        int type = (int)currentCloseup;

        cutsceneAnimator.Play(cutScenes[type]);
        SetRain(type);

        previousCloseup = currentCloseup;
    }

    private void SetRain(int closeType)
    {
        rainSR1.sprite = rainSprites[closeType];
        rainSR2.sprite = rainSprites[closeType];
        Canvas.ForceUpdateCanvases();

        KillRain();
        RainCoroutine = StartCoroutine(RainScroll());
    }

    private IEnumerator RainScroll()
    {
        RectTransform rain1 = rainSR1.rectTransform;
        RectTransform rain2 = rainSR2.rectTransform;

        while (true)
        {
            float height = rain1.rect.height;

            // 한 사이클 동안 내려갈 거리와 시간
            float distance = height;
            float duration = distance / scrollSpeed;

            Vector2 startPos1 = new (rain1.anchoredPosition.x, 0f);
            Vector2 startPos2 = new (rain2.anchoredPosition.x, height);

            rain1.anchoredPosition = startPos1;
            rain2.anchoredPosition = startPos2;
            Debug.Log(rain1.anchoredPosition.y + ", " + rain2.anchoredPosition.y);

            Tween t1 = rain1
                .DOAnchorPosY(startPos1.y - distance, duration)
                .SetEase(Ease.Linear);

            Tween t2 = rain2
                .DOAnchorPosY(startPos2.y - distance, duration)
                .SetEase(Ease.Linear);

            yield return t1.WaitForCompletion();
        }
    }

    private void KillRain()
    {
        DOTween.Kill(rainSR1.rectTransform);
        DOTween.Kill(rainSR2.rectTransform);

        if (RainCoroutine != null)
        {
            StopCoroutine(RainCoroutine);
            RainCoroutine = null;
        }
    }
}
