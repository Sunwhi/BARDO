using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Stage4Elevator : InteractEnter
{
    [Header("Guides")]  
    [SerializeField] private CanvasGroup leftGuide;
    [SerializeField] private CanvasGroup rightGuide;

    [Header("Elevator Points")]
    [SerializeField] private Transform topPoint;        // 위층 위치
    [SerializeField] private Transform bottomPoint;     // 아래층 위치

    [Header("Doors")]
    [SerializeField] private Transform hingeLeft;       // 위층(왼쪽 땅)에서 열리는 문
    [SerializeField] private Transform hingeRight;      // 아래층(오른쪽 땅)에서 열리는 문

    [Header("Exit Points")]
    [SerializeField] private Transform exitPointTop;    // 위층 출구
    [SerializeField] private Transform exitPointBottom; // 아래층 출구

    [Header("Tween Settings")]
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float moveDuration = 7f;
    [SerializeField] private Ease moveEase = Ease.InOutSine;

    // SaveData에 bool isElevatorUp 필드를 하나 추가해서 사용한다고 가정.
    private bool isElevatorUp; // true면 위에 있음, false면 아래에 있음

    protected override void Awake()
    {
        leftGuide.alpha = 0;
        rightGuide.alpha = 0;
    }

    private void Start()
    {
        isElevatorUp = SaveManager.Instance.MySaveData.isElevatorUp;
        guide = isElevatorUp ? leftGuide : rightGuide;

        // 저장된 상태에 따라 초기 위치 스냅
        Transform target = isElevatorUp ? topPoint : bottomPoint;
        if (target != null) transform.position = target.position;

        // 문 상태 초기화 (닫힘)
        if (hingeLeft != null) hingeLeft.localRotation = Quaternion.identity;
        if (hingeRight != null) hingeRight.localRotation = Quaternion.identity;

        StartCoroutine(DoorInteract(true, isElevatorUp));
    }

    protected override IEnumerator InteractCoroutine()
    {
        yield return base.InteractCoroutine();

        // 현재 위치 기준으로 어디로 갈지 결정
        bool goingDown = isElevatorUp;

        // 현재 층의 문 닫기
        yield return DoorInteract(false, isElevatorUp);

        // 엘리베이터 이동
        Transform targetLevel = goingDown ? bottomPoint : topPoint;
        if (targetLevel != null)
        {
            yield return MoveElevatorTo(targetLevel.position);
        }

        isElevatorUp = !goingDown;
        guide = isElevatorUp ? leftGuide : rightGuide;
        SaveManager.Instance.SetSaveData(nameof(SaveData.isElevatorUp), isElevatorUp);

        // 도착 층의 문 열기
        yield return DoorInteract(true, isElevatorUp);

        // 플레이어를 출구까지 자동으로 걷게 한다.
        //Transform exit = isElevatorUp ? exitPointTop : exitPointBottom;
        //yield return StoryManager.Instance.PlayerWalkByPos(exit.position.x);
    }

    private IEnumerator MoveElevatorTo(Vector3 targetPos)
    {
        StoryManager.Instance.Player.isDownAllowed = true;

        CamState prevCamState = CameraManager.Instance.curCamState;

        Tween t = transform
            .DOMove(targetPos, moveDuration)
            .SetEase(moveEase);

        float delay = 1f;
        StartCoroutine(UIManager.Instance.fadeView.FadeOut(delay));
        StartCoroutine(UIManager.Instance.fadeView.FadeOut(fadeDuration - delay));

        yield return new WaitUntil(() => prevCamState != CameraManager.Instance.curCamState);
        StartCoroutine(UIManager.Instance.fadeView.FadeIn(fadeDuration));

        yield return t.WaitForCompletion();

        StoryManager.Instance.Player.isDownAllowed = false;
    }

    private IEnumerator DoorInteract(bool open, bool isTopLevel)
    {
        var player = StoryManager.Instance.Player;
        player.playerInput.enabled = open;

        Transform hingeToUse = isTopLevel ? hingeLeft : hingeRight;
        Transform otherHinge = isTopLevel ? hingeRight : hingeLeft;

        if (hingeToUse != null)
        {
            if (open)
            {
                SoundManager.Instance.PlaySFX(ESFX.Elev_Step_Up);

                // 반대쪽 문은 항상 닫힌 상태로 유지
                if (otherHinge != null)
                    otherHinge.localRotation = Quaternion.identity;

                float rotZ = isTopLevel ? 90f : -125f; 
                yield return hingeToUse
                    .DOLocalRotate(new Vector3(0, 0, rotZ), 0.5f)
                    .SetEase(Ease.InOutSine);
            }
            else
            {
                Debug.Log("elevsfx");
                SoundManager.Instance.PlaySFX(ESFX.Elev_Step_Down);

                // 닫기
                yield return hingeToUse
                    .DOLocalRotate(Vector3.zero, 0.5f)
                    .SetEase(Ease.InOutSine);
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        //TODO : 임시로 4스테이지 이상이면 항상 상호작용 허용
        if (SaveManager.Instance.MySaveData.stageIdx >= stageIdx)
        {
            guide.DOFade(1f, 0.5f).SetUpdate(true);
            interactCoroutine ??= StartCoroutine(InteractCoroutine());
        }
    }
}