using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField, Range(0, 7)] private int activeStage;
    [SerializeField] private float speed = 4; //1일 때 1회 왕복

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform platform;

    Coroutine moveCoroutine;

    private void Start()
    {
        moveCoroutine = StartCoroutine(MoveCoroutine());
    }

    private void Update()
    {
        //TODO : Stage 정보 체크 가능할 때 활성화.

        //if (SaveManager.Instance.MySaveData.stageIdx == activeStage)
        //{
        //    moveCoroutine ??= StartCoroutine(MoveCoroutine());
        //}
        //else if (moveCoroutine != null)
        //{
        //    StopCoroutine(moveCoroutine);
        //    moveCoroutine = null;
        //}
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(platform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
