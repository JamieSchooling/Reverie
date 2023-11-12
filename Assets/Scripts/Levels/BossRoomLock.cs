using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BossRoomLock : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int _requiredCollectibleAmount = 3;
    [SerializeField] private float _unlockDuration = 2f;
    [Header("Display")]
    [SerializeField] private TextMeshProUGUI _displayText;
    [SerializeField] private GameObject _displayUI;

    private int _levelCollectibleCount = 0;

    public int LevelCollectibleCount
    { 
        get
        {
            return _levelCollectibleCount;
        }
        set
        {
            _levelCollectibleCount = value;
            _displayText.text = $"{value} / {_requiredCollectibleAmount}";
        }
    }

    public static BossRoomLock Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(true);
    }

    private void Start()
    {
        if (LevelCollectibleCount >= _requiredCollectibleAmount)
        {
            _displayUI.SetActive(false);
            gameObject.SetActive(false);
        }

        _displayText.text = $"{_levelCollectibleCount} / {_requiredCollectibleAmount}";
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

        _displayUI.SetActive(false);
        gameObject.SetActive(false);
    }
}
