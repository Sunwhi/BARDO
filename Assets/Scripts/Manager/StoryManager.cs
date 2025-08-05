using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class StoryManager : Singleton<StoryManager>
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject Tuto_Move_On;
    [SerializeField] private string dialogueKnotName;
    [SerializeField] Padma padma;
    
    public PlayerController playerController { get; set; }

    public override void Awake()
    {
        base.Awake();
        playerController = new PlayerController(player);
    }
    private void OnEnable()
    {
        GameEventManager.Instance.dialogueEvents.onDialogueFinished += S1_DialogueFinished;
    }
    private void OnDisable()
    {
        if(GameEventManager.Instance != null)
        {
            GameEventManager.Instance.dialogueEvents.onDialogueFinished -= S1_DialogueFinished;
        }
    }
    private IEnumerator Start()
    {
        player.playerInput.enabled = false;

        SoundManager.Instance.PlaySFX(eSFX.Stage_Transition);
        yield return new WaitForSeconds(1f);

        SoundManager.Instance.PlaySFX(eSFX.Opening_Door);
        yield return UIManager.Instance.fadeView.FadeIn();

        SoundManager.Instance.PlayBGM(eBGM.Stage1);
        SoundManager.Instance.PlayAmbientSound(eSFX.Background_Wind);

        yield return S1_PlayerWalkOut();

        //padma.ShowPadma(); // 파드마 페이드 인

        yield return new WaitForSeconds(2f);
        yield return S1_DialogueStart();
    }

    #region Stage1
    private IEnumerator S1_PlayerWalkOut()
    {
        player.ForceMove(new Vector2(1, 0));
        yield return new WaitForSeconds(1f);
        player.ForceMove(Vector2.zero);
    }

    private IEnumerator S1_DialogueStart()
    {
        if (!dialogueKnotName.Equals(""))
        {
            // dialogue 이벤트 호출
            GameEventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
        yield return 1;
    }

    // 파드마와의 대화가 끝나면
    private void S1_DialogueFinished()
    {
        padma.FlyRight(() =>
        {
            // FlyRightPadma의 DoMove가 Complete되면 아래 실행
            Tuto_Move_On.SetActive(true);
            player.playerInput.enabled = true;
            padma.transform.position = new Vector3(210, -285, 0);
            padma.FlipX();
            padma.gameObject.SetActive(false);
        });
    }
    #endregion

    #region Stage2
    public void S2_EnterStage()
    {

    }
    #endregion
    #region Stage3
    #endregion
    #region Stage4
    #endregion
    #region Stage5
    #endregion
}
