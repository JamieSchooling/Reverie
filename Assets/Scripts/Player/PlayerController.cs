using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask _ignoreCollisionsLayers;

    [Header("Gravity")]
    [SerializeField] private float _gravity = 100f;
    [SerializeField] private float _maxFallSpeed = 40f;
    [SerializeField] private float _slowFallSpeed = 0.1f;

    [Header("Movement")]
    [SerializeField] private float _jumpForce = 25f;
    [SerializeField] private Vector2 _wallJumpForce;
    [SerializeField] private float _maxSpeed = 15;
    [SerializeField] private float _acceleration = 100;
    [SerializeField] private float _groundDeceleration = 50;
    [SerializeField] private float _airDeceleration = 1;
    [SerializeField] private float _coyoteTime = 0.5f;
    [SerializeField] private float _jumpBuffer = 0.2f;

    [Header("Dash")]
    [SerializeField] private Vector2 _dashForce;
    [SerializeField] private float _dashDuration = 0.05f;
    [SerializeField] private float _dashCooldown = 0.1f;
    [SerializeField] private DashGhost[] _dashGhosts;
    [SerializeField] private bool _isDashUnlocked = false;

    [Header("Sprites")]
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _dashSprite;
    [SerializeField] private Sprite _slowFallSprite;

    [Header("Audio")]
    [SerializeField] private AudioEventChannel _audioEventChannel;
    [SerializeField] private AudioClip _jumpSFX;
    [SerializeField] private AudioClip _landSFX;

    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private float _movementInputVector;
    private Vector2 _dashAimVector;
    private Vector2 _velocity = Vector2.zero;
    private float _currentGravity;

    private float _time;
    private float _timeJumpPressed;
    private float _timeJumpReleased;
    private float _timePlayerLeftGround;
    private float _timePlayerLeftWall;

    private bool _isGrounded = false;
    private bool _isCoyoteAvailable = false;
    private bool _isJumpBufferAvailable = false;
    private bool _isOnWall = false;
    private float _directionFacingWall;
    private bool _canMove = true;
    private bool _isDashing = false;
    private bool _canDash = true;

    private bool _isFacingRight = true;

    private bool CanUseCoyote =>
        _isCoyoteAvailable
        && !_isGrounded && !_isOnWall
        && _time < _timePlayerLeftGround + _coyoteTime;
    private bool CanUseWallCoyote =>
        _isCoyoteAvailable
        && !_isGrounded && !_isOnWall
        && _time < _timePlayerLeftWall + _coyoteTime;
    private bool HasBufferedJump =>
        _isJumpBufferAvailable
        && _time < _timeJumpPressed + _jumpBuffer
        && _time - _timeJumpReleased > _jumpBuffer
        && _time > _timeJumpPressed + 0.1f;

    public bool IsDashUnlocked { get => _isDashUnlocked; set => _isDashUnlocked = value; }
    public bool IsSlowFalling { get; set; } = false;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _normalSprite;

        _currentGravity = _gravity;
    }

    private void OnEnable()
    {
        _inputReader.OnJumpPressed += JumpPressed;
        _inputReader.OnJumpReleased += JumpReleased;
        _inputReader.OnMoveInput += (inputVector) => _movementInputVector = inputVector;
        _inputReader.OnDashPressed += DashPressed;
        _inputReader.OnDashAimed += (aimVector) => _dashAimVector = aimVector;
    }

    private void OnDisable()
    {
        _inputReader.OnJumpPressed -= JumpPressed;
        _inputReader.OnJumpReleased -= JumpReleased;
        _inputReader.OnMoveInput -= (inputVector) => _movementInputVector = inputVector;
        _inputReader.OnDashPressed -= DashPressed;
        _inputReader.OnDashAimed -= (aimVector) => _dashAimVector = aimVector;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (!_isDashing)
            _currentGravity = _gravity;
    }

    private void FixedUpdate()
    {
        Gravity();
        CalculateHorizontalMovement();
        Flip();

        CheckCollisions();

        ApplyFinalMovement();
    }

    private void CheckCollisions()
    {
        Collider2D hitY = Physics2D.OverlapBox(new Vector2(_collider.bounds.center.x, _collider.bounds.center.y + _velocity.y), _collider.size, 0f, ~_ignoreCollisionsLayers);
        if (hitY)
        {
            Vector2 closestPoint = hitY.ClosestPoint(_collider.bounds.center);
            closestPoint.y -= _collider.offset.y;
            float sign = Mathf.Sign(_velocity.y);
            bool isInBounds = _collider.bounds.max.x > closestPoint.x && closestPoint.x > _collider.bounds.min.x;
            Vector2 newPosition = new(transform.position.x, closestPoint.y + (isInBounds ? (_collider.size.y * 0.5f * -sign) + (0.05f * -sign) : 0));
            transform.position = newPosition;

            bool doBufferedJump = false;
            if (!_isGrounded && _velocity.y <= 0f)
            {
                _audioEventChannel.RequestPlayAudio(_landSFX);

                _isGrounded = true;
                _isCoyoteAvailable = true;
                _spriteRenderer.sprite = _normalSprite;
                IsSlowFalling = false;

                StartCoroutine(EndDash());

                if (HasBufferedJump)
                    doBufferedJump = true;

                _isJumpBufferAvailable = false;

            }
            _velocity.y = 0f;

            if (doBufferedJump)
            {
                ExecuteJump();
            }
        }
        else if (_isGrounded)
        {
            _isGrounded = false;
            _timePlayerLeftGround = _time;
        }

        Collider2D hitX = Physics2D.OverlapBox(new Vector2(_collider.bounds.center.x + _velocity.x, _collider.bounds.center.y), _collider.size, 0f, ~_ignoreCollisionsLayers);
        if (hitX)
        {
            Vector2 closestPoint = hitX.ClosestPoint(_collider.bounds.center);
            float sign = Mathf.Sign(_velocity.x);
            bool isInBounds = _collider.bounds.max.y > closestPoint.y && closestPoint.y > _collider.bounds.min.y;
            Vector2 newPosition = new(closestPoint.x + (isInBounds ? (_collider.size.x * 0.5f * -sign) + (0.05f * -sign) : 0), transform.position.y);
            transform.position = newPosition;

            if (!_isOnWall)
            {
                _isOnWall = true;
                _isCoyoteAvailable = true;
                _directionFacingWall = sign;
            }
            _velocity.x = 0f;
        }
        else if (_isOnWall)
        {
            _isOnWall = false;
            _timePlayerLeftWall = _time;
        }
    }

    private void Gravity()
    {
        if (IsSlowFalling)
        {
            _spriteRenderer.sprite = _slowFallSprite;
            _velocity.y = Mathf.MoveTowards(_velocity.y, -_slowFallSpeed, _currentGravity * Time.fixedDeltaTime);
        }
        else if (_isOnWall && !_isGrounded)
        {
            _velocity.y = Mathf.MoveTowards(_velocity.y, -(_maxFallSpeed * 0.1f), _currentGravity * Time.fixedDeltaTime);
        }
        else
        {
            _velocity.y = Mathf.MoveTowards(_velocity.y, -_maxFallSpeed, _currentGravity * Time.fixedDeltaTime);
        }
    }

    private void JumpPressed()
    {
        if ((_isGrounded || CanUseCoyote) && !_isDashing)
        {
            _timeJumpPressed = _time;
            ExecuteJump();
        }
        else if ((_isOnWall || CanUseWallCoyote) && !_isGrounded)
        {
            _timeJumpPressed = _time;
            ExecuteWallJump();
        }
    }

    private void ExecuteJump()
    {
        _audioEventChannel.RequestPlayAudio(_jumpSFX);

        _velocity.y = _jumpForce;
        _isCoyoteAvailable = false;
        _isJumpBufferAvailable = true;
    }
    
    private void ExecuteWallJump()
    {
        _audioEventChannel.RequestPlayAudio(_jumpSFX);

        _velocity.x = _wallJumpForce.x * -_directionFacingWall;
        _velocity.y = _wallJumpForce.y;
        _isCoyoteAvailable = false;
    }

    private void JumpReleased()
    {
        if (_velocity.y > 0 && !_isDashing)
        {
            _velocity.y = 0f;
            _timeJumpReleased = _time;
        }
    }

    private void DashPressed()
    {
        if (!_isDashing && _canDash && IsDashUnlocked)
            StartCoroutine(ExecuteDash());
    }

    private IEnumerator ExecuteDash()
    {
        _isDashing = true;
        _canDash = false;
        _canMove = false;
        _spriteRenderer.sprite = _dashSprite;
        _currentGravity = 0f;
        _velocity.x = 0f;
        if (_dashAimVector.sqrMagnitude == 0)
        {
            _velocity.y = 0.01f;
            _velocity.x = _dashForce.x * (_isFacingRight ? 1f : -1f);
        }
        else if (_dashAimVector.y == 0)
        {
            _velocity.y = 0.01f;
            _velocity.x = _dashForce.x * _dashAimVector.x;
        }
        else
        {
            _velocity = _dashAimVector * _dashForce;
        }

        StartCoroutine(ShowDashGhosts());

        yield return new WaitForSeconds(_dashDuration);
        _currentGravity = _gravity;
        _canMove = true;

    }

    private IEnumerator ShowDashGhosts()
    {
        for (int i = 0; i < _dashGhosts.Length; i++)
        {
            _dashGhosts[i].transform.position = transform.position;
            _dashGhosts[i].transform.rotation = transform.rotation;
            _dashGhosts[i].Sprite = _spriteRenderer.sprite;
            _dashGhosts[i].gameObject.SetActive(true);

            yield return new WaitForSeconds(_dashDuration / _dashGhosts.Length);
        }
    }

    private IEnumerator EndDash()
    {
        _isDashing = false;
        _velocity.x = 0;
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }

    private void CalculateHorizontalMovement()
    {
        if (!_canMove) return;

        if (_movementInputVector == 0)
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0, (_isGrounded ? _groundDeceleration : _airDeceleration) * Time.fixedDeltaTime);
        }
        else
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, _movementInputVector * _maxSpeed, _acceleration * Time.fixedDeltaTime);
        }
    }

    private void Flip()
    {
        Vector3 rotation = transform.localEulerAngles;
        if (_velocity.x > 0)
        {
            rotation.y = 0f;
            _isFacingRight = true;
        }
        else if (_velocity.x < 0)
        {
            rotation.y = 180f;
            _isFacingRight = false;
        }
        transform.localEulerAngles = rotation;
    }

    private void ApplyFinalMovement()
    {
        Vector2 newPosition = new(transform.position.x + _velocity.x, transform.position.y + _velocity.y);
        transform.position = newPosition;
    }

    public void ResetVelocity()
    {
        _velocity = Vector2.zero;
    }
}
