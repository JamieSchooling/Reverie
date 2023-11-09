using UnityEngine;

public class DashUnlocker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            player.IsDashUnlocked = true;
        }
    }
}
