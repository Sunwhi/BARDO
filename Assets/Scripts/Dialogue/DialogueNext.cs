using UnityEngine;

public class DialogueContinue : MonoBehaviour
{
    private string dialogueKnotName = "start";
    public void OnClickDialogueNext()
    {
        PressedDialogueNext();
    }

    private void PressedDialogueNext()
    {
        if (!dialogueKnotName.Equals(""))
        {
            GameEventManager.Instance.inputEvents.SubmitPressed();
        }
    }
}
