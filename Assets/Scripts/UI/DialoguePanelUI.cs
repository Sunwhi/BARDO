using Ink.Parsed;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections;

public class DialoguePanelUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueChoiceBtn[] choiceButtons;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private float typingSpeed = 0.04f;
    private Coroutine displayLineCoroutine;
    private bool skipDialogue = false;
    private void Start()
    {
        if(UIManager.Instance != null)
        {
            UIManager.Instance.HidePanel("DialoguePanel");
        }
        ResetPanel();
    }
    private void Update()
    {
        Debug.Log("GetSubmitPressed "+UIInputManager.Instance.GetSubmitPressed());
        // ��� ���� ��µ��� �ʾ��� ��, space�� ���� dialogue�� skip�Ѵ�.
        if (UIInputManager.Instance.GetSubmitPressed() && !DialogueManager.Instance.canContinueToNextLine)
        {
            // �̰� ���� ������ �ȵǰ� �ִ�. �� ������ GetSubmitPressed()�� true�� �ȵǰ� ����*************************************************** why??
            UIInputManager.Instance.submitPressed = false; // �����̽��� ���������� false�ǰ�. �̷��� ���ϰ� GetSubmitPressed���� false�ϸ� ����ؼ� false�� ��µż� �۵��� ����. true�� �ȳ���.
            skipDialogue = true;
        }
    }
    private void OnEnable()
    {
        GameEventManager.Instance.dialogueEvents.onDialogueStarted += DialogueStart;
        GameEventManager.Instance.dialogueEvents.onDialogueFinished += DialogueFinished;
        GameEventManager.Instance.dialogueEvents.onDisplayDialogue += DisplayDialogue;
    }

    private void OnDisable()
    {
        if(GameEventManager.Instance != null)
        {
            GameEventManager.Instance.dialogueEvents.onDialogueStarted -= DialogueStart;
            GameEventManager.Instance.dialogueEvents.onDialogueFinished -= DialogueFinished;
            GameEventManager.Instance.dialogueEvents.onDisplayDialogue -= DisplayDialogue;
        }
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
        //ActiveNextBtn();
        if(displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
        }


        // ������ dialogue������ next ��ư ��Ȱ��ȭ
        if(dialogueChoices.Count > 0)
        {
            InActiveNextBtn(); 
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

            //choiceButton.gameObject.SetActive(true);
            choiceButton.SetChoiceText(dialogueChoice.text);
            choiceButton.SetChoiceIndex(inkChoiceIndex);

            if(inkChoiceIndex == 0)
            {
                choiceButton.SelectButton();
                GameEventManager.Instance.dialogueEvents.UpdateChoiceIndex(0);
            }

            choiceButtonIndex--;
        }

        displayLineCoroutine = StartCoroutine(DisplayLine(dialogueLine));
    }
    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";

        yield return new WaitForSeconds(0.1f);
        DialogueManager.Instance.canContinueToNextLine = false;

        // ��ȭ�� �� �߱� ������ next��ư ��Ȱ��ȭ
        if (!DialogueManager.Instance.ContainChoices())
        {
            InActiveNextBtn();
        }

        //Ÿ���� ȿ��, �� ���ھ� ���
        foreach (char letter in line.ToCharArray()) 
        {
            if (skipDialogue)//UIInputManager.Instance.GetSubmitPressed()
            {
                skipDialogue = false;
                dialogueText.text = line;
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        //yield return new WaitForSeconds(1f);
        DialogueManager.Instance.canContinueToNextLine = true;

        // ��ȭ���� �� �߸� next��ư �ٽ� Ȱ��ȭ
        if (!DialogueManager.Instance.ContainChoices())
        {
            ActiveNextBtn(); 
        }
        else
        {
            foreach(var choice in choiceButtons)
            {
                choice.gameObject.SetActive(true);
            }
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

    private void OnDestroy()
    {
        //Debug.Log("destroying dialoguepanelui");
    }
}
