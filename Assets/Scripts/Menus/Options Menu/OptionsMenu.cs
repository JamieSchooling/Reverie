using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Video Options")]
    [SerializeField] private Toggle _fullscreenToggle;

    [Header("Audio Options")]
    [SerializeField] private AudioMixer _gameAudioMixer;
    [SerializeField] private Slider _masterVolumeSlider;

    private SettingsData _settingsData;

    private void OnEnable()
    {
        _settingsData = PersistentDataManager.Instance.GetPlayerPrefs();

        SetFullscreen(_settingsData.isFullscreen);
        _fullscreenToggle.isOn = _settingsData.isFullscreen;
        _fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        SetMasterVolume(_settingsData.masterVolume);
        _masterVolumeSlider.value = _settingsData.masterVolume;
        _masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    private void OnDisable()
    {
        _fullscreenToggle.onValueChanged.RemoveListener(SetFullscreen);

        _masterVolumeSlider.onValueChanged.RemoveListener(SetMasterVolume);

        PersistentDataManager.Instance.SavePlayerPrefs(_settingsData);
    }

    private void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        _settingsData.isFullscreen = fullscreen;
    }

    private void SetMasterVolume(float volume)
    {
        _gameAudioMixer.SetFloat("MasterVolume", volume);
        _settingsData.masterVolume = volume;
    }
}
