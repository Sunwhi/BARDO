using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class RoundTrigger1 : TriggerBase
{
    //[SerializeField] private Padma padma;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float playerWalkDuration = 3f;
    [SerializeField] private GameObject cutscene;

    protected override void OnTriggered()
    {
        StartCoroutine(TriggerEffect());   
    }

    private IEnumerator TriggerEffect()
    {
        //TODO : Player & Padma Cutscene
        cutscene.SetActive(true);

        yield return UIManager.Instance.fadeView.FillWhite();
        StoryManager.Instance.Player.playerInput.enabled = false;
        StoryManager.Instance.Player.transform.position = playerTransform.position;
        yield return UIManager.Instance.fadeView.FadeIn(3f);

        //yield return StoryManager.Instance.PlayerWalkLeft();
        StoryManager.Instance.Padma.Show();

        //TODO : Dialogue 시작
        StoryManager.Instance.S2_EnterStage();


        //TODO : Padma 퇴장
        yield return new WaitUntil(() => !DialogueManager.Instance.dialoguePlaying);
        /*padma.FlyRight(20, 4, () =>
        {
            padma.Hide();
            QuestManager.Instance.ShowQuestUI();
            StoryManager.Instance.Player.playerInput.enabled = true;
        });*/
        //yield return StartCoroutine(StoryManager.Instance.PlayerWalkLeft(playerWalkDuration));
        yield return null;
    }
}