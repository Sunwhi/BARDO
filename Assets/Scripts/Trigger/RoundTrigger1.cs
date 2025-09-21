using System.Collections;
using UnityEngine;

public class RoundTrigger1 : TriggerBase
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float playerWalkDuration = 3f;


    private void OnEnable()
    {
        GameEventBus.Subscribe<TransitionEvents>(roundTransitionDone);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe<TransitionEvents>(roundTransitionDone);
    }

    protected override void OnTriggered()
    {
        QuestManager.Instance.ClearSubQuest(2);
        StartCoroutine(TriggerEffect());   
    }
    private void roundTransitionDone(TransitionEvents ev)
    {
        StoryManager.Instance.roundTransitionDone = true;
    }
    private IEnumerator TriggerEffect()
    {
        Debug.Log(" 1 : " + ContinueManager.Instance.loadedByContinue);
        UIManager.Show<RoundTransition>(2);

        //TODO : Player & Padma Cutscene
        yield return new WaitUntil(() => StoryManager.Instance.roundTransitionDone);
        UIManager.Show<CutScene>();
        StoryManager.Instance.roundTransitionDone = false;

        StoryManager.Instance.Player.playerInput.enabled = false;
        StoryManager.Instance.Player.transform.position = playerTransform.position;

        //TODO : Dialogue 시작
        StoryManager.Instance.S2_EnterStage();

        yield return null;
    }
}