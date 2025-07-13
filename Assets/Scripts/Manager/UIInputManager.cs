using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class UIInputManager : Singleton<UIInputManager>
{
    private bool submitPressed = false;

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
        submitPressed = false;
        if (result) Debug.Log("GSP :" + result);
        return result;
    }
}
