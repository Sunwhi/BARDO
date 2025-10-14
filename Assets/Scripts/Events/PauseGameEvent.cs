
// 게임 멈췄을 때 Event
public enum GameState
{
    pause,
    resume
}
public class PauseGameEvent : IGameEvent
{
    public GameState State { get; private set; }
    public PauseGameEvent(GameState state)
    {
        State = state; 
    }
}
