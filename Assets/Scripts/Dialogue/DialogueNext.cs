using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueContinue : MonoBehaviour
{
    private string dialogueKnotName = "start";

    // Next button 클릭
    public void OnClickDialogueNext()
    {
        // 문장 끝날 때 까지 next 버튼 못 누름
        if (DialogueManager.Instance.canContinueToNextLine)
        {
            PressedDialogueNext();
        }
    }
    /*private void Update()
    {
        //Debug.Log(DialogueManager.Instance.haveChoices);
        if (Input.GetKeyDown(KeyCode.Return) && !DialogueManager.Instance.haveChoices)
        {
            PressedDialogueNext();
        }
    }*/
    private void Update()
    {
        if (UIInputManager.Instance.GetSubmitPressed() && DialogueManager.Instance.canContinueToNextLine)
        {
            UIInputManager.Instance.submitPressed = false; // 스페이사 눌렀을 때만 false되게
            PressedDialogueNext();
        }
    }

    private void PressedDialogueNext()
    {
        if (!dialogueKnotName.Equals(""))
        {
            SoundManager.Instance.PlaySFX(eSFX.UI_Button_Txt);
            GameEventManager.Instance.inputEvents.StartDialogue();
        }
    }
}
