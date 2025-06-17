using Ink.Parsed;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;
using System.Linq;

public class DialoguePanelUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueChoiceBtn[] choiceButtons;
    [SerializeField] private GameObject nextButton;
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
        GameEventManager.Instance.dialogueEvents.onDialogueStarted += DialogueStart;
        GameEventManager.Instance.dialogueEvents.onDialogueFinished += DialogueFinished;
        GameEventManager.Instance.dialogueEvents.onDisplayDialogue += DisplayDialogue;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.dialogueEvents.onDialogueStarted -= DialogueStart;
        GameEventManager.Instance.dialogueEvents.onDialogueFinished -= DialogueFinished;
        GameEventManager.Instance.dialogueEvents.onDisplayDialogue -= DisplayDialogue;
    }

    private void DialogueStart()
    {
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

        // 선택지 dialogue에서는 next 버튼 비활성화
        if(dialogueChoices.Count > 0)
        {
            InActiveNextBtn(); 
        }
        else
        {
            ActiveNextBtn();
        }

        // defensive check - if there are more choices coming in than we can support, Log an error
        if (dialogueChoices.Count > choiceButtons.Length)
        {
            Debug.LogError("More dialogue choices("
                + dialogueChoices.Count + ") came through than are supported ("
                + choiceButtons.Length + ").");
        }

        // start with all of the choice buttons hidden
        foreach (DialogueChoiceBtn choiceBtn in choiceButtons)
        {
            choiceBtn.gameObject.SetActive(false);
        }

        // enable and set info for buttons depending on ink choice information
        int choiceButtonIndex = dialogueChoices.Count - 1;
        for(int inkChoiceIndex = 0; inkChoiceIndex < dialogueChoices.Count; inkChoiceIndex++)
        {
            Ink.Runtime.Choice dialogueChoice = dialogueChoices[inkChoiceIndex];
            DialogueChoiceBtn choiceButton = choiceButtons[choiceButtonIndex];

            choiceButton.gameObject.SetActive(true);
            choiceButton.SetChoiceText(dialogueChoice.text);
            choiceButton.SetChoiceIndex(inkChoiceIndex);

            if(inkChoiceIndex == 0)
            {
                choiceButton.SelectButton();
                GameEventManager.Instance.dialogueEvents.UpdateChoiceIndex(0);
            }

            choiceButtonIndex--;
        }
    }

    private void ActiveNextBtn()
    {
        nextButton.SetActive(true);
    }
    private void InActiveNextBtn()
    {
        nextButton.SetActive(false);
    }
    private void ResetPanel()
    {
        dialogueText.text = "";
    }
}
