using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class CustomContentSizeFitter : MonoBehaviour
{
    [SerializeField] private RectTransform targetRectTransform;
    [SerializeField] private float topPadding = 0f;
    [SerializeField] private float bottomPadding = 30f;

    private RectTransform self;
    private readonly Vector3[] worldCorners = new Vector3[4];

    private void OnEnable()
    {
        self = (RectTransform)transform;
        Canvas.willRenderCanvases += OnWillRenderCanvases;
        Apply();
    }

    private void OnDisable()
    {
        Canvas.willRenderCanvases -= OnWillRenderCanvases;
    }

    private void OnValidate()
    {
        if (self == null) self = (RectTransform)transform;
        Apply();
    }

    private void OnWillRenderCanvases()
    {
        Apply();
    }

    [ContextMenu("Refresh")]
    public void Refresh()
    {
        Apply();
    }

    private void Apply()
    {
        if (self == null || targetRectTransform == null) return;

        targetRectTransform.GetWorldCorners(worldCorners);

        float minLocalY = float.PositiveInfinity;
        for (int i = 0; i < 4; i++)
        {
            float ly = self.InverseTransformPoint(worldCorners[i]).y;
            if (ly < minLocalY) minLocalY = ly;
        }

        // pivot이 top(=1)이라고 가정 → 상단 엣지의 로컬 y는 0
        float requiredHeight = (-minLocalY) + topPadding + bottomPadding;
        if (requiredHeight < 0f) requiredHeight = 0f;

        // 불필요한 반복 설정 방지(지터 최소화)
        if (!Mathf.Approximately(self.rect.height, requiredHeight))
            self.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, requiredHeight);
    }
}