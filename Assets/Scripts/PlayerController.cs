using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    public enum WaterState
    {
        Walking,
        Falling
    }

    [Header("Movement Settings")]
    [Tooltip("The movement speed of the player.")]
    [SerializeField] private float speed = 10f;
    [Tooltip("The vertical force applied when jumping.")]
    [SerializeField] private float jumpForce = 5f;
    [Tooltip("The default gravity scale of the player.")]
    [SerializeField] private float defaultGravity = 1.5f;
    [Tooltip("How much movement control is retained while in the air.")]
    [SerializeField] private float jumpHorizontalMultiplier = 0.5f;
    [SerializeField] private float lighterOffset = -0.7f;
    [SerializeField] private float burningOffset = 0.5f;

    [Header("Death & Respawn")]
    [Tooltip("The prefab instantiated when the player dies.")]
    [SerializeField] private GameObject deathObjectPrefab;
    [Tooltip("The position where the player respawns.")]
    [SerializeField] private Transform respawnPoint;

    [Header("References")]
    [Tooltip("Reference to the player's Rigidbody2D.")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Sprites & Visuals")]
    [Tooltip("Main sprite renderer for the player.")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Tooltip("Sprite used when the player is walking.")]
    [SerializeField] private Sprite walkingSprite;
    [Tooltip("Sprite used when the player is in the 'Falling' state (heavy).")]
    [SerializeField] private Sprite fallingSprite;
    [Space(5)]
    [Tooltip("Visual representation of the lighter (Lighter) item.")]
    [SerializeField] private SpriteRenderer lighterRenderer;
    [Tooltip("Visual representation of the burning state.")]
    [SerializeField] private SpriteRenderer burningRenderer;

    [Header("Collision & Layers")]
    [Tooltip("Transform used to check for ground.")]
    [SerializeField] private Transform groundCheckTransform;
    [Tooltip("Radius of the ground check sphere/box.")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [Space(5)]
    [Tooltip("The layer(s) considered as solid ground.")]
    [SerializeField] private LayerMask groundLayer;
    [Tooltip("The layer(s) considered as water.")]
    [SerializeField] private LayerMask waterLayer;
    [Tooltip("The layer(s) considered as slippery surfaces.")]
    [SerializeField] private LayerMask slipperyLayer;

    private InputSystem_Actions _actions;
    private Collider2D _playerCollider;
    private float _moveInput;
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isOnWater;
    private bool _isOnSlipperySurface;
    private Vector2 _slipperyNormal;
    private bool _isRotated;
    private bool _hasFlintAndSteel;
    private bool _hasLighter;
    private bool _isBurning = false;
    private bool _isAlreadyDead = false;
    private WaterState _currentWaterState = WaterState.Walking;
    public bool IsOnWater => _isOnWater;

    #region Unity Lifecycle

    private void Awake()
    {
        _actions = new InputSystem_Actions();
        _playerCollider = GetComponent<Collider2D>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        originalLighterLocalXPosition = lighterRenderer.transform.localPosition.x;
        UpdateItemVisibility();
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
            if (_playerCollider != null)
            {
                Vector2 boxSize = new Vector2(_playerCollider.bounds.size.x * 0.95f, 0.1f);
                Gizmos.DrawWireCube(groundCheckTransform.position + Vector3.down * groundCheckRadius / 2f, new Vector3(boxSize.x, 0.1f + groundCheckRadius, 1f));
            }
            else
            {
                Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
            }
        }
    }

    #endregion

    #region Public Methods

    public void SetHasFlintAndSteel(bool state)
    {
        Debug.Log($"[PlayerController2D] Flint and Steel state set to: {state}");
        _hasFlintAndSteel = state;
        _hasLighter = state; // Assuming Lighter and Flint and Steel are related/same for this jam
        UpdateItemVisibility();
    }

    public void SetHasLighter(bool state)
    {
        Debug.Log($"[PlayerController2D] Lighter state set to: {state}");
        _hasLighter = state;
        UpdateItemVisibility();
    }

    public void SetWaterState(WaterState state)
    {
        Debug.Log($"[PlayerController2D] Water state changed to: {state}");
        _currentWaterState = state;
        
        if (state == WaterState.Walking)
        {
            rb.excludeLayers &= ~waterLayer;
            rb.gravityScale = defaultGravity;
            if (spriteRenderer != null && walkingSprite != null)
            {
                spriteRenderer.sprite = walkingSprite;
            }
        }
        else if (state == WaterState.Falling)
        {
            if (spriteRenderer != null && fallingSprite != null)
            {
                spriteRenderer.sprite = fallingSprite;
            }
        }
        
        UpdateItemVisibility();
    }

    public void SetBurning(bool state)
    {
        Debug.Log($"[PlayerController2D] Burning state set to: {state}");
        _isBurning = state;
        UpdateBurningVisibility();
    }

    public void Die()
    {
        if (_isAlreadyDead)
        {
            return;
        }
        _isAlreadyDead = true;
        SetBurning(false);

        Debug.Log("[PlayerController2D] Player died.");
        
        if (deathObjectPrefab != null)
        {
            Debug.Log("DeathPreFab reached!");
            Instantiate(deathObjectPrefab, transform.position, transform.rotation);
        }

        Respawn();
    }

    #endregion

    #region Input Callbacks

    private void OnMovement(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>().x;
    }

    private void OnJumping(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && (_isGrounded || _isOnWater) && _currentWaterState == WaterState.Walking && !_isOnSlipperySurface)
        {
            Debug.Log("[PlayerController2D] Jump performed.");
            rb.linearVelocityY = jumpForce;
        }
    }

    private void OnRotatePlayer(InputAction.CallbackContext ctx)
    {
        if (_isOnSlipperySurface)
        {
            return;
        }
        
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
            if (_hasFlintAndSteel || _hasLighter)
            {
                Debug.Log("[PlayerController2D] Burn death object attempt.");
                
                // Find all colliders in a small radius around the player
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);
                foreach (var collider in colliders)
                {
                    if (collider.gameObject.name.Contains("DeathObject") || (deathObjectPrefab != null && collider.gameObject.name.Contains(deathObjectPrefab.name)))
                    {
                        Debug.Log($"[PlayerController2D] Burning and destroying: {collider.gameObject.name}");
                        
                        SpawnCorpse spawnCorpse = collider.gameObject.GetComponent<SpawnCorpse>();
                        if (spawnCorpse != null)
                        {
                            spawnCorpse.ShowBurnSprite();
                        }
                        
                        Destroy(collider.gameObject, 2f);
                        break; // Destroy one at a time
                    }
                }
            }
        }
    }

    #endregion

    #region Private Methods

    private float originalLighterLocalXPosition;
    
    private void UpdateItemVisibility()
    {
        if (lighterRenderer != null)
        {
            lighterRenderer.enabled = _hasLighter;
            if (WaterState.Falling == _currentWaterState && lighterRenderer.enabled)
            {
                lighterRenderer.transform.localPosition = new Vector3(originalLighterLocalXPosition + lighterOffset, lighterRenderer.transform.localPosition.y, lighterRenderer.transform.localPosition.z);
            }
            else
            {
                lighterRenderer.transform.localPosition = new Vector3(originalLighterLocalXPosition, lighterRenderer.transform.localPosition.y, lighterRenderer.transform.localPosition.z);
            }
        }
        UpdateBurningVisibility();
    }

    private void UpdateBurningVisibility()
    {
        if (burningRenderer != null)
        {
            burningRenderer.enabled = _isBurning;
            // Maybe the burning sprite should also follow some offsets? 
            // For now, let's keep it simple as asked.
        }
    }

    private void Respawn()
    {
        transform.position = respawnPoint.position;
        rb.linearVelocity = Vector2.zero;

        // Reset items and states
        _hasFlintAndSteel = false;
        _hasLighter = false;
        _isRotated = false;
        
        // Reset normal position
        transform.rotation = Quaternion.identity;
        SetWaterState(WaterState.Walking);
        StartCoroutine(RespawnDelay());
    }

    private IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(1f);
        _isAlreadyDead = false;
    }

    private void CheckGround()
    {
        if (_playerCollider == null)
        {
            _isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
            _isOnWater = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, waterLayer);
            
            if (_isOnWater && _isBurning)
            {
                SetBurning(false);
            }

            Collider2D slipperyCol = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, slipperyLayer);
            _isOnSlipperySurface = slipperyCol != null;
            if (_isOnSlipperySurface)
            {
                // Simple normal for circle overlap
                _slipperyNormal = Vector2.up; 
            }
            return;
        }

        // Use a BoxCast for better edge detection on ground
        Vector2 origin = groundCheckTransform.position;
        Vector2 boxSize = new Vector2(_playerCollider.bounds.size.x * 0.95f, 0.1f);
        
        _isGrounded = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.down, groundCheckRadius, groundLayer).collider != null;
        _isOnWater = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.down, groundCheckRadius, waterLayer).collider != null;

        if (_isOnWater && _isBurning)
        {
            SetBurning(false);
        }

        RaycastHit2D slipperyHit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.down, groundCheckRadius, slipperyLayer);
        _isOnSlipperySurface = slipperyHit.collider != null;
        if (_isOnSlipperySurface)
        {
            _slipperyNormal = slipperyHit.normal;
        }
    }

    private void CheckWallTouch()
    {
        if (_playerCollider == null || Mathf.Abs(_moveInput) < 0.01f)
        {
            _isTouchingWall = false;
            return;
        }
        
        // Use a BoxCast for better edge detection (e.g. hitting small blocks with feet/head)
        // We use a height slightly smaller than the collider to avoid floor/ceiling hits.
        float direction = _moveInput > 0 ? 1 : -1;
        Vector2 origin = _playerCollider.bounds.center;
        
        // Shrink the height to avoid hitting ground/ceilings accidentally
        // Shrink the width to avoid hitting things behind us if the collider is weirdly shaped
        Vector2 boxSize = new Vector2(_playerCollider.bounds.size.x * 0.5f, _playerCollider.bounds.size.y * 0.8f);
        float castDistance = _playerCollider.bounds.extents.x + 0.05f - (boxSize.x / 2f);
        
        RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.right * direction, castDistance, groundLayer);
        _isTouchingWall = hit.collider != null;
    }

    
    
    private void HandleWaterFalling()
    {
        if (_isOnWater && _currentWaterState == WaterState.Falling)
        {
            Debug.Log("[PlayerController2D] Falling through water.");
            rb.gravityScale = defaultGravity - 0.5f;
            rb.excludeLayers |= waterLayer;
        }

        if (_isGrounded && _currentWaterState == WaterState.Walking)
        {
            rb.gravityScale = defaultGravity;
        }
    }

    private void ApplyMovement()
    {
        if (_isOnSlipperySurface)
        {
            // If on a slope, let's nudge the player in the direction of the slope
            // If the normal is perfectly vertical (Vector2.up), we might still want to slide if there was velocity.
            // But if it's a slope (normal.x != 0), we should definitely slide down.
            if (Mathf.Abs(_slipperyNormal.x) > 0.01f)
            {
                // Slide down the slope: The direction along the slope is perpendicular to the normal.
                // For a 2D slope, if normal is (nx, ny), a tangent is (ny, -nx) or (-ny, nx).
                // We want to go "down", which usually means nx and tangent.x have same sign if slope is positive? 
                // Actually, just applying a small force in the x direction of the normal might suffice to overcome static friction.
                // Or better: calculate the downward direction along the slope.
                Vector2 slideDir = new Vector2(_slipperyNormal.y, -_slipperyNormal.x);
                if (slideDir.y > 0) slideDir = -slideDir; // Ensure we go down
                
                rb.AddForce(slideDir * 5f, ForceMode2D.Force);
            }
            
            // Still disable player control
            return;
        }
        
        float currentSpeed = _isGrounded ? speed : speed * jumpHorizontalMultiplier;

        if (_isTouchingWall && !_isGrounded)
        {
            // If touching a wall and in the air, don't allow movement INTO the wall
            // but still allow movement AWAY from it or falling
            rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, 0, currentSpeed * Time.fixedDeltaTime * 10f);
        }
        else
        {
            rb.linearVelocityX = _moveInput * currentSpeed;
        }
    }

    #endregion
}
