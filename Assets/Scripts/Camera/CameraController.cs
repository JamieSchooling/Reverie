using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private float _speed;
    private bool _shouldMove = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
           _shouldMove = true;
        }
    }
    private void Update()
    {
        if(_shouldMove)
        {

            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, _cameraTarget.position, _speed * Time.deltaTime);
            if (Camera.main.transform.position == _cameraTarget.position)
            {
                _shouldMove = false;
            }
        }
    }
}
