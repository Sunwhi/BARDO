using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerStateMachine fsm) : base(fsm) { }

    public override void Enter()
    {
        Player.controller.Jump();
        Player.isGrounded = false;
        SoundManager.Instance.PlaySFX(ESFX.Character_Jump);
    }

    public override void Update()
    {
        if (Player.isGrounded)
        {
            SoundManager.Instance.PlaySFX(ESFX.Character_land);
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
        
    }
}