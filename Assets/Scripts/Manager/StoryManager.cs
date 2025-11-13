using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class StoryManager : Singleton<StoryManager>
{
    [SerializeField] private VideoController videoController;

    [SerializeField] private Player player;
    [SerializeField] private Padma padma;
    public Player Player => player;
    public Padma Padma => padma;

    [SerializeField] GameObject Tuto_Move_On;
    [SerializeField] private string dialogueKnotName;
    [SerializeField] TextAsset stage1_1InkJson;
    [SerializeField] TextAsset stage2InkJson;
    [SerializeField] TextAsset stage4InkJson;
    [SerializeField] TextAsset elevatorInkJson;

    [SerializeField] private Transform dialogueParent;
    [SerializeField] private Transform transition;
    [SerializeField] private Transform defaultCanvas;

    [Header("Stage1")]
    [SerializeField] private Transform PlayerStopPos;

    public PlayerController playerController { get; set; }
    public bool roundTransitionDone = false; // roundTransition 끝난 후에 dialogue같은 패널 뜨도록 설정
    public bool cutsceneDone = false;

    private enum StoryState { None, Stage1, Stage1_1, Stage2, Stage4 };
    private StoryState currentStoryState = StoryState.None;
    private bool isDialogueDone = false;

    public override void Awake()
    {
        base.Awake();
        playerController = new PlayerController(player);
    }
    private void OnEnable()
    {
        if(videoController != null)
        {
            videoController.OnVideoFinished += HandleVIdeoFinished;
        }
        if(DialogueEventManager.Instance != null)
        {
            DialogueEventManager.Instance.dialogueEvents.onDialogueFinished += OnDialogueFinished;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        if(videoController != null)
        {
            videoController.OnVideoFinished += HandleVIdeoFinished;
        }
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

            if (!SaveManager.Instance.MySaveData.stage1PadmaActive) Destroy(Padma.gameObject);

            videoController.PlayVideo();

            UIManager.Instance.fadeView.FadeOut();
            UIManager.Instance.fadeView.FadeIn();
        }
    }
    
    private IEnumerator MainSceneStart()
    {
        // 새 게임에서 시작할 시에만 스토리 진행
        if (!ContinueManager.Instance.loadedByContinue)
        {
            player.playerInput.enabled = false;

            Tuto_Move_On.SetActive(false);

            //이 코루틴 끝나기 전에 메인화면 나갔을 시, padmaActive false 그리고 SetQuestData.
            //이어하기 시 파드마 없고 quest는 뜬다.
            SaveManager.Instance.SetSaveData(nameof(SaveData.stage1PadmaActive), false); // 다음 이어하기부터 padmaAcive되지 않는다.

            yield return new WaitForSeconds(0.5f);
            UIManager.Show<RoundTransition>(1);
            SoundManager.Instance.PlaySFX(ESFX.Stage_Transition);
            yield return new WaitForSeconds(1f);
           
            //SoundManager.Instance.PlaySFX(ESFX.Opening_Door); 인트로 영상 마지막에 문 소리 난다고 해서 뺌.

            SoundManager.Instance.PlayBGM(EBGM.Stage1);
            SoundManager.Instance.PlayAmbientSound(ESFX.Background_Wind);

            yield return PlayerWalkByPos(PlayerStopPos.position.x);

            yield return new WaitForSeconds(2f);

            QuestManager.Instance.SetQuestData();


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
                isDialogueDone = true;
                break;
            case StoryState.Stage1_1:
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

    public void PlayerWalkTimeCoroutine(float duration = 1f)
    {
        //player.playerInput.enabled = false;
        StartCoroutine(PlayerWalkRight(duration));
        //player.playerInput.enabled = true;
    }

    public IEnumerator PlayerWalkRight(float duration = 1f)
    {
        player.ForceMove(new Vector2(1, 0));
        yield return new WaitForSeconds(duration);
        player.ForceMove(Vector2.zero);
    }

    public IEnumerator PlayerWalkByPos(float xPos, float eps = 0.1f)
    {
        Transform tf = player.transform;

        while (Mathf.Abs(tf.position.x - xPos) > eps)
        {
            float dir = Mathf.Sign(xPos - tf.position.x);
            player.ForceMove(new Vector2(dir, 0f));
            yield return new WaitForFixedUpdate();
        }

        player.ForceMove(Vector2.zero);
    }

    // DialogueParent를 Transition의 자식으로 이동시킨다.  
    private void DialogueToTransition()
    {
        if(dialogueParent != null && transition != null)
        {
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
        bool endFly = false;
        padma.FlyRight(15f, 4f, () =>
        {
            // FlyRightPadma의 DoMove가 Complete되면 아래 실행
            padma.transform.position = new Vector3(210, -285, 0);
            padma.FlipX();
            padma.Hide();
            endFly = true;
        });
        yield return new WaitUntil(() => endFly);
    }

    private void HandleVIdeoFinished()
    {
        Debug.Log("video finished");
        StartCoroutine(MainSceneStart());
    }
    #endregion

    #region Stage2
    public void S2_EnterStage()
    {
        QuestManager.Instance.SetNewQuest();

        currentStoryState = StoryState.Stage2;

        DialogueToTransition();

        DialogueManager.Instance.ChangeDialogueStory(stage2InkJson);
        dialogueKnotName = "Stage2";
        if (!dialogueKnotName.Equals(""))
        {
            DialogueEventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
    }
    public void S2_CutsceneFin()
    {
        Player.playerInput.enabled = true;
        QuestManager.Instance.ShowQuestUI();
    }
    #endregion
    #region Stage3
    #endregion
    #region Stage4
    public void S4_ElevatorIn()
    {
        DialogueToTransition();
        DialogueManager.Instance.ChangeDialogueStory(elevatorInkJson);
        dialogueKnotName = "elevator";
        if (!dialogueKnotName.Equals(""))
        {
            DialogueEventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
    }
    public void S4_EnterStage()
    {
        DialogueToTransition();
        DialogueManager.Instance.ChangeDialogueStory(elevatorInkJson);
        dialogueKnotName = "elevator";
        DialogueEventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
    }
    public IEnumerator S4_1EnterStage()
    {
        currentStoryState = StoryState.Stage4;

        yield return StartCoroutine(UIManager.Instance.fadeView.FadeOut(1f));
        yield return StartCoroutine(UIManager.Instance.fadeView.FadeIn(2f));

        PlayerWalkTimeCoroutine(2);

        player.playerInput.enabled = false;

        QuestManager.Instance.SetNewQuest();
        QuestManager.Instance.ShowQuestUI();

        yield return new WaitForSeconds(3f);

        DialogueManager.Instance.ChangeDialogueStory(stage4InkJson);
        dialogueKnotName = "stage4";
        if (!dialogueKnotName.Equals(""))
        {
            DialogueEventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
    }
    #endregion
    #region Stage5
    #endregion
}
