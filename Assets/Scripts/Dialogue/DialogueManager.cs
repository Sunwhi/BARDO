using UnityEngine;
using Ink.Runtime;
using System.Collections;
using UnityEditor.Build.Content;
/*
 * DialogueManager
 * Dialogue Event를 listen하고 맞는 ink dialogue를 실행한다.
 */
public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJson;

    private Story story;

    public bool haveChoices = false;

    private int currentChoiceIndex = -1;

    private bool dialoguePlaying = false;    // dialogue가 playing중인가?

    private void Update()
    {
        
    }
    public override void Awake()
    {
        base.Awake();
        story = new Story(inkJson.text);
    }

    private void OnEnable()
    {
        if (GameEventManager.Instance != null)
        {
            // 이벤트 추가
            GameEventManager.Instance.dialogueEvents.onEnterDialogue += EnterDialogue;
            GameEventManager.Instance.inputEvents.onSubmitPressed += SubmitPressed;
            GameEventManager.Instance.dialogueEvents.onUpdateChoiceIndex += UpdateChoiceIndex;
        }
    }
    private void OnDisable()
    {
        if (GameEventManager.Instance != null)
        {
            // 이벤트 삭제
            GameEventManager.Instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
            GameEventManager.Instance.inputEvents.onSubmitPressed -= SubmitPressed;
            GameEventManager.Instance.dialogueEvents.onUpdateChoiceIndex -= UpdateChoiceIndex;

        }
    }

    private void UpdateChoiceIndex(int choiceIndex)
    {
        this.currentChoiceIndex = choiceIndex;
    }
    private void SubmitPressed(InputEventContext inputEventContext)
    {
        Debug.Log("submitpressed");
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
        GameEventManager.Instance.dialogueEvents.DialogueStarted();

        // input event context
        GameEventManager.Instance.inputEvents.ChangeInputEventContext(InputEventContext.DIALOGUE);

        // knotName knot로 jump
        if (!knotName.Equals(""))
        {
            story.ChoosePathString(knotName);
        }
        else
        {
            Debug.LogWarning("Knot name was empty when entering the dialogue");
        }
        // story 시작
        ContinueOrExitStory();
    } 

    private void ContinueOrExitStory()
    {
        Debug.Log("continueorexit");
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
                GameEventManager.Instance.dialogueEvents.DisplayDialogue(dialogueLIne, story.currentChoices);
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
        GameEventManager.Instance.dialogueEvents.DialogueFinished();

        // input event context
        GameEventManager.Instance.inputEvents.ChangeInputEventContext(InputEventContext.DEFAULT);

        // reset story state
        story.ResetState();
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
}
