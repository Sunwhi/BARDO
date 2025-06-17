using UnityEngine;

public class RainScroller : MonoBehaviour
{
    public Material rainMaterial;      // �� �ؽ�ó�� �� ��Ƽ����
    public float scrollSpeed = -0.1f;  // �� -> �Ʒ��� �帧

    private Vector2 offset = Vector2.zero;

    void Update()
    {
        offset.y += scrollSpeed * Time.deltaTime;
        rainMaterial.mainTextureOffset = offset;
    }
}