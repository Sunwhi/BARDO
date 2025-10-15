using UnityEngine;
using UnityEngine.InputSystem;
//
// Dialogue 관련해서, PlayerInput이용하여 구현
//
[RequireComponent(typeof(PlayerInput))]
public class UIInputManager : Singleton<UIInputManager>
{
    public bool submitPressed = false;

    // spacebar 누르면 실행, Dialogue에서 
    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (DialogueManager.Instance.dialoguePlaying)
        {
            if (context.performed)
            {
                submitPressed = true;
            }
            else if (context.canceled)
            {
                submitPressed = false;
            }
        }
    }

    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        return result;
    }
}
