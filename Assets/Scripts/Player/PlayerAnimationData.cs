using System;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    // --- Bool Parameters ---
    [SerializeField] private string moveParameterName = "Move";

    // --- Trigger Parameters ---
    [SerializeField] private string jumpTriggerName = "Jump";
    [SerializeField] private string dieTriggerName = "Die";

    // --- Hashes (Read-only) ---
    public int MoveParameterHash { get; private set; }
    public int JumpTriggerHash { get; private set; }
    public int DieTriggerHash { get; private set; }

    public void Initialize()
    {
        MoveParameterHash = Animator.StringToHash(moveParameterName);
        JumpTriggerHash = Animator.StringToHash(jumpTriggerName);
        DieTriggerHash = Animator.StringToHash(dieTriggerName);
    }
}