using DG.Tweening;
using System.Collections;
using UnityEngine;

public class InteractEnter : MonoBehaviour
{
    [SerializeField] private CanvasGroup guide;

    [Header("interact 가능한 시점")]
    [SerializeField] protected int stageIdx;
    [SerializeField] protected int storyIdx;

    Coroutine interactCoroutine;

    private void Awake()
    {
        guide.alpha = 0;
    }

    protected virtual IEnumerator InteractCoroutine()
    {
        SaveData saveData = SaveManager.Instance.MySaveData;
        yield return new WaitUntil(() =>
    Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter));
        guide.DOFade(0f, 0.5f).SetUpdate(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SaveManager.Instance.MySaveData.stageIdx == stageIdx
            && SaveManager.Instance.MySaveData.storyIdx == storyIdx)
        {
            guide.DOFade(1f, 0.5f).SetUpdate(true);
            interactCoroutine ??= StartCoroutine(InteractCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        guide.DOFade(0f, 0.5f).SetUpdate(true);
        if (interactCoroutine != null)
        {
            StopCoroutine(interactCoroutine);
            interactCoroutine = null;
        }
    }
}
