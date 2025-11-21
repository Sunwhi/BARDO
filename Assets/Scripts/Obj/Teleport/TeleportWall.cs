using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TeleportWall : MonoBehaviour
{
    [SerializeField] private TeleportController controller;
    [SerializeField] private int destId;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (controller == null) return;

        if (!collision.gameObject.CompareTag("Player")) return;

        controller.Teleport(destId);
    }
}