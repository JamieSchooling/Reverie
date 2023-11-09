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
        _audioEventChannel.OnAudioRequested += PlayAudio;
    }

    private void OnDisable()
    {
        _audioEventChannel.OnAudioRequested -= PlayAudio;
    }

    private void PlayAudio(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
