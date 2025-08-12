using UnityEngine;

public class CustomCursor : Singleton<CustomCursor>
{
    private void Update()
    {
        this.transform.position = Input.mousePosition;
    }
}
