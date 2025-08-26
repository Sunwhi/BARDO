using UnityEngine;

public class PlayerDieState : PlayerState
{
    public PlayerDieState(PlayerStateMachine fsm) : base(fsm) { }

    public override void Enter()
    {
        Player.animator.SetTrigger(Player.AnimationData.DieParamHash);
        Player.controller.MoveInput = Vector2.zero;

        SoundManager.Instance.PlaySFX(ESFX.Character_Death);
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void Exit()
    {
    }
}