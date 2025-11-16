using System;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    // --- Parameters ---
    [SerializeField] private string moveParamName = "Move";
    [SerializeField] private string SpeedParamName = "Speed";
    [SerializeField] private string airParamName = "IsGrounded";
    [SerializeField] private string velocityParamName = "VelocityY";
    [SerializeField] private string dieParamName = "Die";

    // --- Hashes ---
    public int MoveParamHash { get; private set; }
    public int SpeedParamHash { get; private set; }
    public int GroundParamHash { get; private set; }
    public int VelocityYParamHash { get; private set; }
    public int DieParamHash { get; private set; }

    public void Initialize()
    {
        MoveParamHash = Animator.StringToHash(moveParamName);
        SpeedParamHash = Animator.StringToHash(SpeedParamName);
        GroundParamHash = Animator.StringToHash(airParamName);
        VelocityYParamHash = Animator.StringToHash(velocityParamName);
        DieParamHash = Animator.StringToHash(dieParamName);
    }
}