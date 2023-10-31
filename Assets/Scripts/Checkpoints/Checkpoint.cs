using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;

    public event Action<Vector2> OnCheckpointReached;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger entered");
        if (collision.CompareTag("CheckpointDetector"))
        {
            Debug.Log("checkpoint set");
            OnCheckpointReached?.Invoke(_spawnPoint.position);
        }
    }
}
