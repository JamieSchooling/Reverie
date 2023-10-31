using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Vector2 _defaultRespawnPoint;

    private Vector2 _respawnPoint;

    void Awake()
    {
        _respawnPoint = _defaultRespawnPoint;
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        transform.position = _respawnPoint;
    }
}
