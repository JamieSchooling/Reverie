using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _jumpForce = 25f;
    [SerializeField] private float _gravity = 100f;
    [SerializeField] private float _maxFallSpeed = 40f;
    [SerializeField] private float _collisionCheckDistance = 0.05f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _ceilingCheck;

    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;

    private Vector2 _velocity = Vector2.zero;
    private bool _isGrounded = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();

        _inputReader.OnJumpPressed += JumpPressed;
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        Gravity();

        ApplyMovement();
    }

    private void CheckCollisions()
    {
        bool hitGroundThisFrame = Physics2D.OverlapCircle(_groundCheck.position, _collisionCheckDistance, ~_playerLayer);
        bool hitCeilingThisFrame = Physics2D.OverlapCircle(_ceilingCheck.position, _collisionCheckDistance, ~_playerLayer);


        if (hitCeilingThisFrame)
        {
            _velocity.y = Mathf.Min(0, _velocity.y);
        }

        if (!_isGrounded && hitGroundThisFrame)
        {
            _isGrounded = true;
            RaycastHit2D hit = Physics2D.Raycast(_collider.bounds.center, Vector2.down, _collider.size.y * 0.5f, ~_playerLayer);
            if (hit.collider != null)
            {
                float distanceSunkInGround = hit.collider.bounds.max.y - _collider.bounds.min.y;
                Vector2 newPosition = new(_rigidbody.position.x, _rigidbody.position.y + distanceSunkInGround);
                _rigidbody.position = newPosition;
            }
        }
        else if (_isGrounded && !hitGroundThisFrame)
        {
            _isGrounded = false;
        }
    }

    private void Gravity()
    {
        if (_isGrounded && _velocity.y <= 0f)
        {
            _velocity.y = 0f;
        }
        else
        {
            _velocity.y = Mathf.MoveTowards(_velocity.y, -_maxFallSpeed, _gravity * Time.fixedDeltaTime);
        }
    }

    private void JumpPressed()
    {
        if (_isGrounded)
            _velocity.y = _jumpForce;
    }

    private void ApplyMovement()
    {
        _rigidbody.velocity = _velocity;
    }

    private void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_groundCheck.position, _collisionCheckDistance);
        }

        if (_ceilingCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_ceilingCheck.position, _collisionCheckDistance);
        }
    }
}
