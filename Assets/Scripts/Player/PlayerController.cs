using UnityEngine;

public class PlayerController
{
    private readonly Player player;

    public Vector2 MoveInput { get; set; }
    public bool JumpInput { get; set; }
    public bool InputEnabled { get; set; } = true;

    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public PlayerController(Player player)
    {
        this.player = player;
    }

    public void Move()
    {
        if (!InputEnabled) return;

        Vector2 velocity = player.rb.linearVelocity;
        velocity.x = MoveInput.x * moveSpeed;

        player.rb.linearVelocity = velocity;

        if (MoveInput.x != 0)
        {
            Vector3 scale = player.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (MoveInput.x > 0 ? 1 : -1);
            player.transform.localScale = scale;
        }
    }

    public void Jump()
    {
        if (!InputEnabled) return;
        if (!player.isGrounded) return;

        Vector2 velocity = player.rb.linearVelocity;
        velocity.y = 0f;
        player.rb.linearVelocity = velocity;

        player.rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void ResetInput()
    {
        if (player.transform.localScale.x < 0)
        {
            Vector3 scale = player.transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            player.transform.localScale = scale;
        }
        MoveInput = Vector2.zero;
        JumpInput = false;
    }
}