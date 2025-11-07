using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BreakObs : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D col;
    [SerializeField] private Rigidbody2D rb;

    [Header("Settings")]
    [SerializeField] private float fallDelay = 0.5f;
    [SerializeField] private float respawnDelay = 3f;
    [SerializeField] private float fadeInDuration = 1f;

    private Vector3 initPos;
    private Coroutine fallCoroutine;

    private void Awake()
    {
        rb.bodyType = RigidbodyType2D.Static; 
        
        initPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Vector2.Dot(contact.normal, Vector2.down) > 0.5f)
            {
                fallCoroutine ??= StartCoroutine(Fall());
                break;
            }
        }
    }

    private IEnumerator Fall()
    {
        float duration = fallDelay;
        float strength = 0.1f;   // 흔들림 진폭
        int vibrato = 5;         // 진동 횟수
        float randomness = 45f;   // 방향 랜덤 정도

        Tween shake = transform.DOShakePosition(
            duration,
            strength,
            vibrato,
            randomness,
            false,     // 스냅 안 함
            true       // 상대 좌표 기준
        );

        // 흔들기 끝날 때까지 대기
        yield return shake.WaitForCompletion();


        rb.bodyType = RigidbodyType2D.Dynamic;
        col.enabled = false;

        yield return new WaitForSeconds(respawnDelay - fadeInDuration);
        
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
        transform.position = initPos;

        spriteRenderer.DOColor(Color.white, fadeInDuration)
            .OnComplete(Respawn);
    }

    private void Respawn()
    {   
        col.enabled = true;
        fallCoroutine = null;
    }
}