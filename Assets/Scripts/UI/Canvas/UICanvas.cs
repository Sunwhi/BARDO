using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] private Transform[] uiParents;

    protected virtual void Start()
    {
        UIManager.SetCanvas(uiParents);
    }
}