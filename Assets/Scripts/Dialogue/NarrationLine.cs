using System;
using UnityEngine;

[Serializable]
public class NarrationLine
{
    [SerializeField] private NarrationCharacter _speaker;
    [SerializeField] private string _text;
    [SerializeField] private AudioClip _audio;

    public NarrationCharacter Speaker => _speaker;
    public string Text => _text;
    public AudioClip Audio => _audio;
}
