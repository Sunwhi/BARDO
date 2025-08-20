using System.Collections;
using UnityEngine;

public class RoundCollByStage : MonoBehaviour
{
    [Header("Round Coll Inactive 시점")]
    [SerializeField] private int activeStage = 0;
    [SerializeField] private int activeStory = 0;

    private IEnumerator Start()
    {
        SaveData saveData = SaveManager.Instance.MySaveData;

        yield return new WaitUntil(() =>
        (saveData.stageIdx == activeStage && saveData.storyIdx >= activeStory)
        || saveData.stageIdx > activeStage);

        gameObject.SetActive(false);
    }
}
