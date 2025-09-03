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
using UnityEngine.SocialPlatforms.Impl;

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
    // 대화 출력이 완료되고 next버튼 활성화 전까지 OnSubmit이 여러번 출력되어 skipDialogue가 다음줄까지 true가 되는 버그를 방지하기 위해,
    // 한번 OnSubmit으로 대화창 스킵하면, next버튼 활성화 되기 전까지 다시 OnSubmit이 호출 안되게 함.
    // 즉, 한번 OnSubmit 누르면 next 버튼 활성화 전까지 OnSubmit이 호출이 안되고, 다시 skipDialogue가 true되지 않게 함.
    private bool isOnSubmitPressed = false;

    public TextMeshProUGUI displaySpeakerText;
    private const string SPEAKER_TAG = "speaker";

    private void Start()
    {
        dialoguePanel.SetActive(false);
        ResetPanel();
    }
    private void Update()
    {
        // 모든 줄이 출력되지 않았을 때, space를 눌러 dialogue를 skip한다.
        if ((UIInputManager.Instance.GetSubmitPressed() || DialogueManager.Instance.panelClickForSkip) && !DialogueManager.Instance.canContinueToNextLine)
        {
            if (!isOnSubmitPressed) 
            {
                isOnSubmitPressed = true;
                // 이게 지금 실행이 안되고 있다. 그 이유는 GetSubmitPressed()가 true가 안되고 있음*************************************************** why??
                UIInputManager.Instance.submitPressed = false; // 스페이스바 눌렀을때만 false되게. 이렇게 안하고 GetSubmitPressed에서 false하면 계속해서 false가 출력돼서 작동을 안함. true가 안나옴.
                skipDialogue = true;

                DialogueManager.Instance.panelClickForSkip = false;
            }
        }
    }
    private void OnEnable()
    {
        DialogueEventManager.Instance.dialogueEvents.onDialogueStarted += DialogueStart;
        DialogueEventManager.Instance.dialogueEvents.onDialogueFinished += DialogueFinished;
        DialogueEventManager.Instance.dialogueEvents.onDisplayDialogue += DisplayDialogue;
        //GameEventBus.Subscribe<PauseGameEvent>(OnGamePaused);
    }

    private void OnDisable()
    {
        if(DialogueEventManager.Instance != null)
        {
            DialogueEventManager.Instance.dialogueEvents.onDialogueStarted -= DialogueStart;
            DialogueEventManager.Instance.dialogueEvents.onDialogueFinished -= DialogueFinished;
            DialogueEventManager.Instance.dialogueEvents.onDisplayDialogue -= DisplayDialogue;
        }
    }

    private void DialogueStart()
    {
        dialoguePanel.SetActive(true);
    }

    private void DialogueFinished()
    {
        dialoguePanel.SetActive(false);
        // reset anything for next time
        ResetPanel();
    }

    private void DisplayDialogue(string dialogueLine, List<Ink.Runtime.Choice> dialogueChoices)
    {
        if(displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
        }

        displaySpeakerText.text = DialogueManager.Instance.speaker;

        // 선택지 dialogue에서는 next 버튼 비활성화
        if (dialogueChoices.Count > 0)
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

    int letterCount = 0;
    private IEnumerator DisplayLine(string line, List<Ink.Runtime.Choice> dialogueChoices)
    {
        dialogueText.text = "";

        yield return new WaitForSeconds(0.1f);
        DialogueManager.Instance.canContinueToNextLine = false;

        // 대화줄 다 뜨기 전까지 next버튼 비활성화
        if (!DialogueManager.Instance.ContainChoices())
        {
            InActiveNextBtn();
        }

        //타이핑 효과, 한 글자씩 출력
        foreach (char letter in line.ToCharArray()) 
        {
            if (skipDialogue)//UIInputManager.Instance.GetSubmitPressed()
            {
                skipDialogue = false;
                typingSpeed = 0.005f; // 타이핑 빨라짐.
            }
            dialogueText.text += letter;

            letterCount++;

            if (letterCount % 2 == 0 && typingSpeed != 0.005f)
            {
                SoundManager.Instance.PlaySFX(ESFX.UI_Txt_Scroll);
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        typingSpeed = 0.04f;

        // 대화창 다 나오면 0.5초 이후에 next버튼 뜬다. -> 테스트 위해 임시로 빠르게 설정(0.5 -> 0.01)
        yield return new WaitForSeconds(0.01f);
        DialogueManager.Instance.canContinueToNextLine = true;

        // 대화줄이 다 뜨면 next버튼 다시 활성화
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
                DialogueEventManager.Instance.dialogueEvents.UpdateChoiceIndex(0);
            }

            choiceButtonIndex--;
        }
    }
    private bool OnGamePaused(PauseGameEvent ev)
    {
        if(ev.State == GameState.pause)
        {
            return true;
        }
        else if(ev.State == GameState.resume)
        {
            return false;
        }
        return true;
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
