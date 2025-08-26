using System.Collections;
using UnityEngine;

public class Stage3Door : InteractEnter
{
    //{stage , story}
    private readonly int[] validStoryIndices = { 3, 4 };

    protected override IEnumerator InteractCoroutine()
    {
        yield return base.InteractCoroutine();

        if (stageIdx == validStoryIndices[0] && stageIdx == validStoryIndices[1])
        {
            UIManager.Show<CardPanel>();
        }

        yield return null;
    }
}
