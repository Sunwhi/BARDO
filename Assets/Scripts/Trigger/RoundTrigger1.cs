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
        Debug.Log(" 1 : " + ContinueManager.Instance.loadedByContinue);

        //TODO : Player & Padma Cutscene
        cutscene.SetActive(true);

        StoryManager.Instance.Player.playerInput.enabled = false;
        StoryManager.Instance.Player.transform.position = playerTransform.position;

        // Continue일때는 continue fadein효과 한번만.
        if (!ContinueManager.Instance.loadedByContinue)
        {
            yield return UIManager.Instance.fadeView.FadeIn(3f);

        }
        //TODO : Dialogue 시작
        StoryManager.Instance.S2_EnterStage();


        yield return null;
    }
}