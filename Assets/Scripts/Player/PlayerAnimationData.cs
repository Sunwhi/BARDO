using System;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    // --- Parameters ---
    [SerializeField] private string moveParamName = "Move";
    [SerializeField] private string runParamName = "Run";
    [SerializeField] private string jumpParamName = "Jump";
    [SerializeField] private string dieParamName = "Die";

    // --- Hashes ---
    public int MoveParamHash { get; private set; }
    public int RunParamHash { get; private set; }
    public int JumpParamHash { get; private set; }
    public int DieParamHash { get; private set; }

    public void Initialize()
    {
        MoveParamHash = Animator.StringToHash(moveParamName);
        RunParamHash = Animator.StringToHash(runParamName);
        JumpParamHash = Animator.StringToHash(jumpParamName);
        DieParamHash = Animator.StringToHash(dieParamName);
    }
}