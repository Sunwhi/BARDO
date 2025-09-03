using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerStateMachine fsm { get; private set; }
    public PlayerController controller { get; private set; }

    [Header("Components")]
    public SpriteRenderer spriteRenderer;
    public PlayerInput playerInput;
    public Rigidbody2D rb;
    public Collider2D col;
    public Animator animator;

    [Header("Player Collision Settings")]
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask disableLayer; // 이름 변경
    public float collisionHeight;
    public float collisionWidth;

    [Header("Animation")]
    [SerializeField] private PlayerAnimationData animationData;
    public PlayerAnimationData AnimationData => animationData;

    public bool isGrounded = true;
    private readonly float rayLength = 0.1f;

    private MovingPlatform curPlatform;
    
    private void Awake()
    {
        animationData.Initialize();
        controller = new PlayerController(this);
        fsm = new PlayerStateMachine(this);
    }

    private void Update()
    {
        fsm.Update();
        CheckGroundRay();
    }

    private void FixedUpdate()
    {
        fsm.FixedUpdate();
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe<PauseGameEvent>(OnPauseGame);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe<PauseGameEvent>(OnPauseGame);
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

    private void CheckGroundRay()
    {
        Vector2 center = col.bounds.center;
        Vector2 extents = col.bounds.extents;

        float bottomY = center.y - extents.y;
        Vector2 leftOrigin = new(center.x - extents.x, bottomY);
        Vector2 rightOrigin = new(center.x + extents.x, bottomY);
        Vector2 rayDir = Vector2.down;

        RaycastHit2D leftHit = Physics2D.Raycast(leftOrigin, rayDir, rayLength, groundLayer | platformLayer);
        Debug.DrawRay(leftOrigin, rayDir * rayLength, Color.red);
        RaycastHit2D rightHit = Physics2D.Raycast(rightOrigin, rayDir, rayLength, groundLayer | platformLayer);
        Debug.DrawRay(rightOrigin, rayDir * rayLength, Color.red);

        if (leftHit.collider != null || rightHit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    // 게임 pause할때 player 못 움직이게 막는다.
    private void OnPauseGame(PauseGameEvent ev)
    {
        if(ev.State == GameState.pause)
        {
            playerInput.currentActionMap.Disable();
            Debug.Log($"Action map '{playerInput.currentActionMap.name}' has been disabled.");

        }
        else if(ev.State == GameState.resume)
        {
            Debug.Log("move");
            playerInput.currentActionMap.Enable();
            Debug.Log($"Action map '{playerInput.currentActionMap.name}' has been enabled.");

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out MovingPlatform platform))
        {
            curPlatform = platform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (curPlatform != null 
            && collision.collider.GetComponent<MovingPlatform>() == curPlatform)
        {
            curPlatform = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
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