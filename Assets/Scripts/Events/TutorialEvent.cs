using UnityEngine;

public class TutorialEvent : IGameEvent
{
    public string tutorialId;
    
    public TutorialEvent(string id)
    {
       tutorialId = id;
    }
}
