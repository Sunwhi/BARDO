using UnityEditor.Tilemaps;
using UnityEngine;

public class GameEventManager : Singleton<GameEventManager>
{
    public DialogueEvents dialogueEvents;
    public InputEvents inputEvents;
 
    public override void Awake()
    {
        base.Awake();
        dialogueEvents = gameObject.AddComponent<DialogueEvents>();
        inputEvents = gameObject.AddComponent<InputEvents>();
    }
}
