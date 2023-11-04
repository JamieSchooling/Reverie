using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _defaultCameraTarget;

    private Transform _currentTarget;
    private Transform _targetOnReset;
    private float _currentSpeed;

    private bool _shouldMove;

    private float _time;
    private float _timeStartedMoving;

    private Action _onTargetReached;

    private void Awake()
    {
        _targetOnReset = _defaultCameraTarget;
        _currentTarget = _defaultCameraTarget;

        ResetCamera();
    }

    private void Update()
    {
        _time += Time.unscaledDeltaTime;

        if (_shouldMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, _currentSpeed * Time.unscaledDeltaTime);

            if (transform.position == _currentTarget.position)
            {
                _shouldMove = false;
                Time.timeScale = 1f;
                if (_timeStartedMoving + 0.1f < _time) _onTargetReached?.Invoke();
            }
        }
    }

    public void SetCameraTarget(Transform target, float newSpeed = 40f, bool shouldResetToHere = true, Action onTargetReached = null)
    {
        _currentTarget = target;
        _currentSpeed = newSpeed;
        if (shouldResetToHere)
            _targetOnReset = target;

        _shouldMove = true;
        _timeStartedMoving = _time;
        _onTargetReached = onTargetReached;
        Time.timeScale = 0f;
    }

    public void ResetCamera()
    {
        transform.position = _targetOnReset.position;
        _currentTarget = _targetOnReset;
    }
}
