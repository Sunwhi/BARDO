using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DialogueNext : MonoBehaviour, IPointerEnterHandler
{
    private string dialogueKnotName = "start";

    // Next button 클릭
    public void OnClickDialogueNext()
    {
        // next 버튼 누르면 null을 select하게 해서 더이상 highlighted상태가 되지 않게.(next 누르고도 계속 회색으로 칠해져 있는 버그 수정 코드)
        EventSystem.current.SetSelectedGameObject(null);

        // 문장 끝날 때 까지 next 버튼 못 누름
        if (DialogueManager.Instance.canContinueToNextLine)
        {
            PressedDialogueNext();
        }
    }

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
            SoundManager.Instance.PlaySFX(ESFX.UI_Button_Txt);
            DialogueEventManager.Instance.inputEvents.StartDialogue();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(gameObject.name != "DialoguePanel")
            SoundManager.Instance.PlaySFX(ESFX.UI_Button_Hover);
    }
}
