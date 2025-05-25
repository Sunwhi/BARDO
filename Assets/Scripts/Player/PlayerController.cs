using UnityEngine;

public class PlayerController
{
    private readonly Player player;

    public Vector2 MoveInput { get; set; }
    public bool JumpInput { get; set; }

    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    public PlayerController(Player player)
    {
        this.player = player;
    }

    public void Move()
    {
        player.rb.linearVelocity = new Vector2(MoveInput.x * moveSpeed, player.rb.linearVelocity.y);

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
}