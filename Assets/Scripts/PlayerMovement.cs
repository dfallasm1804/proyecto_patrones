using System;
using System.Diagnostics;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class PlayerMovement : MonoBehaviour, IMovible, ISalto, IGroundcheck, IHandleTrigger
{
    public Rigidbody2D _rb;
    private bool _isFacingRight = true;
    
    [Header("Movimiento")]
    private float _horizontalMovement;
    [SerializeField] private float _moveSpeed = 5f;

    [Header("Salto")]
    [SerializeField] float _jumpForce = 8f;
    
    // Saltos en el aire
    private int _maxJumps = 1;
    private int _jumpsRemaining = 1;

    [Header("GroundCheck")] 
    [SerializeField] private Transform _groundCheckPos;
    private Vector2 _groundCheckSize = new Vector2(0.8f, 0.1f);
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;
    
    [Header("WallCheck")]
    [SerializeField] private Transform _wallCheckPos;
    private Vector2 _wallCheckSize = new Vector2(0.1f, 0.8f);
    [SerializeField] private LayerMask _wallLayer;

    [Header("WallMovement")] 
    private float _wallSlideSpeed = 2f;
    private bool _isWallSliding = false;
    
    private bool _isWallJumping;
    private float _wallJumpDirection;
    private float _wallJumpTime = 0.5f;
    private float _wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 8f);

    [Header("Unlocks")] 
    public Collider2D doubleJumpUnlock;
    private bool hasTripleJump = false;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_horizontalMovement * _moveSpeed, _rb.linearVelocity.y);
        _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, 30); // evita que se pase de una velocidad maxima
    }

    private void Update()
    {
        IsGrounded();
        Flip();
        ProcessWallSlide();
        ProcessWallJump();
    }

    // IMovible
    public void Move(InputAction.CallbackContext context)
    {
        _horizontalMovement = context.ReadValue<Vector2>().x;
    }

    // ISalto
    public void Jump(InputAction.CallbackContext context)
    {
        if (_jumpsRemaining > 0)
        {
            if (context.performed)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
                // _rb.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
                _jumpsRemaining--;
            } 
        }
        
        // Wall jump
        if (context.performed && _wallJumpTimer > 0)
        {
            _isWallJumping = true;
            _rb.linearVelocity = new Vector2(_wallJumpDirection * wallJumpPower.x, wallJumpPower.y);

            if (transform.localScale.x != _wallJumpDirection)
            {
                Flip();
            }
            
            _wallJumpTimer = 0;
            Invoke(nameof(CancelWallJump), _wallJumpTime + 0.1f);
        } 
    }
    
    public void IsGrounded()
    {
        if (Physics2D.OverlapBox(_groundCheckPos.position, _groundCheckSize, 0, _groundLayer))
        {
            _jumpsRemaining = _maxJumps;
            _isGrounded = true;
            if (hasTripleJump)
            {
                _jumpsRemaining = _maxJumps + 1;
            }
        }
        else
        {
            _isGrounded = false;
        }
        
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(_wallCheckPos.position, _wallCheckSize, 0, _wallLayer);
    }

    private void Flip()
    {
        if (_isFacingRight && _horizontalMovement < 0 || !_isFacingRight && _horizontalMovement > 0)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }

    private void ProcessWallSlide()
    {
        if (!_isGrounded && WallCheck() && _horizontalMovement != 0)
        {
            _isWallSliding = true;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -_wallSlideSpeed));
        }
        else
        {
            _isWallSliding = false;
        }
    }

    private void ProcessWallJump()
    {
        if (_isWallSliding)
        {
            _isWallJumping = false;
            _wallJumpDirection = -transform.localScale.x;
            _wallJumpTimer = _wallJumpTime;
            
            CancelInvoke(nameof(CancelWallJump));
        } else if (_wallJumpTimer > 0)
        {
            _wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        _isWallJumping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "DoubleJumpUnlock")
        {
            Debug.Log("Triple Jump Unlock");
            hasTripleJump = true;
        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_groundCheckPos.position, _groundCheckSize);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_wallCheckPos.position, _wallCheckSize);
    }
    
}
