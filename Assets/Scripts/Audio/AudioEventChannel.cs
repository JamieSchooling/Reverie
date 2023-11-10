using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Audio/Audio Event Channel")]
public class AudioEventChannel : ScriptableObject
{
    public event Action<AudioClip> OnAudioPlayRequested;
    public event Action OnAudioStopRequested;

    public void RequestPlayAudio(AudioClip clip)
    {
        OnAudioPlayRequested?.Invoke(clip);
    }

    public void RequestStopAudio()
    {
        OnAudioStopRequested?.Invoke();
    }
}
