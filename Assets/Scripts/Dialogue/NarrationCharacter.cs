using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Character")]
public class NarrationCharacter : ScriptableObject
{
    [SerializeField] private string _characterName;

    public string CharacterName => _characterName;
}
