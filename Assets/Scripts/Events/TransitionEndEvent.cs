using UnityEngine;

public class TransitionEndEvent : IGameEvent
{
    public int stageIdx;
    public TransitionEndEvent(int stageIdx)
    {
        this.stageIdx = stageIdx;
    }
}
