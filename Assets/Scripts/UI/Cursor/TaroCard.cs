using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TaroCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 참조만 인스펙터에서 설정
    [SerializeField] RectTransform overlayRoot;
    [SerializeField] Sprite openedSprite;
    [SerializeField] Button fadeBtn;
    [SerializeField] Button selectBtn;
    [SerializeField] Image img;

    RectTransform target;
    Sprite closedSprite;
    Vector3 baseScale;
    GameObject backdrop;
    bool isExpanded = false;

    // 원상 복구용 스냅샷
    Transform origParent;
    int origSibling;
    Vector2 origAnchorMin, origAnchorMax, origPivot, origAnchoredPos;
    Vector3 origScale;
    Quaternion origRot;
    Vector3 origWorldPos;

    public void OnBtnClicked() => Expand();

    void Awake()
    {
        target = transform as RectTransform;
        closedSprite = img ? img.sprite : null;
        baseScale = target.localScale;

        if (selectBtn != null)
        {
            selectBtn.onClick.RemoveAllListeners();
            selectBtn.onClick.AddListener(Collapse);
        }
    }

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
        origRot = target.localRotation;
        origWorldPos = target.position;

        // 상호작용 정리
        target.DOKill(true);

        // 백드롭 준비
        if (fadeBtn != null)
        {
            fadeBtn.gameObject.SetActive(true);
            fadeBtn.onClick.RemoveAllListeners();
            fadeBtn.onClick.AddListener(Collapse);
        }
        target.SetParent(overlayRoot, true);
        
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

        // 계층 및 Rect 복원
        target.SetParent(origParent, true);
        target.SetSiblingIndex(origSibling);
        fadeBtn.gameObject.SetActive(false);


        var seq = DOTween.Sequence().SetUpdate(true).SetLink(gameObject);

        // 플립(후면 복원)
        seq.Append(target.DORotate(new Vector3(0f, 90f, 0f), 0.12f).SetEase(Ease.InCubic));
        seq.AppendCallback(() => { img.sprite = closedSprite; });
        seq.Append(target.DORotate(Vector3.zero, 0.12f).SetEase(Ease.OutCubic));

        // 원위치 이동+축소
        seq.Join(target.DOMove(origWorldPos, 0.25f).SetEase(Ease.OutCubic));
        seq.Join(target.DOScale(origScale, 0.25f).SetEase(Ease.OutCubic));

        seq.OnComplete(() =>
        {
            target.anchorMin = origAnchorMin;
            target.anchorMax = origAnchorMax;
            target.pivot = origPivot;
            target.anchoredPosition = origAnchoredPos;
            target.localScale = origScale;
            target.localRotation = origRot;

            isExpanded = false;
        });
    }
}
