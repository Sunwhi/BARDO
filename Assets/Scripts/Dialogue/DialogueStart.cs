using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class DialogueStart : MonoBehaviour
{

    [Header("Dialogue (optional)")]
    [SerializeField] private string dialogueKnotName;

    private void OnEnable()
    {
        GameEventManager.Instance.inputEvents.onSubmitPressed += SubmitPressed;
    }
    private void OnDisable()
    {
        Debug.Log("OnDiable");
        GameEventManager.Instance.inputEvents.onSubmitPressed -= SubmitPressed;
    }

    private void Update()
    {
        SubmitPressed(InputEventContext.DEFAULT);
    }

    private void SubmitPressed(InputEventContext inputEventContext)
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(!inputEventContext.Equals(InputEventContext.DEFAULT))
            {
                return;
            }
            if (!dialogueKnotName.Equals(""))
            {
                // dialogue 이벤트 호출
                GameEventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
            }
        }
    }
}
