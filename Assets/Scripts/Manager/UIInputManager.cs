using UnityEngine;
using UnityEngine.InputSystem;
//
// Dialogue 관련해서, PlayerInput이용하여 구현
//
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
        Debug.Log("호출됐잖아");
        bool result = submitPressed;
        submitPressed = false;
        Debug.Log(result); 
        return result;
    }
}
