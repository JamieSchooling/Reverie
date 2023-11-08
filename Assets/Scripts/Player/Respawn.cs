using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] private Transform _defaultRespawnPoint;

    private Vector2 _respawnPoint;

    void Awake()
    {   
        _respawnPoint = _defaultRespawnPoint.transform.position;

        _playerController.ResetVelocity();
        transform.position = _respawnPoint;
    }

    public void SetRespawnPoint(Vector2 respawnPoint)
    {
        _respawnPoint = respawnPoint;
    }

    public void RespawnPlayer()
    {
        _playerController.ResetVelocity();
        transform.position = _respawnPoint;
        if (Camera.main.TryGetComponent(out CameraController cameraController))
        {
            cameraController.ResetCamera();
        }
    }
}
