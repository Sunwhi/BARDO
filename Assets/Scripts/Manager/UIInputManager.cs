using UnityEngine;
using UnityEngine.InputSystem;
//
// Dialogue 관련해서, PlayerInput이용하여 구현
//
public class UIInputManager : Singleton<UIInputManager>
{
    public InputActionAsset inputActions;
    private InputActionMap dialogueMap;
    private InputAction submitAction;
    public bool submitPressed = false;

    override public void Awake()
    {
        dialogueMap = inputActions.FindActionMap("UI");
        submitAction = dialogueMap.FindAction("Submit");

        submitAction.performed += OnSubmit;
    }

    private void OnEnable()
    {
        dialogueMap.Enable();
    }
    private void OnDisable()
    {
        dialogueMap.Disable();
    }

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
