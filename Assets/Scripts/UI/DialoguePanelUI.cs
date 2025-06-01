using Ink.Parsed;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;

public class DialoguePanelUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueChoiceBtn[] choiceButtons;
    private void Start()
    {
        if(UIManager.Instance != null)
        {
            UIManager.Instance.HidePanel("DialoguePanel");
        }
        ResetPanel();
    }

    private void OnEnable()
    {
        Debug.Log($"GameEventManager.Instance is null? {GameEventManager.Instance == null}");
        Debug.Log($"dialogueEvents is null? {GameEventManager.Instance?.dialogueEvents == null}");

        GameEventManager.Instance.dialogueEvents.onDialogueStarted += DialogueStart;
        GameEventManager.Instance.dialogueEvents.onDialogueFinished += DialogueFinished;
        GameEventManager.Instance.dialogueEvents.onDisplayDialogue += DisplayDialogue;
    }

    private void OnDisable()
    {
        Debug.Log($"GameEventManager.Instance is null? {GameEventManager.Instance == null}");
        Debug.Log($"dialogueEvents is null? {GameEventManager.Instance?.dialogueEvents == null}");

        GameEventManager.Instance.dialogueEvents.onDialogueStarted -= DialogueStart;
        GameEventManager.Instance.dialogueEvents.onDialogueFinished -= DialogueFinished;
        GameEventManager.Instance.dialogueEvents.onDisplayDialogue -= DisplayDialogue;
    }

    private void DialogueStart()
    {
        Debug.Log("startpanel");
        UIManager.Instance.ShowPanel("DialoguePanel");
    }

    private void DialogueFinished()
    {
        UIManager.Instance.HidePanel("DialoguePanel");
        // reset anything for next time
        ResetPanel();
    }

    private void DisplayDialogue(string dialogueLine, List<Ink.Runtime.Choice> dialogueChoices)
    {
        dialogueText.text = dialogueLine;
    }

    private void ResetPanel()
    {
        dialogueText.text = "";
    }
}
