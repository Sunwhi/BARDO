using System.Collections;
using UnityEngine;

public class RoundTrigger1 : TriggerBase
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float playerWalkDuration = 3f;

    protected override void OnTriggered()
    {
        StartCoroutine(TriggerEffect());   
    }

    private IEnumerator TriggerEffect()
    {
        Debug.Log(" 1 : " + ContinueManager.Instance.loadedByContinue);
        UIManager.Show<RoundTransition>(2);

        //TODO : Player & Padma Cutscene
        UIManager.Show<CutScene>();

        StoryManager.Instance.Player.playerInput.enabled = false;
        StoryManager.Instance.Player.transform.position = playerTransform.position;

        //TODO : Dialogue 시작
        StoryManager.Instance.S2_EnterStage();


        yield return null;
    }
}