using System.Collections;
using System.Diagnostics;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class StoryManager : Singleton<StoryManager>
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject Tuto_Move_On;
    [SerializeField] private string dialogueKnotName;
    [SerializeField] GameObject padma;
    private float alpha;
    public PlayerController playerController { get; set; }

    public override void Awake()
    {
        base.Awake();
        playerController = new PlayerController(player);
    }
    private void OnEnable()
    {
        GameEventManager.Instance.dialogueEvents.onDialogueFinished += DialogueFinished;
    }
    private void OnDisable()
    {
        if(GameEventManager.Instance != null)
        {
            GameEventManager.Instance.dialogueEvents.onDialogueFinished -= DialogueFinished;
        }
    }
    private IEnumerator Start()
    {
        PlayerMovementDisable();

        SoundManager.Instance.PlaySFX(eSFX.Stage_Transition);
        yield return new WaitForSeconds(1f);

        SoundManager.Instance.PlaySFX(eSFX.Opening_Door);
        yield return UIManager.Instance.fadeView.FadeIn();

        SoundManager.Instance.PlayBGM(eBGM.Stage1);
        SoundManager.Instance.PlayAmbientSound(eSFX.Background_Wind);

        yield return PlayerWalkOut();

        //padma.GetComponent<Padma>().ShowPadma(); // 파드마 페이드 인

        yield return new WaitForSeconds(2f);

        yield return DialogueStart();
    }

    private IEnumerator PlayerWalkOut()
    {
        player.ForceMove(new Vector2(1, 0));
        yield return new WaitForSeconds(1f);
        player.ForceMove(Vector2.zero);
    }

    private IEnumerator DialogueStart()
    {
        if (!dialogueKnotName.Equals(""))
        {
            // dialogue 이벤트 호출
            GameEventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
        yield return 1;
    }

    // 플레이어 움직이지 못하게
    private void PlayerMovementDisable()
    {
        player.GetComponent<PlayerInput>().enabled = false;
    }
    // 파드마와의 대화가 끝나면
    private void DialogueFinished()
    {
        // 파드마가 오른쪽으로 날아간다.
        padma.GetComponent<Padma>().FlyRightPadma(() =>
        {
            // FlyRightPadma의 DoMove가 Complete되면 아래 실행
            Tuto_Move_On.SetActive(true);
            player.GetComponent<PlayerInput>().enabled = true;
            padma.SetActive(false);
        });
    }

}
