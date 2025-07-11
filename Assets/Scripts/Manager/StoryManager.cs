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

        //padma.GetComponent<Padma>().ShowPadma(); // �ĵ帶 ���̵� ��

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
            // dialogue �̺�Ʈ ȣ��
            GameEventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
        yield return 1;
    }

    // �÷��̾� �������� ���ϰ�
    private void PlayerMovementDisable()
    {
        player.GetComponent<PlayerInput>().enabled = false;
    }
    // �ĵ帶���� ��ȭ�� ������
    private void DialogueFinished()
    {
        // �ĵ帶�� ���������� ���ư���.
        padma.GetComponent<Padma>().FlyRightPadma(() =>
        {
            // FlyRightPadma�� DoMove�� Complete�Ǹ� �Ʒ� ����
            Tuto_Move_On.SetActive(true);
            player.GetComponent<PlayerInput>().enabled = true;
            padma.SetActive(false);
        });
    }

}
