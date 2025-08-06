using UnityEngine;

public class CursorChanger : Singleton<CursorChanger>
{ 
    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {
        Cursor.SetCursor(cursorTexture,hotSpot,cursorMode);
    }
}
