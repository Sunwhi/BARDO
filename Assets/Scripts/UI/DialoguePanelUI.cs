using Ink.Parsed;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine.EventSystems;

public class DialoguePanelUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Components")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueChoiceBtn[] choiceButtons;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private float typingSpeed = 0.04f;
    private Coroutine displayLineCoroutine;
    private bool skipDialogue = false;
    // ��ȭ ����� �Ϸ�ǰ� next��ư Ȱ��ȭ ������ OnSubmit�� ������ ��µǾ� skipDialogue�� �����ٱ��� true�� �Ǵ� ���׸� �����ϱ� ����,
    // �ѹ� OnSubmit���� ��ȭâ ��ŵ�ϸ�, next��ư Ȱ��ȭ �Ǳ� ������ �ٽ� OnSubmit�� ȣ�� �ȵǰ� ��.
    // ��, �ѹ� OnSubmit ������ next ��ư Ȱ��ȭ ������ OnSubmit�� ȣ���� �ȵǰ�, �ٽ� skipDialogue�� true���� �ʰ� ��.
    private bool isOnSubmitPressed = false; 
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
        //Debug.Log()
        // ��� ���� ��µ��� �ʾ��� ��, space�� ���� dialogue�� skip�Ѵ�.
        if ((UIInputManager.Instance.GetSubmitPressed() || DialogueManager.Instance.panelClickForSkip) && !DialogueManager.Instance.canContinueToNextLine)
        {
            if (!isOnSubmitPressed) 
            {
                isOnSubmitPressed = true;
                // �̰� ���� ������ �ȵǰ� �ִ�. �� ������ GetSubmitPressed()�� true�� �ȵǰ� ����*************************************************** why??
                UIInputManager.Instance.submitPressed = false; // �����̽��� ���������� false�ǰ�. �̷��� ���ϰ� GetSubmitPressed���� false�ϸ� ����ؼ� false�� ��µż� �۵��� ����. true�� �ȳ���.
                skipDialogue = true;

                DialogueManager.Instance.panelClickForSkip = false;
            }
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

        displayLineCoroutine = StartCoroutine(DisplayLine(dialogueLine, dialogueChoices));
    }
    private IEnumerator DisplayLine(string line, List<Ink.Runtime.Choice> dialogueChoices)
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
                //dialogueText.text = line;
                //break;
                typingSpeed = 0.005f; // Ÿ���� ������.
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        typingSpeed = 0.04f;

        // ��ȭâ �� ������ 0.5�� ���Ŀ� next��ư ���.
        yield return new WaitForSeconds(0.5f);
        DialogueManager.Instance.canContinueToNextLine = true;

        // ��ȭ���� �� �߸� next��ư �ٽ� Ȱ��ȭ
        if (!DialogueManager.Instance.ContainChoices())
        {
            ActiveNextBtn(); 
        }
        else
        {
            SetChoiceBtns(dialogueChoices);
        }

        isOnSubmitPressed = false;
    }
    private void SetChoiceBtns(List<Ink.Runtime.Choice> dialogueChoices)
    {
        // enable and set info for buttons depending on ink choice information
        int choiceButtonIndex = dialogueChoices.Count - 1;
        for (int inkChoiceIndex = 0; inkChoiceIndex < dialogueChoices.Count; inkChoiceIndex++)
        {
            Ink.Runtime.Choice dialogueChoice = dialogueChoices[inkChoiceIndex];
            DialogueChoiceBtn choiceButton = choiceButtons[choiceButtonIndex];

            choiceButton.gameObject.SetActive(true);
            choiceButton.SetChoiceText(dialogueChoice.text);
            choiceButton.SetChoiceIndex(inkChoiceIndex);

            if (inkChoiceIndex == 0)
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
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // ��ȭâ �������߿��� true�� �ٲ���հ�
        /*if (!DialogueManager.Instance.canContinueToNextLine)
        {
            DialogueManager.Instance.panelClickForSkip = true;
        }*/
    }
}
