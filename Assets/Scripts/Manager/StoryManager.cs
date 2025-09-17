using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StoryManager : Singleton<StoryManager>
{
    [SerializeField] private Player player;
    [SerializeField] private Padma padma;
    public Player Player => player;
    public Padma Padma => padma;   
    
    [SerializeField] GameObject Tuto_Move_On;
    [SerializeField] private string dialogueKnotName;
    [SerializeField] TextAsset stage1_1InkJson;
    [SerializeField] TextAsset stage2InkJson;

    [SerializeField] private Transform dialogueParent;
    [SerializeField] private Transform transition;
    [SerializeField] private Transform defaultCanvas;

    public PlayerController playerController { get; set; }
    public bool roundTransitionDone = false; // roundTransition 끝난 후에 dialogue같은 패널 뜨도록 설정
    public bool cutsceneDone = false;

    private enum StoryState { None, Stage1, Stage1_1, Stage2 };
    private StoryState currentStoryState = StoryState.None;
    private bool isDialogueDone = false;

    public override void Awake()
    {
        base.Awake();
        playerController = new PlayerController(player);
    }
    private void OnEnable()
    {
        DialogueEventManager.Instance.dialogueEvents.onDialogueFinished += OnDialogueFinished;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        if(DialogueEventManager.Instance != null)
        {
            DialogueEventManager.Instance.dialogueEvents.onDialogueFinished -= OnDialogueFinished;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MainScene")
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
            padma = GameObject.FindWithTag("Padma").GetComponent<Padma>();
            dialogueKnotName = "Stage1";
            playerController = new PlayerController(player);
            Tuto_Move_On.SetActive(false);
            StartCoroutine(MainSceneStart());
        }
    }
    
    private IEnumerator MainSceneStart()
    {
        // 새 게임에서 시작할 시에만 스토리 진행
        if (!ContinueManager.Instance.loadedByContinue)
        {
            player.playerInput.enabled = false;

            SoundManager.Instance.PlaySFX(ESFX.Stage_Transition);
            yield return new WaitForSeconds(0.5f);
            UIManager.Show<RoundTransition>(1);          
            yield return new WaitForSeconds(1f);
           
            SoundManager.Instance.PlaySFX(ESFX.Opening_Door);

            SoundManager.Instance.PlayBGM(EBGM.Stage1);
            SoundManager.Instance.PlayAmbientSound(ESFX.Background_Wind);

            yield return PlayerWalkLeft();

            yield return new WaitForSeconds(2f);

            S1_DialogueStart();
            yield return new WaitUntil(() => isDialogueDone);
            isDialogueDone = false;

            yield return PadmaFly();

            S1_1DialogueStart();
            yield return new WaitUntil(() => isDialogueDone);
            isDialogueDone = false;

            QuestManager.Instance.ShowQuestUI();
        }

    }
    private void OnDialogueFinished()
    {
        switch (currentStoryState)
        {
            case StoryState.Stage1:
                UnityEngine.Debug.Log("stage1 finished");
                isDialogueDone = true;
                break;
            case StoryState.Stage1_1:
                UnityEngine.Debug.Log("stage1-1 finished");
                Tuto_Move_On.SetActive(true);
                player.playerInput.enabled = true;
                isDialogueDone = true;
                break;
            case StoryState.Stage2:
                DialogueToDefault();
                break;
        }

        currentStoryState = StoryState.None;
    }
    public void PlayerWalkCoroutine(float duration = 1f)
    {
        StartCoroutine(PlayerWalkLeft(duration));
    }

    public IEnumerator PlayerWalkLeft(float duration = 1f)
    {
        player.ForceMove(new Vector2(1, 0));
        yield return new WaitForSeconds(duration);
        player.ForceMove(Vector2.zero);
    }

    // DialogueParent를 Transition의 자식으로 이동시킨다.  
    private void DialogueToTransition()
    {
        if(dialogueParent != null && transition != null)
        {
            UnityEngine.Debug.Log("dialogueToTransition");
            dialogueParent.SetParent(transition, false);
        }
    }
    private void DialogueToDefault()
    {
        if (dialogueParent != null && defaultCanvas != null)
        {
            dialogueParent.SetParent(defaultCanvas, false);
        }
    }

    #region Stage1
    private void S1_DialogueStart()
    {
        currentStoryState = StoryState.Stage1;

        if (!dialogueKnotName.Equals(""))
        {
            // dialogue 이벤트 호출
            DialogueEventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
    }
    private void S1_1DialogueStart()
    {
        currentStoryState = StoryState.Stage1_1;

        DialogueManager.Instance.ChangeDialogueStory(stage1_1InkJson);
        dialogueKnotName = "Stage1_1";
        if (!dialogueKnotName.Equals(""))
        {
            // dialogue 이벤트 호출
            DialogueEventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
    }
    private IEnumerator PadmaFly()
    {
        padma.FlyRight(15f, 4f, () =>
        {
            // FlyRightPadma의 DoMove가 Complete되면 아래 실행
            padma.transform.position = new Vector3(210, -285, 0);
            padma.FlipX();
            padma.Hide();
        });
        yield return 1;
    }    
    #endregion

    #region Stage2
    public void S2_EnterStage()
    {
        currentStoryState = StoryState.Stage2;

        DialogueToTransition();

        DialogueManager.Instance.ChangeDialogueStory(stage2InkJson);
        dialogueKnotName = "Stage2";
        if (!dialogueKnotName.Equals(""))
        {
            DialogueEventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }

    }
    #endregion
    #region Stage3
    #endregion
    #region Stage4
    #endregion
    #region Stage5
    #endregion
}
