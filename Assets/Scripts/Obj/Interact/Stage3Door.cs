using System.Collections;
using UnityEngine;

public class Stage3Door : InteractEnter
{
    [SerializeField] private Transform stage4PlayerPos;

    protected override IEnumerator InteractCoroutine()
    {
        yield return base.InteractCoroutine();

        if (stageIdx == SaveManager.Instance.MySaveData.stageIdx && stageIdx == SaveManager.Instance.MySaveData.storyIdx)
        {
            UIManager.Show<CardPanel>(stage4PlayerPos.position);
        }
    }
}