using System.Collections;
using UnityEngine;

public class RoundCollByStage : MonoBehaviour
{
    [Header("Round Coll Inactive 시점")]
    [SerializeField] private int activeStage = 0;
    [SerializeField] private int activeStory = 0;

    [SerializeField] private bool isStageEffectActive = false;
    [SerializeField] private float playerWalkDuration = 2f; 

    private void OnEnable()
    {
        GameEventBus.Subscribe<NextStageEvent>(OnNextStage);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<NextStageEvent>(OnNextStage);
    }

    private IEnumerator Start()
    {
        SaveData saveData = SaveManager.Instance.MySaveData;

        yield return new WaitUntil(() =>
        (saveData.stageIdx == activeStage && saveData.storyIdx >= activeStory)
        || saveData.stageIdx > activeStage);

        gameObject.SetActive(false);
    }

    private void OnNextStage(NextStageEvent e)
    {
        if (isStageEffectActive)
        {
            CameraManager.Instance.JumpAndCut(CamState.v3_1);
            Vector3 pos = e.playerTransform;

            StoryManager.Instance.Player.transform.position = pos;
            StoryManager.Instance.PlayerWalkCoroutine(playerWalkDuration);
        }
    }
}
