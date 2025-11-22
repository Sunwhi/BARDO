using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerViewport : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private CinemachineBrain brain;

    private readonly float checkInterval = 0.2f;
    private float timer = 0f;

    private bool viewportDelay = false;

    private void Awake()
    {
        if (cam == null)
            cam = Camera.main;

        brain = cam.GetComponent<CinemachineBrain>();
    }

    private void LateUpdate()
    {
        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0f;

        Bounds camBounds = GetCamBounds();
        camBounds.Expand(new Vector3(0.4f, 0.4f, 0));
        Vector3 playerPos = transform.position;
        bool outNow = !camBounds.Contains(playerPos);

        if (outNow && !viewportDelay)
        {
            Debug.Log($"outNow: {outNow} | cambounds: {camBounds} | playerPos: {playerPos}");
            StartCoroutine(AfterTransition(camBounds, playerPos));
        }
    }

    private Bounds GetCamBounds()
    {
        float zDistance = Mathf.Abs(cam.transform.position.z - transform.position.z);

        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, zDistance));

        Bounds b = new Bounds((bottomLeft + topRight) / 2, topRight - bottomLeft);
        return b;
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

            Debug.Log($"outNow: {outNow} | cambounds: {camBounds} | playerPos: {playerPos}");
        }

        viewportDelay = false;
    }
}