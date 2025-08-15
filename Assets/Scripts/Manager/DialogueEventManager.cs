using UnityEditor.Tilemaps;
using UnityEngine;

public class DialogueEventManager : Singleton<DialogueEventManager>
{
    public DialogueEvents dialogueEvents;
    public InputEvents inputEvents;
 
    public override void Awake() 
    {
        base.Awake();
        dialogueEvents = gameObject.AddComponent<DialogueEvents>();
        inputEvents = gameObject.AddComponent<InputEvents>();
    }
    private void OnDestroy()
    {
        //isManagerDestroyed = true;
    }
}
