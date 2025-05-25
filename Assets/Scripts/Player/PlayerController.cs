using UnityEngine;

public class PlayerController
{
    private readonly Player player;

    public Vector2 MoveInput { get; set; }
    public bool JumpInput { get; set; }

    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public PlayerController(Player player)
    {
        this.player = player;
    }

    public void Move()
    {
        Vector2 velocity = player.rb.linearVelocity;
        velocity.x = MoveInput.x * moveSpeed;

        if (velocity.x > 0 && IsTouchingWallRight())
        {
            velocity.x = 0;
        }
        else if (velocity.x < 0 && IsTouchingWallLeft())
        {
            velocity.x = 0;
        }

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
        player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, 0f);
        player.rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        JumpInput = false;
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(player.groundCheck.position, 0.1f, player.groundLayer);
    }

    public bool IsTouchingWallLeft()
    {
        float height = player.spriteRenderer.bounds.size.y * player.collisionHeight;
        Vector2 size = new Vector2(0.05f, height);
        Vector2 origin = player.rb.position + Vector2.left * player.collisionWidth;
        return Physics2D.OverlapBox(origin, size, 0f, player.groundLayer);
    }

    public bool IsTouchingWallRight()
    {
        float height = player.spriteRenderer.bounds.size.y * player.collisionHeight;
        Vector2 size = new Vector2(0.05f, height);
        Vector2 origin = player.rb.position + Vector2.right * player.collisionWidth;
        return Physics2D.OverlapBox(origin, size, 0f, player.groundLayer);
    }


}