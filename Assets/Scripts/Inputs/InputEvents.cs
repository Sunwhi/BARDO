using UnityEngine;
using System;


public class InputEvents : MonoBehaviour
{
    public InputEventContext inputEventContext { get; private set; } = InputEventContext.DEFAULT;

    public void ChangeInputEventContext(InputEventContext newContext)
    {
        this.inputEventContext = newContext;
    }

    public event Action<InputEventContext> onSubmitPressed;
    public void SubmitPressed()
    {
        onSubmitPressed?.Invoke(this.inputEventContext); 
    }
    
}
