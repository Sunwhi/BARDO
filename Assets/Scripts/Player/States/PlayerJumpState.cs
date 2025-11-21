using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerStateMachine fsm) : base(fsm) { }

    public override void Enter()
    {
        Player.controller.Jump();
        Player.isGrounded = false;
        SoundManager.Instance.PlaySFX(ESFX.Character_Jump);
        Player.animator.SetBool(Player.AnimationData.GroundParamHash, false);
    }

    public override void Update()
    {
        if (Player.isGrounded)
        {
            if (Player.controller.MoveInput.x != 0)
                fsm.ChangeState(fsm.MoveState);
            else
            {
                fsm.ChangeState(fsm.IdleState);
            }
                
        }
    }

    public override void FixedUpdate()
    {
        Player.controller.Move();
    }

    public override void Exit()
    {
        Player.animator.SetBool(Player.AnimationData.GroundParamHash, true);
    }
}