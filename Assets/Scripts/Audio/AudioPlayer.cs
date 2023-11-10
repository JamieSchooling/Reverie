using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioEventChannel _audioEventChannel;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _audioEventChannel.OnAudioPlayRequested += PlayAudio;
        _audioEventChannel.OnAudioStopRequested += StopAudio;
    }

    private void OnDisable()
    {
        _audioEventChannel.OnAudioPlayRequested -= PlayAudio;
        _audioEventChannel.OnAudioStopRequested -= StopAudio;
    }

    private void PlayAudio(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    private void StopAudio()
    {
        _audioSource.Stop();
    }
}
