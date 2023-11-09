using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Respawn player))
        {
            player.RespawnPlayer();
            ResetLevel();
        }
    }

    private void ResetLevel()
    {
        List<IResettable> resettables = new List<IResettable>(FindObjectsOfType<MonoBehaviour>().OfType<IResettable>());
        foreach (var resettable in resettables)
        {
            resettable.ResetObject();
        }
    }
}
