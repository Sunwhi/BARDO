using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DialogueNext : MonoBehaviour
{
    private string dialogueKnotName = "start";

    // Next button Ŭ��
    public void OnClickDialogueNext()
    {
        // next ��ư ������ null�� select�ϰ� �ؼ� ���̻� highlighted���°� ���� �ʰ�.(next ������ ��� ȸ������ ĥ���� �ִ� ���� ���� �ڵ�)
        EventSystem.current.SetSelectedGameObject(null);

        // ���� ���� �� ���� next ��ư �� ����
        if (DialogueManager.Instance.canContinueToNextLine)
        {
            PressedDialogueNext();
        }
    }

    private void Update()
    {
        if (UIInputManager.Instance.GetSubmitPressed() && DialogueManager.Instance.canContinueToNextLine)
        {
            UIInputManager.Instance.submitPressed = false; // �����̻� ������ ���� false�ǰ�
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
