using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine fsm) : base(fsm) { }

    public override void Enter()
    {
        Player.animator.SetBool(Player.AnimationData.MoveParamHash, false);
        Vector3 v = Player.rb.linearVelocity;
        v.x = 0;
        Player.rb.linearVelocity = v;
    }

    public override void Update()
    {
        if (Player.controller.MoveInput.x != 0)
        {
            fsm.ChangeState(fsm.MoveState);
            return;
        }

        if (Player.controller.JumpInput)
        {
            fsm.ChangeState(fsm.JumpState);
        }
    }

    public override void FixedUpdate()
    {
        // 지면 고정, 이동 없음
    }

    public override void Exit()
    {
        
    }
}
