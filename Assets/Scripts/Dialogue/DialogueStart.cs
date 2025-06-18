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
            //UIManager.Instance.ShowUiPanels(); ����� �뵵

            if(!inputEventContext.Equals(InputEventContext.DEFAULT))
            {
                return;
            }
            if (!dialogueKnotName.Equals(""))
            {
                // dialogue �̺�Ʈ ȣ��
                GameEventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
            }
        }
    }
}
