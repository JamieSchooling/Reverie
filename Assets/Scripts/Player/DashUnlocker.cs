using System;
using TMPro;
using UnityEngine;

public class DashUnlocker : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private ButtonPromptText _dashButtonPrompt;

    private bool _isDashUnlocked = false;

    private void OnEnable()
    {
        _inputReader.OnDashPressed += OnDashPressed;
    }

    private void OnDisable()
    {
        _inputReader.OnDashPressed -= OnDashPressed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            player.IsDashUnlocked = true;
            _isDashUnlocked = true;
            Time.timeScale = 0f;
            _inputReader.DisablePauseInput();
            _dashButtonPrompt.gameObject.SetActive(true);
        }
    }

    private void OnDashPressed()
    {
        if (_isDashUnlocked)
        {
            Time.timeScale = 1f;
            _inputReader.EnablePauseInput();
            _dashButtonPrompt.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

}
