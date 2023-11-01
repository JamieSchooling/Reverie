using System.Collections;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _reverseDelay;

    private bool _canMove = true;
    private int _direction = 1;

    private void Start()
    {
        transform.position = _startPoint.position;
    }

    private void Update()
    {
        if (!_canMove) return;

        Vector2 target = CurrentMovementTarget();

        transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);

        if ((Vector2)transform.position == target)
        {
            StartCoroutine(ReverseDelayed(_reverseDelay));
        }
    }

    private Vector2 CurrentMovementTarget()
    {
        if (_direction == 1)
            return _endPoint.position;
        else
            return _startPoint.position;
    }

    private IEnumerator ReverseDelayed(float delay)
    {
        _canMove = false;

        yield return new WaitForSeconds(delay);

        _direction *= -1;
        _canMove = true;
    }

    private void OnDrawGizmos()
    {
        if (_startPoint != null && _endPoint != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(_startPoint.position, _endPoint.position);
        }
    }
}
