public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine fsm) : base(fsm) { }

    public override void Enter()
    {
        Player.animator.SetBool(Player.AnimationData.MoveParameterHash, true);
    }

    public override void Update()
    {
        if (Player.controller.MoveInput.x == 0)
        {
            fsm.ChangeState(fsm.IdleState);
            return;
        }

        if (Player.controller.JumpInput)
        {
            fsm.ChangeState(fsm.JumpState);
        }
    }

    public override void FixedUpdate()
    {
        Player.controller.Move(); // ¿Ãµø
    }

    public override void Exit()
    {
        Player.animator.SetBool(Player.AnimationData.MoveParameterHash, false);
    }
}
