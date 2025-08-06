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
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        col.enabled = false;

        yield return new WaitForSeconds(respawnDelay - fadeInDuration);
        
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
        transform.position = initPos;

        spriteRenderer.DOColor(Color.black, fadeInDuration)
            .OnComplete(Respawn);
    }

    private void Respawn()
    {   
        col.enabled = true;
        fallCoroutine = null;
    }
}