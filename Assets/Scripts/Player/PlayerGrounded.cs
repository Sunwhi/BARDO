using UnityEngine;

public class PlayerGrounded : MonoBehaviour
{
    [SerializeField] private Player player;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            player.isGrounded = true;
        }
    }
}
