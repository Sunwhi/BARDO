using UnityEngine;
using UnityEngine.InputSystem;
//
// Dialogue �����ؼ�, PlayerInput�̿��Ͽ� ����
//
[RequireComponent(typeof(PlayerInput))]
public class UIInputManager : Singleton<UIInputManager>
{
    private bool submitPressed = false;

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
        Debug.Log("ȣ����ݾ�");
        bool result = submitPressed;
        submitPressed = false;
        Debug.Log(result); 
        return result;
    }
}
