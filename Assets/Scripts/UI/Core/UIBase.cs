using UnityEngine;
using UnityEngine.Events;

public class UIBase : MonoBehaviour
{
    public bool isDestroyAtClosed = false;

    public UnityAction<object[]> opened;
    public UnityAction<object[]> closed;

    protected virtual void Awake()
    {
        opened += Opened;
        closed += Closed;
    }

    public virtual void Opened(object[] param) { }
    public virtual void Closed(object[] param) { }

    public virtual void OnUICloseBtn()
    {
        UIManager.Hide();
    }
}