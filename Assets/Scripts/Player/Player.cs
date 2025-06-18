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
    public LayerMask disableLayer; // 이름 변경
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

    public void ForceMove(Vector2 moveInput)
    {
        controller.MoveInput = moveInput;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isGrounded)
                controller.JumpInput = true;
        }
        else if (context.canceled)
        {
            controller.JumpInput = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & disableLayer) != 0)
        {
            controller.InputEnabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & disableLayer) != 0)
        {
            controller.InputEnabled = true;
        }
    }
}