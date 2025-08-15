using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class RoundTrigger1 : TriggerBase
{
    [SerializeField] private Padma padma;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float playerWalkDuration = 3f;

    protected override void OnTriggered()
    {
        StartCoroutine(TriggerEffect());   
    }

    private IEnumerator TriggerEffect()
    {
        yield return UIManager.Instance.fadeView.FillWhite();
        StoryManager.Instance.Player.playerInput.enabled = false;
        StoryManager.Instance.Player.transform.position = playerTransform.position;
        yield return UIManager.Instance.fadeView.FadeIn(2f);

        //yield return StoryManager.Instance.PlayerWalkLeft();
        StoryManager.Instance.Padma.Show();

        //TODO : Dialogue 시작
        StoryManager.Instance.S2_EnterStage();

        //TODO : Player & Padma Cutscene
        //FocusOnPlayer();
        //yield return new WaitForSeconds(2f);
        //FocusOnPadma();
        //yield return new WaitForSeconds(2f);

        //ResetCamera();
        yield return new WaitForSeconds(1f);

        //TODO : Padma 퇴장
        yield return new WaitUntil(() => !DialogueManager.Instance.dialoguePlaying);
        padma.FlyRight(20, 4, () =>
        {
            padma.Hide();
            QuestManager.Instance.ShowQuestUI();
            StoryManager.Instance.Player.playerInput.enabled = true;
        });

        //yield return StartCoroutine(StoryManager.Instance.PlayerWalkLeft(playerWalkDuration));
        yield return null;
    }
    /*public void FocusOnPlayer()
    {
        playerCam.Priority = 20;
        padmaCam.Priority = 10;
    }
    public void FocusOnPadma()
    {
        padmaCam.Priority = 20;
        playerCam.Priority = 10;
    }
    public void ResetCamera()
    {
        playerCam.Priority = 0;
        padmaCam.Priority = 0;
        playerCam.gameObject.SetActive(false);
        padmaCam.gameObject.SetActive(false);
        //mainCam.Priority = 20;
    }*/
}