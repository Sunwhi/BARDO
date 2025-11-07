using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }
}
