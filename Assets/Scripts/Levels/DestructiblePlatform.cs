using System.Collections;
using UnityEngine;

public class DestructiblePlatform : MonoBehaviour, IResettable
{
    [SerializeField] private GameObject _platform;
    [SerializeField] private float _destructionDelay = 1f;
    [SerializeField] private float _destructionDuration = 1.5f;
    [SerializeField] private bool _shouldReset = true;

    private bool _canDestroy = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player) && _canDestroy)
        {
            StartCoroutine(DestroyPlatform());
        }
    }

    private IEnumerator DestroyPlatform()
    {
        _canDestroy = false;

        yield return new WaitForSeconds(_destructionDelay);

        _platform.SetActive(false);

        yield return new WaitForSeconds(_destructionDuration);

        _platform.SetActive(true);
        _canDestroy = true;
    }

    public void ResetObject()
    {
        if (!_shouldReset) return;

        StopAllCoroutines();

        _platform.SetActive(true);
        _canDestroy = true;
    }
}
