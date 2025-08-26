using UnityEngine;

public class UICanvas : MonoBehaviour
{
    protected virtual void Start()
    {
        UIManager.SetCanvas(transform);
    }
}