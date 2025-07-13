using UnityEngine;
using UnityEngine.InputSystem;
//
// Dialogue �����ؼ�, PlayerInput�̿��Ͽ� ����
//
[RequireComponent(typeof(PlayerInput))]
public class UIInputManager : Singleton<UIInputManager>
{
    public bool submitPressed = false;

    // spacebar ������ ����, Dialogue���� 
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
        Debug.Log(result); 
        return result;
    }
}
