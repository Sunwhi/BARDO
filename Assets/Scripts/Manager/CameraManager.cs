using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private Player player;

    private void OnEnable()
    {
        GameEventBus.Subscribe<ViewportExitEvent>(OnViewportExit);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<ViewportExitEvent>(OnViewportExit);
    }

    private void OnViewportExit(ViewportExitEvent e)
    {
        if (e.direction == ViewportExitDirection.Left)
        {
            GoBack();
        }
        else if (e.direction == ViewportExitDirection.Right)
        {
            GoFront();
        }
    }

    public void GoFront()
    {
        cameraAnimator.SetTrigger("GoFront");
    }

    public void GoBack()
    {
        cameraAnimator.SetTrigger("GoBack");
    }
}
