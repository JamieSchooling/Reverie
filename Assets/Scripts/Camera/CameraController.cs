using System;
using UnityEngine;

public class CameraController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Transform _defaultCameraTarget;

    private Vector3 _currentTarget;
    private Vector3 _targetOnReset;
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
        Debug.Log("Loading Camera Data");
        _targetOnReset = data.cameraTargetOnReset;
        Debug.Log(_targetOnReset);

        if (_targetOnReset == Vector3.zero) _targetOnReset = _defaultCameraTarget.position;

        _currentTarget = _targetOnReset;
        Debug.Log(_currentTarget);

        transform.position = _currentTarget;
        Debug.Log(transform.position);
    }

    public void SaveData(ref GameData data)
    {
        data.cameraTargetOnReset = _targetOnReset;
    }
}
