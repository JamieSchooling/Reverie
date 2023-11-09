using UnityEngine;

public class CameraMoveTrigger : MonoBehaviour
{
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private float _speed;
    [SerializeField] private bool _shouldResetToHere = true;
    [SerializeField] private bool _shouldAdjustPlayerPosition = true;

    private CameraController _cameraController;

    private void Start()
    {
        _cameraController = Camera.main.GetComponent<CameraController>();
        if (_cameraController == null )
        {
            throw new System.Exception("Camera Controller not found on Main Camera. " +
                "Make sure that your camera has the 'Main Camera' tag AND has the Camera Controller component");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Respawn player))
        {
            if (_cameraController == null)
            {
                throw new System.Exception("Camera Controller not found on Main Camera. " +
                    "Make sure that your camera has the 'Main Camera' tag AND has the Camera Controller component");
            }

            _cameraController.SetCameraTarget(
                _cameraTarget, _speed, _shouldResetToHere, 
                _shouldAdjustPlayerPosition ? player.ResetObject : null);
        }
    }
}
