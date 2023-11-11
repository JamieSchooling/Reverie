using UnityEngine;

public class SlowFallPowerUp : MonoBehaviour, IResettable
{
    [SerializeField] private GameObject _visual;

    public void ResetObject()
    {
        _visual.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            player.IsSlowFalling = true;
            _visual.SetActive(false);
        }
    }
}
