using System;
using UnityEngine;

public class CameraController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Transform _defaultCameraTarget;
    [SerializeField] private bool _shouldSaveResetPoint = true;

    private Vector3 _currentTarget;
    private Vector3 _targetOnReset = Vector3.zero;
    private float _currentSpeed;

    private bool _shouldMove;

    private float _time;
    private float _timeStartedMoving;

    private Action _onTargetReached;

    private void Update()
    {
        _time += Time.unscaledDeltaTime;

        if (_shouldMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget, _currentSpeed * Time.unscaledDeltaTime);

            if (transform.position == _currentTarget)
            {
                _shouldMove = false;
                Time.timeScale = 1f;
                if (_timeStartedMoving + 0.1f < _time) _onTargetReached?.Invoke();
            }
        }
    }

    public void SetCameraTarget(Transform target, float newSpeed = 40f, bool shouldResetToHere = true, Action onTargetReached = null)
    {
        _currentTarget = target.position;
        _currentSpeed = newSpeed;
        if (shouldResetToHere)
            _targetOnReset = target.position;

        _shouldMove = true;
        _timeStartedMoving = _time;
        _onTargetReached = onTargetReached;
        Time.timeScale = 0f;
    }

    public void ResetCamera()
    {
        transform.position = _targetOnReset;
        _currentTarget = _targetOnReset;
    }

    public void LoadData(GameData data)
    {
        if (_shouldSaveResetPoint) _targetOnReset = data.cameraTargetOnReset;

        if (_targetOnReset == Vector3.zero) _targetOnReset = _defaultCameraTarget.position;

        _currentTarget = _targetOnReset;

        transform.position = _currentTarget;
    }

    public void SaveData(ref GameData data)
    {
         if (_shouldSaveResetPoint) data.cameraTargetOnReset = _targetOnReset;
    }
}
