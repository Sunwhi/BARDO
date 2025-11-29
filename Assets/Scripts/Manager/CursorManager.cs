using UnityEngine;

public class CursorChanger : Singleton<CursorChanger>
{
    [Header("Cursor Textures")]
    public Texture2D cursorTexture;      // 기본용 (Windows/공용)
    public Texture2D cursorTextureMac;   // macOS용 (해상도 더 작은 버전 추천)

    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {
        Texture2D texToUse = cursorTexture;

        RuntimePlatform platform = Application.platform;

        // macOS인 경우 mac 전용 텍스처가 있으면 그걸 사용
        if ((platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.OSXEditor) &&
            cursorTextureMac != null)
        {
            texToUse = cursorTextureMac;
        }

        Cursor.SetCursor(texToUse, hotSpot, cursorMode);
        // Cursor.lockState = CursorLockMode.Confined;  // 화면 밖으로 안 나가게 할 때 사용
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }*/
}