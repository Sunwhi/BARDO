using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TaroCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // 참조만 인스펙터에서 설정
    [SerializeField] RectTransform overlayRoot;
    [SerializeField] Sprite openedSprite;
    [SerializeField] Button selectedButton;

    RectTransform target;
    Image img;
    Sprite closedSprite;

    // 원상 복구용 스냅샷
    Transform origParent;
    int origSibling;
    Vector2 origAnchorMin, origAnchorMax, origPivot, origAnchoredPos;
    Vector3 origScale;
    Vector3 origWorldPos;

    GameObject backdrop;
    bool isExpanded;
    Vector3 baseScale;

    void Awake()
    {
        target = transform as RectTransform;
        img = GetComponent<Image>();
        closedSprite = img ? img.sprite : null;
        baseScale = target.localScale;

        if (selectedButton != null)
        {
            selectedButton.gameObject.SetActive(false);
            selectedButton.onClick.RemoveAllListeners();
            selectedButton.onClick.AddListener(Collapse);
        }
    }

    // 버튼 이벤트로 연결 가능
    public void OnBtnClicked() => Toggle();

    // 카드 자체 클릭으로도 동작
    public void OnPointerClick(PointerEventData eventData) => Toggle();

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isExpanded) return;
        target.DOKill(false);
        target.DOScale(baseScale * 1.06f, 0.18f)
              .SetEase(Ease.OutBack)
              .SetUpdate(true)
              .SetLink(gameObject, LinkBehaviour.KillOnDisable);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isExpanded) return;
        target.DOKill(false);
        target.DOScale(baseScale, 0.14f)
              .SetEase(Ease.OutQuad)
              .SetUpdate(true)
              .SetLink(gameObject, LinkBehaviour.KillOnDisable);
    }

    void OnDisable()
    {
        target.DOKill(false);
        target.localScale = baseScale;
        if (backdrop != null) backdrop.SetActive(false);
    }

    void Toggle()
    {
        if (isExpanded) Collapse();
        else Expand();
    }

    void Expand()
    {
        if (isExpanded || overlayRoot == null) return;

        // 스냅샷
        origParent = target.parent;
        origSibling = target.GetSiblingIndex();
        origAnchorMin = target.anchorMin;
        origAnchorMax = target.anchorMax;
        origPivot = target.pivot;
        origAnchoredPos = target.anchoredPosition;
        origScale = target.localScale;
        origWorldPos = target.position;

        // 상호작용 정리
        target.DOKill(true);

        // 오버레이 루트로 이동(월드 좌표 유지)
        target.SetParent(overlayRoot, true);
        target.SetAsLastSibling();

        // 백드롭 준비
        EnsureBackdrop();
        backdrop.transform.SetAsLastSibling();
        target.SetAsLastSibling();

        if (selectedButton != null)
        {
            selectedButton.gameObject.SetActive(true);
            selectedButton.transform.SetAsLastSibling();
        }

        // 중앙 월드 좌표(overlayRoot 중심)
        Vector3 centerWorld = overlayRoot.TransformPoint(Vector3.zero);

        // 이동+확대
        var seq = DOTween.Sequence().SetUpdate(true).SetLink(gameObject);
        seq.Join(target.DOMove(centerWorld, 0.25f).SetEase(Ease.OutCubic));
        seq.Join(target.DOScale(baseScale * 1.35f, 0.25f).SetEase(Ease.OutCubic));

        // 플립(전면 교체)
        seq.Append(target.DORotate(new Vector3(0f, 90f, 0f), 0.12f).SetEase(Ease.InCubic));
        seq.AppendCallback(() => { if (img != null && openedSprite != null) img.sprite = openedSprite; });
        seq.Append(target.DORotate(Vector3.zero, 0.12f).SetEase(Ease.OutCubic));

        seq.OnComplete(() => { isExpanded = true; });
    }

    void Collapse()
    {
        if (!isExpanded) return;

        target.DOKill(true);

        if (selectedButton != null)
            selectedButton.gameObject.SetActive(false);

        var seq = DOTween.Sequence().SetUpdate(true).SetLink(gameObject);

        // 플립(후면 복원)
        seq.Append(target.DORotate(new Vector3(0f, 90f, 0f), 0.12f).SetEase(Ease.InCubic));
        seq.AppendCallback(() => { if (img != null && closedSprite != null) img.sprite = closedSprite; });
        seq.Append(target.DORotate(Vector3.zero, 0.12f).SetEase(Ease.OutCubic));

        // 원위치 이동+축소
        seq.Join(target.DOMove(origWorldPos, 0.25f).SetEase(Ease.OutCubic));
        seq.Join(target.DOScale(origScale, 0.25f).SetEase(Ease.OutCubic));

        seq.OnComplete(() =>
        {
            // 계층 및 Rect 복원
            target.SetParent(origParent, true);
            target.SetSiblingIndex(origSibling);
            target.anchorMin = origAnchorMin;
            target.anchorMax = origAnchorMax;
            target.pivot = origPivot;
            target.anchoredPosition = origAnchoredPos;
            target.localScale = origScale;
            target.localRotation = Quaternion.identity;

            DestroyBackdrop();
            isExpanded = false;
        });
    }

    void EnsureBackdrop()
    {
        if (backdrop != null) { backdrop.SetActive(true); return; }

        backdrop = new GameObject("CardBackdrop", typeof(RectTransform), typeof(Image), typeof(Button));
        var rt = backdrop.GetComponent<RectTransform>();
        rt.SetParent(overlayRoot, false);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        var imgBg = backdrop.GetComponent<Image>();
        imgBg.color = new Color(0f, 0f, 0f, 0f); // 투명 클릭 차단막

        var btn = backdrop.GetComponent<Button>();
        btn.transition = Selectable.Transition.None;
        btn.onClick.AddListener(Collapse);
    }

    void DestroyBackdrop()
    {
        if (backdrop != null) Destroy(backdrop);
        backdrop = null;
    }
}
