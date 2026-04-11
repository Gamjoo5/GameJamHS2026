using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    public enum WaterState
    {
        Walking,
        Falling
    }

    [Header("Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private GameObject deathObjectPrefab;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask waterLayer;

    private InputSystem_Actions _actions;
    private Collider2D _playerCollider;
    private float _moveInput;
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isOnWater;
    private bool _isRotated;
    private bool _hasFlintAndSteel;
    private WaterState _currentWaterState = WaterState.Walking;

    #region Unity Lifecycle

    private void Awake()
    {
        _actions = new InputSystem_Actions();
        _playerCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        _actions.Player.Enable();
        _actions.Player.Move.performed += OnMovement;
        _actions.Player.Jump.performed += OnJumping;
        _actions.Player.Crouch.performed += OnRotatePlayer;
        _actions.Player.Interact.performed += OnBurnDeathObject;

        _actions.Player.Move.canceled += OnMovement;
        _actions.Player.Jump.canceled += OnJumping;
    }

    private void OnDisable()
    {
        _actions.Player.Disable();
        _actions.Player.Move.performed -= OnMovement;
        _actions.Player.Jump.performed -= OnJumping;
        _actions.Player.Crouch.performed -= OnRotatePlayer;
        _actions.Player.Interact.performed -= OnBurnDeathObject;

        _actions.Player.Move.canceled -= OnMovement;
        _actions.Player.Jump.canceled -= OnJumping;
    }

    private void Update()
    {
        CheckGround();
        CheckWallTouch();
        HandleWaterFalling();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
        }
    }

    #endregion

    #region Public Methods

    public void SetHasFlintAndSteel(bool state)
    {
        Debug.Log($"[PlayerController2D] Flint and Steel state set to: {state}");
        _hasFlintAndSteel = state;
    }

    public void SetWaterState(WaterState state)
    {
        Debug.Log($"[PlayerController2D] Water state changed to: {state}");
        _currentWaterState = state;
    }

    public void Die()
    {
        Debug.Log("[PlayerController2D] Player died.");
        
        if (deathObjectPrefab != null)
        {
            Instantiate(deathObjectPrefab, transform.position, transform.rotation);
        }
        
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;
        
        // Reset normal position
        transform.rotation = Quaternion.identity;
        _isRotated = false;
        SetWaterState(WaterState.Walking);
    }

    #endregion

    #region Input Callbacks

    private void OnMovement(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>().x;
    }

    private void OnJumping(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && _isGrounded && _currentWaterState == WaterState.Walking)
        {
            Debug.Log("[PlayerController2D] Jump performed.");
            rb.linearVelocityY = jumpForce;
        }
    }

    private void OnRotatePlayer(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!_isRotated)
            {
                // 90° Rotation
                float rotateDirection = _moveInput >= 0 ? 1 : -1;
                transform.rotation = Quaternion.Euler(0, 0, 90 * rotateDirection);
                _isRotated = true;
                Debug.Log($"[PlayerController2D] Player rotated 90 degrees (direction: {rotateDirection}).");
            }
            else
            {
                transform.rotation = Quaternion.identity;
                _isRotated = false;
                Debug.Log("[PlayerController2D] Player rotation reset.");
            }
        }
    }

    private void OnBurnDeathObject(InputAction.CallbackContext ctx)
    {
        Debug.Log("[PlayerController2D] Burn death object detected.");
        if (ctx.performed)
        {
            if (_hasFlintAndSteel)
            {
                Debug.Log("[PlayerController2D] Burn death object attempt.");
                
                // Find all colliders in a small radius around the player
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);
                foreach (var collider in colliders)
                {
                    if (collider.gameObject.name.Contains("DeathObject") || (deathObjectPrefab != null && collider.gameObject.name.Contains(deathObjectPrefab.name)))
                    {
                        Debug.Log($"[PlayerController2D] Burning and destroying: {collider.gameObject.name}");
                        Destroy(collider.gameObject);
                        break; // Destroy one at a time?
                    }
                }
            }
        }
    }
    
    #endregion

    #region Private Methods

    private void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
        _isOnWater = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, waterLayer);
    }

    private void CheckWallTouch()
    {
        if (_playerCollider == null || Mathf.Abs(_moveInput) < 0.01f)
        {
            _isTouchingWall = false;
            return;
        }
        
        // Check for walls in the movement direction
        float direction = _moveInput > 0 ? 1 : -1;
        Vector2 rayOrigin = (Vector2)transform.position + _playerCollider.offset;
        float rayDistance = _playerCollider.bounds.extents.x + 0.1f;
        
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * direction, rayDistance, groundLayer);
        _isTouchingWall = hit.collider != null;
    }

    private void HandleWaterFalling()
    {
        if (_isOnWater && _currentWaterState == WaterState.Falling)
        {
            Debug.Log("[PlayerController2D] Falling through water.");
            rb.gravityScale = 0.5f;
            rb.excludeLayers |= waterLayer;
        }

        if (_isGrounded)
        {
            rb.gravityScale = 1;
        }
    }

    private void ApplyMovement()
    {
        if (_isTouchingWall && !_isGrounded)
        {
            // If touching a wall and in the air, don't allow movement into the wall
            // but still allow movement away from it
            rb.linearVelocityX = 0;
        }
        else
        {
            rb.linearVelocityX = _moveInput * speed;
        }
    }

    #endregion
}
