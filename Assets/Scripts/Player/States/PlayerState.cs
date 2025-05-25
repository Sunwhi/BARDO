public abstract class PlayerState
{
    public Player Player => fsm.Player;
    public PlayerStateMachine fsm { get; private set; }

    public PlayerState(PlayerStateMachine fsm)
    {
        this.fsm = fsm;
    }

    public abstract void Enter();
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract void Exit();
}