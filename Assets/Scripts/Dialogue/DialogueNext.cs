using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueContinue : MonoBehaviour
{
    private string dialogueKnotName = "start";

    // Next button Ŭ��
    public void OnClickDialogueNext()
    {
        PressedDialogueNext();
    }
    /*private void Update()
    {
        //Debug.Log(DialogueManager.Instance.haveChoices);
        if (Input.GetKeyDown(KeyCode.Return) && !DialogueManager.Instance.haveChoices)
        {
            PressedDialogueNext();
        }
    }*/

    private void PressedDialogueNext()
    {
        if (!dialogueKnotName.Equals(""))
        {
            SoundManager.Instance.PlaySFX(eSFX.UI_Button_Txt);
            GameEventManager.Instance.inputEvents.SubmitPressed();
        }
    }
}
