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

        if (velocity.x > 0 && IsTouchingWall(Vector2.right))
            velocity.x = 0;
        else if (velocity.x < 0 && IsTouchingWall(Vector2.left))
            velocity.x = 0;

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

        Vector2 velocity = player.rb.linearVelocity;
        velocity.y = 0f;
        player.rb.linearVelocity = velocity;

        player.rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        JumpInput = false;
    }

    public bool IsTouchingWall(Vector2 direction)
    {
        float height = player.spriteRenderer.bounds.size.y * player.collisionHeight;
        Vector2 size = new Vector2(0.05f, height);
        Vector2 origin = player.rb.position + direction.normalized * player.collisionWidth;
        return Physics2D.OverlapBox(origin, size, 0f, player.groundLayer);
    }

    public void ResetInput()
    {
        MoveInput = Vector2.zero;
        JumpInput = false;
    }
}