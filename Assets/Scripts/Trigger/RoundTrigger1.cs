using System.Collections;
using UnityEngine;

public class RoundTrigger1 : TriggerBase
{
    [SerializeField] private Transform playerTransform;


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
        SoundManager.Instance.PlaySFX(ESFX.Stage_Transition);

        SaveData curData = SaveManager.Instance.MySaveData;
        if (curData.stageIdx > 1) return; //1스테이지에서 넘어올 때만 round trigger 1 발동.

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