using UnityEngine;

public class CheckPointEvent : IGameEvent
{
    public string checkpointID;

    public CheckPointEvent(string checkpointID)
    {
        this.checkpointID = checkpointID;
    }
}
