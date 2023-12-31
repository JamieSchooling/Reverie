using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private bool _shouldSaveThisRespawnPoint = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Respawn respawn))
        {
            respawn.SetRespawnPoint(_spawnPoint.position, _shouldSaveThisRespawnPoint);
        }
    }
}
