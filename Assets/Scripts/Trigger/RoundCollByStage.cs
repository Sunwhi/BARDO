using System.Collections;
using UnityEngine;

public class RoundCollByStage : MonoBehaviour
{
    [Header("Round Coll Inactive 시점")]
    [SerializeField] private int activeStage = 0;
    [SerializeField] private int activeStory = 0;

    [SerializeField] private bool isStageEffectActive = false;
    
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
        (SaveManager.Instance.MySaveData.stageIdx == activeStage && SaveManager.Instance.MySaveData.storyIdx >= activeStory)
        || SaveManager.Instance.MySaveData.stageIdx > activeStage);

        gameObject.SetActive(false);
    }

    private void OnNextStage(NextStageEvent e)
    {
        if (isStageEffectActive)
        {
            switch (activeStage)
            {
                case 3:
                    switch (activeStory)
                    {
                        case 0:
                            CameraManager.Instance.JumpAndCut(CamState.v3_1);
                            break;
                    }
                    break;
            }
            
            Vector3 pos = e.playerTransform;
            StoryManager.Instance.Player.transform.position = pos;
        }
    }
}
