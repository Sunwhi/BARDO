using UnityEngine;
using Ink.Runtime;
using System.Collections;
using UnityEditor.Build.Content;
/*
 * DialogueManager
 * Dialogue Event�� listen�ϰ� �´� ink dialogue�� �����Ѵ�.
 */
public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJson;

    private Story story;
    private bool dialoguePlaying = false;    // dialogue�� playing���ΰ�?


    public override void Awake()
    {
        base.Awake();
        story = new Story(inkJson.text);
    }

    private void OnEnable()
    {
        if (GameEventManager.Instance != null)
        {
            // �̺�Ʈ �߰�
            GameEventManager.Instance.dialogueEvents.onEnterDialogue += EnterDialogue;
            GameEventManager.Instance.inputEvents.onSubmitPressed += SubmitPressed;
        }
    }
    private void OnDisable()
    {
        if (GameEventManager.Instance != null)
        {
            // �̺�Ʈ ����
            GameEventManager.Instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
            GameEventManager.Instance.inputEvents.onSubmitPressed -= SubmitPressed;
        }
    }

    private void SubmitPressed(InputEventContext inputEventContext)
    {
        // if context isn't dialogue, we never want to register input here
        if(!inputEventContext.Equals(InputEventContext.DIALOGUE))
        {
            return;
        }

        ContinueOrExitStory();
    }
    private void EnterDialogue(string knotName)
    {
        // �̹� dialogue�� �� �ִٸ� �� �ٽ� dialogue�� ���� �ʴ´�.
        if (dialoguePlaying)
        {
            return;
        }
        dialoguePlaying = true;

        // inform other parts of our system that we've started diagram
        GameEventManager.Instance.dialogueEvents.DialogueStarted();

        // input event context
        GameEventManager.Instance.inputEvents.ChangeInputEventContext(InputEventContext.DIALOGUE);

        // knotName knot�� jump
        if (!knotName.Equals(""))
        {
            story.ChoosePathString(knotName);
        }
        else
        {
            Debug.LogWarning("Knot name was empty when entering the dialogue");
        }
        // story ����
        ContinueOrExitStory();
    } 

    private void ContinueOrExitStory()
    {
        if (story.canContinue)
        {
            string dialogueLIne = story.Continue();
            GameEventManager.Instance.dialogueEvents.DisplayDialogue(dialogueLIne, story.currentChoices);
        }
        else
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
}
