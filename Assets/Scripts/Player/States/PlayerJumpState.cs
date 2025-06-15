using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerStateMachine fsm) : base(fsm) { }

    public override void Enter()
    {
        Player.controller.Jump();
        Player.animator.SetTrigger(Player.AnimationData.JumpTriggerHash);
    }

    public override void Update()
    {
        if (Player.isGrounded && Mathf.Abs(Player.rb.linearVelocity.y) < 0.01f)
        {
            if (Player.controller.MoveInput.x != 0)
                fsm.ChangeState(fsm.MoveState);
            else
                fsm.ChangeState(fsm.IdleState);
        }
    }

    public override void FixedUpdate()
    {
        Player.controller.Move();
    }

    public override void Exit()
    {
    }
}