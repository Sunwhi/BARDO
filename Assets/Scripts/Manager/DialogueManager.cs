using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;

public enum Speaker
{
    Bardo,
    Padma,
    Unknown
}
public enum Closeup
{
    None,
    Bardo,
    Padma,
    Double,
    PadmaFly
}
/*
 * DialogueManager
 * Dialogue Event를 listen하고 맞는 ink dialogue를 실행한다.
 */
public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Ink Story")]
    [SerializeField] public TextAsset stage1InkJson;
    //public DialoguePanelUI dialoguePanelUI;

    public Story story;

    public bool haveChoices = false;

    public bool dialoguePaused = false;

    private int currentChoiceIndex = -1;

    public Speaker speaker;
    public Closeup closeup;
    private const string SPEAKER_TAG = "speaker";
    private const string CLOSEUP_TAG = "closeup"; 



    public bool dialoguePlaying { get; private set; } = false;    // dialogue가 playing중인가?

    // Dialogue 다음 라인으로 넘어갈 수 있는가? 타이핑 도중 next로 넘어가지 못하게
    public bool canContinueToNextLine { get; set; } = false;

    public bool panelClickForSkip = false;
    public override void Awake()
    {
        base.Awake();
        story = new Story(stage1InkJson.text);
    }

    private void OnEnable()
    {
        if (DialogueEventManager.Instance != null)
        {
            // 이벤트 추가
            DialogueEventManager.Instance.dialogueEvents.onEnterDialogue += EnterDialogue;
            DialogueEventManager.Instance.inputEvents.onStartDialogue += StartDialogue;
            DialogueEventManager.Instance.dialogueEvents.onUpdateChoiceIndex += UpdateChoiceIndex;
            GameEventBus.Subscribe<PauseGameEvent>(OnPauseGame);
        }
    }
    private void OnDisable()
    {
        if (DialogueEventManager.Instance != null)
        {
            // 이벤트 삭제
            DialogueEventManager.Instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
            DialogueEventManager.Instance.inputEvents.onStartDialogue -= StartDialogue;
            DialogueEventManager.Instance.dialogueEvents.onUpdateChoiceIndex -= UpdateChoiceIndex;
            GameEventBus.Unsubscribe<PauseGameEvent>(OnPauseGame);
        }
    }

    private void UpdateChoiceIndex(int choiceIndex)
    {
        this.currentChoiceIndex = choiceIndex;
    }
    private void StartDialogue(InputEventContext inputEventContext)
    {
        haveChoices = false;

        // if context isn't dialogue, we never want to register input here
        if(!inputEventContext.Equals(InputEventContext.DIALOGUE))
        {
            return;
        }

        ContinueOrExitStory();
    }
    private void EnterDialogue(string knotName)
    {
        
        // 이미 dialogue에 들어가 있다면 또 다시 dialogue에 들어가지 않는다.
        if (dialoguePlaying)
        {
            return;
        }
        dialoguePlaying = true;

        // inform other parts of our system that we've started diagram
        DialogueEventManager.Instance.dialogueEvents.DialogueStarted();

        // input event context
        DialogueEventManager.Instance.inputEvents.ChangeInputEventContext(InputEventContext.DIALOGUE);

        // knotName knot로 jump
        if (!knotName.Equals(""))
        {
            story.ChoosePathString(knotName);
        }
        else
        {
            Debug.LogWarning("Knot name was empty when entering the dialogue");
        }
        Debug.Log("enterdialogue");

        // story 시작
        ContinueOrExitStory();
    } 

    private void ContinueOrExitStory() 
    {
        // make a choice, if applicable
        if (story.currentChoices.Count > 0 && currentChoiceIndex != -1)
        {
            story.ChooseChoiceIndex(currentChoiceIndex);
            // reset choice index for next time
            currentChoiceIndex = -1;
        }
        if (story.canContinue)
        {
            string dialogueLIne = story.Continue();

            // handle tags
            HandleTags(story.currentTags);

            // handle the case where there's an empty line of dialogue
            // by continuing until we get a line with content
            while(IsLineBlank(dialogueLIne) && story.canContinue)
            {
                dialogueLIne = story.Continue();
            }
            // handle the case where the last line of dialogue is blank
            // (empty choice, external function, etc..)
            if(IsLineBlank(dialogueLIne) && !story.canContinue)
            {
                ExitDialogue();
            }
            else
            {
                haveChoices = story.currentChoices.Count > 0;
                DialogueEventManager.Instance.dialogueEvents.DisplayDialogue(dialogueLIne, story.currentChoices);
            }
        }
        else if (story.currentChoices.Count == 0)
        {
            ExitDialogue();
        }
    }
    private void ExitDialogue()
    {
        Debug.Log("Exiting Dialogue");

        dialoguePlaying = false;

        // inform other parts of our system that we've finished dialogue
        DialogueEventManager.Instance.dialogueEvents.DialogueFinished();

        // input event context
        DialogueEventManager.Instance.inputEvents.ChangeInputEventContext(InputEventContext.DEFAULT);

        // reset story state
        story.ResetState();
    }

    // ink dialogue내 tag들을 처리한다. speaker 관련
    private void HandleTags(List<string> currentTags)
    {
        foreach(string tag in currentTags)
        {
            string[] splitTag = tag.Split(":");
            if(splitTag.Length != 2 )
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0];
            string tagValue = splitTag[1];

            switch(tagKey)
            {
                case SPEAKER_TAG:
                    if (tagValue == "Bardo" || tagValue == "b") speaker = Speaker.Bardo;
                    else if (tagValue == "Padma" || tagValue == "p") speaker = Speaker.Padma;
                    else if (tagValue == "?") speaker = Speaker.Unknown;
                    break;
                case CLOSEUP_TAG:
                    if (tagValue == "b") closeup = Closeup.Bardo;
                    else if (tagValue == "p") closeup = Closeup.Padma;
                    else if (tagValue == "b_p") closeup = Closeup.Double;
                    else if (tagValue == "pf") closeup = Closeup.PadmaFly;
                    break;
            }
        }
    
    }
    private bool IsLineBlank(string dialogueLine)
    {
        return dialogueLine.Trim().Equals("") || dialogueLine.Trim().Equals("\n");
    }

    // 현재 문장이 선택지가 있는 문장인가?
    public bool ContainChoices()
    {
        if (story.currentChoices.Count != 0) return true;
        else return false;
    }

    public void ChangeDialogueStory(TextAsset inkJson)
    {
        story = new Story(inkJson.text);
    }

    private void OnPauseGame(PauseGameEvent ev)
    {
        if (ev.State == GameState.pause) dialoguePaused = true;
        else if(ev.State == GameState.resume) dialoguePaused = false;
    }
}
