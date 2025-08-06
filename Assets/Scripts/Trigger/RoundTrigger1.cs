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
        StoryManager.Instance.Player.playerInput.enabled = false;
        StoryManager.Instance.Player.transform.position = playerTransform.position;
        StoryManager.Instance.Padma.Show();

        //TODO : Dialogue 시작
        //TODO : Player & Padma Cutscene
        //TODO : Padma 퇴장

        QuestManager.Instance.ShowQuestUI();

        yield return StartCoroutine(StoryManager.Instance.PlayerWalkLeft(playerWalkDuration));

        StoryManager.Instance.Player.playerInput.enabled = true;
    }
}