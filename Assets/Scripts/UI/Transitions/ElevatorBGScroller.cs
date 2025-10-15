using UnityEngine;
using UnityEngine.UI;

public class ElevatorBGScroller : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private RectTransform imgRectTransform;

    public float scrollSpeed = 200f;

    private float imgHeight;
    private float resetPositionY;

    void Start()
    {
        imgHeight = imgRectTransform.rect.height;
        resetPositionY = imgHeight * 2f;
    }

    // Update is called once per frame
    void Update()
    {
        imgRectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        if(imgRectTransform.anchoredPosition.y >= imgHeight)
        {
            imgRectTransform.anchoredPosition -= new Vector2(0, resetPositionY);
        }
    }
}
