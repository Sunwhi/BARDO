using UnityEngine;

public class PlayerDieState : PlayerState
{
    public PlayerDieState(PlayerStateMachine fsm) : base(fsm) { }

    public override void Enter()
    {
        Player.animator.SetTrigger(Player.AnimationData.DieTriggerHash);
        Player.controller.MoveInput = Vector2.zero;
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