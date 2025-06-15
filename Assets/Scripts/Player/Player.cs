using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerStateMachine fsm { get; private set; }
    public PlayerController controller { get; private set; }

    [Header("Components")]
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Player Collision Settings")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public float collisionHeight;
    public float collisionWidth;

    [Header("Animation")]
    [SerializeField] private PlayerAnimationData animationData;
    public PlayerAnimationData AnimationData => animationData;

    [HideInInspector]
    public bool isGrounded = true;

    private void Awake()
    {
        animationData.Initialize();
        controller = new PlayerController(this);
        fsm = new PlayerStateMachine(this);
    }

    private void Update()
    {
        fsm.Update();
    }

    private void FixedUpdate()
    {
        fsm.FixedUpdate();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        controller.MoveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed || isGrounded)
        {
            controller.JumpInput = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (spriteRenderer == null) return;

        float height = spriteRenderer.bounds.size.y * collisionHeight;
        Vector2 size = new Vector2(0.05f, height);
        Vector2 leftOrigin = (Vector2)transform.position + Vector2.left * collisionWidth;
        Vector2 rightOrigin = (Vector2)transform.position + Vector2.right * collisionWidth;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(leftOrigin, size);
        Gizmos.DrawWireCube(rightOrigin, size);

        Gizmos.color = Color.red;
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
}