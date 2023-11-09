using UnityEngine;
using UnityEngine.Audio;

public class OptionsInitialiser : MonoBehaviour
{
    [SerializeField] private AudioMixer _gameAudioMixer;

    private void Start()
    {
        InitialiseSettings();
    }

    private void InitialiseSettings()
    {
        SettingsData settingsData = PersistentDataManager.Instance.GetPlayerPrefs();

        Screen.fullScreen = settingsData.isFullscreen;
        _gameAudioMixer.SetFloat("MasterVolume", settingsData.masterVolume);
    }
}
