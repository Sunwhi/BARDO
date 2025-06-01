using UnityEngine;

public enum ViewportExitDirection
{
    Left,
    Right,
    Top,
    Bottom
}

public class ViewportExitEvent : IGameEvent
{
    public Vector3 worldPosition;
    public ViewportExitDirection direction;

    public ViewportExitEvent(Vector3 pos, ViewportExitDirection dir)
    {
        worldPosition = pos;
        direction = dir;
    }
}
