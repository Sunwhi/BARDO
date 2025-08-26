using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public enum CamState
{
    v1, v2_0, v2_1, v2_2, v2_3, v3_1, v3_2, v3_3,
}

public class CameraManager : Singleton<CameraManager>
{
    [Header("Cinemachine")]
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private CinemachineBrain brain;

    [Header("Input Ignore")]
    [SerializeField] private float ignoreDuringSwitch = 0.5f; // 전환 중 무시 시간(초)
    private float ignoreUntil; // Time.time 기준

    [SerializeField] private int animLayer = 0;

    public void GoFront()
    {
        if (Time.time < ignoreUntil) return;
        cameraAnimator.SetTrigger("GoFront");
    }

    public void GoBack()
    {
        if (Time.time < ignoreUntil) return;
        cameraAnimator.SetTrigger("GoBack");
    }

    private readonly Dictionary<CamState, int> stateHash = new()
    {
        { CamState.v1,   Animator.StringToHash("Stage1") },
        { CamState.v2_0, Animator.StringToHash("Stage2-0") },
        { CamState.v2_1, Animator.StringToHash("Stage2-1") },
        { CamState.v2_2, Animator.StringToHash("Stage2-2") },
        { CamState.v2_3, Animator.StringToHash("Stage2-3") },
        { CamState.v3_1, Animator.StringToHash("Stage3-1") },
        { CamState.v3_2, Animator.StringToHash("Stage3-2") },
        { CamState.v3_3, Animator.StringToHash("Stage3-3") },
    };

    private void OnEnable()
    {
        GameEventBus.Subscribe<ViewportExitEvent>(OnViewportExit);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<ViewportExitEvent>(OnViewportExit);
    }

    private void OnViewportExit(ViewportExitEvent e)
    {
        if (e.direction == ViewportExitDirection.Left) GoBack();
        else if (e.direction == ViewportExitDirection.Right) GoFront();
    }

    // 기본 무시 시간 사용
    public void JumpAndCut(CamState target)
    {
        StartCoroutine(JumpRoutine(target, ignoreDuringSwitch));
    }

    // 호출부에서 무시 시간을 지정하고 싶을 때
    public void JumpAndCut(CamState target, float ignoreSeconds)
    {
        StartCoroutine(JumpRoutine(target, ignoreSeconds));
    }

    private IEnumerator JumpRoutine(CamState target, float ignoreSeconds)
    {
        // 입력 무시 시작
        SetIgnore(ignoreSeconds);

        ResetAll(cameraAnimator);

        var prevBlend = brain.DefaultBlend;
        brain.DefaultBlend = new CinemachineBlendDefinition(
            CinemachineBlendDefinition.Styles.Cut, 0f);

        cameraAnimator.Play(stateHash[target], animLayer, 0f);
        cameraAnimator.Update(0f);

        yield return null; // Brain 반영

        brain.DefaultBlend = prevBlend;
        // 입력 무시는 SetIgnore로 설정된 시간까지 자동 유지
    }

    private void SetIgnore(float seconds)
    {
        if (seconds <= 0f) return;
        float until = Time.time + seconds;
        if (until > ignoreUntil) ignoreUntil = until;
    }

    private static void ResetAll(Animator a)
    {
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
}