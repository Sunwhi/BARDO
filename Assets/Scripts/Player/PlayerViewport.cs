using System.Collections;
using UnityEngine;

public class PlayerViewport : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private readonly float checkInterval = 0.2f;
    private float timer = 0f;

    private bool viewportDelay = false;

    private void Awake()
    {
        if (cam == null)
            cam = Camera.main;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0f;

        Bounds camBounds = GetCamBounds();
        camBounds.Expand(new Vector3(0.6f, 0, 0));
        Vector3 playerPos = transform.position;
        bool outNow = !camBounds.Contains(playerPos);

        if (outNow && !viewportDelay)
        {
            StartCoroutine(AfterTransition(camBounds, playerPos));
        }
    }

    private Bounds GetCamBounds()
    {
        float zDistance = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, zDistance));
        return new Bounds((bottomLeft + topRight) / 2, topRight - bottomLeft);
    }

    private ViewportExitDirection GetExitDirection(Bounds bounds, Vector3 pos)
    {
        if (pos.y < bounds.min.y) return ViewportExitDirection.Bottom;
        if (pos.y > bounds.max.y) return ViewportExitDirection.Top;
        if (pos.x > bounds.max.x) return ViewportExitDirection.Right;
        return ViewportExitDirection.Left;
    }

    private IEnumerator AfterTransition(Bounds camBounds, Vector3 playerPos)
    {
        viewportDelay = true;
        bool outNow = true;

        while (outNow)
        {
            var direction = GetExitDirection(camBounds, playerPos);
            GameEventBus.Raise(new ViewportExitEvent(transform.position, direction));
            yield return new WaitForSeconds(0.5f);

            camBounds = GetCamBounds();
            camBounds.Expand(new Vector3(0.6f, 0, 0));
            playerPos = transform.position;
            outNow = !camBounds.Contains(playerPos);
        }

        viewportDelay = false;
    }
}   