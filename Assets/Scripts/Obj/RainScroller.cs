using UnityEngine;

public class RainScroller : MonoBehaviour
{
    public Material rainMaterial;      // 비 텍스처가 들어간 머티리얼
    public float scrollSpeed = -0.1f;  // 위 -> 아래로 흐름

    private Vector2 offset = Vector2.zero;

    void Update()
    {
        offset.y += scrollSpeed * Time.deltaTime;
        rainMaterial.mainTextureOffset = offset;
    }
}