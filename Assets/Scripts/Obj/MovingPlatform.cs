using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private CamState activeStage;
    [SerializeField] private float speed = 4; //1일 때 1회 왕복

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform platform;

    Coroutine moveCoroutine;

    private void Update()
    {
        //TODO : Stage 정보 체크 가능할 때 활성화.

        if (activeStage == CameraManager.Instance.curCamState)
        {
            moveCoroutine ??= StartCoroutine(MoveCoroutine());
        }
        else if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    private IEnumerator MoveCoroutine()
    {
        Vector3 target = endPoint.position;

        while (true)
        {
            platform.position = Vector3.MoveTowards(platform.position, target, speed * Time.deltaTime);

            if (Vector3.Distance(platform.position, target) < 0.01f)
            {
                target = (target == endPoint.position) ? startPoint.position : endPoint.position;
            }

            yield return null;
        }
    }

    Coroutine SettleCoroutine;
    private IEnumerator WaitUntilSettled(Collider2D collision)
    {
        yield return new WaitUntil(() => StoryManager.Instance.Player.isGrounded);
        collision.transform.SetParent(platform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (SettleCoroutine != null) StopCoroutine(SettleCoroutine);
            SettleCoroutine = StartCoroutine(WaitUntilSettled(collision));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopCoroutine(SettleCoroutine);
            SettleCoroutine = null;
            collision.transform.SetParent(null);
        }
    }
}
