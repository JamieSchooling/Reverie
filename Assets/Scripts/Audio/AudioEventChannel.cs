using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Audio/Audio Event Channel")]
public class AudioEventChannel : ScriptableObject
{
    public event Action<AudioClip> OnAudioRequested;

    public void RequestPlayAudio(AudioClip clip)
    {
        OnAudioRequested?.Invoke(clip);
    }
}
