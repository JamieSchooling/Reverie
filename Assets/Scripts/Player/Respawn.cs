using UnityEngine;

public class Respawn : MonoBehaviour, IPersistentData, IResettable
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] private Transform _defaultRespawnPoint;
    [SerializeField] private bool _shouldSaveRespawnPoint = true;

    private Vector2 _respawnPoint = Vector2.zero;
    private Vector2 _currentRespawnPoint = Vector2.zero;
    private int _deathCount = 0;

    public void SetRespawnPoint(Vector2 respawnPoint, bool shouldSave)
    {
        _currentRespawnPoint = respawnPoint;
        if (shouldSave) _respawnPoint = _currentRespawnPoint;
    }

    public void RespawnPlayer()
    {
        _deathCount++;
        if (Camera.main.TryGetComponent(out CameraController cameraController))
        {
            cameraController.ResetCamera();
        }
        ResetObject();
    }

    public void ResetObject()
    {
        _playerController.ResetVelocity();
        transform.position = _currentRespawnPoint;
    }

    public void LoadData(GameData data)
    {
        _deathCount = data.deathCount;
        if (_shouldSaveRespawnPoint) _respawnPoint = data.playerRespawnPoint;

        if (_respawnPoint == Vector2.zero) _respawnPoint = _defaultRespawnPoint.position;

        _currentRespawnPoint = _respawnPoint;

        transform.position = _respawnPoint;
    }

    public void SaveData(ref GameData data)
    {
        data.deathCount = _deathCount;
        if (_shouldSaveRespawnPoint) data.playerRespawnPoint = _respawnPoint;
    }
}
