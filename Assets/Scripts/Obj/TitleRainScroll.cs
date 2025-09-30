using UnityEngine;

public class TitleRainScroll : MonoBehaviour
{
    public float scrollSpeed = -0.1f;

    [SerializeField] Renderer meshRenderer;
    private Vector2 offset;

    private void Start()
    {
        offset = meshRenderer.material.mainTextureOffset;
    }
    private void Update()
    {
        offset.y += scrollSpeed * Time.unscaledDeltaTime;

        meshRenderer.material.mainTextureOffset = offset;
    }
}
