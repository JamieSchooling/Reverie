using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Respawn respawn))
        {
            respawn.SetRespawnPoint(_spawnPoint.position);
        }
    }
}
