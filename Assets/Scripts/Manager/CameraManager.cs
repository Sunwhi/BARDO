using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public enum CamState
{
    v1, 
    v2_0, v2_1, v2_2, v2_3, 
    v3_1, v3_2, v3_3, 
    v4_0, v4_1, v4_2, v4_3
}

public class CameraManager : Singleton<CameraManager>
{
    [Header("Cinemachine")]
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private CinemachineBrain brain;

    [Header("Switch Control")]
    [SerializeField] private float ignoreDuringSwitch = 0.5f;
    [SerializeField] private int animLayer = 0;

    public CamState curCamState { get; private set; } = CamState.v1;

    // Animator shortNameHash ↔ CamState
    private static readonly Dictionary<int, CamState> HashToState = new()
    {
        { Animator.StringToHash("Stage1"),   CamState.v1   },
        { Animator.StringToHash("Stage2-0"), CamState.v2_0 },
        { Animator.StringToHash("Stage2-1"), CamState.v2_1 },
        { Animator.StringToHash("Stage2-2"), CamState.v2_2 },
        { Animator.StringToHash("Stage2-3"), CamState.v2_3 },
        { Animator.StringToHash("Stage3-1"), CamState.v3_1 },
        { Animator.StringToHash("Stage3-2"), CamState.v3_2 },
        { Animator.StringToHash("Stage3-3"), CamState.v3_3 },
        { Animator.StringToHash("Satge4-0"), CamState.v4_0 },
        { Animator.StringToHash("Satge4-1"), CamState.v4_1 },
        { Animator.StringToHash("Satge4-2"), CamState.v4_2 },
        { Animator.StringToHash("Satge4-3"), CamState.v4_3 },
    };

    // 점프용: CamState → Animator shortNameHash
    private static readonly Dictionary<CamState, int> StateToHash = new()
    {
        { CamState.v1,   Animator.StringToHash("Stage1") },
        { CamState.v2_0, Animator.StringToHash("Stage2-0") },
        { CamState.v2_1, Animator.StringToHash("Stage2-1") },
        { CamState.v2_2, Animator.StringToHash("Stage2-2") },
        { CamState.v2_3, Animator.StringToHash("Stage2-3") },
        { CamState.v3_1, Animator.StringToHash("Stage3-1") },
        { CamState.v3_2, Animator.StringToHash("Stage3-2") },
        { CamState.v3_3, Animator.StringToHash("Stage3-3") },
        { CamState.v4_0, Animator.StringToHash("Stage4-0") },
        { CamState.v4_1, Animator.StringToHash("Stage4-1") },
        { CamState.v4_2, Animator.StringToHash("Stage4-2") },
        { CamState.v4_3, Animator.StringToHash("Stage4-3") },
    };

    private float ignoreUntil;

    private void OnEnable()
    {
        GameEventBus.Subscribe<ViewportExitEvent>(OnViewportExit);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<ViewportExitEvent>(OnViewportExit);
    }

    public void GoFront()
    {
        if (Time.time < ignoreUntil) return;
        cameraAnimator.SetTrigger("GoFront");
        SetIgnore(ignoreDuringSwitch);
    }

    public void GoBack()
    {
        if (Time.time < ignoreUntil) return;
        cameraAnimator.SetTrigger("GoBack");
        SetIgnore(ignoreDuringSwitch);
    }

    private void OnViewportExit(ViewportExitEvent e)
    {
        if (Time.time < ignoreUntil) return;
        if (e.direction == ViewportExitDirection.Left) GoBack();
        else if (e.direction == ViewportExitDirection.Right) GoFront();
    }

    public void JumpAndCut(CamState target) => StartCoroutine(JumpRoutine(target, ignoreDuringSwitch));
    public void JumpAndCut(CamState target, float ignoreSeconds) => StartCoroutine(JumpRoutine(target, ignoreSeconds));

    private IEnumerator JumpRoutine(CamState target, float ignoreSeconds)
    {
        SetIgnore(ignoreSeconds);
        ResetAll(cameraAnimator);

        var prevBlend = brain.DefaultBlend;
        brain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, 0f);

        cameraAnimator.Play(StateToHash[target], animLayer, 0f);
        cameraAnimator.Update(0f);

        yield return null;

        brain.DefaultBlend = prevBlend;
        // curCamState 동기화는 StateMachineBehaviour에서 수행
    }

    private void SetIgnore(float seconds)
    {
        if (seconds <= 0f) return;
        var until = Time.time + seconds;
        if (until > ignoreUntil) ignoreUntil = until;
    }

    private static void ResetAll(Animator a)
    {
        if (a == null) return;
        for (int i = 0; i < a.parameterCount; i++)
        {
            var p = a.GetParameter(i);
            switch (p.type)
            {
                case AnimatorControllerParameterType.Trigger:
                    a.ResetTrigger(p.name);
                    break;
                case AnimatorControllerParameterType.Bool:
                    a.SetBool(p.nameHash, false);
                    break;
            }
        }
    }

    // 해시 기반 동기화. StateMachineBehaviour가 호출.
    public void SyncStateByHash(int shortNameHash)
    {
        if (HashToState.TryGetValue(shortNameHash, out var s))
            curCamState = s;
    }
}