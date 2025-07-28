using System;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    // --- Parameters ---
    [SerializeField] private string moveParamName = "Move";
    [SerializeField] private string jumpParamName = "Jump";
    [SerializeField] private string dieParamName = "Die";

    // --- Hashes ---
    public int MoveParamHash { get; private set; }
    public int JumpParamHash { get; private set; }
    public int DieParamHash { get; private set; }

    public void Initialize()
    {
        MoveParamHash = Animator.StringToHash(moveParamName);
        JumpParamHash = Animator.StringToHash(jumpParamName);
        DieParamHash = Animator.StringToHash(dieParamName);
    }
}