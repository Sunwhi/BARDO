using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine fsm) : base(fsm) { }

    public override void Enter()
    {
        Player.animator.SetBool(Player.AnimationData.MoveParameterHash, false);
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
        // ���� ����, �̵� ����
    }

    public override void Exit()
    {
        
    }
}
