using System;
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
    public float wallCheckDistance = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;
    private bool isTouchingWall;

    [SerializeField] private GameObject deathObjectPrefab;
    private bool isRotated = false;

    void Awake()
    {
        actions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += Movment;
        actions.Player.Jump.performed += Jumping;
        actions.Player.Crouch.performed += RotatePlayer;

        actions.Player.Move.canceled += Movment;
        actions.Player.Jump.canceled += Jumping;
    }

    private void OnDisable()
    {
        actions.Player.Disable();
        actions.Player.Move.performed -= Movment;
        actions.Player.Jump.performed -= Jumping;
        actions.Player.Crouch.performed -= RotatePlayer;

        actions.Player.Move.canceled -= Movment;
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

    void Movment(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>().x;
    }

    void Jumping(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded)
        { 
            rb.linearVelocityY = jumpForce;
        }
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
        
        float direction = move >= 0 ? 1 : -1;
        isTouchingWall = Physics2D.Raycast(transform.position, Vector2.right * direction, wallCheckDistance, groundLayer);

        rb.linearVelocityX = move * speed;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);

        Gizmos.color = Color.blue;
        float direction = move >= 0 ? 1 : -1;
        Gizmos.DrawRay(transform.position, Vector2.right * direction * wallCheckDistance);
    }
}