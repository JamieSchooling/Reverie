using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject _pauseScreen;

    private bool _isPaused;

    private void Awake()
    {
        _inputReader.OnPausePressed += OnPausePressed;
        _pauseScreen.SetActive(false);
    }

    private void OnDestroy()
    {
        _inputReader.OnPausePressed -= OnPausePressed;
    }

    private void OnPausePressed()
    {
        if (_isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        _inputReader.DisableGameplayInput();
        _inputReader.DisableDialogueInput();
        _pauseScreen.SetActive(true);
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        StartCoroutine(EnableInputDelayed());

        // Player jumps when using a controller without this delay.
        IEnumerator EnableInputDelayed()
        {
            yield return new WaitForSeconds(0.01f);
            _inputReader.EnableGameplayInput();
            _inputReader.EnableDialogueInput();
            _pauseScreen.SetActive(false);
        }
    }

    public void Retry()
    {
        List<IResettable> resettables = new List<IResettable>(FindObjectsOfType<MonoBehaviour>().OfType<IResettable>());
        foreach(var resettable in resettables)
        {
            resettable.ResetObject();
        }
        Resume();
    }

    public void SaveAndQuitToMenu()
    {
        PersistentDataManager.Instance.SaveGame();
        SceneManager.LoadSceneAsync("Main Menu").completed += (asyncOperation) =>
        {
            Time.timeScale = 1f;
            _inputReader.EnableGameplayInput();
        };
    }
}
