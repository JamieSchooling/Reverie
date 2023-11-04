using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _defaultCameraTarget;

    private Transform _currentTarget;
    private Transform _targetOnReset;
    private float _currentSpeed;

    private void Awake()
    {
        _targetOnReset = _defaultCameraTarget;
        _currentTarget = _defaultCameraTarget;

        ResetCamera();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, _currentSpeed * Time.deltaTime);
    }

    public void SetCameraTarget(Transform target, float newSpeed = 40f, bool shouldResetToHere = true)
    {
        _currentTarget = target;
        _currentSpeed = newSpeed;
        if (shouldResetToHere)
            _targetOnReset = target;
    }

    public void ResetCamera()
    {
        transform.position = _targetOnReset.position;
        _currentTarget = _targetOnReset;
    }
}
