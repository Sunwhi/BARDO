using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class DialogueStart : MonoBehaviour
{

    [Header("Dialogue (optional)")]
    [SerializeField] private string dialogueKnotName;

    private void OnEnable()
    {
        DialogueEventManager.Instance.inputEvents.onStartDialogue += StartDialogue;
        //GameEventBus.Subscribe<>
    }
    private void OnDisable()
    {   
        if(DialogueEventManager.Instance != null)
        {
            DialogueEventManager.Instance.inputEvents.onStartDialogue -= StartDialogue;
        }
    }

    private void Update()
    {
        StartDialogue(InputEventContext.DEFAULT);
    }

    private void StartDialogue(InputEventContext inputEventContext)
    {
        /*if (Input.GetKeyDown(KeyCode.S))
        {
            //UIManager.Instance.ShowUiPanels(); 디버깅 용도

            if(!inputEventContext.Equals(InputEventContext.DEFAULT))
            {
                return;
            }
            if (!dialogueKnotName.Equals(""))
            {
                // dialogue 이벤트 호출
                GameEventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName);
            }
        }*/
    }
}
