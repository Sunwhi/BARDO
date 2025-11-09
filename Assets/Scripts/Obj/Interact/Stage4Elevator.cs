using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Stage4Elevator : InteractEnter
{
    [SerializeField] private ElevatorController evCtrl;

    [SerializeField] private Transform hingeTransform;
    [SerializeField] private bool IsDoorOpen = false;

    private CamState curState => CameraManager.Instance.curCamState;

    protected override IEnumerator InteractCoroutine()
    {
        yield return base.InteractCoroutine();

        //TODO : 위치이동
        if (SaveManager.Instance.MySaveData.stageIdx == 4 && !IsDoorOpen)
            SaveManager.Instance.SetSaveData(nameof(SaveData.storyIdx), 1);

        bool isDown = curState == CamState.v4_0;
        if (isDown)
        {
            yield return DoorInteract(IsDoorOpen, true);
        }
        else
        {
            yield return DoorInteract(IsDoorOpen, false);
        }

        if (!IsDoorOpen) evCtrl.OnElevatorMove(isDown);
        IsDoorOpen = !IsDoorOpen;
    }

    public IEnumerator DoorInteract(bool isOpen, bool isLeft)
    {
        StoryManager.Instance.Player.playerInput.enabled = isOpen;

        float rotZ = isOpen ? (isLeft ? 90f : -125f) : 0f;
        yield return hingeTransform.DOLocalRotate(new Vector3(0, 0, rotZ), 0.5f)
            .SetEase(Ease.InOutSine);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (SaveManager.Instance.MySaveData.stageIdx >= 4)
        {
            guide.DOFade(1f, 0.5f).SetUpdate(true);
            interactCoroutine ??= StartCoroutine(InteractCoroutine());
        }
    }
}
