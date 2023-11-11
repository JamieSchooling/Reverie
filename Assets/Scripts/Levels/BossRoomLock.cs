using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class BossRoomLock : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int _requiredCollectibleAmount = 3;
    [SerializeField] private float _unlockDuration = 2f;
    [Header("Display")]
    [SerializeField] private TextMeshProUGUI _displayText;

    public static BossRoomLock Instance { get; private set; }

    public int LevelCollectibleCount
    { 
        get
        {
            return LevelCollectibleCount;
        }
        set
        {
            LevelCollectibleCount = value;
            _displayText.text = $"{value} / {_requiredCollectibleAmount}";
        }
    }

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player) 
            && LevelCollectibleCount >= _requiredCollectibleAmount)
        {
            StartCoroutine(Unlock());
        }
    }

    private IEnumerator Unlock()
    {
        yield return new WaitForSeconds(_unlockDuration);

        gameObject.SetActive(false);
    }
}
