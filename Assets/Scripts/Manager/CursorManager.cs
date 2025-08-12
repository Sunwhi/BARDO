using UnityEngine;

public class CursorChanger : Singleton<CursorChanger>
{ 
    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 resolution = new Vector2(1920, 1080);
    private Vector3 mousePos = Vector3.zero;

    private void Start()
    {
        Cursor.SetCursor(cursorTexture,hotSpot,cursorMode);
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
