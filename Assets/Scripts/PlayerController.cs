using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    public InputSystem_Actions actions;
    public float speed = 10;
    public float jumpForce = 5;
    private float move;
    [SerializeField] Rigidbody2D rb;
    
    public Transform groundCheckTransform;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;
    private bool isTouchingWall;
    private Collider2D playerCollider;

    [SerializeField] private GameObject deathObjectPrefab;
    private bool isRotated = false;

    void Awake()
    {
        actions = new InputSystem_Actions();
        playerCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += Movement;
        actions.Player.Jump.performed += Jumping;
        actions.Player.Crouch.performed += RotatePlayer;

        actions.Player.Move.canceled += Movement;
        actions.Player.Jump.canceled += Jumping;
    }

    private void OnDisable()
    {
        actions.Player.Disable();
        actions.Player.Move.performed -= Movement;
        actions.Player.Jump.performed -= Jumping;
        actions.Player.Crouch.performed -= RotatePlayer;

        actions.Player.Move.canceled -= Movement;
        actions.Player.Jump.canceled -= Jumping;
    }

    void RotatePlayer(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!isRotated)
            {
                // 90° Rotation
                float rotateDirection = move >= 0 ? 1 : -1;
                transform.rotation = Quaternion.Euler(0, 0, 90 * rotateDirection);
                isRotated = true;
            }
            else
            {
                transform.rotation = Quaternion.identity;
                isRotated = false;
            }
        }
    }

    public void Die()
    {
        if (deathObjectPrefab != null)
        {
            Instantiate(deathObjectPrefab, transform.position, transform.rotation);
        }
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;
        
        // Reset normal position
        transform.rotation = Quaternion.identity;
        isRotated = false;
    }

    private void Movement(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>().x;
    }

    private void Jumping(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded)
        { 
            rb.linearVelocityY = jumpForce;
        }
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
        
        CheckWallTouch();
    }

    private void CheckWallTouch()
    {
        if (playerCollider == null || Mathf.Abs(move) < 0.01f)
        {
            isTouchingWall = false;
            return;
        }
        
        // Check for walls in the movement direction
        float direction = move > 0 ? 1 : -1;
        Vector2 rayOrigin = (Vector2)transform.position + playerCollider.offset;
        float rayDistance = playerCollider.bounds.extents.x + 0.1f;
        
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * direction, rayDistance, groundLayer);
        isTouchingWall = hit.collider != null;
    }

    private void FixedUpdate()
    {
        if (isTouchingWall && !isGrounded)
        {
            // If touching a wall and in the air, don't allow movement into the wall
            // but still allow movement away from it
            
            // If move is in the direction of the raycast (the wall), zero it
            // direction in CheckWallTouch was (move >= 0 ? 1 : -1)
            // But we only want to zero it if we are actually pushing INTO that wall
            
            rb.linearVelocityX = 0;
        }
        else
        {
            rb.linearVelocityX = move * speed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
        }
    }
}