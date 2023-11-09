using System.Collections;
using UnityEngine;

public class ChapterSelectCameraController : MonoBehaviour
{
    [SerializeField] Transform _initialPosition;
    [SerializeField] private Transform _defaultCameraTarget;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _rotateSpeed = 10f;

    private Transform _currentTarget;
    private bool _shouldMove;

    private void Awake()
    {
        transform.position = _initialPosition.position;
        transform.rotation = _initialPosition.rotation;
        StartCoroutine(StartDelayed());

        IEnumerator StartDelayed()
        {
            yield return new WaitForSeconds(0.5f);
            _currentTarget = _defaultCameraTarget;
            _shouldMove = true;
        }
    }

    private void Update()
    {
        if (_shouldMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, _moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _currentTarget.rotation, _rotateSpeed * Time.deltaTime);

            if (transform.position == _currentTarget.position && transform.rotation == _currentTarget.rotation)
            {
                _shouldMove = false;
            }
        }
    }

    public void SetCameraTarget(Transform target)
    {
        _currentTarget = target;
        _shouldMove = true;
    }
}
