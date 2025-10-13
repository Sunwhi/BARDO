using UnityEngine;

public class TransitionEvents : IGameEvent
{
    public int stageId;
    
    public TransitionEvents(int stageId)
    {
        this.stageId = stageId;
    }
}
