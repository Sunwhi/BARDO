using UnityEngine;
using System;


public class InputEvents : MonoBehaviour
{
    public InputEventContext inputEventContext { get; private set; } = InputEventContext.DEFAULT;

    public void ChangeInputEventContext(InputEventContext newContext)
    {
        this.inputEventContext = newContext;
    }

    public event Action<InputEventContext> onStartDialogue;
    public void StartDialogue()
    {
        onStartDialogue?.Invoke(this.inputEventContext); 
    }    
}