using UnityEngine;

public class Respawn : MonoBehaviour, IDataPersistence
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] private Transform _defaultRespawnPoint;

    private Vector2 _respawnPoint;
    private int _deathCount = 0;

    public void SetRespawnPoint(Vector2 respawnPoint)
    {
        _respawnPoint = respawnPoint;
    }

    public void RespawnPlayer()
    {
        _deathCount++;
        Debug.Log(_deathCount);
        _playerController.ResetVelocity();
        transform.position = _respawnPoint;
        if (Camera.main.TryGetComponent(out CameraController cameraController))
        {
            cameraController.ResetCamera();
        }
    }

    public void LoadData(GameData data)
    {
        _deathCount = data.deathCount;
        _respawnPoint = data.playerRespawnPoint;

        if (_respawnPoint == Vector2.zero) _respawnPoint = _defaultRespawnPoint.position;

        transform.position = _respawnPoint;
    }

    public void SaveData(ref GameData data)
    {
        data.deathCount = _deathCount;
        data.playerRespawnPoint = _respawnPoint;
    }
}
