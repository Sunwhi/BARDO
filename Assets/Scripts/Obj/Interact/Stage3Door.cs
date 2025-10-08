using System.Collections;
using UnityEngine;

public class Stage3Door : InteractEnter
{
    protected override IEnumerator InteractCoroutine()
    {
        yield return base.InteractCoroutine();

        if (stageIdx == SaveManager.Instance.MySaveData.stageIdx && stageIdx == SaveManager.Instance.MySaveData.storyIdx)
        {
            UIManager.Show<CardPanel>();
        }
    }
}