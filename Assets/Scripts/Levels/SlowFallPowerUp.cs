using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFallPowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            player.IsSlowFalling = true;
        }
    }
}
