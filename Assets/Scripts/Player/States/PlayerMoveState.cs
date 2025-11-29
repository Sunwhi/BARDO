using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine fsm) : base(fsm) { }

    public override void Enter()
    {
        Player.animator.SetBool(Player.AnimationData.MoveParamHash, true);

        //SoundManager.Instance.PlaySFX(ESFX.Character_Walk);
        //SoundManager.Instance.sfxSource.loop = true;
        if(!Player.audioSource.isPlaying)
        {
            Player.audioSource.Play();
        }
    }

    public override void Update()
    {
        if (Player.controller.RunInput)
        {
            Player.audioSource.pitch = 1.5f;
        }
        else
        {
            Player.audioSource.pitch = 1f;
        }
        if (Player.controller.MoveInput.x == 0)
        {
            fsm.ChangeState(fsm.IdleState);
            return;
        }

        if (Player.controller.JumpInput)
        {
            fsm.ChangeState(fsm.JumpState);
            return;
        }
    }
   
    public override void FixedUpdate()
    {
        Player.controller.Move(); // 이동
    }

    public override void Exit()
    {
        //SoundManager.Instance.StopSFX();
        Player.audioSource.Stop();
    }
}
