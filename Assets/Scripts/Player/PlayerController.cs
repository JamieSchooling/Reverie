using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask _playerLayer;
    [Header("Gravity")]
    [SerializeField] private float _gravity = 100f;
    [SerializeField] private float _maxFallSpeed = 40f;
    [Header("Movement")]
    [SerializeField] private float _jumpForce = 25f;
    [SerializeField] private float _maxSpeed = 15;
    [SerializeField] private float _acceleration = 100;
    [SerializeField] private float _deceleration = 50;

    private CapsuleCollider2D _collider;

    private Vector2 _velocity = Vector2.zero;

    private float _movementInputVector;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider2D>();

        _inputReader.OnJumpPressed += JumpPressed;
        _inputReader.OnMoveInput += (inputVector) => _movementInputVector = inputVector;
    }

    private void FixedUpdate()
    {
        Gravity();
        CalculateHorizontalMovement();

        CheckCollisions();

        ApplyFinalMovement();
    }
    
    private void CheckCollisions()
    {
        if (Physics2D.OverlapCapsule(new Vector2(transform.position.x, transform.position.y + _velocity.y), _collider.bounds.size, _collider.direction, 0f, ~_playerLayer))
        {
            while (!Physics2D.OverlapCapsule(new Vector2(transform.position.x, transform.position.y + Mathf.Sign(_velocity.y)), _collider.bounds.size, _collider.direction, 0f, ~_playerLayer))
            {
                Vector2 newPosition = new(transform.position.x, transform.position.y + Mathf.Sign(_velocity.y));
                transform.position = newPosition;
            }
            _velocity.y = 0f;
        }

        if (Physics2D.OverlapCapsule(new Vector2(transform.position.x + _velocity.x, transform.position.y), _collider.bounds.size, _collider.direction, 0f, ~_playerLayer))
        {
            while (!Physics2D.OverlapCapsule(new Vector2(transform.position.x + Mathf.Sign(_velocity.x), transform.position.y), _collider.bounds.size, _collider.direction, 0f, ~_playerLayer))
            {
                Vector2 newPosition = new(transform.position.x + Mathf.Sign(_velocity.x), transform.position.y);
                transform.position = newPosition;
            }
            _velocity.x = 0f;
        }
    }

    private bool IsGrounded()
    {
        if (Physics2D.OverlapCapsule(new Vector2(transform.position.x, transform.position.y - 0.4f), _collider.bounds.size, _collider.direction, 0f, ~_playerLayer))
        {
            return true;
        }
        return false;
    }

    private void Gravity()
    {
        _velocity.y = Mathf.MoveTowards(_velocity.y, -_maxFallSpeed, _gravity * Time.fixedDeltaTime);
    }

    private void JumpPressed()
    {
        if (IsGrounded())
            _velocity.y = _jumpForce;
    }

    private void CalculateHorizontalMovement()
    {
        if (_movementInputVector == 0)
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0, _deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, _movementInputVector * _maxSpeed, _acceleration * Time.fixedDeltaTime);
        }
    }

    private void ApplyFinalMovement()
    {
        Vector2 newPosition = new(transform.position.x + _velocity.x, transform.position.y + _velocity.y);
        transform.position = newPosition;
    }
}
