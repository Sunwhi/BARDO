using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerStateMachine fsm { get; private set; }
    public PlayerController controller { get; private set; }

    [Header("Player Speed")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;

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
    public bool isDownAllowed = false;
    private readonly float rayLength = 0.1f;

    public Transform curPlatform { get; private set; }

    private Coroutine groundCheckCoroutine;

    private void Awake()
    {
        animationData.Initialize();
        controller = new PlayerController(this);
        fsm = new PlayerStateMachine(this);
    }

    private void OnEnable()
    {
        groundCheckCoroutine = StartCoroutine(CheckGroundRay());
    }

    private void Update()
    {
        fsm.Update();
    }

    private void FixedUpdate()
    {
        fsm.FixedUpdate();
    }

    private void OnDisable()
    {
        if (groundCheckCoroutine != null)
        {
            StopCoroutine(groundCheckCoroutine);
            groundCheckCoroutine = null;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        controller.MoveInput = context.ReadValue<Vector2>();
    }

    public void ForceMove(Vector2 moveInput)
    {
        controller.MoveInput = moveInput;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            controller.RunInput = true;
        }
        else if (context.canceled)
        {
            controller.RunInput = false;
        }

        fsm.OnRunEvent(controller.RunInput);
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

    private IEnumerator CheckGroundRay()
    {
        float repeatTime = 0.1f;
        WaitForSeconds groundDelay = new WaitForSeconds(repeatTime);
        Vector2 center, extents, leftOrigin, rightOrigin, rayDir = Vector2.down;
        float bottomY;
        RaycastHit2D leftHit, rightHit;
        LayerMask combinedMask = groundLayer | platformLayer;

        while (true)
        {
            center = col.bounds.center;
            extents = col.bounds.extents;
            bottomY = center.y - extents.y;

            leftOrigin = new(center.x - extents.x, bottomY);
            rightOrigin = new(center.x + extents.x, bottomY);

            leftHit = Physics2D.Raycast(leftOrigin, rayDir, rayLength, combinedMask);
            rightHit = Physics2D.Raycast(rightOrigin, rayDir, rayLength, combinedMask);

#if UNITY_EDITOR
            Debug.DrawRay(leftOrigin, rayDir * rayLength, Color.red, repeatTime);
            Debug.DrawRay(rightOrigin, rayDir * rayLength, Color.red, repeatTime);
#endif

            isGrounded = (leftHit.collider != null) | (rightHit.collider != null);
            yield return groundDelay;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (1 << collision.gameObject.layer == platformLayer)
        {
            Vector2 dirBA = collision.GetContact(0).normal;
            bool fromBelow = dirBA.y > 0f;
            if (!fromBelow) return;

            Transform platform = collision.collider.transform.parent;
            if (platform != null)
            {
                curPlatform = platform;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (curPlatform != null)
        {
            curPlatform = null;
        }
    }
}