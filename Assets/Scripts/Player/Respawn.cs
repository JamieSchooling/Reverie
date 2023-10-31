using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Vector2 _defaultRespawnPoint;
    [SerializeField] private Checkpoint _checkpoint;

    private Vector2 _respawnPoint;


    void Awake()
    {
        _checkpoint.OnCheckpointReached += SetRespawnPoint;
        _respawnPoint = _defaultRespawnPoint;
        RespawnPlayer();

    }
    private void SetRespawnPoint(Vector2 respawnPoint)
    {
        _respawnPoint = respawnPoint;
    }

    public void RespawnPlayer()
    {
        transform.position = _respawnPoint;
    }
}
